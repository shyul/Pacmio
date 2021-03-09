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
        public BarDataFile(BarTable bt) : this(bt.Contract, bt.BarFreq, bt.Type) { }

        public BarDataFile(Contract c, BarFreq freq, BarType type)
        {
            Contract = c;
            BarFreq = freq;
            Type = type;
            Frequency = BarFreq.GetAttribute<BarFreqInfo>().Frequency;
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
                if (m_Contract is null || m_FundamentalData is null)
                {
                    m_Contract = ContractManager.GetByKey(ContractKey);
                    m_FundamentalData = m_Contract.GetOrCreateFundamentalData();
                }
                return m_Contract;

            }
            private set
            {
                m_Contract = value;
                ContractKey = value.Key;
                m_FundamentalData = m_Contract.GetOrCreateFundamentalData();
            }
        }

        [IgnoreDataMember]
        private Contract m_Contract = null;

        [IgnoreDataMember]
        private FundamentalData FundamentalData
        {
            get
            {
                if (m_Contract is null || FundamentalData is null)
                {
                    m_Contract = ContractManager.GetByKey(ContractKey);
                    m_FundamentalData = m_Contract.GetOrCreateFundamentalData();
                }
                return m_FundamentalData;
            }
        }

        [IgnoreDataMember]
        private FundamentalData m_FundamentalData = null;

        [DataMember]
        public BarFreq BarFreq { get; set; }

        [DataMember]
        public BarType Type { get; set; }

        public BarTable GetBarTable() => new BarTable(Contract, BarFreq, Type);

        [IgnoreDataMember]
        public Frequency Frequency { get; private set; } //=> BarFreq.GetAttribute<BarFreqInfo>().Frequency;

        [IgnoreDataMember]
        public DateTime EarliestDataTime => DataSourceSegments.Start;

        [IgnoreDataMember]
        public DateTime HistoricalHeadTime
        {
            get => (m_HistoricalHeadTime != DateTime.MaxValue && BarFreq < BarFreq.Minute && m_HistoricalHeadTime < DateTime.Now.Date.AddMonths(-6)) ? DateTime.Now.Date.AddMonths(-6) : m_HistoricalHeadTime;
            set => m_HistoricalHeadTime = value;
        }

        [DataMember]
        private DateTime m_HistoricalHeadTime = DateTime.MaxValue;

        [DataMember]
        public DateTime LastUpdateTime { get; private set; } = DateTime.MinValue;

        [IgnoreDataMember]
        private object DataLockObject { get; set; } = new object();

        [DataMember]
        private Dictionary<DateTime, (DataSourceType SRC, double O, double H, double L, double C, double V)> Rows { get; set; }
            = new Dictionary<DateTime, (DataSourceType SRC, double O, double H, double L, double C, double V)>();

        [IgnoreDataMember]
        public int Count => Rows.Count;

        [DataMember]
        private MultiPeriod<DataSourceType> DataSourceSegments { get; set; } = new MultiPeriod<DataSourceType>();

        public DateTime LastTimeBy(DataSourceType source)
        {
            lock (DataLockObject)
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

        /// <summary>
        /// Similar to BarTable add ticks.
        /// </summary>
        /// <param name="pd"></param>
        /// <param name="minimumSource"></param>
        /// <returns></returns>
        public MultiPeriod GetMissingPeriods(Period period, DataSourceType minimumSource = DataSourceType.IB)
        {
            DateTime stoplimit = DateTime.Now.AddDays(1).Date;
            DateTime stop = period.Stop.Date.AddDays(1);

            DateTime start = period.Start < HistoricalHeadTime ? HistoricalHeadTime : period.Start.Date; // new DateTime(period.Start.Year, period.Start.Month, period.Start.Day);
            stop = period.IsCurrent || stop > stoplimit ? stoplimit : stop;

            if (start < stop)
            {
                WorkHours wh = Contract.WorkHoursExtended;
                MultiPeriod missing_period_list = new MultiPeriod(new Period(start, stop));

                DateTime check = start, rm_start = start;
                bool start_chk_pd = true;

                while (check < stop)
                {
                    if (!wh.IsWorkTime(check))
                    {
                        if (start_chk_pd)
                        {
                            start_chk_pd = false;
                            rm_start = check;
                        }
                    }
                    else
                    {
                        if (!start_chk_pd)
                        {
                            start_chk_pd = true;
                            missing_period_list.Remove(new Period(rm_start, check));
                        }
                    }

                    check.AddMinutes(10);
                }

                foreach (Period existingPd in DataSourceSegments.Keys.Where(n => DataSourceSegments[n] <= minimumSource))
                {
                    missing_period_list.Remove(existingPd);
                    Console.WriteLine("BarDataFile Already Existing: " + existingPd);
                }

                return missing_period_list;
            }

            return null;
        }

        public void AddRows(
            IEnumerable<(DateTime time, double O, double H, double L, double C, double V)> rows,
            DataSourceType sourceType,
            Period segmentPeriod,
            bool counterAdjust = false,
            bool adjustDividend = false)
        {
            lock (rows)
                if (rows.Count() > 0)
                {
                    var sortedList = rows.OrderBy(n => n.time).ToList();

                    if (counterAdjust)
                    {
                        MultiPeriod<(double Price, double Volume)> barTableAdjust = FundamentalData.BarTableAdjust(adjustDividend);

                        // Please notice b.Time is the start time of the Bar
                        // When the adjust event (split or dividend) happens at d 
                        // The adjust will happen in d-1, which belongs to the
                        // prior adjust segment.
                        //                    S
                        // ---------------------------------------
                        //                   AD
                        // aaaaaaaaaaaaaaaaaaadddddddddddddddddddd
                        for (int i = 0; i < sortedList.Count; i++)
                        {
                            var row = sortedList[i];
                            var (adj_price, adj_vol) = barTableAdjust[row.time];
                            sortedList[i] = (row.time, row.O / adj_price, row.H / adj_price, row.L / adj_price, row.C / adj_price, row.V * adj_vol);
                        }
                    }

                    Period pd = segmentPeriod is null ? new Period(sortedList.First().time, sortedList.Last().time + Frequency.Span) : segmentPeriod;

                    lock (DataLockObject)
                    {
                        DataSourceSegments.Add(pd, sourceType);
                        foreach (var row in sortedList)
                        {
                            Rows[row.time] = (sourceType, row.O, row.H, row.L, row.C, row.V);
                        }
                    }

                    LastUpdateTime = DateTime.Now;
                    IsModified = true;
                }
        }

        public List<Bar> LoadBars(BarTable bt, Period pd, bool adjustDividend = false)
        {
            if (this != bt) throw new Exception("BarTable must match!");

            List<Bar> sortedList = null;

            lock (DataLockObject)
            {
                sortedList = Rows.
                    Where(n => pd.Contains(n.Key)).
                    OrderBy(n => n.Key).
                    Select(n => new Bar(bt, n.Key, n.Value.O, n.Value.H, n.Value.L, n.Value.C, n.Value.V, n.Value.SRC)).ToList();
            }

            return AdjustBars(bt, sortedList, adjustDividend);
        }

        public List<Bar> LoadBars(BarTable bt, bool adjustDividend = false)
        {
            if (this != bt) throw new Exception("BarTable must match!");

            List<Bar> sortedList = null;

            lock (DataLockObject)
            {
                sortedList = Rows.
                    OrderBy(n => n.Key).
                    Select(n => new Bar(bt, n.Key, n.Value.O, n.Value.H, n.Value.L, n.Value.C, n.Value.V, n.Value.SRC)).ToList();
            }

            return AdjustBars(bt, sortedList, adjustDividend);
        }

        private List<Bar> AdjustBars(BarTable bt, List<Bar> sortedList, bool adjustDividend)
        {
            if (sortedList is not null && sortedList.Count > 0)
            {
                MultiPeriod<(double Price, double Volume)> barTableAdjust = FundamentalData.BarTableAdjust(adjustDividend);
                // Please notice b.Time is the start time of the Bar
                // When the adjust event (split or dividend) happens at d 
                // The adjust will happen in d-1, which belongs to the
                // prior adjust segment.
                //                    S
                // ---------------------------------------
                //                   AD
                // aaaaaaaaaaaaaaaaaaadddddddddddddddddddd
                for (int i = 0; i < sortedList.Count; i++)
                {
                    Bar b = sortedList[i];
                    var (adj_price, adj_vol) = barTableAdjust[b.Time];
                    b.Open *= adj_price;
                    b.High *= adj_price;
                    b.Low *= adj_price;
                    b.Close *= adj_price;
                    b.Volume /= adj_vol;
                }
            }

            return sortedList;

            //bt.LoadBars(sortedList);
        }

        public void Clear(Period pd)
        {
            if (pd.Span >= Frequency.Span)
            {
                lock (DataLockObject)
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
            lock (DataLockObject)
            {
                IsModified = !(DataSourceSegments.Count == 0 && Rows.Count == 0);
                LastUpdateTime = DateTime.MinValue;
                DataSourceSegments.Clear();
                Rows.Clear();
            }
        }


        #region Download / Fetch Operation
        /*
        public static void Download(IEnumerable<Contract> contracts, IEnumerable<(BarFreq freq, BarType type, Period period)> settings, CancellationTokenSource cts, IProgress<float> progress)
        {
            List<(BarFreq freq, BarType type, Period period)> settings_list = new List<(BarFreq freq, BarType type, Period period)>() { (BarFreq.Daily, BarType.Trades, new Period(new DateTime(1000, 1, 1), DateTime.Now)) };

            var priority_settings = settings.Where(n => n.type == BarType.Trades && (n.freq == BarFreq.Hourly || n.freq == BarFreq.Minute)).OrderByDescending(n => n.freq);
            settings_list.AddRange(priority_settings);

            var remaining_settings = settings.Where(n => !priority_settings.Contains(n) && !(n.freq == BarFreq.Daily && n.type == BarType.Trades)).OrderByDescending(n => n.freq);
            settings_list.AddRange(remaining_settings);

            ParallelOptions po = new ParallelOptions()
            {
                MaxDegreeOfParallelism = Math.Ceiling(Root.DegreeOfParallelism / 3D).ToInt32(1)
            };

            int i = 0, count = contracts.Count() * settings_list.Count();
            Parallel.ForEach(contracts, po, c =>
            {
                if (cts.IsContinue())
                {
                    foreach (var (freq, type, period) in settings_list)
                    {
                        if (cts.IsContinue())
                        {
                            BarTable bt = new BarTable(c, freq, type);
                            bt.Fetch(period, cts);
                            i++;
                            if (cts.IsContinue()) progress?.Report(100.0f * i / count);
                        }
                        else
                            return;
                    }
                }
                else
                    return;
            });
        }*/

        #endregion Download / Fetch Operation

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
            lock (DataLockObject)
                this.SerializeJsonFile(DataFileName);
        }

        public static BarDataFile LoadFile(((string name, Exchange exchange, string typeName) ContractKey, BarFreq BarFreq, BarType Type) info)
        {
            var bdf = Serialization.DeserializeJsonFile<BarDataFile>(GetDataFileName(info));
            bdf.DataLockObject = new object();
            bdf.Frequency = bdf.BarFreq.GetAttribute<BarFreqInfo>().Frequency;
            return bdf;
        }

        #endregion File Operation

        #region Export CSV

        public void ExportCSV(string fileName)
        {
            lock (DataLockObject)
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
