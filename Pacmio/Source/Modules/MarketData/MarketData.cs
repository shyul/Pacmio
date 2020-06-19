/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Xu;
using System.Linq;

namespace Pacmio
{
    public enum MarketQuoteStatus : int
    {
        Unknown = 0,
        RealTime = 1,
        Delayed = 3,
        Frozen = 2,
        DelayedFrozen = 4,
    }

    public enum HaltedStatus : int
    {
        Unknown = -1,
        Nothalted = 0,
        GeneralHalt = 1,
        VolatilityHalt = 2,
    }

    /// <summary>
    /// Describe a Market Quote
    /// https://interactivebrokers.github.io/tws-api/tick_types.html
    /// </summary>
    [Serializable]
    public class MarketData : IRow, IEquatable<MarketData>, IEquatable<Contract>
    {
        public MarketData(Contract c) { Contract = c; }

        public Contract Contract { get; protected set; }

        public int TickerId { get; set; } = 0;

        public DateTime UpdateTime { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Code 45: Time of the last trade (in UNIX time).
        /// Code 88: Delayed time of the last trade (in UNIX time)
        /// Received Tickstring: (0)"46"-(1)"6"-(2)"10000001"-(3)"45"-(4)"1580173196"
        /// 
        /// </summary>
        public DateTime LastTradeTime { get; set; } = DateTime.MinValue;

        /// <summary>
        /// ParseMarketDataType(string[] fields)
        /// </summary>
        public MarketQuoteStatus Status { get; set; } = MarketQuoteStatus.Unknown;

        /// <summary>
        /// Code 49: Indicates if a contract is halted.
        /// </summary>
        public HaltedStatus HaltedStatus { get; set; } = HaltedStatus.Unknown;

        public double MinimumTick { get; set; }

        public string BBOExchangeId { get; set; }

        /// <summary>
        /// Valid Exchanges for trade
        /// </summary>
        public HashSet<string> ValidExchanges { get; private set; } = new HashSet<string>();

        /// <summary>
        /// DerivativeSecTypes
        /// </summary>
        public List<string> DerivativeSecTypes { get; private set; } = new List<string>();

        /// <summary>
        /// Order Types
        /// </summary>
        public HashSet<string> OrderTypes { get; private set; } = new HashSet<string>();

        public HashSet<string> MarketRules { get; private set; } = new HashSet<string>();

        //public string AccountCode { get; protected set; }

        /// <summary>
        /// Positive -> Long position
        /// Negative -> Short position
        /// </summary>
        //public double TotalQuantity { get; set; } = 0;

        //public double AverageUnitCost { get; set; } = 0;


        /// <summary>
        /// Code 46: The shortable tick is an indicative on the amount of shares which can be sold short for the contract:
        /// Value higher than 2.5	There are at least 1000 shares available for short selling.
        /// Value higher than 1.5	This contract will be available for short selling if shares can be located.
        /// 1.5 or less	Contract is not available for short selling.
        /// </summary>
        [Description("Describes the level of difficulty with which the contract can be sold short.")]
        public double Shortable { get; set; }

        /// <summary>
        /// Code 89: Number of shares available to short
        /// </summary>
        [Description("Number of shares available to short.")]
        public double ShortableShares { get; set; }

        /// <summary>
        /// Exchange Look up for ticks
        /// </summary>
        public Dictionary<string, Exchange> TickExchange { get; } = new Dictionary<string, Exchange>();


        /// <summary>
        /// Code 2: Lowest price offer on the contract.
        /// </summary>
        [Description("Lowest price offer on the contract.")]
        public double Ask { get; set; }

        /// <summary>
        /// Code 3: Number of contracts or lots offered at the ask price.
        /// </summary>
        [Description("Number of contracts or lots offered at the ask price.")]
        public double AskSize { get { return AskQ.Value.Size; } set { if (value > 0 && Ask > 0) AskQ.Value = (Ask, value); } }

        [NonSerialized]
        public readonly QueueValue<(double Price, double Size)> AskQ = new QueueValue<(double Price, double Size)>();

        /// <summary>
        /// Code 33: Exchange of ask price
        /// </summary>
        [Description("Exchange of ask price")]
        public string AskExchange { get; set; }

        /// <summary>
        /// Code 1: Highest priced bid for the contract.
        /// </summary>
        [Description("Highest priced bid for the contract.")]
        public double Bid { get; set; }

        /// <summary>
        /// Code 0: Number of contracts or lots offered at the bid price.
        /// </summary>
        [Description("Number of contracts or lots offered at the bid price.")]
        public double BidSize { get { return BidQ.Value.Size; } set { if (value > 0 && Bid > 0) BidQ.Value = (Bid, value); } }

        [NonSerialized]
        public readonly QueueValue<(double Price, double Size)> BidQ = new QueueValue<(double Price, double Size)>();



        /// <summary>
        /// Code 32: Exchange of bid price
        /// </summary>
        [Description("Exchange of bid price")]
        public string BidExchange { get; set; }

        /// <summary>
        /// Code 4: Last price at which the contract traded (does not include some trades in RTVolume).
        /// </summary>
        [Description("Last price at which the contract traded (does not include some trades in RTVolume).")]
        public double Last
        {
            get
            {
                return m_last;
            }
            set
            {
                m_last = value;

                //BarTableList.MarketTicks.Enqueue((Contract, DateTime.Now.ToDestination(Contract.TimeZone), m_last, 0));
            }
        }

        private double m_last;

        /// <summary>
        /// Code 5: Number of contracts or lots traded at the last price.
        /// </summary>
        [Description("Number of contracts or lots traded at the last price.")]
        public double LastSize
        {
            get { return LastQ.Value.Size; }
            set
            {
                if (value > 0 && Last > 0) 
                {
                    LastQ.Value = (Last, value);
                }
            }
        }

        [NonSerialized]
        public readonly QueueValue<(double Price, double Size)> LastQ = new QueueValue<(double Price, double Size)>();

        /// <summary>
        /// Code 84: Exchange of last traded price
        /// </summary>
        [Description("Exchange of last traded price")]
        public string LastExchange { get; set; }

        /// <summary>
        /// Code 14: Current session's opening price. Before open will refer to previous day.
        /// The official opening price requires a market data subscription to the native exchange of the instrument.
        /// </summary>
        [Description("Current session's opening price. Before open will refer to previous day.")]
        public double Open { get; set; }

        /// <summary>
        /// Code 6: High price for the day.
        /// </summary>
        [Description("High price for the day.")]
        public double High { get; set; }

        /// <summary>
        /// Code 7: Low price for the day.
        /// </summary>
        [Description("Low price for the day.")]
        public double Low { get; set; }

        /// <summary>
        /// Code 9: The last available closing price for the previous day.
        /// For US Equities, we use corporate action processing to get the closing price,
        /// so the close price is adjusted to reflect forward and reverse splits and cash and stock dividends.
        /// </summary>
        [Description("The last available closing price for the previous day.")]
        public double LastClose { get; set; }

        /// <summary>
        /// Code 8: Trading volume for the day for the selected contract (US Stocks: multiplier 100).
        /// </summary>
        [Description("Trading volume for the day for the selected contract.")]
        public double Volume
        {
            get
            {
                return m_volume;
            }
            set
            {
                DeltaVolume = value - m_volume;
                //if(DeltaVolume > 0 && m_volume > 0) BarTableList.MarketTicks.Enqueue((Contract, DateTime.Now.ToDestination(Contract.TimeZone), m_last, DeltaVolume));
                m_volume = value;
            }
        }

        private double m_volume = 0;

        public double DeltaVolume { get; private set; }

        public double TargetPrice { get; set; }


        public readonly Dictionary<int, (DateTime Time, double Price, double Size, Exchange MarketMaker)> MarketDepth
            = new Dictionary<int, (DateTime Time, double Price, double Size, Exchange MarketMaker)>();


        public double PositionQuantityAll => AccountManager.List.Count() > 0 ? AccountManager.List.Select(n => PositionQuantity(n)).Sum() : 0;

        public double PositionQuantity(Account ac) => ac.Positions.ContainsKey(Contract) ? ac.Positions[Contract].Quantity : 0;

        public double CostBasisAll => AccountManager.List.Count() > 0 ? AccountManager.List.Select(n => PositionQuantity(n)).Sum() : 0;

        public double CostBasis(Account ac) => ac.Positions.ContainsKey(Contract) ? ac.Positions[Contract].CostBasis : 0;

        public double UnrealizedProfitAll => AccountManager.List.Count() > 0 ? AccountManager.List.Select(n => UnrealizedProfit(n)).Sum() : 0;

        public double UnrealizedProfit(Account ac) => ac.Positions.ContainsKey(Contract) ? ac.Positions[Contract].Quantity * (LastClose - ac.Positions[Contract].CostBasis) : 0;




        public bool Equals(Contract other) => Contract == other;
        public static bool operator ==(MarketData s1, Contract s2) => s1.Equals(s2);
        public static bool operator !=(MarketData s1, Contract s2) => !s1.Equals(s2);
        public bool Equals(MarketData other) => Contract == other.Contract;
        public static bool operator ==(MarketData s1, MarketData s2) => s1.Equals(s2);
        public static bool operator !=(MarketData s1, MarketData s2) => !s1.Equals(s2);

        public override bool Equals(object other)
        {
            if (this is null || other is null) // https://stackoverflow.com/questions/4219261/overriding-operator-how-to-compare-to-null
                return false;
            else if (other is Contract c)
                return Equals(c);
            else if (other is MarketData md)
                return Equals(md);
            else
                return false;
        }
        public override int GetHashCode() => GetType().GetHashCode() ^ Contract.GetHashCode();

        public object this[Column column]
        {
            get
            {
                return column switch
                {
                    ContractColumn _ => Contract,
                    StringColumn sc when sc == Column_Status => Status.ToString(),
                    StringColumn sc when sc == Column_TradeTime => LastTradeTime.ToString(),

                    StringColumn sc when sc == Column_BidExchange => BidExchange,
                    NumericColumn dc when dc == Column_BidSize => BidSize,
                    NumericColumn dc when dc == Column_Bid => Bid,

                    NumericColumn dc when dc == Column_Ask => Ask,
                    NumericColumn dc when dc == Column_AskSize => AskSize,
                    StringColumn sc when sc == Column_AskExchange => AskExchange,

                    NumericColumn dc when dc == Column_Last => Last,
                    NumericColumn dc when dc == Column_LastSize => LastSize,
                    StringColumn sc when sc == Column_LastExchange => LastExchange,

                    NumericColumn dc when dc == Column_Open => Open,
                    NumericColumn dc when dc == Column_High => High,
                    NumericColumn dc when dc == Column_Low => Low,
                    NumericColumn dc when dc == Column_Close => LastClose,
                    NumericColumn dc when dc == Column_Volume => Volume,

                    NumericColumn dc when dc == Column_Short => Shortable,
                    NumericColumn dc when dc == Column_ShortShares => ShortableShares,

                    _ => null,
                };
            }
        }

        public static readonly StringColumn Column_Status = new StringColumn("STATUS");
        public static readonly ContractColumn Column_Contract = new ContractColumn("Contract");
        public static readonly StringColumn Column_TradeTime = new StringColumn("TRADE_TIME");

        public static readonly StringColumn Column_BidExchange = new StringColumn("BID_EXCHANGE");
        public static readonly NumericColumn Column_BidSize = new NumericColumn("BID_SIZE");
        public static readonly NumericColumn Column_Bid = new NumericColumn("BID");

        public static readonly NumericColumn Column_Ask = new NumericColumn("ASK");
        public static readonly NumericColumn Column_AskSize = new NumericColumn("ASK_SIZE");
        public static readonly StringColumn Column_AskExchange = new StringColumn("ASK_EXCHANGE");

        public static readonly NumericColumn Column_Last = new NumericColumn("LAST");
        public static readonly NumericColumn Column_LastSize = new NumericColumn("LAST_SIZE");
        public static readonly StringColumn Column_LastExchange = new StringColumn("LAST_EXCHANGE");

        public static readonly NumericColumn Column_Open = new NumericColumn("OPEN");
        public static readonly NumericColumn Column_High = new NumericColumn("HIGH");
        public static readonly NumericColumn Column_Low = new NumericColumn("LOW");
        public static readonly NumericColumn Column_Close = new NumericColumn("CLOSE");
        public static readonly NumericColumn Column_Volume = new NumericColumn("VOLUME");

        public static readonly NumericColumn Column_Short = new NumericColumn("SHORT");
        public static readonly NumericColumn Column_ShortShares = new NumericColumn("S_SHARES");
    }
}
