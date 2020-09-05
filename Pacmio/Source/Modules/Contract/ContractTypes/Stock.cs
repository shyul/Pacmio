/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// https://interactivebrokers.github.io/tws-api/basic_contracts.html#stk
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class Stock : Contract, IBusiness
    {
        #region Ctor

        public Stock(string name, Exchange exchange)
        {
            Name = name;
            Exchange = exchange;
            //m_StockData = new StockData();
        }

        [IgnoreDataMember]
        public StockData StockData
        {
            get
            {
                if (m_StockData is null) LoadMarketData();
                return m_StockData;
            }
        }

        [IgnoreDataMember]
        private StockData m_StockData = null;

        public override void LoadMarketData()
        {
            m_StockData = File.Exists(MarketDataFileName) ? Serialization.DeserializeJsonFile<StockData>(MarketDataFileName) : new StockData();
            m_StockData.Initialize(this);
            //if (m_StockData.LiveBarTables is null) m_StockData.LiveBarTables = new List<BarTable>();
        }

        public override void SaveMarketData()
        {
            if (m_StockData is StockData sd)
            {
                if (!Directory.Exists(MarketDataFilePath)) 
                    Directory.CreateDirectory(MarketDataFilePath);

                sd.SerializeJsonFile(MarketDataFileName);
            }
        }

        [IgnoreDataMember]
        public override MarketData MarketData => StockData;

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type")]
        public override string TypeName => "STOCK";

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type Full Name")]
        public override string TypeFullName => "Stock";
        /*
        [DataMember]
        public bool IsETF { get; set; } = false;
        */
        #endregion Ctor

        [IgnoreDataMember]
        public override bool NeedUpdate => (DateTime.Now - UpdateTime).Days > 2 && (base.NeedUpdate || ISIN.Length < 2);

        #region Identification

        [IgnoreDataMember, Browsable(true), DisplayName("Full Name")]
        public override string FullName
        {
            get
            {
                if (BusinessInfo is BusinessInfo bi)
                    if (bi.FullName.Length > 3)
                        m_fullName = bi.FullName;
                    else
                    {
                        bi.FullName = m_fullName;
                        bi.IsModified = true;
                    }

                return m_fullName;
            }
            set
            {
                if (BusinessInfo is BusinessInfo bi)
                {
                    bi.FullName = value;
                    bi.IsModified = true;
                }
                m_fullName = value;
            }
        }

        [DataMember, Browsable(true), Category("IDs"), DisplayName("ISIN")]
        public string ISIN { get; set; } = string.Empty;

        [IgnoreDataMember]
        public BusinessInfo BusinessInfo => BusinessInfoList.GetOrAdd(ISIN);

        [DataMember]
        public string Industry { get; set; }

        [DataMember]
        public string Category { get; set; }

        [DataMember]
        public string Subcategory { get; set; }

        #endregion Identification

        #region Order and Trade Data



        #endregion Order and Trade Data

        #region Market Depth

        public virtual bool Request_MarketDepth() => IB.Client.SendRequest_MarketDepth(this);

        /// <summary>
        /// TODO: Cancel_MarketDepth()
        /// </summary>
        public virtual void Cancel_MarketDepth() { }

        #endregion Market Depth

        #region Equality

        public bool Equals(BusinessInfo other) => ISIN == other.ISIN;

        public static bool operator ==(Stock s1, BusinessInfo s2) => s1.Equals(s2);
        public static bool operator !=(Stock s1, BusinessInfo s2) => !s1.Equals(s2);

        public override bool Equals(object obj) => base.Equals(obj);

        public override int GetHashCode() => Info.GetHashCode();

        #endregion Equality
    }
}
