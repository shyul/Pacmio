﻿/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// BarTable Data Types
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xu;
using Pacmio.Analysis;

namespace Pacmio
{
    /// <summary>
    /// BarTable: the ultimate data holder for technical analysis with fundamental awareness
    /// </summary>
    public sealed class BarTable : ITagTable, IEquatable<BarTable>,
        IEquatable<(Contract c, BarFreq barFreq, BarType type)>, IDisposable
    {
        #region Ctor

        public BarTable(Contract c, BarFreq barFreq, BarType type)
        {
            Contract = c;
            BarFreq = barFreq;
            Frequency = BarFreq.GetAttribute<BarFreqInfo>().Frequency;
            Type = type;

            CalculateTickCancelTs = new CancellationTokenSource();
            CalculateTickTask = new Task(() => CalculateTickWorker(), CalculateTickCancelTs.Token);
            CalculateTickTask.Start();
        }

        public void Dispose()
        {
            IsLive = false;
            CalculateTickCancelTs.Cancel();

            lock (DataViews)
            {
                foreach (IDataView dv in DataViews)
                {
                    dv.RemoveData();
                }
            }

            if (Contract.MarketData is StockData sd) { sd.LiveBarTables.CheckRemove(this); }

            lock (Rows) Rows.Clear();
            lock (TimeToRows) TimeToRows.Clear();
            GC.Collect();
        }

        public override string ToString() => Name;

        public string Name => Contract.TypeFullName + ": " + Contract.Name + " (" + Contract.ExchangeName +
                " / " + Contract.CurrencySymbol + Contract.CurrencyCode + " / " + Frequency + ")";

        public bool Enabled { get; set; } = true;

        public bool AdjustDividend { get; set; } = false;

        public Contract Contract { get; }

        public BarFreq BarFreq { get; }

        public Frequency Frequency { get; }

        public BarType Type { get; }

        public (Contract c, BarFreq barFreq, BarType type) Info => (Contract, BarFreq, Type);

        #endregion Ctor

        #region Bars Properties and Methods

        /// <summary>
        /// The Rows Data Storage
        /// The Storage is not directly accessible outside of the class.
        /// </summary>
        private List<Bar> Rows { get; } = new List<Bar>();

        /// <summary>
        /// Lookup Bar by Index. Mostly used in the Chart.
        /// </summary>
        /// <param name="i">Index of the Bar in the Rows</param>
        /// <returns>Bar according to the given index</returns>
        public Bar this[int i]
        {
            get
            {
                if (i >= Count || i < 0)
                    return null;
                else
                    return Rows[i];
            }
        }

        /// <summary>
        /// Get bars of index i and the past length amount bars
        /// TODO: Fixed sequence
        /// </summary>
        /// <param name="i"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<Bar> this[int i, int count]
        {
            get
            {
                //int skip = i - count + 1;
                //if (skip < 0) skip = 0;
                int cnt = count - 1;
                if (i < cnt) cnt = i;
                return Rows.Skip(i - cnt).Take(cnt + 1); //.Reverse();
            }
        }

        public double this[int i, NumericColumn column]
        {
            get
            {
                if (i >= Count || i < 0)
                    return double.NaN;
                else
                    return Rows[i][column];
            }
        }

        public TagInfo this[int i, TagColumn column]
        {
            get
            {
                if (i >= Count || i < 0)
                    return null;
                else
                    return Rows[i][column];
            }
        }

        private void Clear()
        {
            lock (DataLockObject)
            {
                TimeToRows.Clear();
                Rows.Clear();
                ResetCalculationPointer();
                DataSourceSegments.Clear();
            }
        }

        /// <summary>
        /// Returns if the BarTable is has no Bars.
        /// </summary>
        public bool IsEmpty => (Count < 1);

        /// <summary>
        /// Returns the number of the Rows in the BarTable.
        /// </summary>
        public int Count => Rows.Count;

        /// <summary>
        /// Returns Last Row's Index
        /// </summary>
        private int LastIndex => Count - 1;

        /// <summary>
        /// Add single Bar into the BarTable. Will disregard if the Bar with exactly the same time already in the Table. 
        /// </summary>
        /// <param name="b"></param>
        private bool Add(Bar b)
        {
            if (b.Table == this && !Contains(b.Time)) // Shall we also rectify the time according to the BarSize property?
            {
                Rows.Add(b);
                if (Count > 0)
                {
                    if (b.Time < LastTime) // If bars are added to the head or in the middle of the table
                        Sort(); // Sort without adjust -- you never know if it needs reverse adjust or forward adjust here.
                    else           //else // If bars are add to the tail of the table, then we just append
                        TimeToRows.CheckAdd(b.Time, Count - 1);
                }
                else
                    TimeToRows.CheckAdd(b.Time, 0);

                return true;
            }
            else
                return false;
        }

        public void Add(DataSource Source, DateTime Time, TimeSpan Span, double Open, double High, double Low, double Close, double Volume, bool IsAdjusted)
        {
            if (Span == Frequency.Span)
            {
                Bar b = GetOrAdd(Time);
                if (b.Source >= Source)
                {
                    b.Source = Source;
                    if (IsAdjusted)
                    {
                        b.Open = Open;
                        b.High = High;
                        b.Low = Low;
                        b.Close = Close;
                        b.Volume = Volume;
                    }
                    else
                    {
                        b.Actual_Open = Open;
                        b.Actual_High = High;
                        b.Actual_Low = Low;
                        b.Actual_Close = Close;
                        b.Actual_Volume = Volume;
                    }
                }
            }
        }

        private bool Add(DateTime tickTime, double last, double volume)
        {
            bool isModified = false;
            DateTime time = Frequency.Align(tickTime);

            if (this[time] is Bar b)
            {
                if (b.Source >= DataSource.IB)
                {
                    if (last > b.High) // New High
                    {
                        b.Actual_High = b.High = last; // Also update 
                        isModified = true;
                    }

                    if (last < b.Low) // New Low
                    {
                        b.Actual_Low = b.Low = last;
                        isModified = true;
                    }

                    if (tickTime <= b.DataSourcePeriod.Start && tickTime > b.Period.Start) // Eariler Open
                    {
                        b.Actual_Open = b.Open = last;
                        b.DataSourcePeriod.Insert(tickTime);
                        isModified = true;
                    }

                    if (tickTime >= b.DataSourcePeriod.Stop && tickTime < b.Period.Stop) // Later Close
                    {
                        b.Actual_Close = b.Close = last;
                        b.DataSourcePeriod.Insert(tickTime);
                        isModified = true;

                        Console.WriteLine("]]]]]]]]]]]]]]]]]]]] " + b.DataSourcePeriod + " | " + b.Period);
                    }
                    else
                    {
                        Console.WriteLine("********** Inbound Tick Time Overflow ***********");
                    }

                    b.Volume += volume;
                    b.Actual_Volume = b.Volume;
                    b.Source = DataSource.Realtime;

                    Console.WriteLine("###### Inbound Tick Here ###### " + b.Source + " | " + tickTime + " | " + b.DataSourcePeriod.Start + " -> " + b.DataSourcePeriod.Stop + ", IsCurrent = " + b.DataSourcePeriod.IsCurrent + " | " + b.Period);
                }
                else
                {
                    Console.WriteLine("###### Inbound Tick Ignored, because source = " + b.Source);
                }
            }
            else
            {
                if (Count > 0 && this[LastIndex].Source >= DataSource.IB)
                    this[LastIndex].Source = DataSource.Realtime;

                isModified = Add(new Bar(this, tickTime, last, volume));
            }

            return isModified;
        }

        public bool MergeFromSmallerBar(Bar b)
        {
            bool isModified = false;

            if (b.BarFreq < BarFreq)
            {
                if (this[Frequency.Align(b.Time)] is Bar nb)
                {
                    if (b.High > nb.High) // New High
                    {
                        nb.High = b.High;
                        isModified = true;
                    }

                    if (b.Low < nb.Low) // New Low
                    {
                        nb.Low = b.Low;
                        isModified = true;
                    }

                    if (b.Period.Stop <= nb.DataSourcePeriod.Start) // Eariler Open
                    {
                        nb.Open = b.Open;
                        nb.Volume += b.Volume;
                        nb.DataSourcePeriod.Insert(b.Period.Start);
                        isModified = true;
                    }

                    if (b.Period.Start >= nb.DataSourcePeriod.Stop) // Later Close
                    {
                        nb.Close = b.Close;
                        nb.Volume += b.Volume;
                        nb.DataSourcePeriod.Insert(b.Period.Stop);
                        isModified = true;
                    }

                    if (nb.Source < b.Source) nb.Source = b.Source; // Worse Source
                }
                else
                {
                    return Add(new Bar(this, b));
                }
            }
            else if (b.BarFreq == BarFreq && b.Table == this && !Contains(b.Time))
            {
                Rows.Add(b);

                if (Count > 0)
                {
                    if (b.Time < LastTime) // If bars are added to the head or in the middle of the table
                        Sort(); // Sort without adjust -- you never know if it needs reverse adjust or forward adjust here.
                    else           //else // If bars are add to the tail of the table, then we just append
                        TimeToRows.CheckAdd(b.Time, Count - 1);
                }
                else
                    TimeToRows.CheckAdd(b.Time, 0);

                return true;
            }
            return isModified;
        }

        #endregion Bars Properties and Methods

        #region Time

        private Dictionary<DateTime, int> TimeToRows { get; } = new Dictionary<DateTime, int>();

        public IEnumerable<Bar> this[Period pd] => Rows.Where(n => pd.Contains(n.Time)).OrderBy(n => n.Time);

        public bool Remove(Period pd)
        {
            var list = this[pd].ToList();
            bool isMod = list.Count > 0;

            if (isMod)
            {
                foreach (Bar b in list)
                    Rows.Remove(b);

                Sort();
            }

            return isMod;
        }

        /// <summary>
        /// Lookup Bar by Time. Time is rounded to the closest next time in the Rows.
        /// </summary>
        /// <param name="time">time of the Bar</param>
        /// <returns>Bar closest to the given time</returns>

        public Bar this[DateTime time]
        {
            get
            {
                if (TimeToRows.ContainsKey(time))
                    return this[TimeToRows[time]];
                else
                    return null;
                /*
                int i = IndexOf(ref time);
                if (i < 0)
                    return null;
                else
                    return this[i];*/
            }
        }

        /// <summary>
        /// Returns if the Rows already has time stamp
        /// </summary>
        public bool Contains(DateTime time) //=> Rows.Where(n => n.Time == time).Count() > 0;
        {
            lock (TimeToRows)
            {
                return TimeToRows.ContainsKey(time);
            }
        }

        /// <summary>
        /// Returns the earlies Row's time
        /// </summary>
        public DateTime FirstTime
        {
            get
            {
                if (Count > 0)
                    return Rows[0].Time;
                else
                    return DateTime.Now;
            }
        }

        /// <summary>
        /// Returns Last Row's time
        /// </summary>
        public DateTime LastTime
        {
            get
            {
                if (Count > 0)
                    return Rows[LastIndex].Time;
                else
                    return DateTime.MinValue.AddYears(500);
            }
        }

        /// <summary>
        /// Returns current BarTable's maximum time span
        /// </summary>
        public Period Period
        {
            get
            {
                if (Count > 0)
                {
                    if (IsLive)
                        return new Period(FirstTime, true);
                    else
                        return new Period(FirstTime, LastTimeBound);
                }
                else
                    return Period.Empty;
            }
        }


        /// <summary>
        /// Last Most time including the Bar Period
        /// </summary>
        public DateTime LastTimeBound
        {
            get
            {
                if (Count > 0)
                    return Rows[LastIndex].Period.Stop; //// Shall we use 
                else
                    return DateTime.MinValue.AddYears(500);
            }
        }

        /// <summary>
        /// Last Time by specific data source
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public DateTime LastTimeBy(DataSource source)
        {
            var res = Rows.Where(n => n.Source <= source).OrderBy(n => n.Time);
            return (res.Count() > 0) ? res.Last().Time : DateTime.MinValue.AddYears(500);
        }

        public DateTime IndexToTime(int i)
        {
            if (i < 0)
            {
                return FirstTime - Frequency * (-i);
            }
            else if (i >= Count)
            {
                return LastTime + Frequency * (i - Count + 1);
            }
            else
            {
                return this[i].Time;
            }
        }

        /// <summary>
        /// Get the Index from a given time
        /// </summary>
        /// <param name="time">time is rectified to the index found in "M_Rows"</param>
        /// <returns>Index of the nearest time</returns>
        public int IndexOf(ref DateTime time)
        {
            if (Count == 0)
            {
                return -1;
            }

            DateTime t = time;

            if (TimeToRows.ContainsKey(t))
            {
                return TimeToRows[t];
            }
            else if (time <= FirstTime)
            {
                time = FirstTime;
                return 0;
            }
            else if (time >= LastTime)
            {
                time = LastTime;
                return LastIndex;
            }
            else
            {
                int pt = 0;
                for (int i = 0; i < Count; i++)
                {
                    if (time <= Rows[i].Time)
                    {
                        pt = i;
                        break;
                    }
                }

                if (pt < 0) pt = 0;
                else if (pt >= Count) pt = LastIndex;

                time = Rows[pt].Time;

                return pt;
            }
        }

        public Bar GetOrAdd(DateTime time)
        {
            time = Frequency.Align(time, 0); // Align time

            if (!Contains(time))
                Add(new Bar(this, time));

            return this[time];
        }

        #endregion Time

        #region Sort / Intrinsic Data Prepare before Technical Analysis

        /// Always refresh gain
        /// Always clear all Analysis Pointers.
        /// Triggering conditions --> Split != 1, Dividend !=0 && adj_div == true
        private void Adjust(bool forwardAdjust = true)
        {
            //Sort();
            if (Contract.MarketData is StockData sd)
            {
                MultiPeriod<(double Price, double Volume)> barTableAdjust = sd.BarTableAdjust(AdjustDividend);

                // Please notice b.Time is the start time of the Bar
                // When the adjust event (split or dividend) happens at d 
                // The adjust will happen in d-1, which belongs to the
                // prior adjust segment.
                //                    S
                // ---------------------------------------
                //                   AD
                // aaaaaaaaaaaaaaaaaaadddddddddddddddddddd
                for (int i = 0; i < Count; i++)
                {
                    Bar b = this[i];

                    var (adj_price, adj_vol) = barTableAdjust[b.Time];
                    b.Adjust(adj_price, adj_vol, forwardAdjust);
                }
            }
            //ResetCalculationPointer();
        }

        private void Sort()
        {
            TimeToRows.Clear();
            Rows.Sort((t1, t2) => t1.Time.CompareTo(t2.Time));
            for (int i = 0; i < Count; i++)
            {
                Bar b = Rows[i];
                TimeToRows[b.Time] = i;
                b.Index = i;
            }
            //ResetCalculationPointer();
            //Console.WriteLine("Sorted Table " + ToString() + " | Count: " + Count + " | Period: " + Period.ToString());
        }

        #endregion Sort / Intrinsic Data Prepare before Technical Analysis

        #region Basic Data

        public static GainAnalysis GainAnalysis { get; } = new GainAnalysis(); // { ChartEnabled = false };

        public static TrueRange TrueRangeAnalysis { get; } = new TrueRange(); // { ChartEnabled = false };

        public static TrendStrength TrendStrengthAnalysis { get; } = new TrendStrength(); // { ChartEnabled = false };

        public static PivotPointAnalysis PivotPointAnalysis { get; } = new PivotPointAnalysis(); // { ChartEnabled = false };

        //public static PivotRanges CalculatePivotRange { get; } = new PivotRanges();

        #endregion Basic Data

        #region Data/Bar Analysis (TA) Calculation

        private Dictionary<BarAnalysis, BarAnalysisPointer> BarAnalysisPointerList { get; } = new Dictionary<BarAnalysis, BarAnalysisPointer>();

        private BarAnalysisPointer GetBarAnalysisPointer(BarAnalysis ba)
        {
            if (!BarAnalysisPointerList.ContainsKey(ba))
                BarAnalysisPointerList.Add(ba, new BarAnalysisPointer(this, ba));

            return BarAnalysisPointerList[ba];
        }

        private void ResetCalculationPointer() => SetCalculationPointer(0);

        private void SetCalculationPointer(int pt)
        {
            if (pt < 0)
                pt = 0;

            foreach (BarAnalysisPointer bap in BarAnalysisPointerList.Values)
                if (bap.StartPt > pt)
                    bap.StartPt = pt;
        }

        /*
        public void SetCalculationPointer(ref DateTime time)
        {
            int pt = IndexOf(ref time) - 1;
            if (pt < 0) pt = 0;
            SetCalculationPointer(pt);
        }*/

        public int LastCalculateIndex { get; private set; } = -1;

        /// <summary>
        /// Last most Close
        /// </summary>
        public double LastClose => (LastBar is null) ? -1 : LastBar.Close;

        /// <summary>
        /// Returns the Last Bar in the Table. Null is the BarTable is empty.
        /// </summary>
        public Bar LastBar
        {
            get
            {
                if (LastCalculateIndex < 0)
                    return null;
                else
                    return this[LastCalculateIndex];
            }
        }

        public Bar LastBar_1
        {
            get
            {
                if (LastCalculateIndex < 1)
                    return null;
                else
                    return this[LastCalculateIndex - 1];
            }
        }

        private BarAnalysisPointer Calculate(BarAnalysis ba)
        {
            BarAnalysisPointer bap = GetBarAnalysisPointer(ba);
            ba.Update(bap);
            return bap;
        }

        /// <summary>
        /// The mighty calculate for all technicial analysis
        /// </summary>
        private void Calculate(IEnumerable<BarAnalysis> analyses, bool debugInfo = true)
        {
            if (debugInfo)
            {
                Console.WriteLine("\n==================");
                Console.WriteLine("Table: " + Name + " | Count: " + Count);
            }

            DateTime total_time = DateTime.Now;

            int startPt = Count;

            if (Count > 0)
            {
                startPt = Math.Min(startPt, Calculate(GainAnalysis).StartPt);
                startPt = Math.Min(startPt, Calculate(TrueRangeAnalysis).StartPt);
                startPt = Math.Min(startPt, Calculate(TrendStrengthAnalysis).StartPt);
                startPt = Math.Min(startPt, Calculate(PivotPointAnalysis).StartPt);

                foreach (BarAnalysis ba in analyses)
                {
                    DateTime single_time = DateTime.Now;

                    BarAnalysisPointer bap = GetBarAnalysisPointer(ba);
                    int original_start = bap.StartPt;
                    int original_stop = bap.StopPt;
                    ba.Update(bap);
                    startPt = Math.Min(startPt, bap.StartPt);

                    if (debugInfo)
                    {
                        Console.WriteLine(ba.Name + " | (" + original_start + "->" + bap.StartPt + ") | Time " + (DateTime.Now - single_time).TotalMilliseconds.ToString() + "ms");
                    }
                }
            }
            LastCalculateIndex = startPt;

            if (debugInfo)
            {
                Console.WriteLine("------------------");
                Console.WriteLine(Name + " | Calculate(): " + (DateTime.Now - total_time).TotalMilliseconds.ToString() + "ms" + " | Stopped at: " + LastCalculateIndex);
                Console.WriteLine(Name + " | LastTime = " + LastTime + " | LastBar.Close = " + LastBar.Close);
                Console.WriteLine("==================\n");
            }
        }

        #endregion Data/Bar Analysis (TA) Calculation

        #region Pattern

        /// <summary>
        /// Intermediate Storage for Patterns
        /// Yes, all has to be gone when the Bars Are sorted....
        /// TODO: This part can go back to the Bars!!! By linq
        /// </summary>
        //public Dictionary<int, PatternDatum> Patterns { get; } = new Dictionary<int, PatternDatum>();

        #endregion Pattern

        #region BarChart / DataView

        public List<IDataView> DataViews { get; } = new List<IDataView>();

        #endregion BarChart / DataView

        #region Operations

        /// <summary>
        /// For Multi Thread Access
        /// </summary>
        public object DataLockObject { get; } = new object();

        public bool IsLive
        {
            get => m_IsLive;

            private set
            {
                m_IsLive = value;

                if (Contract.MarketData is StockData sd)
                    if (m_IsLive)
                    {
                        // Add BarTable to the tick receiver
                        sd.LiveBarTables.CheckAdd(this);
                    }
                    else
                    {
                        // Remove BarTable from the tick receiver
                        sd.LiveBarTables.CheckRemove(this);
                    }
            }
        }

        private bool m_IsLive = false;

        public bool ReadyToShow => Count > 0 && Status != TableStatus.Default && Status != TableStatus.Loading && Status != TableStatus.Downloading && Status != TableStatus.Maintaining;// (Status == TableStatus.Ready || Status == TableStatus.CalculateFinished || Status == TableStatus.TickingFinished);

        public TableStatus Status
        {
            get => m_Status;

            set
            {
                m_Status = value;

                if (m_Status == TableStatus.CalculateFinished)
                {
                    lock (DataViews) DataViews.ForEach(n => { n.PointerToEnd(); });
                }
                else if (m_Status == TableStatus.TickingFinished)
                {
                    lock (DataViews) DataViews.ForEach(n => { n.PointerToNextTick(); });
                }
                else if (!ReadyToShow)
                {
                    lock (DataViews) DataViews.ForEach(n => { n.SetAsyncUpdateUI(); });
                }
            }
        }

        private TableStatus m_Status = TableStatus.Default;

        public void Load() => Load(Period.Full);

        public void Load(Period period)
        {
            if (Enabled)
                lock (DataLockObject)
                {
                    Status = TableStatus.Loading;
                    IsLive = period.IsCurrent;
                    SyncFile(period);
                    Sort();
                    Adjust(); // Forward Adjust

                    Status = TableStatus.LoadFinished;
                    // lead it to calculation... here
                }
        }

        public void CalculateOnly(BarAnalysisSet bas)
        {
            if (Enabled && Count > 0 && bas is BarAnalysisSet)
                lock (DataLockObject)
                {
                    Status = TableStatus.Calculating;
                    Calculate(bas);
                }
        }

        public void CalculateFinish(BarAnalysisSet bas)
        {
            if (Enabled && Count > 0 && bas is BarAnalysisSet)
                lock (DataLockObject)
                {
                    if (Status == TableStatus.Calculating)
                    {
                        lock (DataViews)
                        {
                            DataViews.Where(n => n is BarChart bc && bc.BarAnalysisSet == bas).ToList().ForEach(n => { n.PointerToEnd(); });
                        }
                        m_Status = TableStatus.Ready;
                    }
                }
        }

        public void CalculateRefresh(BarAnalysisSet bas)
        {
            if (Enabled && Count > 0 && bas is BarAnalysisSet)
                lock (DataLockObject)
                {
                    Status = TableStatus.Calculating;
                    Calculate(bas);
                    lock (DataViews)
                    {
                        DataViews.Where(n => n is BarChart bc && bc.BarAnalysisSet == bas).ToList().ForEach(n => { n.PointerToEnd(); });
                    }
                    m_Status = TableStatus.Ready;
                }
        }

        private bool CalculateTickRequested { get; set; } = false;

        public void AddPriceTick(DateTime tickTime, double last, double volume)
        {
            if (Enabled && IsLive)
            {

                //lock (DataLockObject) 
                Add(tickTime, last, volume);

                if (Status == TableStatus.Ready)
                {
                    Status = TableStatus.Ticking;
                    SetCalculationPointer(LastCalculateIndex - 1);
                    CalculateTickRequested = true;
                }
            }
        }

        private Task CalculateTickTask { get; }

        private CancellationTokenSource CalculateTickCancelTs { get; }

        private void CalculateTickWorker()
        {
            while (CalculateTickCancelTs is CancellationTokenSource cts && !cts.IsCancellationRequested)
            {
                if (CalculateTickRequested)
                {
                    CalculateTickRequested = false;

                    lock (DataLockObject)
                    {
                        Calculate(BarAnalysisPointerList.Keys);
                        Status = TableStatus.TickingFinished;
                    }

                    Status = TableStatus.Ready;
                }
                else
                    Thread.Sleep(1);
            }
        }

        public void ResetCalculateData()
        {
            lock (DataLockObject)
            {
                while (Status != TableStatus.Ready && Status != TableStatus.LoadFinished) ;

                TableStatus last_status = Status;
                Status = TableStatus.Maintaining;
                //ResetCalculationPointer();
                // Remove any non-existing analysis
                //var non_existing_list = BarAnalysisPointerList.Keys.Where(n => !analyses.Contains(n)).ToList();
                //non_existing_list.ForEach(n => BarAnalysisPointerList.Remove(n));

                BarAnalysisPointerList.Clear();
                Rows.AsParallel().ForAll(n => n.ClearAllCalculationData());
                Status = last_status;
            }
        }

        public void Save()
        {
            lock (DataLockObject)
            {
                TableStatus last_status = Status;
                Status = TableStatus.Saving;
                SaveFile();
                Status = last_status;
            }
        }

        public void Fetch(Period period, CancellationTokenSource cts)
        {
            if (cts.Continue() && Enabled)
                lock (DataLockObject)
                {
                    Status = TableStatus.Downloading;

                    IsLive = period.IsCurrent;
                    //if (IsLive) Contract.Request_MarketTicks();
                    if (IsLive) Contract.MarketData.StartTicks();
                    
                    ResetCalculationPointer();
                    SyncFile(period);
                    Sort();
                    Adjust();

                    if (BarFreq == BarFreq.Daily)
                    {
                        Fetch_Daily(this, period, cts);
                        SaveFile();
                    }
                    else if (BarFreq > BarFreq.Daily)
                    {
                        Period download_time_period = new Period(Frequency.Align(period.Start, -1), Frequency.Align(period.Stop, 1));

                        using BarTable referenceTable = new BarTable(Contract, BarFreq.Daily, Type);
                        referenceTable.Load(download_time_period); // Then add the Bar to the Data Object
                        Fetch_Daily(referenceTable, download_time_period, cts); // sorted, adjusted, and saved as well // Forward Adjust, Getting the adjusted OHLC from Actual OHLC
                                                                                // Fetch_Daily will sort the reference table

                        download_time_period = new Period(Frequency.Align(LastTime, -1), Frequency.Align(referenceTable.LastTimeBound, 1));
                        Remove(download_time_period); // Remove the updating period from this table, becuase it is obsolete!! Remove the tail end

                        //ReferenceTable[download_time_period].AsParallel().ForAll(b => MergeFromSmallerBar(b));
                        referenceTable[download_time_period].ToList().ForEach(b => MergeFromSmallerBar(b));
                        AddDataSourceSegment(download_time_period, DataSource.Consolidated); // update the period segment

                        referenceTable.Save(); // Blocking the process and save...

                        Sort();
                        Adjust(false);

                        SaveFile();
                    }
                    else if (BarFreq > BarFreq.Minute && BarFreq < BarFreq.Daily) // TODO: TEST intraday BarFreq from 1 minute bars
                    {
                        MultiPeriod missing_period_list = new MultiPeriod(period);
                        foreach (Period existingPd in DataSourceSegments.Keys.Where(n => DataSourceSegments[n] <= DataSource.IB))
                        {
                            missing_period_list.Remove(existingPd);
                            Console.WriteLine(MethodBase.GetCurrentMethod().Name + "(BarFreq > BarFreq.Minute || BarFreq < BarFreq.Daily) | Already Existing: " + existingPd);
                        }

                        // Now get the missing periods from reference table
                        foreach (Period missing_period in missing_period_list)
                        {
                            Period transfer_reference_time_period = new Period(Frequency.Align(missing_period.Start, -1), Frequency.Align(missing_period.Stop, 1));

                            using BarTable referenceTable = new BarTable(Contract, BarFreq.Minute, Type);
                            referenceTable.Load(transfer_reference_time_period); // Then add the Bar to the Data Object
                            referenceTable.Sort();

                            if (referenceTable.Count > 0)
                            {
                                // Remove bar yielding partial result!!
                                DateTime first_valid_time_in_reference_table = Frequency.Align(referenceTable.FirstTime, 1);
                                DateTime last_valid_time_in_reference_table = Frequency.Align(referenceTable.LastTime, -1);

                                if (last_valid_time_in_reference_table > first_valid_time_in_reference_table)
                                {
                                    transfer_reference_time_period = new Period(first_valid_time_in_reference_table, last_valid_time_in_reference_table);
                                    Remove(transfer_reference_time_period); // Remove the updating period from this table, becuase it is obsolete!! Remove the tail end
                                    referenceTable[transfer_reference_time_period].ToList().ForEach(b => MergeFromSmallerBar(b));
                                    AddDataSourceSegment(transfer_reference_time_period, DataSource.Consolidated);
                                }
                            }
                        }

                        if (missing_period_list.Count() > 0)
                        {
                            Sort();
                            Adjust(false);
                        }

                        // Use IB to download the rest
                        Fetch_IB(this, period, cts);
                        Sort();
                        Adjust(false);
                        SaveFile();
                    }
                    else if (BarFreq <= BarFreq.Minute)
                    {
                        Fetch_IB(this, period, cts);
                        Sort();
                        Adjust(false);
                        SaveFile();
                    }

                    Status = TableStatus.LoadFinished;
                }
        }

        #endregion Operations

        #region Download / Fetch Operation

        private static bool Fetch_Daily(BarTable bt, Period period, CancellationTokenSource cts)
        {
            // The table is loaded but not sorted, with only Actual Values
            // Do not sort the table, becasuse it is going to be refreshed by quandl with only actual values later.

            bool success = false;

            DateTime quandlTime = bt.LastTimeBy(DataSource.Quandl); //period.Start;

            //if (quandlTime < bt.LastTimeBy(DataSource.Quandl)) quandlTime = bt.LastTimeBy(DataSource.Quandl);

            if (period.Stop > quandlTime) // The requested time is later than the Quandl time
            {
                bool quandl_is_available = bt.Contract.Country == "US" && bt.BarFreq == BarFreq.Daily && bt.Type == BarType.Trades && bt.Contract is Stock;

                DateTime now = DateTime.Now.Date;
                while (!bt.Contract.WorkHours.IsWorkDate(now)) now = now.AddDays(-1);
                Period download_time_period = new Period(quandlTime, now); // Get the missing part

                if (quandl_is_available && (now - quandlTime).TotalHours >= 24) // After 4:00 PM the next day.
                {
                    // If Quandl fails, please still try IB
                    success = quandl_is_available = Quandl.Download(bt, download_time_period);
                }
                else
                    Console.WriteLine(MethodBase.GetCurrentMethod().Name + "Quandl: We already have the latest data, no need to download.");

                if (success)
                {
                    bt.Save();
                }

                if (period.Start > quandlTime)
                    bt.Remove(new Period(DateTime.MinValue, period.Start.AddDays(-1))); // Trim away extra downloaded bars

                bt.Sort();
                bt.Adjust(); // With all the quandl bars loaded (or not... we still have bars loaded from the file system), we can do Sort and Forward Adjust now.

                // Load IB Daily if Quandle fails
                if (!quandl_is_available && IB.Client.Connected)
                {
                    Console.WriteLine(MethodBase.GetCurrentMethod().Name + "Quandl is not available, try getting the Daily Bars from IB!");
                    Fetch_IB(bt, period, cts);
                    bt.Sort();
                    bt.Adjust(false);
                }
            }
            else
            {
                bt.Sort();
                bt.Adjust();
            }

            return success;

            // The table is downloaded with new candles, sorted and adjusted, but not calculated
            // The problem of calculate is because of the candle stick takes a long time!
        }

        /// <summary>
        /// If a request requires more than several minutes to return data, it would be best to cancel the request using the IBApi.EClient.cancelHistoricalData function.
        /// </summary>
        /// <param name="bt"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        private static void Fetch_IB(BarTable bt, Period period, CancellationTokenSource cts)
        {
            if (bt.Contract.Status != ContractStatus.Error)
            {
                if (bt.BarFreq.GetAttribute<BarFreqInfo>() is BarFreqInfo bfi && IB.Client.Connected && IB.Client.HistoricalData_Connected) // && HistoricalData_Connected)
                {
                    Console.WriteLine(MethodBase.GetCurrentMethod().Name + " | Initial Request: " + period);

                    DateTime upToDate = bt.Contract.CurrentTime.AddMinutes(30);
                    //if (period.IsCurrent) period = new Period(period.Start, DateTime.Now.AddDays(1));
                    if (period.IsCurrent) period = new Period(period.Start, upToDate);

                    MultiPeriod missing_period_list = new MultiPeriod(period);

                    foreach (Period existingPd in bt.DataSourceSegments.Keys.Where(n => bt.DataSourceSegments[n] <= DataSource.IB))
                    {
                        missing_period_list.Remove(existingPd);
                        Console.WriteLine(MethodBase.GetCurrentMethod().Name + " | Already Existing: " + existingPd);
                    }

                    //If EarliestTime is unset, then request it here.
                    if (bt.EarliestTime == DateTime.MaxValue)
                    {
                        if (cts.Cancelled()) goto End;

                        IB.Client.Fetch_HistoricalDataHeadTimestamp(bt, cts);
                    }

                    // https://interactivebrokers.github.io/tws-api/historical_limitations.html
                    DateTime earliestTime = (bt.BarFreq < BarFreq.Minute) ? DateTime.Now.AddMonths(-6) : bt.EarliestTime;

                    if (bt.Contract.Status != ContractStatus.Error && earliestTime < DateTime.Now)
                    {
                        List<Period> api_request_pd_list = new List<Period>();

                        foreach (Period missing_period in missing_period_list)
                        {
                            if (cts.Cancelled()) goto End;

                            Console.WriteLine(MethodBase.GetCurrentMethod().Name + " | This is what we miss: " + missing_period);

                            DateTime endTimeBound = DateTime.Now.AddDays(1);

                            if (missing_period.Start < earliestTime)
                                if (missing_period.Stop > earliestTime)
                                    missing_period.SetStart(earliestTime); // Get Head time please, and reduce the period to limit
                                else
                                    continue;
                            else if (missing_period.Stop > endTimeBound)
                                if (missing_period.Start < endTimeBound)
                                    missing_period.SetStop(endTimeBound);
                                else
                                    continue;

                            api_request_pd_list.AddRange(missing_period.Split(bfi.Duration));
                        }

                        foreach (Period api_request_pd in api_request_pd_list.OrderBy(n => n.Start))
                        {
                            if (cts.Cancelled())
                                goto End;
                            else
                                Thread.Sleep(2000);

                            Console.WriteLine("\n" + MethodBase.GetCurrentMethod().Name + " | Sending Api Request: " + api_request_pd);
                            IB.Client.Fetch_HistoricalData(bt, api_request_pd, cts);
                        }
                    }
                }
            }

        End:
            return;
        }

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
                if (cts.Continue())
                {
                    foreach (var (freq, type, period) in settings_list)
                    {
                        if (cts.Continue())
                        {
                            BarTable bt = new BarTable(c, freq, type);
                            bt.Fetch(period, cts);
                            i++;
                            if (cts.Continue()) progress?.Report(100.0f * i / count);
                        }
                        else
                            return;
                    }
                }
                else
                    return;
            });
        }

        #endregion Download / Fetch Operation

        #region File Operation

        public DateTime EarliestTime => (Contract.MarketData is StockData sd) ? sd.BarTableEarliestTime : DateTime.MinValue;

        public DateTime LastDownloadRequestTime { get; set; } = DateTime.MinValue;

        public MultiPeriod<DataSource> DataSourceSegments { get; } = new MultiPeriod<DataSource>(); // => BarTableFileData.DataSourceSegments;

        public void AddDataSourceSegment(Period pd, DataSource source)
        {
            DataSourceSegments.Add(pd, source);
        }

        public DateTime GetDataSourceStartTime(DateTime endTime, DataSource source)
        {
            var res = DataSourceSegments.Where(n => n.Value <= source && n.Key.Contains(endTime));
            if (res.Count() > 0) return res.Last().Key.Start;
            else return endTime;
        }

        private BarTableFileData BarTableFileData
        {
            get
            {
                string fileName = BarTableFileData.GetFileName((Contract.Info, BarFreq, Type));

                BarTableFileData btd = Serialization.DeserializeJsonFile<BarTableFileData>(fileName);
                if (btd == this)
                    return btd;

                return new BarTableFileData(this);
            }
        }

        private void LoadFile(BarTableFileData btd, Period pd)
        {
            ResetCalculationPointer();
            TimeToRows.Clear();
            Rows.Clear();

            var bars = btd.Bars.Where(n => pd.Contains(n.Key)).OrderBy(n => n.Key);
            Range<DateTime> Invalid_Period = null;

            foreach (var pb in bars)
            {
                if (pb.Value.O < 0 || pb.Value.H < 0 || pb.Value.L < 0 || pb.Value.C < 0 || pb.Value.V < 0)
                {
                    if (Invalid_Period is null) Invalid_Period = new Range<DateTime>(pb.Key, Frequency.Align(pb.Key, 0));
                    else
                    {
                        Invalid_Period.Insert(pb.Key);
                        Invalid_Period.Insert(Frequency.Align(pb.Key, 0));
                    }
                }
                else
                {
                    if (pb.Value.SRC < DataSource.Tick)
                    {
                        Bar b = GetOrAdd(pb.Key);
                        b.Source = pb.Value.SRC;
                        b.Actual_Open = pb.Value.O;
                        b.Actual_High = pb.Value.H;
                        b.Actual_Low = pb.Value.L;
                        b.Actual_Close = pb.Value.C;
                        b.Actual_Volume = pb.Value.V;
                    }
                }
            }

            if (Invalid_Period is Range<DateTime> rgt)
            {
                btd.DataSourceSegments.Remove(new Period(rgt.Minimum, rgt.Maximum));
                btd.Bars.Where(n => rgt.Contains(n.Key)).ToList().ForEach(n => btd.Bars.Remove(n.Key));
                btd.SerializeJsonFile(BarTableFileData.GetFileName((Contract.Info, BarFreq, Type)));
            }

            DataSourceSegments.Clear();
            DataSourceSegments.Merge(btd.DataSourceSegments);
        }

        private void SaveFile()
        {
            using BarTableFileData btd = BarTableFileData;
            SaveFile(btd);
        }

        private void SaveFile(BarTableFileData btd)
        {
            if (Count > 0)
            {
                Range<DateTime> Invalid_Period = null;

                lock (Rows)
                    foreach (var b in Rows.Where(n => n.Source < DataSource.Tick))
                    {
                        if (b.Actual_Open < 0 || b.Actual_High < 0 || b.Actual_Low < 0 || b.Actual_Close < 0 || b.Actual_Volume < 0)
                        {
                            if (Invalid_Period is null) Invalid_Period = new Range<DateTime>(b.Period.Start, b.Period.Stop);
                            else
                            {
                                Invalid_Period.Insert(b.Period.Start);
                                Invalid_Period.Insert(b.Period.Stop);
                            }
                        }
                        else if (!btd.Bars.ContainsKey(b.Time) || b.Source < btd.Bars[b.Time].SRC)
                            btd.Bars[b.Time] = (b.Source, b.Actual_Open, b.Actual_High, b.Actual_Low, b.Actual_Close, b.Actual_Volume);
                    }

                btd.DataSourceSegments.Merge(DataSourceSegments);
                if (Invalid_Period is Range<DateTime> rgt) btd.DataSourceSegments.Remove(new Period(rgt.Minimum, rgt.Maximum));

                if (btd.EarliestTime < EarliestTime)
                    btd.EarliestTime = EarliestTime;

                if (btd.LastUpdateTime < LastDownloadRequestTime)
                    btd.LastUpdateTime = LastDownloadRequestTime;

                btd.SerializeJsonFile(BarTableFileData.GetFileName((Contract.Info, BarFreq, Type)));
            }
        }

        public void SyncFile(Period pd)
        {
            using BarTableFileData btd = BarTableFileData;
            SaveFile(btd);
            LoadFile(btd, pd);
        }

        public void ClearFile(Period pd)
        {
            using BarTableFileData btd = BarTableFileData;
            btd.DataSourceSegments.Remove(pd);
            var to_remove_list = btd.Bars.Where(n => pd.Contains(n.Key)).ToList();
            to_remove_list.ForEach(n => btd.Bars.Remove(n.Key));
            btd.SerializeJsonFile(BarTableFileData.GetFileName((Contract.Info, BarFreq, Type)));
            Clear();

        }

        public void ClearFile()
        {
            using BarTableFileData btd = BarTableFileData;
            btd.DataSourceSegments.Clear();
            btd.Bars.Clear();
            btd.SerializeJsonFile(BarTableFileData.GetFileName((Contract.Info, BarFreq, Type)));
            Clear();
        }



        #endregion File Operation

        /// <summary>
        /// Export the table to CSV file
        /// </summary>
        /// <param name="fileName"></param>
        /*
        public bool ExportCSV(string fileName)
        {
            lock (BarAnalysisLock)
                lock (DataObjectLock)
                {
                    Calculate();

                    StringBuilder sb = new StringBuilder("Source,Time,Open,High,Low,Close,Volume,Adj_Open,Adj_High,Adj_Low,Adj_Close,Adj_Volume");

                    var DataColumnList = BarAnalysisPointerList.Where(n => n is NumericAnalysis).Where(n => n.Enabled).OrderBy(n => n.Order);

                    foreach (NumericAnalysis bc in DataColumnList)
                    {
                        string p = bc.Name;
                        if (p.Contains(",")) p = "\"" + p + "\"";
                        sb.Append("," + p);
                    }
                    sb.Append("\n");

                    for (int i = LastIndex; i >= 0; i--)
                    {
                        sb.Append(this[i].Source + "," +
                            this[i].Time.ToString() + "," +
                            this[i].Actual_Open.ToString() + "," +
                            this[i].Actual_High.ToString() + "," +
                            this[i].Actual_Low.ToString() + "," +
                            this[i].Actual_Close.ToString() + "," +
                            this[i].Actual_Volume.ToString() + "," +
                            this[i].Open.ToString() + "," +
                            this[i].High.ToString() + "," +
                            this[i].Low.ToString() + "," +
                            this[i].Close.ToString() + "," +
                            this[i].Volume.ToString());

                        foreach (NumericAnalysis bc in DataColumnList)
                        {
                            sb.Append("," + this[i][bc]);
                        }

                        sb.Append("\n");
                    }

                    return sb.ToFile(fileName);
                }
        }*/

        #region Equality

        public bool Equals(BarTable other) => Info == other.Info;
        public bool Equals((Contract c, BarFreq barFreq, BarType type) other) => Info == other;

        public static bool operator ==(BarTable s1, BarTable s2) => s1.Equals(s2);
        public static bool operator !=(BarTable s1, BarTable s2) => !s1.Equals(s2);
        public static bool operator ==(BarTable s1, (Contract c, BarFreq barFreq, BarType type) s2) => s1.Equals(s2);
        public static bool operator !=(BarTable s1, (Contract c, BarFreq barFreq, BarType type) s2) => !s1.Equals(s2);

        public override bool Equals(object other)
        {
            if (this is null || other is null) // https://stackoverflow.com/questions/4219261/overriding-operator-how-to-compare-to-null
                return false;
            else if (other is BarTable bt)
                return Equals(bt);
            else if (other.GetType() == typeof((Contract c, BarFreq barFreq, BarType type)))
                return Equals(((Contract c, BarFreq barFreq, BarType type))other);
            else
                return false;
        }

        public override int GetHashCode() => Info.GetHashCode();

        #endregion Equality
    }
}
