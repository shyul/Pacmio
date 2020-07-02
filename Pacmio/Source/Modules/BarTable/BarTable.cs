/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// BarTable Data Types
/// 
/// ***************************************************************************

using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using Xu;

namespace Pacmio
{
    /// <summary>
    /// BarTable: the ultimate data holder for technical analysis with fundamental awareness
    /// </summary>
    public sealed class BarTable : ITagTable, IEquatable<BarTable>,
        IEquatable<(Contract c, BarFreq barFreq, BarType type)>, IDisposable
    {
        #region Ctor

        public BarTable(Contract c, BarFreq barFreq, BarType type) // = TickerType.Trades
        {
            Contract = c;
            BarFreq = barFreq;
            Frequency = BarFreq.GetAttribute<BarFreqInfo>().Result.Frequency;
            Type = type;
        }

        public void Dispose()
        {
            IsLive = false;
            if (Contract.MarketData is HistoricalData hd) { hd.LiveBarTables.CheckRemove(this); }
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
        private readonly List<Bar> Rows = new List<Bar>();

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
        /// <param name="length"></param>
        /// <returns></returns>
        public IEnumerable<Bar> this[int i, int length]
        {
            get
            {
                int len = length - 1;
                if (i < len) len = i;
                return Rows.Skip(i - len).Take(len + 1).Reverse();
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

        public (string description, double score) this[int i, SignalColumn column]
        {
            get
            {
                string description = string.Empty;
                double score = 0;

                for (int j = 0; j < column.MaxEffectLength; j++)
                {
                    int k = i - j;
                    if (k >= 0 && k < Count)
                    {
                        var sd = Rows[k][column];
                        if (sd is SignalDatum sdd)
                        {
                            if (string.IsNullOrEmpty(description)) description = sdd.Description;
                            score += sdd.Scores[j];
                        }
                    }
                }

                return (description, score);
            }
        }

        public (double bullish, double bearish) SignalScore(int i)
        {
            double bull = 0, bear = 0;
            foreach (SignalColumn sc in SignalColumns)
            {
                var (_, score) = this[i, sc];
                if (score > 0) bull += score;
                else if (score < 0) bear += score;
            }
            return (bull, bear);
        }

        public readonly List<SignalColumn> SignalColumns = new List<SignalColumn>();

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
        public int LastIndex => Count - 1;

        /// <summary>
        /// Returns the Last Bar in the Table. Null is the BarTable is empty.
        /// </summary>
        public Bar LastBar
        {
            get
            {
                if (IsEmpty)
                    return null;
                else
                    return this[LastIndex];
            }
        }

        public Bar LastBar_1
        {
            get
            {
                if (LastIndex < 1)
                    return null;
                else
                    return this[LastIndex - 1];
            }
        }

        /// <summary>
        /// Last most Close
        /// </summary>
        public double LastClose => (LastBar is null) ? -1 : LastBar.Close;

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

        public void Add(BarData bd)
        {
            if (bd.Span == Frequency.Span)
            {
                Bar b = GetOrAdd(bd.Time);
                if (b.Source >= bd.Source)
                {
                    b.Source = bd.Source;
                    if (bd.IsAdjusted)
                    {
                        b.Open = bd.Open;
                        b.High = bd.High;
                        b.Low = bd.Low;
                        b.Close = bd.Close;
                        b.Volume = bd.Volume;
                    }
                    else
                    {
                        b.Actual_Open = bd.Open;
                        b.Actual_High = bd.High;
                        b.Actual_Low = bd.Low;
                        b.Actual_Close = bd.Close;
                        b.Actual_Volume = bd.Volume;
                    }
                }
            }
        }

        private bool Add(DateTime tickTime, double last, double volume)
        {
            bool isModified = false;
            DateTime time = Frequency.Align(tickTime, 0);

            if (!Contains(time))
            {
                if (Count > 0 && LastBar.Source == DataSource.Tick)
                    LastBar.Source = DataSource.Realtime;

                Bar nb = new Bar(this, time, DataSource.Tick,
                            last, last, last, last, volume, // - 1, -1, -1, -1, -1, // Ticks are actual trade values
                            last, last, last, last, volume)
                {
                    DataSourcePeriod = new Period(time) // Make sure it knows the Bar data sample time is shorter than the BarSize
                };

                isModified = Add(nb);
            }
            else
            {
                Bar nb = this[time];

                if (nb.Source >= DataSource.Realtime)
                {
                    if (last > nb.High) // New High
                    {
                        nb.Actual_High = nb.High = last; // Also update 
                        isModified = true;
                    }

                    if (last < nb.Low) // New Low
                    {
                        nb.Actual_Low = nb.Low = last;
                        isModified = true;
                    }

                    if (tickTime <= nb.DataSourcePeriod.Start) // Eariler Open
                    {
                        nb.Actual_Open = nb.Open = last;
                        nb.DataSourcePeriod.Insert(tickTime);
                        isModified = true;
                    }

                    if (tickTime >= nb.DataSourcePeriod.Stop) // Later Close
                    {
                        nb.Actual_Close = nb.Close = last;
                        nb.DataSourcePeriod.Insert(tickTime);
                        isModified = true;
                    }

                    nb.Volume += volume;
                    nb.Actual_Volume = nb.Volume;

                    nb.Source = DataSource.Realtime;
                }
            }

            return isModified;
        }

        private bool MergeFromSmallerBar(Bar b)
        {
            bool isModified = false;
            if (b.BarFreq < BarFreq)
            {
                // Get time and index of time
                DateTime time = Frequency.Align(b.Time, 0);
                if (!Contains(time))
                {
                    Bar nb = new Bar(this, time, b.Source,
                                -1, -1, -1, -1, -1,
                                b.Open, b.High, b.Low, b.Close, b.Volume)
                    {
                        DataSourcePeriod = b.Period // Make sure it knows the Bar data sample time is shorter than the BarSize
                    };
                    isModified = Add(nb);
                }
                else
                {
                    Bar nb = this[time];
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
            }
            return isModified;
        }

        #endregion Bars Properties and Methods

        #region Time

        private readonly Dictionary<DateTime, int> TimeToRows = new Dictionary<DateTime, int>();

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
        /// Returns current BarTable's maximum time span
        /// </summary>
        public Period Period => new Period(FirstTime, LastTimeBound);

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
            if (Contract.MarketData is HistoricalData sd)
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

        /// <summary>
        /// Valid data start pointer
        /// </summary>
        private int BasicData_StartPt { get; set; } = 0;

        /// <summary>
        /// Pointer for calculated data end
        /// </summary>
        private int BasicData_StopPt { get; set; } = 0;

        private void BasicData_Calculate()
        {
            double high_1, low_1, close_1, trend_1;

            int min_peak_start = BasicData_StopPt - MaximumPeakProminence * 2 - 1;
            if (BasicData_StartPt > min_peak_start) BasicData_StartPt = min_peak_start;

            //if (Gain_StartPt < 0) return;

            // Define the bondary condition
            if (BasicData_StartPt < 1)
            {
                if (BasicData_StartPt < 0) BasicData_StartPt = 0;
                //open_1 = this[0].Open;
                high_1 = this[0].High;
                low_1 = this[0].Low;
                close_1 = this[0].Close;
                trend_1 = this[0].TrendStrength = 0;
            }
            else
            {
                Bar b_1 = this[BasicData_StartPt - 1];
                //open_1 = b_1.Open;
                high_1 = b_1.High;
                low_1 = b_1.Low;
                close_1 = b_1.Close;
                trend_1 = b_1.TrendStrength;
            }

            for (int i = BasicData_StartPt; i < BasicData_StopPt; i++)
            {
                Bar b = this[i];

                double open = b.Open;
                double high = b.High;
                double low = b.Low;
                double close = b.Close;

                // Get Gain
                double gain = b.Gain = close - close_1;
                b.Percent = (close_1 == 0) ? 0 : (100 * gain / close_1);

                double gap = b.Gap = open - close_1;
                b.GapPercent = (close_1 == 0) ? 0 : (100 * gap / close_1);

                // Get Trend
                double[] list = new double[] { (high - low), Math.Abs(high - close_1), Math.Abs(low - close_1) };
                b.TrueRange = list.Max();
                b.Typical = (high + low + close) / 3.0;

                double trend = 0;
                if (gain > 0 || (high > high_1 && low > low_1))
                {
                    trend = (trend_1 > 0) ? trend_1 + 1 : 1;
                }
                else if (gain < 0 || (high < high_1 && low < low_1))
                {
                    trend = (trend_1 < 0) ? trend_1 - 1 : -1;
                }

                // Get Peak and Troughs
                int peak_result = 0;
                int j = 1;
                bool test_high = true, test_low = true;

                while (j < MaximumPeakProminence)
                {
                    if ((!test_high) && (!test_low)) break;

                    int right_index = i + j;
                    if (right_index >= BasicData_StopPt) break; // right_index = Gain_StopPt - 1;

                    int left_index = i - j;
                    if (left_index < 0) break; // left_index = 0;

                    if (test_high)
                    {
                        //double high_data = bt[i][High_Column];
                        double left_high = this[left_index].High;
                        double right_high = this[right_index].High;

                        if (high >= left_high && high >= right_high)
                        {
                            peak_result = j;
                            if (high == left_high) this[left_index].Peak = 0;
                        }
                        else
                            test_high = false;
                    }

                    if (test_low)
                    {
                        //double low_data = bt[i][Low_Column];
                        double left_low = this[left_index].Low;
                        double right_low = this[right_index].Low;

                        if (low <= left_low && low <= right_low)
                        {
                            peak_result = -j;
                            if (low == left_low) this[left_index].Peak = 0;
                        }
                        else
                            test_low = false;
                    }
                    j++;
                }

                b.Peak = peak_result;

                if (peak_result > MinimumTagPeakProminence)
                {
                    b.PeakTag = new TagInfo(i, close.ToString("G5"), DockStyle.Top, Upper_TextTheme);
                }
                else if (peak_result < -MinimumTagPeakProminence)
                {
                    b.PeakTag = new TagInfo(i, close.ToString("G5"), DockStyle.Bottom, Lower_TextTheme);
                }

                b.TrendStrength = trend_1 = trend;
                high_1 = high;
                low_1 = low;
                close_1 = close;
            }
        }

        public int MaximumPeakProminence
        {
            get
            {
                return m_MaximumPeakProminence;
            }
            set
            {
                if (m_MaximumPeakProminence != value)
                {
                    ResetCalculationPointer();
                    m_MaximumPeakProminence = value;
                }
            }
        }
        private int m_MaximumPeakProminence = 100;

        public int MinimumTagPeakProminence { get; set; } = 5;

        #endregion Basic Data

        #region Data/Bar Analysis (TA) Calculation

        private readonly Dictionary<BarAnalysis, BarAnalysisPointer> BarAnalysisPointerList = new Dictionary<BarAnalysis, BarAnalysisPointer>();

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

            if (BasicData_StartPt > pt)
                BasicData_StartPt = pt;

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

        public int LatestCalculatePointer { get; private set; } = 0;

        /// <summary>
        /// The mighty calculate for all technicial analysis
        /// </summary>
        private void Calculate(IEnumerable<BarAnalysis> analyses)
        {
            Console.WriteLine("\n==================");
            Console.WriteLine("Table: " + Name + " | Count: " + Count);
            DateTime total_time = DateTime.Now;

            int startPt = BasicData_StopPt = Count;
            if (Count > 0)
            {
                if (BasicData_StartPt < 0) BasicData_StartPt = 0;
                BasicData_Calculate();
                BasicData_StartPt = BasicData_StopPt;

                foreach (BarAnalysis ba in analyses) //BarAnalysisPointerList.Keys)
                {
                    DateTime single_time = DateTime.Now;

                    BarAnalysisPointer bap = GetBarAnalysisPointer(ba);
                    int original_start = bap.StartPt;
                    int original_stop = bap.StopPt;
                    ba.Update(bap);
                    startPt = Math.Min(startPt, bap.StartPt);

                    Console.WriteLine(ba.Name + " | (" + original_start + "->" + bap.StartPt + ") | Time " + (DateTime.Now - single_time).TotalMilliseconds.ToString() + "ms");
                }
            }
            LatestCalculatePointer = startPt;

            Console.WriteLine("------------------");
            Console.WriteLine(Name + " | Calculate(): " + (DateTime.Now - total_time).TotalMilliseconds.ToString() + "ms" + " | Stopped at: " + LatestCalculatePointer);
            Console.WriteLine("==================\n");
        }

        #endregion Data/Bar Analysis (TA) Calculation

        #region Position / Simulation Information

        public ITradeSetting CurrentTradeSetting { get; set; }

        #endregion Position / Simulation Information

        #region BarChart / DataView

        public readonly List<IDataView> DataViews = new List<IDataView>();

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

                if (Contract.MarketData is HistoricalData hd)
                    if (m_IsLive)
                    {
                        // Add BarTable to the tick receiver
                        hd.LiveBarTables.CheckAdd(this);
                    }
                    else
                    {
                        // Remove BarTable from the tick receiver
                        hd.LiveBarTables.CheckRemove(this);
                    }
            }
        }

        private bool m_IsLive = false;

        public bool ReadyToShow => Count > 0 && (Status == TableStatus.Ready || Status == TableStatus.CalculateFinished || Status == TableStatus.TickingFinished);

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
                else
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

                    SyncFile(period);
                    Sort();
                    Adjust(); // Forward Adjust

                    Status = TableStatus.LoadFinished;
                    // lead it to calculation... here
                }
        }

        public void CalculateOnly(BarAnalysisSet bas)
        {
            if (Enabled)
                lock (DataLockObject)
                {
                    Status = TableStatus.Calculating;
                    // Send Signal

                    Calculate(bas.List);

                    Status = TableStatus.CalculateFinished;
                    Status = TableStatus.Ready;
                    // Send Signal
                }
        }

        public void AddPriceTick(MarketTick mt)
        {
            if (Enabled && IsLive)
                lock (DataLockObject)
                {
                    TableStatus last_status = Status;
                    Status = TableStatus.Ticking;
                    Add(mt.Time, mt.Price, mt.Size);

                    if (last_status == TableStatus.Ready)
                    {
                        SetCalculationPointer(LatestCalculatePointer - 2);
                        Calculate(BarAnalysisPointerList.Keys);
                        Status = TableStatus.TickingFinished;
                        // lead it to calculation... here
                    }

                    Status = last_status;
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
            if (Enabled)
                lock (DataLockObject)
                {
                    Status = TableStatus.Downloading;

                    ResetCalculationPointer();
                    SyncFile(period);

                    if (BarFreq == BarFreq.Daily)
                    {
                        Fetch_Daily(this, period, cts);
                    }
                    else if (BarFreq > BarFreq.Daily)
                    {
                        Sort();
                        Adjust();

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
                    else if (BarFreq > BarFreq.Minute || BarFreq < BarFreq.Daily) // TODO: TEST intraday BarFreq from 1 minute bars
                    {
                        Sort();
                        Adjust();

                        MultiPeriod missing_period_list = new MultiPeriod(period);
                        foreach (Period existingPd in DataSourceSegments.Keys.Where(n => DataSourceSegments[n] <= DataSource.IB))
                        {
                            missing_period_list.Remove(existingPd);
                            Console.WriteLine("RequestHistoricalData | Already Existing: " + existingPd);
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
                        if (Fetch_IB(this, period, cts))
                        {
                            Sort();
                            Adjust(false);
                        }

                        SaveFile();
                    }
                    else if (BarFreq <= BarFreq.Minute)
                    {
                        Sort();
                        Adjust();
                        if (Fetch_IB(this, period, cts))
                        {
                            Sort();
                            Adjust(false);
                        }
                    }

                    Status = TableStatus.LoadFinished;
                }
        }

        #endregion Operations

        #region Download

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
                    Console.WriteLine("Quandl: We already have the latest data, no need to download.");

                if (success)
                {
                    bt.Save();
                }

                if (period.Start > quandlTime)
                    bt.Remove(new Period(DateTime.MinValue, period.Start.AddDays(-1))); // Trim away extra downloaded bars

                bt.Sort();
                bt.Adjust(); // With all the quandl bars loaded (or not... we still have bars loaded from the file system), we can do Sort and Forward Adjust now.

                // Load IB Daily if Quandle fails
                if (!quandl_is_available && Root.IBConnected)
                {
                    Console.WriteLine("Quandl is not available, try getting the Daily Bars from IB!");
                    success = Fetch_IB(bt, period, cts);
                    if (success)
                    {
                        bt.Sort();
                        bt.Adjust(false);
                    }
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
        private static bool Fetch_IB(BarTable bt, Period period, CancellationTokenSource cts)
        {
            //int time = 0;

            bool isModified = false;

            var (bfi_valid, bfi) = bt.BarFreq.GetAttribute<BarFreqInfo>();

            if (bfi_valid && Root.IBConnected && IB.Client.HistoricalData_Connected) // && HistoricalData_Connected)
            {
                Console.WriteLine("RequestHistoricalData | Initial Request: " + period);

                DateTime upToDate = bt.Contract.CurrentTime.AddMinutes(30);
                //if (period.IsCurrent) period = new Period(period.Start, DateTime.Now.AddDays(1));
                if (period.IsCurrent) period = new Period(period.Start, upToDate);

                MultiPeriod missing_period_list = new MultiPeriod(period);

                foreach (Period existingPd in bt.DataSourceSegments.Keys.Where(n => bt.DataSourceSegments[n] <= DataSource.IB))
                {
                    missing_period_list.Remove(existingPd);
                    Console.WriteLine("RequestHistoricalData | Already Existing: " + existingPd);
                }

                //If EarliestTime is unset, then request it here.
                if (bt.EarliestTime == DateTime.MinValue)
                {
                    if (cts is CancellationTokenSource cs && cs.IsCancellationRequested)
                        goto End;

                    IB.Client.Fetch_HistoricalDataHeadTimestamp(bt, cts);
                }

                // https://interactivebrokers.github.io/tws-api/historical_limitations.html
                DateTime earliestTime = (bt.BarFreq < BarFreq.Minute) ? DateTime.Now.AddMonths(-6) : bt.EarliestTime;
                List<Period> api_request_pd_list = new List<Period>();

                foreach (Period missing_period in missing_period_list)
                {
                    Console.WriteLine("RequestHistoricalData | This is what we miss: " + missing_period);

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
                    if (cts is CancellationTokenSource cs && cs.IsCancellationRequested)
                        goto End;
                    else
                        Thread.Sleep(2000);

                    Console.WriteLine("RequestHistoricalData: | Sending Api Request: " + api_request_pd);
                    IB.Client.Fetch_HistoricalData(bt, api_request_pd, cts);
                }
            }

        End:
            return isModified;
        }

        #endregion Download

        #region File Operation

        public DateTime EarliestTime => (Contract.MarketData is HistoricalData sd) ? sd.BarTableEarliestTime : DateTime.MinValue;

        public DateTime LastDownloadRequestTime { get; set; } = DateTime.MinValue;

        public MultiPeriod<DataSource> DataSourceSegments { get; } = new MultiPeriod<DataSource>(); // => BarTableFileData.DataSourceSegments;

        //public ICollection<Period> DataSourcePeriods => DataSourceSegments.Keys;

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

                if (File.Exists(fileName))
                {
                    BarTableFileData btd = Serialization.DeserializeJsonFile<BarTableFileData>(fileName);
                    if (btd == this) return btd;
                }

                return new BarTableFileData(this);
            }
        }
        /*
        private void LoadFile(Period pd)
        {
            using BarTableFileData btd = BarTableFileData;
            LoadFile(btd, pd);
        }*/

        private void LoadFile(BarTableFileData btd, Period pd)
        {
            DataSourceSegments.Clear();
            DataSourceSegments.Merge(btd.DataSourceSegments);

            IsLive = pd.IsCurrent;
            ResetCalculationPointer();
            Rows.Clear();
            TimeToRows.Clear();

            var bars = btd.Bars.Where(n => pd.Contains(n.Key)).OrderBy(n => n.Key);

            foreach (var pb in bars)
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

        private void SaveFile()
        {
            using BarTableFileData btd = BarTableFileData;
            SaveFile(btd);
        }

        private void SaveFile(BarTableFileData btd)
        {
            btd.DataSourceSegments.Merge(DataSourceSegments);

            foreach (var b in Rows.Where(n => n.Source < DataSource.Tick))
                if (!btd.Bars.ContainsKey(b.Time) || b.Source < btd.Bars[b.Time].SRC)
                    btd.Bars[b.Time] = (b.Source, b.Actual_Open, b.Actual_High, b.Actual_Low, b.Actual_Close, b.Actual_Volume);

            if (btd.EarliestTime < EarliestTime)
                btd.EarliestTime = EarliestTime;

            if (btd.LastUpdateTime < LastDownloadRequestTime)
                btd.LastUpdateTime = LastDownloadRequestTime;

            btd.SerializeJsonFile(BarTableFileData.GetFileName((Contract.Info, BarFreq, Type)));
        }

        private void SyncFile(Period pd)
        {
            using BarTableFileData btd = BarTableFileData;
            SaveFile(btd);
            LoadFile(btd, pd);
        }

        #endregion File Operation

        #region Download / Fetch Operation




        #endregion Download / Fetch Operation

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

        public static readonly ColorTheme Upper_Theme = new ColorTheme();

        public static readonly ColorTheme Upper_TextTheme = new ColorTheme();

        public static readonly ColorTheme Lower_Theme = new ColorTheme();

        public static readonly ColorTheme Lower_TextTheme = new ColorTheme();

        public static Color UpperColor
        {
            get
            {
                return Upper_Theme.ForeColor;
            }
            set
            {
                Upper_Theme.ForeColor = value;

                Upper_TextTheme.EdgeColor = value.Opaque(255);
                Upper_TextTheme.FillColor = Upper_TextTheme.EdgeColor.GetBrightness() < 0.6 ? Upper_TextTheme.EdgeColor.Brightness(0.85f) : Upper_TextTheme.EdgeColor.Brightness(-0.85f);
                Upper_TextTheme.ForeColor = Upper_TextTheme.EdgeColor;
            }
        }

        public static Color LowerColor
        {
            get
            {
                return Lower_Theme.ForeColor;
            }
            set
            {
                Lower_Theme.ForeColor = value;

                Lower_TextTheme.EdgeColor = value.Opaque(255);
                Lower_TextTheme.FillColor = Lower_TextTheme.EdgeColor.GetBrightness() < 0.6 ? Lower_TextTheme.EdgeColor.Brightness(0.85f) : Lower_TextTheme.EdgeColor.Brightness(-0.85f);
                Lower_TextTheme.ForeColor = Lower_TextTheme.EdgeColor;
            }
        }

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
