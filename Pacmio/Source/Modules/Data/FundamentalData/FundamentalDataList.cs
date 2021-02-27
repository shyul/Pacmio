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
    [Serializable, DataContract]
    public class FundamentalDataList : IDataFile
    {
        public FundamentalDataList(Contract c)
            => ContractKey = c.Key;

        public FundamentalDataList((string name, Exchange exchange, string typeName) key)
            => ContractKey = key;

        [DataMember]
        public (string name, Exchange exchange, string typeName) ContractKey { get; }

        [IgnoreDataMember]
        public Contract Contract => ContractManager.GetByKey(ContractKey);

        [DataMember]
        private Dictionary<(Type, DateTime), FundamentalDatum> DataLUT { get; } = new Dictionary<(Type, DateTime), FundamentalDatum>();

        public T[] GetList<T>() where T : FundamentalDatum
        {
            lock (DataLUT)
                return DataLUT.Values.Where(n => n is T).Select(n => n as T).ToArray();
        }

        public T GetOrCreateDatum<T>(T fd) where T : FundamentalDatum
        {
            var key = fd.Key;
            lock (DataLUT)
            {
                if (!DataLUT.ContainsKey(key))
                {
                    DataLUT[key] = fd;
                }
                return DataLUT[key] as T;
            }
        }

        public bool Remove(FundamentalDatum fd)
        {
            lock (DataLUT)
                if (DataLUT.ContainsKey(fd.Key))
                {
                    DataLUT.Remove(fd.Key);
                    return true;
                }
            return false;
        }

        public void Remove<T>() where T : FundamentalDatum
        {
            lock (DataLUT)
            {
                var listToRemove = DataLUT.Values.Where(n => n is T).Select(n => n.Key).ToList();
                listToRemove.ForEach(n => DataLUT.Remove(n));
            }
        }

        public MultiPeriod<(double Price, double Volume)> BarTableAdjust(bool includeDividend = false)
        {
            MultiPeriod<(double Price, double Volume)> list = new MultiPeriod<(double Price, double Volume)>();

            var split_list = GetList<SplitDatum>().Select(n => (n.AsOfDate, true, n.Split));
            var dividend_list = GetList<DividendDatum>().Select(n => (n.AsOfDate, false, n.Percent));
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
                this.SerializeJsonFile(DataFileName);
        }

        public static FundamentalDataList LoadFile((string name, Exchange exchange, string typeName) key)
            => Serialization.DeserializeJsonFile<FundamentalDataList>(GetDataFileName(key));

        public static FundamentalDataList LoadFile(Contract c)
            => Serialization.DeserializeJsonFile<FundamentalDataList>(GetDataFileName(c.Key));

        #endregion File Operation
    }
}
