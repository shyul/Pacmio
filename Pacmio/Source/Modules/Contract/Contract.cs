/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using Xu;
using Xu.GridView;

namespace Pacmio
{
    [Serializable, DataContract]
    [KnownType(typeof(Stock))]
    [KnownType(typeof(Index))]
    [KnownType(typeof(Future))]
    [KnownType(typeof(Option))]
    [KnownType(typeof(MutualFund))]
    [KnownType(typeof(Forex))]
    public abstract class Contract : IEquatable<Contract>, IEquatable<(string name, Exchange exchange, string typeName)>, IComparable<Contract>, IComparable
    {
        public override string ToString() => "[" + Name + "] " + TypeName + " " + CurrencyCode + " @ " + Exchange;

        #region Identification

        /// <summary>
        /// The Contract's IB's unique id
        /// </summary>
        [DataMember, Browsable(true), ReadOnly(true), Category("IDs"), DisplayName("Contract ID")]
        public int ConId { get; set; } = -2;

        [DataMember, Browsable(true), ReadOnly(true), Category("IDs"), DisplayName("Symbol")]
        public virtual string Name { get; protected set; }

        [IgnoreDataMember]
        public virtual string FullName { get => m_fullName; set => m_fullName = value; }

        [DataMember]
        protected string m_fullName = string.Empty;

        [DataMember]
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
        [DataMember]
        public Exchange Exchange { get; protected set; }

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Exchange"), Category("Basic Information")]
        public virtual string ExchangeName
        {
            get
            {
                if (Exchange.GetAttribute<ExchangeInfo>() is ExchangeInfo exi)
                    return exi.Name + ((ExchangeSuffix.Length > 0) ? (" " + ExchangeSuffix) : string.Empty);
                else
                    return "UNKNOWN";
            }
        }

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Exchange Full Name"), Category("Basic Information")]
        public virtual string ExchangeFullName
        {
            get
            {
                if (Exchange.GetAttribute<ExchangeInfo>() is ExchangeInfo exi)
                    return exi.FullName + ((ExchangeSuffix.Length > 0) ? (" " + ExchangeSuffix) : string.Empty);
                else
                    return "Unknown Exchange";
            }
        }

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Exchange Country"), Category("Basic Information")]
        public virtual string Country
        {
            get
            {
                if (Exchange.GetAttribute<ExchangeInfo>() is ExchangeInfo exi)
                    return exi.Region.Name;
                else
                    return "Unknown Country";
            }
        }

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Suffix"), Category("Exchange"),]
        public string ExchangeSuffix { get; set; } = string.Empty;

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Currency Symbol")]
        public virtual string CurrencySymbol
        {
            get
            {
                if (Exchange.GetAttribute<ExchangeInfo>() is ExchangeInfo exi)
                    return exi.Region.CurrencySymbol;
                else
                    return "X";
            }
        }

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Currency Code")]
        public virtual string CurrencyCode
        {
            get
            {
                if (Exchange.GetAttribute<ExchangeInfo>() is ExchangeInfo exi)
                    return exi.Region.ISOCurrencySymbol;
                else
                    return string.Empty;
            }
        }

        [IgnoreDataMember]
        public virtual WorkHours WorkHours
        {
            get
            {
                if (Exchange.GetAttribute<ExchangeInfo>() is ExchangeInfo exi)
                    return exi.WorkHours;
                else
                    return ExchangeInfo.WorkHoursAll;
            }
        }

        [IgnoreDataMember]
        public virtual WorkHours WorkHoursExtended
        {
            get
            {
                if (Exchange.GetAttribute<ExchangeInfo>() is ExchangeInfo exi)
                    return exi.WorkHoursExtended;
                else
                    return ExchangeInfo.WorkHoursAll;
            }
        }

        [IgnoreDataMember]
        public virtual TimeZoneInfo TimeZone => WorkHours.TimeZoneInfo;

        [IgnoreDataMember]
        public virtual DateTime CurrentTime => WorkHours.CurrentTime;

        [IgnoreDataMember]
        public bool IsTrading => WorkHours.IsWorkTime();

        [IgnoreDataMember]
        public virtual DateTime LatestClosingDateTime
        {
            get
            {
                DateTime dt = CurrentTime;

                while (!WorkHours.IsWorkDate(dt) || WorkHours[dt].TimePeriod >= dt)
                {
                    dt = dt.AddHours(-1);
                }

                return dt.Date.AddSeconds(WorkHours[dt.Date].Stop.TotalSeconds);
            }
        }

        [IgnoreDataMember]
        public virtual DateTime LatestExtendedClosingDateTime
        {
            get
            {
                DateTime dt = CurrentTime;

                while (!WorkHoursExtended.IsWorkDate(dt) || WorkHoursExtended[dt].TimePeriod >= dt)
                {
                    dt = dt.AddHours(-1);
                }

                return dt.Date.AddSeconds(WorkHoursExtended[dt.Date].Stop.TotalSeconds);
            }
        }

        [IgnoreDataMember]
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
        public (string name, Exchange exchange, string typeName) Key => (Name, Exchange, TypeName);

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type")]
        public abstract string TypeName { get; }

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type Full Name")]
        public abstract string TypeFullName { get; }

        #endregion Contract Type Information

        #region Data Update

        [IgnoreDataMember]
        public MarketData MarketData
        {
            get
            {
                if (m_MarketData is null)
                    m_MarketData = this.GetOrCreateMarketData();

                return m_MarketData;
            }
        }

        [IgnoreDataMember]
        private MarketData m_MarketData = null;

        [IgnoreDataMember]
        public virtual bool NeedUpdate => ((DateTime.Now - UpdateTime).Days > 2 && MarketData.NeedUpdate) || FullName.Length < 2;

        [DataMember]
        public DateTime UpdateTime { get; set; } = TimeTool.MinInvalid;

        [DataMember]
        public DateTime HistoricalHeadTime { get; set; } = TimeTool.MinInvalid;

        [DataMember]
        public MultiPeriod<SymbolHistory> History { get; private set; } = new MultiPeriod<SymbolHistory>();

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Security Status"), Category("Basic Information")]
        public ContractStatus Status { get; set; } = ContractStatus.Unknown;

        #endregion Data Update

        #region Equality

        public override int GetHashCode() => Key.GetHashCode();

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

        public bool Equals(Contract other) => other is Contract c && GetType() == c.GetType() && ((ConId > 0 && ConId == other.ConId) || Key == other.Key);

        public bool Equals((string name, Exchange exchange, string typeName) other) => Key == other;

        public static bool operator ==(Contract s1, Contract s2) => s1.Equals(s2);
        public static bool operator !=(Contract s1, Contract s2) => !s1.Equals(s2);

        public static bool operator ==(Contract s1, (string, Exchange, string) s2) => s1.Equals(s2);
        public static bool operator !=(Contract s1, (string, Exchange, string) s2) => !s1.Equals(s2);

        #endregion Equality

        public int CompareTo(Contract other) => Name.CompareTo(other.Name);

        public int CompareTo(object other) => other is Contract c ? CompareTo(c) : 0;
    }
}
