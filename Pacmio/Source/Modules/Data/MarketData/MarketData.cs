/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Xu;
using Xu.GridView;

namespace Pacmio
{
    [Serializable, DataContract]
    public sealed class MarketData : IDataFile, IDataProvider, IEquatable<MarketData>, IEquatable<Contract>
    {
        public MarketData((string name, Exchange exchange, string typeName) key)
        {
            ContractKey = key;
            m_Contract = ContractManager.GetByKey(ContractKey);
            ConId = m_Contract.ConId;
        }

        public MarketData(Contract c) => Contract = c;

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Contract"), GridColumnOrder(1, 0, 0), GridRenderer(typeof(ContractGridRenderer), 150, true)]
        public Contract Contract
        {
            get
            {
                if (m_Contract is null)
                {
                    m_Contract = ContractManager.GetByKey(ContractKey);
                    ConId = m_Contract.ConId;
                }
                return m_Contract;

            }
            private set
            {
                m_Contract = value;
                ContractKey = value.Key;
                ConId = value.ConId;
            }
        }

        [IgnoreDataMember]
        private Contract m_Contract = null;

        [DataMember]
        public (string name, Exchange exchange, string typeName) ContractKey { get; private set; }

        [DataMember]
        public int ConId { get; private set; } = -1;

        /// <summary>
        /// For Multi Thread Access
        /// </summary>
        [IgnoreDataMember]
        public object DataLockObject { get; private set; } = new();

        #region Status

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Status"), GridColumnOrder(0, 1, 0), GridRenderer(typeof(TextGridRenderer), 100)]
        public MarketDataStatus Status { get; set; } = MarketDataStatus.Unknown;

        [DataMember]
        public double MinimumTick { get; set; } = double.NaN;

        #endregion Status

        #region IB Subscription Setting / Status

        [IgnoreDataMember]
        public bool IsLive => TickerId > 0 && !IsSnapshot;

        [IgnoreDataMember]
        public int TickerId { get; set; } = int.MinValue;

        [DataMember]
        public string BBOExchangeId { get; set; } = string.Empty;

        [IgnoreDataMember]
        public int SnapshotPermissions { get; set; }

        [IgnoreDataMember]
        public bool IsSnapshot { get; set; } = false;

        [IgnoreDataMember]
        public bool RegulatorySnaphsot { get; set; } = false;

        [DataMember]
        public bool FilteredTicks { get; set; } = true;

        /// <summary>
        /// Be aware of toggling changes
        /// </summary>
        [DataMember]
        public bool EnableShortableShares { get; set; } = true;

        [DataMember]
        public bool EnableNews { get; set; } = true;

        /// <summary>
        /// genericTickList:
        ///     100 Option Volume(currently for stocks)
        ///     101 Option Open Interest(currently for stocks)
        ///     104 Historical Volatility(currently for stocks)
        ///     105 Average Option Volume(currently for stocks)
        ///     106 Option Implied Volatility(currently for stocks)
        ///     162 Index Future Premium
        ///     165 Miscellaneous Stats
        ///     221 Mark Price(used in TWS P&L computations)
        ///     225 Auction values(volume, price and imbalance)
        ///     233 RTVolume - contains the last trade price, last trade size, last trade time, total volume, VWAP, and single trade flag.
        ///     236 Shortable
        ///     256 Inventory
        ///     258 Fundamental Ratios
        ///     411 Realtime Historical Volatility
        ///     456 IBDividends
        ///     375 RT Volume filtered for BarTable
        /// </summary>
        [IgnoreDataMember]
        public string GenericTickList
        {
            get
            {
                string genericTickList = "221,";

                genericTickList += FilteredTicks ? "233,375," : "233,375,";
                if (EnableShortableShares) genericTickList += "236,";
                if (EnableNews) genericTickList += "292,";

                return genericTickList.TrimEnd(',');
            }
        }

        #endregion IB Subscription Setting / Status

        #region Basic Information

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("P.Close"), GridColumnOrder(15), GridRenderer(typeof(NumberGridRenderer), 60)]
        public double PreviousClose { get; set; } = double.NaN;

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Open"), GridColumnOrder(12), GridRenderer(typeof(NumberGridRenderer), 60)]
        public double Open { get; set; } = double.NaN;

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("High"), GridColumnOrder(13), GridRenderer(typeof(NumberGridRenderer), 60)]
        public double High { get; set; } = double.NaN;

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Low"), GridColumnOrder(14), GridRenderer(typeof(NumberGridRenderer), 60)]
        public double Low { get; set; } = double.NaN;

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Volume"), GridColumnOrder(16), GridRenderer(typeof(NumberGridRenderer), 70)]
        public double Volume { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Last"), GridColumnOrder(9, 5), GridRenderer(typeof(NumberGridRenderer), 60, false)]
        public double LastPrice { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Last Size"), GridColumnOrder(10), GridRenderer(typeof(NumberGridRenderer), 70)]
        public double LastSize { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Last.Ex"), GridColumnOrder(11), GridRenderer(typeof(TextGridRenderer), 50)]
        public string LastExchange { get; set; } = string.Empty;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Trade Time"), GridColumnOrder(2, 0, 0), GridRenderer(typeof(TextGridRenderer), 120, true)]
        public DateTime LastTradeTime { get; set; } = DateTime.MinValue;

        #endregion Basic Information

        #region Bid Ask

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Ask"), GridColumnOrder(6, 10), GridRenderer(typeof(NumberGridRenderer), 60)]
        public double Ask { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Ask Size"), GridColumnOrder(7, 11), GridRenderer(typeof(NumberGridRenderer), 70)]
        public double AskSize { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Ask Exchange"), GridColumnOrder(8, 12), GridRenderer(typeof(TextGridRenderer), 80, true)]
        public string AskExchange { get; set; } = string.Empty;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Bid"), GridColumnOrder(5, 10), GridRenderer(typeof(NumberGridRenderer), 60)]
        public double Bid { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Bid Size"), GridColumnOrder(4, 11), GridRenderer(typeof(NumberGridRenderer), 70)]
        public double BidSize { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Bid Exchange"), GridColumnOrder(3, 12), GridRenderer(typeof(TextGridRenderer), 80, true)]
        public string BidExchange { get; set; } = string.Empty;

        #endregion Bid Ask

        #region Realtime Ticks

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
                    lock (DataLockObject)
                    {
                        Parallel.ForEach(DataConsumers.Where(n => n is BarTable).Select(n => n as BarTable), bt =>
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

                                bt.DataIsUpdated(this);
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

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("S.Shares"), GridColumnOrder(18), GridRenderer(typeof(NumberGridRenderer), 80)]
        public double ShortableShares { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Short"), GridColumnOrder(17), GridRenderer(typeof(NumberGridRenderer), 60)]
        public double ShortStatus { get; set; } = double.NaN;

        #endregion Realtime Ticks

        #region Trade Parameters

        [DataMember]
        public MultiPeriod TradingSchedule { get; private set; } = new MultiPeriod();

        [IgnoreDataMember]
        public bool NeedUpdate => (DateTime.Now - TradingSchedule.Stop).Days > TradingSchedule.Count;

        [DataMember]
        public HashSet<string> DerivativeTypes { get; private set; } = new HashSet<string>();

        [DataMember]
        public HashSet<string> ValidExchanges { get; private set; } = new HashSet<string>();

        [DataMember]
        public HashSet<string> OrderTypes { get; private set; } = new HashSet<string>();

        [DataMember]
        public HashSet<string> MarketRules { get; private set; } = new HashSet<string>();

        [DataMember]
        public double MarketPrice { get; set; } = double.NaN;

        #endregion Trade Parameters

        #region Data Provider

        [DataMember]
        public DateTime UpdateTime { get; private set; } = DateTime.MinValue;

        [IgnoreDataMember] // Initialize
        private List<IDataConsumer> DataConsumers { get; set; } = new List<IDataConsumer>();

        public bool AddDataConsumer(IDataConsumer idk)
        {
            lock (DataLockObject)
            {
                if (!DataConsumers.Contains(idk))
                {
                    DataConsumers.Add(idk);
                    return true;
                }
                return false;
            }
        }

        public bool RemoveDataConsumer(IDataConsumer idk)
        {
            lock (DataLockObject)
            {
                if (DataConsumers.Contains(idk))
                {
                    DataConsumers.RemoveAll(n => n == idk);
                    return true;
                }
                return false;
            }
        }

        public void Update()
        {
            lock (DataLockObject)
            {
                UpdateTime = DateTime.Now;
                Parallel.ForEach(DataConsumers, idk => idk.DataIsUpdated(this));
            }
        }

        #endregion Data Provider

        #region File Operation

        public static string GetDataFileName((string name, Exchange exchange, string typeName) ContractKey)
            => Root.HistoricalDataPath(ContractKey) +
            "\\_MarketData\\" +
            (ContractKey.typeName == "INDEX" ? "^" : "$") +
            ContractKey.name + ".json";

        [IgnoreDataMember]
        public string DataFileName => GetDataFileName(ContractKey);

        [IgnoreDataMember]
        public bool IsModified { get; set; } = false;

        public void SaveFile() => this.SerializeJsonFile(DataFileName);

        public static MarketData LoadFile((string name, Exchange exchange, string typeName) key)
        {
            if (Serialization.DeserializeJsonFile<MarketData>(GetDataFileName(key)) is MarketData md)
            {
                md.Status = MarketDataStatus.Unknown;
                md.RTLastTime = DateTime.MinValue;
                md.RTLastPrice = -1;
                md.TickerId = int.MinValue;
                md.IsModified = false;
                md.DataLockObject = new();
                md.DataConsumers = new List<IDataConsumer>();
                return md;
            }
            else
                return new MarketData(key) { IsModified = false };
        }

        public static MarketData LoadFile(Contract c) => LoadFile(c.Key);

        #endregion File Operation

        #region Equality 

        // https://stackoverflow.com/questions/4219261/overriding-operator-how-to-compare-to-null
        public bool Equals(MarketData other) => other is MarketData md && Contract == md.Contract;
        public static bool operator ==(MarketData s1, MarketData s2) => s1.Equals(s2);
        public static bool operator !=(MarketData s1, MarketData s2) => !s1.Equals(s2);

        public bool Equals(Contract other) => Contract == other;
        public static bool operator ==(MarketData s1, Contract s2) => s1.Equals(s2);
        public static bool operator !=(MarketData s1, Contract s2) => !s1.Equals(s2);

        public override bool Equals(object other) => other is MarketData md && Equals(md);

        public override int GetHashCode() => Contract.GetHashCode() ^ GetType().GetHashCode();

        public override string ToString() => GetType().Name + " for " + Contract.ToString();

        #endregion Equality
    }
}
