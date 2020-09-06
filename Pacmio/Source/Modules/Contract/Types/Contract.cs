/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    [Serializable, DataContract]
    [KnownType(typeof(Stock))]
    [KnownType(typeof(Index))]
    [KnownType(typeof(Future))]
    [KnownType(typeof(Option))]
    [KnownType(typeof(MutualFund))]
    [KnownType(typeof(Forex))]
    public abstract class Contract : IRow, IEquatable<Contract>, IEquatable<(string name, Exchange exchange, string typeName)>
    {
        public override string ToString() => "[" + Name + "] " + TypeName + " " + CurrencyCode + " @ " + Exchange;

        #region Identification

        /// <summary>
        /// The Contract's IB's unique id
        /// </summary>
        [DataMember, Browsable(true), ReadOnly(true), Category("IDs"), DisplayName("Contract ID")]
        public int ConId { get; set; } = -2;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Symbol")]
        public virtual string Name { get; protected set; }

        [IgnoreDataMember]
        public virtual string FullName { get => m_fullName; set => m_fullName = value; }

        [DataMember]
        protected string m_fullName = string.Empty;

        [DataMember, Browsable(false)]
        public HashSet<string> NameSuffix { get; set; } = new HashSet<string>();

        #endregion Identification

        #region Exchange Information

        [DataMember]
        public bool SmartExchangeRoute { get; set; } = true;

        /// <summary>
        /// The contract's primary exchange.
        /// For smart routed contracts, used to define contract in case of ambiguity.
        /// Should be defined as native exchange of contract, e.g. ISLAND for MSFT
        /// For exchanges which contain a period in name, will only be part of exchange name prior to period, i.e. ENEXT for ENEXT.BE
        /// </summary>

        /// <summary>
        /// The destination exchange.
        /// </summary>
        [DataMember, Browsable(false)]
        public Exchange Exchange { get; protected set; }

        [IgnoreDataMember, Browsable(true), Category("Basic Information"), DisplayName("Exchange"), ReadOnly(true)]
        public virtual string ExchangeName
        {
            get
            {
                (bool valid, ExchangeInfo exi) = Exchange.GetAttribute<ExchangeInfo>();

                if (valid)
                    return exi.Name + ((ExchangeSuffix.Length > 0) ? (" " + ExchangeSuffix) : string.Empty);
                else
                    return "UNKNOWN";
            }
        }

        [IgnoreDataMember, Browsable(true), Category("Basic Information"), DisplayName("Exchange Full Name"), ReadOnly(true)]
        public virtual string ExchangeFullName
        {
            get
            {
                (bool valid, ExchangeInfo exi) = Exchange.GetAttribute<ExchangeInfo>();
                if (valid)
                    return exi.FullName + ((ExchangeSuffix.Length > 0) ? (" " + ExchangeSuffix) : string.Empty);
                else
                    return "Unknown Exchange";
            }
        }

        [IgnoreDataMember, Browsable(true), Category("Basic Information"), DisplayName("Exchange Country"), ReadOnly(true)]
        public virtual string Country
        {
            get
            {
                (bool valid, ExchangeInfo exi) = Exchange.GetAttribute<ExchangeInfo>();
                if (valid)
                    return exi.Region.Name;
                else
                    return "Unknown Country";
            }
        }

        [DataMember, Browsable(true), ReadOnly(true), Category("Exchange"), DisplayName("Suffix")]
        public string ExchangeSuffix { get; set; } = string.Empty;

        [IgnoreDataMember, ReadOnly(true), DisplayName("Currency Symbol")]
        public virtual string CurrencySymbol
        {
            get
            {
                (bool valid, ExchangeInfo exi) = Exchange.GetAttribute<ExchangeInfo>();
                if (valid)
                    return exi.Region.CurrencySymbol;
                else
                    return "X";
            }
        }

        [IgnoreDataMember, ReadOnly(true), DisplayName("Currency Code")]
        public virtual string CurrencyCode
        {
            get
            {
                (bool valid, ExchangeInfo exi) = Exchange.GetAttribute<ExchangeInfo>();
                if (valid)
                    return exi.Region.ISOCurrencySymbol;
                else
                    return string.Empty;
            }
        }

        [IgnoreDataMember, Browsable(false), ReadOnly(true)]
        public virtual TimeZoneInfo TimeZone => WorkHours.TimeZoneInfo;

        [IgnoreDataMember, Browsable(false), ReadOnly(true)]
        public virtual WorkHours WorkHours
        {
            get
            {
                (bool valid, ExchangeInfo exi) = Exchange.GetAttribute<ExchangeInfo>();
                if (valid)
                    return exi.WorkHours;
                else
                    return ExchangeInfo.WorkHoursAll;
            }
        }

        public virtual DateTime CurrentTime => DateTime.Now.ToDestination(TimeZone);

        public bool IsWorkHour => WorkHours.IsWorkTime(CurrentTime);

        #endregion Exchange Information

        #region Contract Type Information

        /**
         * @brief The security's type:
         *      STK - stock (or ETF)
         *      OPT - option
         *      FUT - future
         *      IND - index
         *      FOP - futures option
         *      CASH - forex pair
         *      BAG - combo
         *      WAR - warrant
         *      BOND- bond
         *      CMDTY- commodity
         *      NEWS- news
         *		FUND- mutual fund
         */
        [IgnoreDataMember]
        public (string name, Exchange exchange, string typeName) Info => (Name, Exchange, TypeName);

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type")]
        public abstract string TypeName { get; }

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type Full Name")]
        public abstract string TypeFullName { get; }

        #endregion Contract Type Information

        #region Data Update

        [IgnoreDataMember]
        public virtual bool NeedUpdate => (DateTime.Now - UpdateTime).Days > 2 && ((DateTime.Now - MarketData.TradingPeriods.Stop).Days > MarketData.TradingPeriods.Count || FullName.Length < 2);

        [DataMember, Browsable(false)]
        public DateTime UpdateTime { get; set; } = DateTime.MinValue;

        [DataMember]
        public MultiPeriod<SymbolHistory> History { get; private set; } = new MultiPeriod<SymbolHistory>();

        [DataMember, Browsable(true), Category("Basic Information"), DisplayName("Security Status")]
        public ContractStatus Status { get; set; } = ContractStatus.Unknown;

        #endregion Data Update

        #region Status and Market Data

        /// <summary>
        /// https://interactivebrokers.github.io/tws-api/tick_types.html
        /// string genericTickList = "236,375";  // 292 is news and 233 is RTVolume
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual bool Request_MarketTicks(string param = "233,236,375") => IB.Client.SendRequest_MarketTicks(this, param);

        public virtual void Cancel_MarketTicks() => IB.Client.SendCancel_MarketTicks(MarketData.TickerId);

        [IgnoreDataMember]
        public virtual bool IsActiveMarketTick => IB.Client.ActiveMarketTicks.Values.Contains(this);

        [IgnoreDataMember]
        public virtual MarketData MarketData { get; }

        public abstract void LoadMarketData();

        public abstract void SaveMarketData();

        [IgnoreDataMember]
        public virtual string MarketDataFilePath => Root.ResourcePath + "HistoricalData\\" + Info.typeName.ToString() + "\\" + Info.exchange.ToString() + "\\_MarketData\\";

        [IgnoreDataMember]
        public virtual string MarketDataFileName => MarketDataFilePath + "$" + Name + ".json";

        #endregion Status and Market Data

        #region Order and Trade Data

        [IgnoreDataMember]
        public virtual TradeData TradeData
        {
            get
            {
                if (m_TradeData is null) LoadTradeData();
                return m_TradeData;
            }
        }

        [IgnoreDataMember]
        private TradeData m_TradeData = null;

        public virtual void LoadTradeData()
        {
            m_TradeData = File.Exists(TradeDataFileName) ? Serialization.DeserializeJsonFile<TradeData>(TradeDataFileName) : new TradeData();
        }

        public virtual void SaveTradeData()
        {
            if (m_TradeData is TradeData td)
            {
                if (!Directory.Exists(TradeDataFilePath)) Directory.CreateDirectory(TradeDataFilePath);
                td.SerializeJsonFile(TradeDataFileName);
            }
        }

        [IgnoreDataMember]
        public virtual string TradeDataFilePath => Root.ResourcePath + "HistoricalData\\" + Info.typeName.ToString() + "\\" + Info.exchange.ToString() + "\\_TradeData\\";

        [IgnoreDataMember]
        public virtual string TradeDataFileName => TradeDataFilePath + "$" + Name + ".json";

        #endregion Order and Trade Data

        #region Equality

        public override int GetHashCode() => Info.GetHashCode();

        public override bool Equals(object other)
        {
            if (this is null || other is null) // https://stackoverflow.com/questions/4219261/overriding-operator-how-to-compare-to-null
                return false;
            else if (other is Contract c)
                return Equals(c);
            else if (other.GetType() == typeof((string, Exchange, string)))
                return Equals(((string, Exchange, string))other);
            else if (other is BusinessInfo bi)
                return Equals(bi);
            else
                return false;
        }

        public bool Equals(Contract other)
        {
            if (this is null || other is null)
                return false;
            else if (ConId > 0)
                return ConId == other.ConId;
            else
                return Info == other.Info;
        }

        public bool Equals((string name, Exchange exchange, string typeName) other) => Info == other;

        public static bool operator ==(Contract s1, Contract s2) => s1.Equals(s2);
        public static bool operator !=(Contract s1, Contract s2) => !s1.Equals(s2);

        public static bool operator ==(Contract s1, (string, Exchange, string) s2) => s1.Equals(s2);
        public static bool operator !=(Contract s1, (string, Exchange, string) s2) => !s1.Equals(s2);

        #endregion Equality

        #region Grid View

        public virtual object this[Column column]
        {
            get
            {
                return column switch
                {
                    ContractColumn _ => this,

                    StringColumn sc when sc == Column_Status => Status.ToString(),
                    StringColumn sc when sc == Column_TradeTime => MarketData.LastTradeTime.ToString(),

                    StringColumn sc when sc == Column_BidExchange && MarketData is BidAskData q => q.BidExchange,
                    NumericColumn dc when dc == Column_BidSize && MarketData is BidAskData q => q.BidSize,
                    NumericColumn dc when dc == Column_Bid && MarketData is BidAskData q => q.Bid,

                    NumericColumn dc when dc == Column_Ask && MarketData is BidAskData q => q.Ask,
                    NumericColumn dc when dc == Column_AskSize && MarketData is BidAskData q => q.AskSize,
                    StringColumn sc when sc == Column_AskExchange && MarketData is BidAskData q => q.AskExchange,

                    NumericColumn dc when dc == Column_Last => MarketData.LastPrice,
                    NumericColumn dc when dc == Column_LastSize && MarketData is BidAskData q => q.LastSize,
                    StringColumn sc when sc == Column_LastExchange && MarketData is BidAskData q => q.LastExchange,

                    NumericColumn dc when dc == Column_Open && MarketData is BidAskData q => q.Open,
                    NumericColumn dc when dc == Column_High && MarketData is BidAskData q => q.High,
                    NumericColumn dc when dc == Column_Low && MarketData is BidAskData q => q.Low,
                    NumericColumn dc when dc == Column_Close && MarketData is BidAskData q => q.PreviousClose,
                    NumericColumn dc when dc == Column_Volume && MarketData is BidAskData q => q.Volume,

                    NumericColumn dc when dc == Column_Short && MarketData is StockData q => q.ShortStatus,
                    NumericColumn dc when dc == Column_ShortShares && MarketData is StockData q => q.ShortableShares,

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

        #endregion Grid View
    }
}
