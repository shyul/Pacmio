/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.Serialization;
using Xu;
using System.Threading.Tasks;

namespace Pacmio
{
    [Serializable, DataContract]
    public class HistoricalData : BidAskData
    {
        [DataMember]
        public DateTime BarTableEarliestTime { get; set; } = DateTime.MinValue;

        [DataMember]
        public Dictionary<int, (DateTime Time, double Price, double Size, Exchange MarketMaker)> MarketDepth { get; private set; }
            = new Dictionary<int, (DateTime Time, double Price, double Size, Exchange MarketMaker)>();

        [DataMember]
        public double ShortStatus { get; set; } = double.NaN;

        [DataMember]
        public double ShortableShares { get; set; } = double.NaN;

        [DataMember]
        public Dictionary<DateTime, (DataSource DataSource, double Close, double Dividend)> DividendTable { get; private set; } = new Dictionary<DateTime, (DataSource DataSource, double Close, double Dividend)>();

        [DataMember]
        public Dictionary<DateTime, (DataSource DataSource, double Split)> SplitTable { get; private set; } = new Dictionary<DateTime, (DataSource DataSource, double Split)>();

        [DataMember]
        public Dictionary<DateTime, (DataSource DataSource, double EPS)> EPSTable { get; private set; } = new Dictionary<DateTime, (DataSource DataSource, double EPS)>();

        [DataMember]
        public Dictionary<DateTime, (DataSource DataSource, double Target)> TargetPriceList { get; private set; } = new Dictionary<DateTime, (DataSource DataSource, double Target)>();

        public MultiPeriod<(double Price, double Volume)> BarTableAdjust(bool includeDividend = false)
        {
            MultiPeriod<(double Price, double Volume)> list = new MultiPeriod<(double Price, double Volume)>();

            var split_list = SplitTable.Select(n => (n.Key, true, n.Value.Split));
            var dividend_list = DividendTable.Select(n => (n.Key, false, n.Value.Dividend / n.Value.Close));
            var split_dividend_list = split_list.Concat(dividend_list).OrderByDescending(n => n.Key);

            DateTime latestTime = DateTime.MaxValue;
            double adj_price = 1;
            double adj_vol = 1;

            foreach (var pair in split_dividend_list)
            {
                DateTime time = pair.Key;
                double value = pair.Item3;

                //Console.WriteLine("->> Loading: " + time + " / " + pair.Key.Type + " / " + pair.Value.Value);

                if (pair.Item2 && value != 1)
                {
                    list.Add(time, latestTime, (adj_price, adj_vol));
                    adj_price /= value;
                    adj_vol /= value;
                    latestTime = time;
                }

                if (!pair.Item2 && value != 0 && includeDividend)
                {
                    list.Add(time, latestTime, (adj_price, adj_vol));
                    adj_price *= 1 / (1 + value);
                    latestTime = time;
                }
            }

            list.Add(latestTime, DateTime.MinValue, (adj_price, adj_vol));

            return list;
        }

        //public HashSet<BarTable> LiveBarTables { get; set; } = new HashSet<BarTable>();

        public void InboundLiveTick(MarketTick mt) => Parallel.ForEach(BarTables.Values.Where(n => n.IsLive), n => n.AddPriceTick(mt));

        [IgnoreDataMember]
        public ConcurrentDictionary<(BarFreq barFreq, BarType type), BarTable> BarTables { get; set; }
    }
}
