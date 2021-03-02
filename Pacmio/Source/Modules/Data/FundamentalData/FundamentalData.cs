/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    [Serializable, DataContract]
    public class FundamentalData : IDataFile
    {
        public FundamentalData(Contract c)
            => ContractKey = c.Key;

        public FundamentalData((string name, Exchange exchange, string typeName) key)
            => ContractKey = key;

        [DataMember]
        public (string name, Exchange exchange, string typeName) ContractKey { get; private set; }

        [IgnoreDataMember]
        public Contract Contract => ContractManager.GetByKey(ContractKey);

        [DataMember]
        public DateTime EarliestTime { get; set; } = DateTime.MinValue;

        [DataMember]
        private Dictionary<(string, DateTime), FundamentalDatum> DataLUT { get; set; } = new Dictionary<(string, DateTime), FundamentalDatum>();

        public FundamentalDatum[] GetList()
        {
            lock (DataLUT)
                return DataLUT.Values.ToArray();
        }

        public FundamentalDatum[] GetList<T>() where T : FundamentalDatum
        {
            lock (DataLUT)
                return DataLUT.Values.Where(n => n is T).ToArray();
        }

        public FundamentalDatum[] GetList(DateTime asOfDate)
        {
            lock (DataLUT)
                return DataLUT.Values.Where(n => n.AsOfDate == asOfDate.Date).ToArray();
        }

        public T GetOrCreateDatum<T>(T data) where T : FundamentalDatum
        {
            lock (DataLUT)
            {
                if (!DataLUT.ContainsKey(data.Key))
                {
                    DataLUT[data.Key] = data;
                }
                return DataLUT[data.Key] as T;
            }
        }

        public bool Remove(FundamentalDatum fd) => Remove(fd.Key);

        public bool Remove((string type, DateTime asOfDate) key)
        {
            lock (DataLUT)
                if (DataLUT.ContainsKey(key))
                {
                    DataLUT.Remove(key);
                    return true;
                }
            return false;
        }

        public bool Remove<T>(DateTime asOfDate) where T : FundamentalDatum
        {
            //var key = (typeof(T).GetAttribute((DataContractAttribute d) => d.Name), asOfDate);
            lock (DataLUT)
            {
                var listToRemove = DataLUT.Values.Where(n => n is T && n.AsOfDate == asOfDate).Select(n => n.Key).ToList();
                listToRemove.ForEach(n => DataLUT.Remove(n));
                return listToRemove.Count() > 0;
            }
        }

        public bool Remove<T>() where T : FundamentalDatum
        {
            lock (DataLUT)
            {
                var listToRemove = DataLUT.Values.Where(n => n is T).Select(n => n.Key).ToList();
                listToRemove.ForEach(n => DataLUT.Remove(n));
                return listToRemove.Count() > 0;
            }
        }

        public void Remove(DataSourceType source)
        {
            lock (DataLUT)
            {
                FundamentalDatum[] datums = DataLUT.Values.Where(n => n.DataSource == source).ToArray();
                foreach (var item in datums) DataLUT.Remove(item.Key);
            }
        }

        public void SetSplit(DateTime asOfDate, double close, double split, DataSourceType dataSource)
        {
            lock (DataLUT)
            {
                asOfDate = asOfDate.Date;
                if (split <= 0) throw new Exception("Split can't be negative: " + split);

                if (split != 1)
                {
                    SplitDatum fdm = GetOrCreateDatum(new SplitDatum(asOfDate));
                    fdm.Close_Price = close;
                    fdm.Split = split;
                    fdm.DataSource = dataSource;
                }
                else
                {
                    Remove<SplitDatum>(asOfDate);
                }
            }
        }

        public void SetDividend(DateTime asOfDate, double close, double dividend, DataSourceType dataSource)
        {
            lock (DataLUT)
            {
                if (dividend != 0)
                {
                    DividendDatum fdm = GetOrCreateDatum(new DividendDatum(asOfDate));
                    fdm.Close_Price = close;
                    fdm.Dividend = dividend;
                    fdm.DataSource = dataSource;
                }
                else
                {
                    Remove<DividendDatum>(asOfDate);
                }
            }
        }

        public MultiPeriod<(double Price, double Volume)> BarTableAdjust(bool includeDividend = false)
        {
            MultiPeriod<(double Price, double Volume)> list = new MultiPeriod<(double Price, double Volume)>();

            List<(DateTime AsOfDate, bool isSplit, double adjust_ratio)> split_dividend_list = new List<(DateTime AsOfDate, bool isSplit, double adjust_ratio)>();

            lock (DataLUT)
            {
                split_dividend_list = includeDividend ?

                DataLUT.Values.Select(n => {
                    if (n is SplitDatum sp)
                        return (sp.AsOfDate, true, sp.Split);
                    if (n is DividendDatum dv)
                        return (dv.AsOfDate, false, (1 + dv.Ratio));
                    else
                        return (DateTime.MinValue, false, -1);
                }).Where(n => n.Item3 > 0).OrderByDescending(n => n.AsOfDate).ToList() :

                DataLUT.Values.Select(n => {
                    if (n is SplitDatum sp)
                        return (sp.AsOfDate, true, sp.Split);
                    else
                        return (DateTime.MinValue, false, -1);
                }).Where(n => n.Item3 > 0).OrderByDescending(n => n.AsOfDate).ToList();
            }

            DateTime latestTime = DateTime.MaxValue;
            double adj_price = 1;
            double adj_vol = 1;

            foreach (var pair in split_dividend_list)
            {
                DateTime asOfDate = pair.AsOfDate;
                double value = pair.adjust_ratio;

                //Console.WriteLine("->> Loading: " + time + " / " + pair.Key.Type + " / " + pair.Value.Value);

                if (pair.isSplit && value != 1)
                {
                    list.Add(asOfDate, latestTime, (adj_price, adj_vol));
                    adj_price /= value;
                    adj_vol /= value;
                    latestTime = asOfDate;
                }

                if (!pair.isSplit && value != 0 && includeDividend)
                {
                    list.Add(asOfDate, latestTime, (adj_price, adj_vol));
                    //adj_price *= 1 / (1 + value);
                    adj_price /= value;
                    latestTime = asOfDate;
                }
            }

            list.Add(latestTime, DateTime.MinValue, (adj_price, adj_vol));

            return list;
        }

        #region File Operation

        public static string GetDataFileName((string name, Exchange exchange, string typeName) ContractKey)
            => Root.HistoricalDataPath(ContractKey) + "\\_FundamentalData\\$" + ContractKey.name + ".json";

        [IgnoreDataMember]
        public string DataFileName => GetDataFileName(ContractKey);

        public void SaveFile()
        {
            lock (DataLUT)
            {
                this.SerializeJsonFile(DataFileName);
            }
        }

        public static FundamentalData LoadFile((string name, Exchange exchange, string typeName) key)
        {
            if (Serialization.DeserializeJsonFile<FundamentalData>(GetDataFileName(key)) is FundamentalData fd)
            {
                return fd;
            }
            else
                return new FundamentalData(key);
        }

        public static FundamentalData LoadFile(Contract c) => LoadFile(c.Key);

        #endregion File Operation

        #region Export CSV

        public void ExportCSV(string fileName)
        {
            var rows = GetList().OrderByDescending(n => n.AsOfDate).ThenBy(n => n.TypeName).ThenBy(n => n.DataSource);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Type,Date Time,Data Source,Close Price,Value,Other Info");

            foreach (var row in rows)
            {
                sb.AppendLine(string.Join(",", new string[] {
                    row.TypeName,
                    row.AsOfDate.ToString("MM-dd-yyyy"),
                    row.DataSource.ToString(),
                    row.Close_Price.ToString(),
                    row.Value.ToString(),
                    row.Comment
                }));
            }

            sb.ToFile(fileName);
        }

        #endregion Export CSV
    }
}
