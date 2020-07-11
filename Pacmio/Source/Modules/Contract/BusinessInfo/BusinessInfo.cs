/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;
using Xu;

namespace Pacmio
{
    using FundamentalDataTable = Dictionary<
        (FundamentalDataType Type, DateTime Time, Frequency Freq),
        (DataSource Source, string Param, double Value, double AdjValue)>;
    using FundamentalDatum = KeyValuePair<
    (FundamentalDataType Type, DateTime Time, Frequency Freq),
    (DataSource Source, string Param, double Value, double AdjValue)>;

    [Serializable, DataContract]
    public sealed class BusinessInfo : IEquatable<BusinessInfo>, IEquatable<IBusiness>, IEquatable<string>
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

        #region Fundamental Data

        [DataMember, Browsable(false)]
        public FundamentalDataTable FundamentalData { get; private set; } = new FundamentalDataTable();

        public FundamentalDatum GetFundamentalDataLatest(FundamentalDataType type, Frequency freq, DataSource source) =>
            FundamentalData.Where(n => n.Key.Type == type && n.Key.Freq == freq && n.Value.Source == source)
            .OrderBy(n => n.Key.Time)
            .Last();

        public FundamentalDatum GetFundamentalDataLatest(FundamentalDataType type, Frequency freq) =>
            FundamentalData.Where(n => n.Key.Type == type && n.Key.Freq == freq)
            .OrderBy(n => n.Key.Time)
            .Last();

        public IOrderedEnumerable<FundamentalDatum> GetFundamentalData() =>
            FundamentalData
            .OrderBy(n => n.Key.Time)
            .ThenBy(n => n.Value.Source);

        public IOrderedEnumerable<FundamentalDatum> GetFundamentalData(FundamentalDataType type) =>
            FundamentalData
            .Where(n => n.Key.Type == type)
            .OrderBy(n => n.Key.Time)
            .ThenBy(n => n.Value.Source);

        public IOrderedEnumerable<FundamentalDatum> GetFundamentalData(FundamentalDataType type, Frequency freq) =>
            FundamentalData.Where(n => n.Key.Type == type && n.Key.Freq == freq)
            .OrderBy(n => n.Key.Time)
            .ThenBy(n => n.Value.Source);

        public IOrderedEnumerable<FundamentalDatum> GetFundamentalData(FundamentalDataType type, Frequency freq, Period period) =>
            FundamentalData.Where(n => n.Key.Type == type && n.Key.Freq == freq && period.Contains(n.Key.Time))
            .OrderBy(n => n.Key.Time)
            .ThenBy(n => n.Value.Source);

        // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/introduction-to-linq-queries
        // The actual execution of the query is deferred until you iterate over the query variable in a foreach statement. 
        // This concept is referred to as deferred execution and is demonstrated in the following example:

        public void ExportFundamentalData(IOrderedEnumerable<FundamentalDatum> res)
        {
            Root.SaveFile.Filter = "Comma-separated values file (*.csv) | *.csv";

            if (Root.SaveFile.ShowDialog() == DialogResult.OK)
            {
                string fileName = Root.SaveFile.FileName;

                StringBuilder sb = new StringBuilder("Type,Time,Frequency,Source,Param,Value\n");

                lock (FundamentalData)
                {
                    foreach (var item in res)
                    {
                        string[] items = new string[]
                        {
                        item.Key.Type.ToString(),
                        item.Key.Time.ToString("yyyy-MM-dd"),
                        item.Key.Freq.ToString(),
                        item.Value.Source.ToString(),
                        item.Value.Param,
                        item.Value.Value.ToString(),
                        };
                        sb.AppendLine(string.Join(",", items));
                    }
                }

                try
                {
                    if (File.Exists(fileName)) File.Delete(fileName);
                    File.WriteAllText(fileName, sb.ToString());
                }
                catch (Exception e) when (e is IOException || e is FormatException)
                {

                }
            }
        }

        [DataMember, Browsable(false)]
        public HashSet<FinancialStatement> FinancialStatements = new HashSet<FinancialStatement>();
        /*
        [DataMember, Browsable(false)]
        public SortedDictionary<DateTime, FinancialEventType> FinancialEvents { get; private set; } = new SortedDictionary<DateTime, FinancialEventType>();
        */
        #endregion Fundamental Data

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
