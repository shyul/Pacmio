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
        private Dictionary<DateTime, SplitDatum> SplitLUT { get; set; } = new Dictionary<DateTime, SplitDatum>();

        private SplitDatum GetOrCreateSplitDatum(DateTime asOfDate)
        {
            if (!SplitLUT.ContainsKey(asOfDate))
                SplitLUT[asOfDate] = new SplitDatum(asOfDate);

            return SplitLUT[asOfDate];
        }

        public void SetSplit(DateTime asOfDate, double close, double split, DataSourceType dataSource)
        {
            lock (SplitLUT)
            {
                asOfDate = asOfDate.Date;
                if (split <= 0) throw new Exception("Split can't be negative: " + split);

                if (split != 1)
                {
                    SplitDatum fdm = GetOrCreateSplitDatum(asOfDate);

                    fdm.Close_Price = close;
                    fdm.Split = split;
                    fdm.DataSource = dataSource;
                }
                else if (SplitLUT.ContainsKey(asOfDate))
                {
                    SplitLUT.Remove(asOfDate);
                }
            }
        }

        [DataMember]
        private Dictionary<DateTime, DividendDatum> DividendLUT { get; set; } = new Dictionary<DateTime, DividendDatum>();

        private DividendDatum GetOrCreateDividendDatum(DateTime asOfDate)
        {
            if (!DividendLUT.ContainsKey(asOfDate))
                DividendLUT[asOfDate] = new DividendDatum(asOfDate);

            return DividendLUT[asOfDate];
        }

        public void SetDividend(DateTime asOfDate, double close, double dividend, DataSourceType dataSource)
        {
            lock (DividendLUT)
            {
                if (dividend != 0)
                {
                    DividendDatum fdm = GetOrCreateDividendDatum(asOfDate);
                    fdm.Close_Price = close;
                    fdm.Dividend = dividend;
                    fdm.DataSource = dataSource;
                }
                else if (DividendLUT.ContainsKey(asOfDate))
                {
                    DividendLUT.Remove(asOfDate);
                }
            }
        }

        public void Remove(DataSourceType source)
        {
            lock (SplitLUT)
            {
                SplitDatum[] splits = SplitLUT.Values.Where(n => n.DataSource == source).ToArray();
                foreach (var item in splits) SplitLUT.Remove(item.AsOfDate);
            }

            lock (DividendLUT)
            {
                DividendDatum[] dividends = DividendLUT.Values.Where(n => n.DataSource == source).ToArray();
                foreach (var item in dividends) DividendLUT.Remove(item.AsOfDate);
            }

            lock (OtherDataLUT)
            {
                FundamentalDatum[] datums = OtherDataLUT.Values.Where(n => n.DataSource == source).ToArray();
                foreach (var item in datums) OtherDataLUT.Remove(item.Key);
            }
        }

        [DataMember]
        private Dictionary<(FundamentalType, DateTime), FundamentalDatum> OtherDataLUT { get; set; } = new Dictionary<(FundamentalType, DateTime), FundamentalDatum>();

        public FundamentalDatum[] GetList()
        {
            lock (OtherDataLUT)
                return OtherDataLUT.Values.ToArray();
        }

        public FundamentalDatum[] GetList(FundamentalType type)
        {
            lock (OtherDataLUT)
                return OtherDataLUT.Values.Where(n => n.Type == type).ToArray();
        }

        public FundamentalDatum[] GetList(DateTime asOfDate)
        {
            lock (OtherDataLUT)
                return OtherDataLUT.Values.Where(n => n.AsOfDate == asOfDate.Date).ToArray();
        }

        public FundamentalDatum GetOrCreateDatum(FundamentalType type, DateTime asOfDate)
            => GetOrCreateDatum((type, asOfDate));

        public FundamentalDatum GetOrCreateDatum((FundamentalType, DateTime) key)
        {
            lock (OtherDataLUT)
            {
                if (!OtherDataLUT.ContainsKey(key))
                {
                    OtherDataLUT[key] = new FundamentalDatum(key);
                }
                return OtherDataLUT[key];
            }
        }

        public bool Remove(FundamentalDatum fd) => Remove(fd.Key);

        public bool Remove(FundamentalType type, DateTime asOfDate) => Remove((type, asOfDate));

        public bool Remove((FundamentalType, DateTime) key)
        {
            lock (OtherDataLUT)
                if (OtherDataLUT.ContainsKey(key))
                {
                    OtherDataLUT.Remove(key);
                    return true;
                }
            return false;
        }

        public void Remove(FundamentalType type)
        {
            lock (OtherDataLUT)
            {
                var listToRemove = OtherDataLUT.Values.Where(n => n.Type == type).Select(n => n.Key).ToList();
                listToRemove.ForEach(n => OtherDataLUT.Remove(n));
            }
        }

        public MultiPeriod<(double Price, double Volume)> BarTableAdjust(bool includeDividend = false)
        {
            MultiPeriod<(double Price, double Volume)> list = new MultiPeriod<(double Price, double Volume)>();

            List<(DateTime AsOfDate, bool isSplit, double adjust_ratio)> split_dividend_list = new List<(DateTime AsOfDate, bool isSplit, double adjust_ratio)>();

            lock (SplitLUT)
            {
                // var split_list = SplitLUT.Values.Select(n => (n.AsOfDate, true, n.Split));
                split_dividend_list = SplitLUT.Values.Select(n => (n.AsOfDate, true, n.Split)).ToList();
            }

            if (includeDividend)
                lock (DividendLUT)
                {
                    //var dividend_list = DividendLUT.Values.Select(n => (n.PayDate, false, n.Ratio));
                    var dividend_list = DividendLUT.Values.Select(n => (n.AsOfDate, false, (1 + n.Ratio)));
                    split_dividend_list = split_dividend_list.Concat(dividend_list).OrderByDescending(n => n.AsOfDate).ToList();
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
            lock (OtherDataLUT)
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
            IEnumerable<(string type, DateTime date, DataSourceType source, double close, double value, string info)> rows
                = GetList().Select(n => (n.Type.ToString(), n.AsOfDate, n.DataSource, n.Close_Price, n.Value, "P = " + n.Value_Preliminary + " | R = " + n.Value_Restated + " | A = " + n.Value_Audited));

            lock (SplitLUT)
                rows = rows.Concat(SplitLUT.Values.Select(n => ("Split", n.AsOfDate, n.DataSource, n.Close_Price, n.Split, string.Empty)));

            lock (DividendLUT)
                rows = rows.Concat(DividendLUT.Values.Select(n => ("Dividend", n.AsOfDate, n.DataSource, n.Close_Price, n.Dividend, (n.Ratio * 100).ToString("0.###") + "%")));

            rows = rows.OrderByDescending(n => n.date).ThenBy(n => n.type).ThenBy(n => n.source);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Type,Date Time,Data Source,Close Price,Value,Other Info");

            foreach (var row in rows)
            {
                sb.AppendLine(string.Join(",", new string[] {
                    row.type.ToString(),
                    row.date.ToString("MM-dd-yyyy"),
                    row.source.ToString(),
                    row.close.ToString(),
                    row.value.ToString(),
                    row.info
                }));
            }

            sb.ToFile(fileName);
        }

        #endregion Export CSV
    }
}
