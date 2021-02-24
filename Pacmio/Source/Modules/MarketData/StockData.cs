/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.ComponentModel;
using Xu;
using Xu.GridView;

namespace Pacmio
{
    [Serializable, DataContract]
    public class StockData : BidAskData
    {
        public override void Initialize(Contract c)
        {
            base.Initialize(c);
            if (m_MarketDepth is null) m_MarketDepth = new ConcurrentDictionary<int, MarketDepthDatum>();

            RTLastTime = DateTime.MinValue;
            RTLastPrice = -1;
        }

        [DataMember]
        public DateTime BarTableEarliestTime { get; set; } = DateTime.MinValue;

        [DataMember]
        public Dictionary<DateTime, (DataSource DataSource, double Close, double Dividend)> DividendTable { get; private set; }
            = new Dictionary<DateTime, (DataSource DataSource, double Close, double Dividend)>();

        [DataMember]
        public Dictionary<DateTime, (DataSource DataSource, double Split)> SplitTable { get; private set; }
            = new Dictionary<DateTime, (DataSource DataSource, double Split)>();

        [DataMember]
        public Dictionary<DateTime, (DataSource DataSource, double EPS)> EPSTable { get; private set; }
            = new Dictionary<DateTime, (DataSource DataSource, double EPS)>();

        [DataMember]
        public Dictionary<DateTime, (DataSource DataSource, double Target)> TargetPriceList { get; private set; }
            = new Dictionary<DateTime, (DataSource DataSource, double Target)>();

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

        [DataMember]
        public bool IsFilteredRTStream { get; set; } = true;

        [IgnoreDataMember]
        public DateTime RTLastTime { get; private set; } = DateTime.MinValue;

        [IgnoreDataMember]
        public double RTLastPrice { get; private set; } = -1;

        // TODO: 
        // Queue the Tape Here

        public void InboundLiveTick(DateTime time, double price, double size)
        {
            if (time > RTLastTime)
            {
                RTLastTime = time;

                if (double.IsNaN(price))
                {
                    price = RTLastPrice;

                    // Even tick
                }
                else
                {
                    if (price > RTLastPrice)
                    {
                        // Is an advancing tick
                    }
                    else
                    {
                        // Is a declining tick
                    }

                    RTLastPrice = price;
                }

                if (price > 0)
                {
                    lock (DataConsumers)
                    {
                        Parallel.ForEach(DataConsumers.Where(n => n is BarTable b && b.IsLive).Select(n => n as BarTable), bt =>
                        {
                            if (bt.BarFreq < BarFreq.Daily)// || bt.LastTime == time.Date)
                            {
                                bt.AddPriceTick(time, price, size);
                            }
                            else if (bt.BarFreq >= BarFreq.Daily)// && bt.LastTime < time.Date)
                            {
                                DateTime date = time.Date;
                                //Console.WriteLine(">>> [[[[ Received for " + bt.ToString() + " | LastTime = " + bt.LastTime.Date + " | time = " + time.Date);
                                if (bt.LastTime.Date < date)
                                {
                                    // Also check the Stock Data time stamp here! Is it current???
                                    if (bt.Status == TableStatus.Ready && (!double.IsNaN(Open)) && (!double.IsNaN(High)) && (!double.IsNaN(Low)) && (!double.IsNaN(LastPrice)) && (!double.IsNaN(Volume)))
                                    {
                                        Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> [[[[ Adding new candle: " + date + " | " + Open + " | " + High + " | " + Low + " | " + LastPrice + " | " + Volume);
                                        Bar b = new Bar(bt, date, Open, High, Low, LastPrice, Volume);
                                        Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> [[[[ Adding new candle: " + b.DataSourcePeriod.Stop);
                                        bt.MergeFromSmallerBar(b);
                                        Console.WriteLine("bt.LastBar.DataSourcePeriod.Stop = " + bt.LastBar.DataSourcePeriod.Stop);
                                        Console.WriteLine("bt.LastBar.DataSourcePeriod.Stop = " + bt.LastBar.DataSourcePeriod.Stop);

                                    }
                                }
                                else if (bt.LastTime == date)
                                {
                                    bt.AddPriceTick(time, price, size);
                                }

                                //Console.WriteLine("bt.Status = " + bt.Status);
                            }
                        });
                    }
                }
            }
        }

        [DataMember]
        public double MarketCap { get; set; } = double.NaN;

        [DataMember]
        public double FloatShares { get; set; } = double.NaN;

        [DataMember]
        public double ShortPercent { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("S.Shares"), GridColumnOrder(18), CellRenderer(typeof(NumberCellRenderer), 80)]
        public double ShortableShares { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Short"), GridColumnOrder(17), CellRenderer(typeof(NumberCellRenderer), 60)]
        public double ShortStatus { get; set; } = double.NaN;

        [DataMember]
        public bool EnableNews { get; set; } = true;

        /// <summary>
        /// Be aware of toggling changes
        /// </summary>
        [DataMember]
        public bool EnableShortableShares { get; set; } = true;

        [DataMember]
        public bool FilteredTicks { get; set; } = true;

        public override bool StartTicks()
        {
            FilteredTicks = true;
            EnableNews = true;
            EnableShortableShares = true;
            return IB.Client.SendRequest_MarketData(this);
        }

        [DataMember]
        public bool EnableMarketDepth { get; set; } = true;

        [IgnoreDataMember]
        private ConcurrentDictionary<int, MarketDepthDatum> m_MarketDepth { get; set; }

        public MarketDepthDatum[] MarketDepth()
        {
            if (m_MarketDepth is null) m_MarketDepth = new ConcurrentDictionary<int, MarketDepthDatum>();

            lock (m_MarketDepth)
            {
                return m_MarketDepth.OrderBy(n => n.Key).Select(n => n.Value).ToArray();
            }
        }

        public MarketDepthDatum GetMarketDepth(int position)
        {
            if (m_MarketDepth is null) m_MarketDepth = new ConcurrentDictionary<int, MarketDepthDatum>();

            lock (m_MarketDepth)
            {
                if (!m_MarketDepth.ContainsKey(position))
                    m_MarketDepth.TryAdd(position, new MarketDepthDatum(position));

                return m_MarketDepth[position];
            }
        }

        #region Market Depth

        //public virtual bool Request_MarketDepth() => IB.Client.SendRequest_MarketDepth(Contract);

        /// <summary>
        /// TODO: Cancel_MarketDepth()
        /// </summary>
        //public virtual void Cancel_MarketDepth() { }

        #endregion Market Depth

        // Tape

        // Position in Range

        // L2



        // News






        // Social Media
    }
}
