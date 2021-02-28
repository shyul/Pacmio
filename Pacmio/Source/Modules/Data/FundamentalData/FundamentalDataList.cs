/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    [Serializable, DataContract(Name = "FundamentalData")]
    public class FundamentalDataList : IDataFile
    {
        public FundamentalDataList(Contract c)
            => ContractKey = c.Key;

        public FundamentalDataList((string name, Exchange exchange, string typeName) key)
            => ContractKey = key;

        [DataMember]
        public (string name, Exchange exchange, string typeName) ContractKey { get; private set; }

        [IgnoreDataMember]
        public Contract Contract => ContractManager.GetByKey(ContractKey);

        [DataMember]
        public DateTime EarliestTime { get; set; } = DateTime.MinValue;

        [DataMember]
        private Dictionary<(FundamentalType, DateTime), FundamentalDatum> DataLUT { get; set; } = new Dictionary<(FundamentalType, DateTime), FundamentalDatum>();

        public FundamentalDatum[] GetList(FundamentalType type)
        {
            lock (DataLUT)
                return DataLUT.Values.Where(n => n.Type == type).ToArray();
        }

        public FundamentalDatum GetOrCreateDatum(FundamentalType type, DateTime asOfDate)
            => GetOrCreateDatum((type, asOfDate));

        public FundamentalDatum GetOrCreateDatum((FundamentalType, DateTime) key)
        {
            lock (DataLUT)
            {
                if (!DataLUT.ContainsKey(key))
                {
                    DataLUT[key] = new FundamentalDatum(key);
                }
                return DataLUT[key];
            }
        }

        public void SetSplit(DateTime asOfDate, double close, double split, DataSourceType dataSource)
        {
            var key = (FundamentalType.Split, asOfDate);

            lock (DataLUT)
            {
                if (split <= 0) throw new Exception("Split can't be negative: " + split);

                if (split != 1)
                {
                    FundamentalDatum fdm = GetOrCreateDatum(key);
                    fdm.Close_Price = close;
                    fdm.Value = split;
                    fdm.DataSource = dataSource;
                }
                else if (DataLUT.ContainsKey(key))
                {
                    DataLUT.Remove(key);
                }
            }
        }

        public void SetDividend(DateTime asOfDate, double close, double dividend, DataSourceType dataSource)
        {
            var key = (FundamentalType.Dividend, asOfDate);

            lock (DataLUT)
            {
                if (dividend != 0)
                {
                    FundamentalDatum fdm = GetOrCreateDatum(key);
                    fdm.Close_Price = close;
                    fdm.Value = dividend;
                    fdm.DataSource = dataSource;
                }
                else if (DataLUT.ContainsKey(key))
                {
                    DataLUT.Remove(key);
                }
            }
        }

        public bool Remove(FundamentalDatum fd) => Remove(fd.Key);

        public bool Remove(FundamentalType type, DateTime asOfDate) => Remove((type, asOfDate));

        public bool Remove((FundamentalType, DateTime) key)
        {
            lock (DataLUT)
                if (DataLUT.ContainsKey(key))
                {
                    DataLUT.Remove(key);
                    return true;
                }
            return false;
        }

        public void Remove(FundamentalType type)
        {
            lock (DataLUT)
            {
                var listToRemove = DataLUT.Values.Where(n => n.Type == type).Select(n => n.Key).ToList();
                listToRemove.ForEach(n => DataLUT.Remove(n));
            }
        }

        public MultiPeriod<(double Price, double Volume)> BarTableAdjust(bool includeDividend = false)
        {
            MultiPeriod<(double Price, double Volume)> list = new MultiPeriod<(double Price, double Volume)>();

            var split_list = GetList(FundamentalType.Split).Select(n => (n.AsOfDate, true, n.Value));
            var dividend_list = GetList(FundamentalType.Dividend).Select(n => (n.AsOfDate, false, n.Close_Price > 0 ? n.Value / n.Close_Price : 0));
            var split_dividend_list = split_list.Concat(dividend_list).OrderByDescending(n => n.AsOfDate).ToArray();

            DateTime latestTime = DateTime.MaxValue;
            double adj_price = 1;
            double adj_vol = 1;

            foreach (var pair in split_dividend_list)
            {
                DateTime asOfDate = pair.AsOfDate;
                double value = pair.Item3;

                //Console.WriteLine("->> Loading: " + time + " / " + pair.Key.Type + " / " + pair.Value.Value);

                if (pair.Item2 && value != 1)
                {
                    list.Add(asOfDate, latestTime, (adj_price, adj_vol));
                    adj_price /= value;
                    adj_vol /= value;
                    latestTime = asOfDate;
                }

                if (!pair.Item2 && value != 0 && includeDividend)
                {
                    list.Add(asOfDate, latestTime, (adj_price, adj_vol));
                    adj_price *= 1 / (1 + value);
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

        public static FundamentalDataList LoadFile((string name, Exchange exchange, string typeName) key)
        {
            if (Serialization.DeserializeJsonFile<FundamentalDataList>(GetDataFileName(key)) is FundamentalDataList fd)
            {
                return fd;
            }
            else
                return new FundamentalDataList(key);
        }

        public static FundamentalDataList LoadFile(Contract c) => LoadFile(c.Key);

        #endregion File Operation
    }
}
