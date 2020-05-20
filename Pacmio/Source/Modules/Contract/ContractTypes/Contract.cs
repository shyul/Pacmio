/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
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
    [KnownType(typeof(BagComboContract))]
    public abstract class Contract : IEquatable<Contract>, IEquatable<(string name, Exchange exchange, string typeName)>
    {
        [IgnoreDataMember, Browsable(false)]
        public bool IsModified
        {
            get
            {
                return m_IsModified;
            }

            set
            {
                m_IsModified = value;
                if (value) UpdateTime = DateTime.Now;
            }
        }

        [IgnoreDataMember, NonSerialized]
        private bool m_IsModified;

        [DataMember, Browsable(false)]
        public DateTime UpdateTime { get; set; } = DateTime.MinValue;

        #region Identification

        /// <summary>
        /// The Contract's IB's unique id
        /// </summary>
        [DataMember, Browsable(true), ReadOnly(true), Category("IDs"), DisplayName("Contract ID")]
        public int ConId { get; set; } = -2;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Symbol")]
        public string Name { get; protected set; }

        [IgnoreDataMember]
        public virtual string FullName { get => m_fullName; set => m_fullName = value; }

        [DataMember]
        protected string m_fullName = string.Empty;

        [DataMember, Browsable(false)]
        public HashSet<string> NameSuffix { get; set; } = new HashSet<string>();

        #endregion Identification

        #region Exchange Information

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

        [IgnoreDataMember, Browsable(false), ReadOnly(true)]
        public abstract string TypeApiCode { get; }

        #endregion Contract Type Information

        #region Status and Market Data

        [DataMember, Browsable(true), Category("Basic Information"), DisplayName("Security Status")]
        public ContractStatus Status { get; set; } = ContractStatus.Unknown;

        [IgnoreDataMember] //, NonSerialized]
        public MarketData MarketData { get; set; }

        public abstract bool RequestQuote(string genericTickList);

        public abstract void StopQuote();

        [DataMember]
        public DateTime BarTableEarliestTime { get; set; } = DateTime.MinValue;

        [DataMember]
        public MultiPeriod<SymbolHistory> History { get; private set; } = new MultiPeriod<SymbolHistory>();
        /*
        public SymbolHistory GetISIN(DateTime time)
        { 
            foreach (Period p in History.Keys)
            {
                if (p.Contains(time)) return History[p];
            }
            return ISIN;
        }*/

        #endregion Status and Market Data

        #region Equality

        public bool Equals((string name, Exchange exchange, string typeName) other) => Info == other;

        public bool Equals(Contract other)
        {
            if (this is null || other is null)
                return false;
            else if (ConId > 0)
                return ConId == other.ConId;
            else
                return Info == other.Info;
        }

        public override bool Equals(object other)
        {
            if (this is null || other is null) // https://stackoverflow.com/questions/4219261/overriding-operator-how-to-compare-to-null
                return false;
            else if (other is Contract c) // else if (other.GetType().IsSubclassOf(typeof(IContract)))
                return Equals(c);
            else if (other.GetType() == typeof((string, Exchange, string)))
                return Equals(((string, Exchange, string))other);
            else if (other is BusinessInfo bi)
                return Equals(bi);
            else
                return false;
        }

        public static bool operator ==(Contract s1, Contract s2) => s1.Equals(s2);
        public static bool operator !=(Contract s1, Contract s2) => !s1.Equals(s2);

        public static bool operator ==(Contract s1, (string, Exchange, string) s2) => s1.Equals(s2);
        public static bool operator !=(Contract s1, (string, Exchange, string) s2) => !s1.Equals(s2);

        public override int GetHashCode() => Info.GetHashCode();
        public override string ToString() => "[" + Name + "] " + TypeName + " " + CurrencyCode + " @ " + Exchange;

        #endregion Equality
    }
}
