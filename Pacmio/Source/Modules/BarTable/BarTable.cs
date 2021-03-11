/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
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
    public sealed class BarTable : ITagTable, IDataProvider, IDataConsumer, IEquatable<BarTable>,
        IEquatable<(Contract, BarFreq, BarType)>,
        IEquatable<((string name, Exchange exchange, string typeName) ContractKey, BarFreq BarFreq, BarType Type)>
    {
        #region Ctor

        public BarTable(Contract c, BarFreq barFreq, BarType type)
        {
            Contract = c;
            BarFreq = barFreq;
            Frequency = BarFreq.GetAttribute<BarFreqInfo>().Frequency;
            Type = type;
            //FundamentalData = Contract.GetOrCreateFundamentalData();

            CalculateTickCancelTs = new CancellationTokenSource();
            CalculateTickTask = new Task(() => CalculateTickWorker(), CalculateTickCancelTs.Token);
            CalculateTickTask.Start();
        }

        ~BarTable() => Dispose();

        public void Dispose()
        {
            lock (DataConsumers)
            {
                foreach (IDataRenderer idr in DataConsumers)
                {
                    RemoveDataConsumer(idr);
                    if (idr is BarChart bc)
                        bc.Close();
                    else
                        RemoveDataConsumer(idr);
                }
            }

            IsLive = false;
            Contract.MarketData.RemoveDataConsumer(this);
            CalculateTickCancelTs.Cancel();
            Clear();
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

        public ((string name, Exchange exchange, string typeName) ContractKey, BarFreq barFreq, BarType type) Key => (Contract.Key, BarFreq, Type);

        #endregion Ctor

        #region Data

        /// <summary>
        /// For Multi Thread Access
        /// </summary>
        public object DataLockObject { get; } = new object();

        /// <summary>
        /// The Rows Data Storage
        /// The Storage is not directly accessible outside of the class.
        /// </summary>
        private List<Bar> Rows { get; } = new List<Bar>();

        private Dictionary<DateTime, int> TimeToRows { get; } = new Dictionary<DateTime, int>();

        /// <summary>
        /// Returns the number of the Rows in the BarTable.
        /// </summary>
        public int Count
        {
            get
            {
                lock (DataLockObject)
                {
                    return Rows.Count;
                }
            }
        }

        /// <summary>
        /// Returns if the BarTable is has no Bars.
        /// </summary>
        public bool IsEmpty => (Count < 1);

        /// <summary>
        /// Returns Last Row's Index
        /// </summary>
        private int LastIndex => Count - 1;

        #region Time

        /// <summary>
        /// Returns if the Rows already has time stamp
        /// </summary>
        public bool Contains(DateTime time) //=> Rows.Where(n => n.Time == time).Count() > 0;
        {
            lock (DataLockObject)
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
                lock (DataLockObject)
                {
                    return Count > 0 ? this[0].Time : DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Returns Last Row's time
        /// </summary>
        public DateTime LastTime
        {
            get
            {
                lock (DataLockObject)
                {
                    return Count > 0 ? TimeToRows.Last().Key : DateTime.MinValue;
                }
            }
        }

        /// <summary>
        /// Last Most time including the Bar Period
        /// </summary>
        public DateTime LastTimeBound
        {
            get
            {
                lock (DataLockObject)
                {
                    return Count > 0 ? Rows.Last().DataSourcePeriod.Stop : DateTime.MinValue;
                }
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
        /// Last Time by specific data source
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public DateTime LastTimeBy(DataSourceType source)
        {
            lock (DataLockObject)
            {
                var res = Rows.Where(n => n.Source <= source).OrderBy(n => n.Time);
                return (res.Count() > 0) ? res.Last().Time : DateTime.MinValue;
            }
        }

        public DateTime IndexToTime(int i)
        {
            lock (DataLockObject)
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
        }

        /// <summary>
        /// Get the Index from a given time
        /// </summary>
        /// <param name="time">time is rectified to the index found in "M_Rows"</param>
        /// <returns>Index of the nearest time</returns>
        public int IndexOf(ref DateTime time)
        {
            lock (DataLockObject)
            {
                if (Count > 0)
                {
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
                    else if (Rows.Where(n => n.Period.Contains(t)).FirstOrDefault() is Bar b)
                    {
                        time = b.Time;
                        return b.Index;
                    }
                }
            }

            return -1;
        }

        #endregion Time

        #endregion Data

        #region Load Bars

        public void Clear()
        {
            Status = TableStatus.Default;

            lock (DataLockObject)
            {
                TimeToRows.Clear();
                Rows.Clear();
                ResetCalculationPointer();
            }
        }

        public void LoadBars(List<Bar> sorted_bars)
        {
            if (sorted_bars.Count > 0)
            {
                if (this != sorted_bars.FirstOrDefault().Table)
                    throw new Exception("bar's table has to match with this table!");

                Status = TableStatus.Default;

                lock (DataLockObject)
                {
                    TimeToRows.Clear();
                    Rows.Clear();
                    ResetCalculationPointer();

                    for (int i = 0; i < sorted_bars.Count; i++)
                    {
                        Bar b = sorted_bars[i];
                        b.Index = i;
                        Rows.Add(b);
                        TimeToRows.Add(b.Time, i);
                    }
                }

                Status = TableStatus.Ready;
            }
        }

        public void LoadFromSmallerBar(List<Bar> sorted_bars)
        {
            if (sorted_bars.Count > 0)
            {
                BarTable bt = sorted_bars.FirstOrDefault().Table;
                if (Contract != bt.Contract || Type != bt.Type)
                    throw new Exception("bar's table has to match with this table!");

                Status = TableStatus.Default;

                lock (DataLockObject)
                {
                    TimeToRows.Clear();
                    Rows.Clear();
                    ResetCalculationPointer();

                    int j = 0;
                    for (int i = 0; i < sorted_bars.Count; i++)
                    {
                        Bar sb = sorted_bars[i];

                        if (this[sb.Time] is Bar b)
                        {
                            b.MergeFromSmallerBar(sb);
                        }
                        else
                        {
                            Bar nb = new Bar(this, sb);
                            nb.Index = j;
                            Rows.Add(nb);
                            TimeToRows.Add(nb.Time, nb.Index);
                            j++;
                        }
                    }
                }

                Status = TableStatus.Ready;
            }
        }

        public void MergeFromSmallerBar(Bar sb)
        {
            lock (DataLockObject)
            {
                if (this[sb.Time] is Bar b)
                {
                    b.MergeFromSmallerBar(sb);
                }
                else
                {
                    Bar nb = new Bar(this, sb);
                    nb.Index = Count;
                    Rows.Add(nb);
                    TimeToRows.Add(nb.Time, nb.Index);
                }
            }
        }

        #endregion Load Bars

        #region Access Bars

        /// <summary>
        /// Lookup Bar by Index. Mostly used in the Chart.
        /// </summary>
        /// <param name="i">Index of the Bar in the Rows</param>
        /// <returns>Bar according to the given index</returns>
        public Bar this[int i]
        {
            get
            {
                //lock (DataLockObject)
                //{
                return i >= Count || i < 0 ? null : Rows[i];
                //}
            }
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
                time = Frequency.Align(time);
                lock (DataLockObject)
                {
                    if (TimeToRows.ContainsKey(time))
                        return Rows[TimeToRows[time]];
                    else
                        return null; // Rows.Where(n => n.Period.Contains(time)).FirstOrDefault();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start">Start and included index</param>
        /// <param name="stop">End and not included index</param>
        /// <returns></returns>
        public List<Bar> this[int start, int stop]
        {
            get
            {
                lock (DataLockObject)
                {
                    if (stop > Count) stop = Count;
                    int cnt = stop - start;

                    if (cnt > 0)
                    {
                        return Rows.Skip(start).Take(cnt).ToList();
                    }
                    return
                        null;
                }
            }
        }

        public List<Bar> this[Period pd]
        {
            get
            {
                lock (DataLockObject)
                {
                    return Rows.Where(n => pd.Contains(n.Time)).OrderBy(n => n.Time).ToList();
                }
            }
        }

        public double this[int i, NumericColumn column] => this[i] is Bar b ? b[column] : double.NaN;

        public TagInfo this[int i, TagColumn column] => this[i] is Bar b ? b[column] : null;

        #endregion Access Bars

        #region Add Ticks

        public void AddPriceTick(DateTime tickTime, double last, double volume, DataSourceType minimumSource = DataSourceType.IB)
        {
            DateTime time = Frequency.Align(tickTime);

            lock (DataLockObject)
            {
                if (this[time] is Bar b)
                {
                    if (b.Source >= minimumSource)
                    {
                        if (last > b.High) // New High
                        {
                            b.High = last; // Also update 
                        }

                        if (last < b.Low) // New Low
                        {
                            b.Low = last;
                        }

                        if (tickTime <= b.DataSourcePeriod.Start && tickTime >= b.Period.Start) // Eariler Open
                        {
                            b.Open = last;
                            b.DataSourcePeriod.Insert(tickTime);
                        }

                        if (tickTime >= b.DataSourcePeriod.Stop && tickTime < b.Period.Stop) // Later Close
                        {
                            b.Close = last;
                            b.DataSourcePeriod.Insert(tickTime);
                            Console.WriteLine("Added inbound tick[ " + b.Source + " | " + tickTime + " | " + b.DataSourcePeriod.Start + " -> " + b.DataSourcePeriod.Stop + ", IsCurrent = " + b.DataSourcePeriod.IsCurrent + " | " + b.Period + "] to existing bar: " + b.DataSourcePeriod + " | " + b.Period);
                        }
                        else
                        {
                            Console.WriteLine("********** Inbound Tick Time Overflow ***********");
                        }

                        b.Volume += volume;
                        b.Source = DataSourceType.Realtime;
                    }
                    else
                    {
                        Console.WriteLine("###### Inbound Tick Ignored, because source = " + b.Source);
                    }
                }
                else
                {
                    if (Count > 0 && this[LastIndex] is Bar lb && lb.Source >= DataSourceType.IB)
                    {
                        lb.Source = DataSourceType.Realtime;
                    }
                    Bar nb = new Bar(this, tickTime, last, volume);
                    nb.Index = Count;
                    Rows.Add(nb);
                    TimeToRows.Add(nb.Time, nb.Index);
                }
            }

            if (Status == TableStatus.Ready)
            {
                Status = TableStatus.Ticking;
                SetCalculationPointer(LastCalculateIndex - 1);
                CalculateTickRequested = true;
            }
        }

        #endregion Add Ticks

        #region Data/Bar Analysis (TA) Calculation

        #region Basic Analysis

        public static GainAnalysis GainAnalysis { get; } = new GainAnalysis(); // { ChartEnabled = false };

        public static TrueRange TrueRangeAnalysis { get; } = new TrueRange(); // { ChartEnabled = false };

        public static TrendStrength TrendStrengthAnalysis { get; } = new TrendStrength(); // { ChartEnabled = false };

        public static PivotPointAnalysis PivotPointAnalysis { get; } = new PivotPointAnalysis(); // { ChartEnabled = false };

        #endregion Basic Analysis

        private Dictionary<BarAnalysis, BarAnalysisPointer> BarAnalysisPointerList { get; } = new Dictionary<BarAnalysis, BarAnalysisPointer>();

        public int LastCalculateIndex { get; private set; } = -1;

        /// <summary>
        /// Last most Close
        /// </summary>
        public double LastClose => (LastBar is null) ? -1 : LastBar.Close;

        /// <summary>
        /// Returns the Last Bar in the Table. Null is the BarTable is empty.
        /// </summary>
        public Bar LastBar => LastCalculateIndex < 0 ? null : this[LastCalculateIndex];

        public Bar LastBar_1 => LastCalculateIndex < 1 ? null : this[LastCalculateIndex - 1];

        private BarAnalysisPointer GetBarAnalysisPointer(BarAnalysis ba)
        {
            lock (BarAnalysisPointerList)
            {
                if (!BarAnalysisPointerList.ContainsKey(ba))
                    BarAnalysisPointerList.Add(ba, new BarAnalysisPointer(this, ba));

                return BarAnalysisPointerList[ba];
            }
        }

        private void ResetCalculationPointer()
        {
            lock (BarAnalysisPointerList)
            {
                BarAnalysisPointerList.Clear();
            }
        }

        private void SetCalculationPointer(int pt)
        {
            if (pt < 0)
                pt = 0;

            lock (BarAnalysisPointerList)
            {
                foreach (BarAnalysisPointer bap in BarAnalysisPointerList.Values)
                    if (bap.StartPt > pt)
                        bap.StartPt = pt;
            }
        }

        public void SetCalculationPointer(ref DateTime time)
        {
            int pt = IndexOf(ref time) - 1;
            SetCalculationPointer(pt);
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

            DateTime total_start_time = DateTime.Now;

            int startPt = Count;

            if (Count > 0)
            {
                startPt = Math.Min(startPt, Calculate(GainAnalysis).StartPt);
                startPt = Math.Min(startPt, Calculate(TrueRangeAnalysis).StartPt);
                startPt = Math.Min(startPt, Calculate(TrendStrengthAnalysis).StartPt);
                startPt = Math.Min(startPt, Calculate(PivotPointAnalysis).StartPt);

                foreach (BarAnalysis ba in analyses)
                {
                    DateTime single_start_time = DateTime.Now;

                    BarAnalysisPointer bap = GetBarAnalysisPointer(ba);
                    int original_start = bap.StartPt;
                    int original_stop = bap.StopPt;
                    ba.Update(bap);
                    startPt = Math.Min(startPt, bap.StartPt);

                    if (debugInfo)
                    {
                        Console.WriteLine(ba.Name + " | (" + original_start + "->" + bap.StartPt + ") | Time " + (DateTime.Now - single_start_time).TotalMilliseconds.ToString() + "ms");
                    }
                }
            }
            LastCalculateIndex = startPt;

            if (debugInfo)
            {
                Console.WriteLine("------------------");
                Console.WriteLine(Name + " | Calculate(): " + (DateTime.Now - total_start_time).TotalMilliseconds.ToString() + "ms" + " | Stopped at: " + LastCalculateIndex);
                Console.WriteLine(Name + " | LastTime = " + LastTime + " | LastBar.Close = " + LastBar.Close);
                Console.WriteLine("==================\n");
            }
        }

        public void CalculateRefresh(BarAnalysisSet bas)
        {
            if (Enabled && Count > 0 && bas is BarAnalysisSet)
                lock (DataLockObject)
                {
                    Status = TableStatus.Calculating;
                    Calculate(bas);
                    lock (DataConsumers)
                    {
                        DataConsumers.Where(n => n is BarChart bc && bc.BarAnalysisSet == bas).Select(n => n as BarChart).ToList().ForEach(n => { n.PointerToEnd(); });
                    }
                    m_Status = TableStatus.Ready;
                }
        }

        private Task CalculateTickTask { get; }

        private CancellationTokenSource CalculateTickCancelTs { get; }

        private bool CalculateTickRequested { get; set; } = false;

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
            CalculateTickRequested = false;

            lock (DataLockObject)
            {
                while (Status != TableStatus.Ready && Status != TableStatus.LoadFinished) ;

                TableStatus last_status = Status;
                Status = TableStatus.Maintaining;
                ResetCalculationPointer();
                Rows.AsParallel().ForAll(n => n.ClearAllCalculationData());
                Status = last_status;
            }
        }

        #endregion Data/Bar Analysis (TA) Calculation

        #region Data Consumers

        private List<IDataConsumer> DataConsumers { get; } = new List<IDataConsumer>();

        public bool AddDataConsumer(IDataConsumer idk)
        {
            return DataConsumers.CheckAdd(idk);
        }

        public bool RemoveDataConsumer(IDataConsumer idk)
        {
            return DataConsumers.CheckRemove(idk);
        }

        public DateTime UpdateTime { get; private set; }

        public void DataIsUpdated(IDataProvider provider)
        {
            UpdateTime = DateTime.Now;
        }

        public bool IsLive
        {
            get => m_IsLive;

            private set
            {
                m_IsLive = value;
                if (m_IsLive)
                {
                    // Add BarTable to the tick receiver
                    Contract.MarketData.AddDataConsumer(this);
                }
                else
                {
                    // Remove BarTable from the tick receiver
                    Contract.MarketData.RemoveDataConsumer(this);
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
                    lock (DataConsumers) DataConsumers.ForEach(n => { if (n is IDataRenderer idr) idr.PointerToEnd(); });
                }
                else if (m_Status == TableStatus.TickingFinished)
                {
                    lock (DataConsumers) DataConsumers.ForEach(n => { if (n is IDataRenderer idr) idr.PointerToNextTick(); });
                }
                else if (!ReadyToShow)
                {
                    lock (DataConsumers) DataConsumers.ForEach(n => { n.DataIsUpdated(this); });
                }
            }
        }

        private TableStatus m_Status = TableStatus.Default;

        #endregion Data Consumers

        #region Equality

        public bool Equals(BarTable other) => Key == other.Key;
        public bool Equals(((string name, Exchange exchange, string typeName) ContractKey, BarFreq BarFreq, BarType Type) other) => Key == other;
        public bool Equals((Contract, BarFreq, BarType) other) => (Contract, BarFreq, Type) == other;

        public static bool operator ==(BarTable s1, BarTable s2) => s1.Equals(s2);
        public static bool operator !=(BarTable s1, BarTable s2) => !s1.Equals(s2);
        public static bool operator ==(BarTable s1, (Contract c, BarFreq barFreq, BarType type) s2) => s1.Equals(s2);
        public static bool operator !=(BarTable s1, (Contract c, BarFreq barFreq, BarType type) s2) => !s1.Equals(s2);

        /// <summary>
        /// https://stackoverflow.com/questions/4219261/overriding-operator-how-to-compare-to-null
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(object other)
        {
            if (other is BarTable bt)
                return Equals(bt);
            else if (other.GetType() == typeof((Contract, BarFreq, BarType)))
                return Equals(((Contract, BarFreq, BarType))other);
            else if (other.GetType() == typeof(((string name, Exchange exchange, string typeName) ContractKey, BarFreq BarFreq, BarType Type)))
                return Equals((((string name, Exchange exchange, string typeName) ContractKey, BarFreq BarFreq, BarType Type))other);
            else
                return false;
        }

        public override int GetHashCode() => Key.GetHashCode();

        #endregion Equality
    }
}
