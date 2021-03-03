/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    [Serializable, DataContract]
    public class BarDataFile : IDataFile, IEquatable<BarDataFile>, IEquatable<BarTable>, IDisposable
    {
        public BarDataFile(Contract c, BarFreq freq, BarType type)
        {
            Contract = c;
            BarFreq = freq;
            Type = type;
            EarliestTime = c.GetOrCreateFundamentalData().EarliestTime;
            //Frequency = BarFreq.GetAttribute<BarFreqInfo>().Frequency;
        }

        public BarDataFile(BarTable bt)
        {
            Contract = bt.Contract;
            BarFreq = bt.BarFreq;
            Type = bt.Type;
            EarliestTime = bt.Contract.GetOrCreateFundamentalData().EarliestTime;
            LastUpdateTime = bt.LastDownloadRequestTime;
            //Frequency = BarFreq.GetAttribute<BarFreqInfo>().Frequency;
        }

        public void Dispose()
        {
            Rows.Clear();
            GC.Collect();
        }

        [DataMember]
        public (string name, Exchange exchange, string typeName) ContractKey { get; private set; }

        [IgnoreDataMember]
        public Contract Contract
        {
            get
            {
                if (m_Contract is null || FundamentalData is null)
                {
                    m_Contract = ContractManager.GetByKey(ContractKey);
                    FundamentalData = m_Contract.GetOrCreateFundamentalData();
                }
                return m_Contract;

            }
            private set
            {
                m_Contract = value;
                ContractKey = value.Key;
                FundamentalData = m_Contract.GetOrCreateFundamentalData();
            }
        }

        [IgnoreDataMember]
        private Contract m_Contract = null;

        [IgnoreDataMember]
        private FundamentalData FundamentalData { get; set; } = null;

        [DataMember]
        public BarFreq BarFreq { get; set; }

        [DataMember]
        public BarType Type { get; set; }

        [IgnoreDataMember]
        private Frequency Frequency => BarFreq.GetAttribute<BarFreqInfo>().Frequency;

        [DataMember]
        public DateTime EarliestTime { get; set; } = DateTime.MinValue;

        [DataMember]
        public DateTime LastUpdateTime { get; set; } = DateTime.MinValue;

        [DataMember]
        private MultiPeriod<DataSourceType> DataSourceSegments { get; set; } = new MultiPeriod<DataSourceType>();

        [DataMember]
        private Dictionary<DateTime, (DataSourceType SRC, double O, double H, double L, double C, double V)> Rows { get; set; }
            = new Dictionary<DateTime, (DataSourceType SRC, double O, double H, double L, double C, double V)>();

        public DateTime LastTimeBy(DataSourceType source)
        {
            lock (DataSourceSegments)
            {
                if (DataSourceSegments.Where(n => n.Value == source).OrderBy(n => n.Key).Select(n => n.Key).LastOrDefault() is Period pd)
                {
                    return pd.Stop;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
            //var res = Bars.Where(n => n.Source <= source).OrderBy(n => n.Time);
            //return (res.Count() > 0) ? res.Last().Time : DateTime.MinValue.AddYears(500);
        }

        public void AddRows(IEnumerable<(DateTime time, double O, double H, double L, double C, double V)> rows, DataSourceType sourceType, bool counteradjust)
        {
            lock (rows)
                if (rows.Count() > 0)
                {
                    if (counteradjust) 
                    {
                        // load counter adjust table...
                    }

                    var sortedRows = rows.OrderBy(n => n.time);


                    Period pd = new Period(sortedRows.First().time, sortedRows.Last().time + Frequency.Span);

                    lock (Rows)
                        lock (DataSourceSegments)
                        {
                            DataSourceSegments.Add(pd, sourceType);
                            foreach (var row in sortedRows)
                            {
                                Rows[row.time] = (sourceType, row.O, row.H, row.L, row.C, row.V);
                            }
                        }

                    IsModified = true;
                }
        }

        public List<(DateTime time, DataSourceType SRC, double O, double H, double L, double C, double V)> GetRows(Period pd, bool adjust_dividend)
        {
            lock (Rows)
                lock (DataSourceSegments)
                {
                    // adjust by
                    MultiPeriod<(double Price, double Volume)> barTableAdjust = FundamentalData.BarTableAdjust(adjust_dividend); //sd.BarTableAdjust(AdjustDividend);

                    return Rows.Where(n => pd.Contains(n.Key)).Select(n => (n.Key, n.Value.SRC, n.Value.O, n.Value.H, n.Value.L, n.Value.C, n.Value.V)).ToList();
                }
        }

        public void RemoveRows(Period pd)
        {
            if (pd.Span >= Frequency.Span)
            {
                lock (Rows)
                    lock (DataSourceSegments)
                    {
                        DataSourceSegments.Remove(pd);
                        var listToRemove = Rows.Select(n => n.Key).Where(n => pd.Contains(n)).ToList();
                        listToRemove.ForEach(n => Rows.Remove(n));

                        if (listToRemove.Count() > 0) 
                            IsModified = true;
                    }
            }
        }

        public void Clear()
        {
            lock (Rows)
                lock (DataSourceSegments)
                {
                    IsModified = !(DataSourceSegments.Count == 0 && Rows.Count == 0);
                    
                    DataSourceSegments.Clear();
                    Rows.Clear();
                }
        }

        #region File Operation

        private static string GetDataFileName(((string name, Exchange exchange, string typeName) ContractKey, BarFreq BarFreq, BarType Type) info)
        {
            string dir = Root.HistoricalDataPath(info.ContractKey) + "\\" + info.BarFreq.ToString() + "_" + info.Type.ToString() + "\\";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            return dir + (info.ContractKey.typeName == "INDEX" ? "^" : "$") + info.ContractKey.name + ".json";
        }

        [IgnoreDataMember]
        public string DataFileName => GetDataFileName((ContractKey, BarFreq, Type));

        // TODO: Handle this
        [IgnoreDataMember]
        public bool IsModified { get; private set; } = false;

        public void SaveFile()
        {
            lock (Rows)
                lock (DataSourceSegments)
                    this.SerializeJsonFile(DataFileName);
        }

        public static BarDataFile LoadFile(((string name, Exchange exchange, string typeName) ContractKey, BarFreq BarFreq, BarType Type) info)
            => Serialization.DeserializeJsonFile<BarDataFile>(GetDataFileName(info));

        public static BarDataFile LoadFile(BarTable bt) => LoadFile(bt.Key);

        #endregion File Operation

        #region Export CSV

        public void ExportCSV(string fileName)
        {
            var list = Rows.OrderByDescending(n => n.Key);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Time,Data Source,Open,High,Low,Close,Volume,Event");

            var fd = ContractKey.GetOrCreateFundamentalData();

            FundamentalDatum[] fdlist = null;

            DateTime date = DateTime.MaxValue.Date;
            string fd_event = string.Empty;

            foreach (var row in list)
            {
                DateTime newDate = row.Key.Date;

                if (date > newDate)
                {
                    fdlist = fd.GetList(newDate);
                    foreach (var fdm in fdlist)
                    {
                        fd_event += fdm.TypeName + " = " + fdm.Value + " | ";
                    }
                    fd_event = fd_event.Trim(new char[] { ' ', '|' }).Trim();
                    date = newDate;
                }

                sb.AppendLine(string.Join(",", new string[] {
                    row.Key.ToString(),
                    row.Value.SRC.ToString(),
                    row.Value.O.ToString(),
                    row.Value.H.ToString(),
                    row.Value.L.ToString(),
                    row.Value.C.ToString(),
                    row.Value.V.ToString(),
                    fd_event
                }));

                fd_event = string.Empty;
            }

            sb.ToFile(fileName);
        }

        #endregion Export CSV

        #region Equality

        public bool Equals(BarDataFile other) => (ContractKey == other.ContractKey) && (BarFreq == other.BarFreq) && (Type == other.Type);
        public static bool operator ==(BarDataFile left, BarDataFile right) => left.Equals(right);
        public static bool operator !=(BarDataFile left, BarDataFile right) => !left.Equals(right);

        public bool Equals(BarTable other) => (ContractKey, BarFreq, Type) == (other.Contract.Key, other.BarFreq, other.Type);
        public static bool operator ==(BarDataFile left, BarTable right) => left is BarDataFile btd && btd.Equals(right);
        public static bool operator !=(BarDataFile left, BarTable right) => !(left == right);

        public bool Equals(Contract other) => other is Contract c && c.Key == ContractKey;
        public static bool operator ==(BarDataFile left, Contract right) => left is BarDataFile btd && btd.Equals(right);
        public static bool operator !=(BarDataFile left, Contract right) => !(left == right);

        public override bool Equals(object other)
        {
            if (other is BarDataFile btd)
                return Equals(btd);
            else if (other is BarTable bt)
                return Equals(bt);
            else if (other is Contract c)
                return Equals(c);
            else
                return false;
        }

        public override int GetHashCode() => ContractKey.GetHashCode() ^ BarFreq.GetHashCode() ^ Type.GetHashCode();

        #endregion Equality
    }
}
