/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// https://interactivebrokers.github.io/tws-api/basic_contracts.html#stk
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    /// <summary>
    /// Inflated Symbol with more information
    /// This type is to be serialized into file blobs
    /// </summary>
    [Serializable, DataContract(Name = "Stock")]
    public class Stock : Contract, ITradable, IMarketDepth
    {
        #region Ctor

        public Stock(string name, Exchange exchange)
        {
            Name = name;
            Exchange = exchange;
        }

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type")]
        public override string TypeName => "STOCK";

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type Full Name")]
        public override string TypeFullName => "Stock";

        #endregion Ctor

        #region Identification

        [IgnoreDataMember, Browsable(true), DisplayName("Full Name")]
        public override string FullName
        {
            get
            {
                (bool valid, BusinessInfo bi) = GetBusinessInfo();

                if (valid)
                    if (bi.FullName.Length > 3)
                        m_fullName = bi.FullName;
                    else
                        bi.FullName = m_fullName;

                return m_fullName;
            }
            set
            {
                (bool valid, BusinessInfo bi) = GetBusinessInfo();
                if (valid)
                {
                    if (bi.FullName.Length < 5) bi.FullName = value;
                    bi.IsModified = true;
                }
                m_fullName = value;
            }
        }

        [DataMember, Browsable(true), Category("IDs"), DisplayName("ISIN")]
        public string ISIN { get; set; } = string.Empty;

        public (bool valid, BusinessInfo bi) GetBusinessInfo() => BusinessInfoList.GetOrAdd(this);

        #endregion Identification

        #region Order and Trade

        [DataMember]
        public bool SmartExchangeRoute { get; set; } = true;

        #endregion

        [DataMember]
        public double TargetPrice { get; set; }


        #region Status and Market Data

        [DataMember]
        public double Ask { get; set; } = -double.MinValue;

        [DataMember]
        public double AskSize { get; set; } = -double.MinValue;

        [DataMember]
        public string AskExchange { get; set; } = string.Empty;

        [DataMember]
        public double Bid { get; set; } = -double.MinValue;

        [DataMember]
        public double BidSize { get; set; } = -double.MinValue;

        [DataMember]
        public string BidExchange { get; set; } = string.Empty;

        [DataMember]
        public double Open { get; set; } = -double.MinValue;

        [DataMember]
        public double High { get; set; } = -double.MinValue;

        [DataMember]
        public double Low { get; set; } = -double.MinValue;

        [DataMember]
        public double Last { get => Price; set => Price = value; }

        [DataMember]
        public double LastSize { get; set; } = -double.MinValue;

        [DataMember]
        public double Volume { get; set; } = -double.MinValue;

        [DataMember]
        public string LastExchange { get; set; } = string.Empty;

        [DataMember]
        public double LastClose { get; set; } = -double.MinValue;




        #endregion Status and Market Data

        #region Short Status

        [DataMember]
        public double ShortStatus { get; set; }

        [DataMember]
        public double ShortableShares { get; set; }

        #endregion Short Status



        public virtual bool Request_MarketDepth() => IB.Client.SendRequest_MarketDepth(this);

        /// <summary>
        /// TODO: Cancel_MarketDepth()
        /// </summary>
        public virtual void Cancel_MarketDepth() { }

        [DataMember]
        public Dictionary<int, (DateTime Time, double Price, double Size, Exchange MarketMaker)> MarketDepth { get; private set; }
            = new Dictionary<int, (DateTime Time, double Price, double Size, Exchange MarketMaker)>();

        #region Equality

        public bool Equals(BusinessInfo other) => ISIN == other.ISIN;

        public static bool operator ==(Stock s1, BusinessInfo s2) => s1.Equals(s2);
        public static bool operator !=(Stock s1, BusinessInfo s2) => !s1.Equals(s2);

        public override bool Equals(object obj) => base.Equals(obj);

        public override int GetHashCode() => Info.GetHashCode();

        #endregion Equality
    }
}
