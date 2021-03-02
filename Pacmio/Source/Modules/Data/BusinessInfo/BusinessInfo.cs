/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Xu;

namespace Pacmio
{
    [Serializable, DataContract]
    public sealed class BusinessInfo : IDataFile, IDataProvider, IEquatable<BusinessInfo>, IEquatable<IBusiness>, IEquatable<string>
    {
        public BusinessInfo(string isin)
        {
            ISIN = isin;

            // Automatically generate CUSIP for US ISINs
            if (ISIN.Substring(0, 2) == "US")
                CUSIP = ISIN.Substring(2, 9);
        }

        [IgnoreDataMember]
        public bool IsModified { get; set; } = false;

        [DataMember, Browsable(false)]
        public DateTime UpdateTime { get; set; } = DateTime.MinValue;

        [DataMember, Browsable(true), Category("IDs"), DisplayName("ISIN")]
        public string ISIN { get; private set; }

        [DataMember, Browsable(true), Category("IDs"), DisplayName("CUSIP")]
        public string CUSIP { get; set; } = string.Empty;

        [DataMember, Browsable(true), Category("IDs")]
        public Dictionary<string, string> IDs = new Dictionary<string, string>();

        [DataMember]
        public bool FullNameLocked { get; set; } = false;

        [DataMember]
        private string m_FullName = string.Empty;

        [IgnoreDataMember, Browsable(true), DisplayName("Full Name")]
        public string FullName
        {
            get
            {
                return m_FullName;
            }
            set
            {
                if (string.IsNullOrEmpty(m_FullName))
                {
                    m_FullName = value;
                    IsModified = true;
                }
                else if (!FullNameLocked)
                {
                    m_FullName = value;
                    IsModified = true;
                }
            }
        }

        [DataMember, Browsable(false)]
        public Dictionary<(DateTime LastModified, string Type), string> SummaryList { get; private set; } = new Dictionary<(DateTime LastModified, string Type), string>();

        [IgnoreDataMember, Browsable(true)]
        public string BusinessSummary
        {
            get
            {
                var res = SummaryList.Where(n => n.Key.Type == "Business Summary").OrderBy(n => n.Key.LastModified);
                if (res.Count() > 0)
                    return res.Last().Value;
                else
                    return string.Empty;
            }
        }

        [IgnoreDataMember, Browsable(true)]
        public string FinancialSummary
        {
            get
            {
                var res = SummaryList.Where(n => n.Key.Type == "Financial Summary").OrderBy(n => n.Key.LastModified);
                if (res.Count() > 0)
                    return res.Last().Value;
                else
                    return string.Empty;
            }
        }

        [DataMember, Browsable(false)]
        public SortedDictionary<DateTime, PeerInfo> PeerInfo { get; private set; } = new SortedDictionary<DateTime, PeerInfo>();

        [DataMember, Browsable(false)]
        public HashSet<Employee> Officers { get; set; } = new HashSet<Employee>();

        [DataMember, Browsable(false)]
        public SortedDictionary<DateTime, Address> AddressList { get; private set; } = new SortedDictionary<DateTime, Address>();

        [DataMember, Browsable(false)]
        public Dictionary<(DateTime Time, ContactDataType Type, string Param), string> Contact { get; private set; }
            = new Dictionary<(DateTime Time, ContactDataType Type, string Param), string>();

        #region Data Provider

        public List<IDataConsumer> DataConsumers { get; } = new List<IDataConsumer>();

        public bool AddDataConsumer(IDataConsumer idk)
        {
            return DataConsumers.CheckAdd(idk);
        }

        public bool RemoveDataConsumer(IDataConsumer idk)
        {
            if (idk is DockForm df) df.ReadyToShow = false;
            return DataConsumers.CheckRemove(idk);
        }

        public void Updated()
        {
            UpdateTime = DateTime.Now;

            IDataConsumer[] dataConsumerList = null;

            lock (DataConsumers)
            {
                dataConsumerList = DataConsumers.ToArray();
            }

            Parallel.ForEach(dataConsumerList, idk => idk.DataIsUpdated(this));
        }

        #endregion Data Provider

        #region File Operation







        #endregion File Operation

        #region Equality

        public bool Equals(BusinessInfo other) => ISIN == other.ISIN;
        public bool Equals(IBusiness other) => ISIN == other.ISIN;
        public bool Equals(string other) => ISIN == other;

        public static bool operator ==(BusinessInfo s1, BusinessInfo s2) => s1.Equals(s2);
        public static bool operator !=(BusinessInfo s1, BusinessInfo s2) => !s1.Equals(s2);
        public static bool operator ==(BusinessInfo s1, IBusiness s2) => s1.Equals(s2);
        public static bool operator !=(BusinessInfo s1, IBusiness s2) => !s1.Equals(s2);
        public static bool operator ==(BusinessInfo s1, string s2) => s1.Equals(s2);
        public static bool operator !=(BusinessInfo s1, string s2) => !s1.Equals(s2);

        public override bool Equals(object obj)
        {
            if (obj is null)
                return this is null;
            else if (obj is BusinessInfo bi)
                return Equals(bi);
            else if (obj is IBusiness tr)
                return Equals(tr);
            else if (obj is string isin)
                return Equals(isin);
            else
                return false;
        }

        public override int GetHashCode() => ISIN.GetHashCode();

        #endregion Equality
    }
}
