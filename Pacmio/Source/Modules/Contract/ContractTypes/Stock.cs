/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
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
            IsModified = false;
            MarketData = new MarketData(this);
        }

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type")]
        public override string TypeName => "STOCK";

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type Full Name")]
        public override string TypeFullName => "Stock";

        [IgnoreDataMember, Browsable(false), ReadOnly(true)]
        public override string TypeApiCode => "STK";

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
        public bool AutoExchangeRoute { get; set; } = true;

        #endregion

        #region Status and Market Data

        public virtual bool Request_MarketDepth() => IB.Client.SendRequest_MarketDepth(this);

        public virtual void Cancel_MarketDepth() { }

        #endregion Status and Market Data

        #region Equality

        public bool Equals(BusinessInfo other) => ISIN == other.ISIN;

        public static bool operator ==(Stock s1, BusinessInfo s2) => s1.Equals(s2);
        public static bool operator !=(Stock s1, BusinessInfo s2) => !s1.Equals(s2);

        public override bool Equals(object obj) => base.Equals(obj);

        public override int GetHashCode() => Info.GetHashCode();

        #endregion Equality
    }
}
