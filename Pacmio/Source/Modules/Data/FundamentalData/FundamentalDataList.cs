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
    public class FundamentalDataList
    {
        [DataMember]
        public (string name, Exchange exchange, string typeName) ContractInfo { get; set; }

        //[IgnoreDataMember]
        //public Contract Contract => ContractManager

        [DataMember]
        private Dictionary<(Type, DateTime), FundamentalDatum> DataLUT { get; } = new Dictionary<(Type, DateTime), FundamentalDatum>();

        public IEnumerable<T> GetList<T>() where T : FundamentalDatum => DataLUT.Values.Where(n => n is T).Select(n => n as T);

        public T GetOrCreateDatum<T>(T datum) where T : FundamentalDatum
        {
            var key = datum.Key;
            if (!DataLUT.ContainsKey(key))
            {
                DataLUT[key] = datum;
            }
            return DataLUT[key] as T;
        }

        public MultiPeriod<(double Price, double Volume)> BarTableAdjust(bool includeDividend = false)
        {
            MultiPeriod<(double Price, double Volume)> list = new MultiPeriod<(double Price, double Volume)>();

            var split_list = GetList<SplitDatum>().Select(n => (n.AsOfDate, true, n.Split));
            var dividend_list = GetList<DividendDatum>().Select(n => (n.AsOfDate, false, n.Percent));
            var split_dividend_list = split_list.Concat(dividend_list).OrderByDescending(n => n.AsOfDate);

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
    }
}
