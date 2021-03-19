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
using System.Linq.Expressions;
using Xu;
using Xu.Chart;
using Pacmio.Analysis;

namespace Pacmio
{
    /// <summary>
    /// One Single Bar is not as simple as it sounds.
    /// </summary>
    public class Bar
    {
        #region Ctor

        public Bar(BarTable bt, DateTime time)
        {
            Table = bt;
            Period = Frequency.AlignPeriod(time);
            DataSourcePeriod = new Period(time);
        }

        public Bar(BarTable bt, Bar small_b)
        {
            Table = bt;

            if (BarFreq >= small_b.BarFreq) // Merge From A Smaller Bar
            {
                Period = Frequency.AlignPeriod(small_b.Time);
                DataSourcePeriod = new Period(small_b.Period);
                Source = small_b.Source;
                Open = small_b.Open;
                High = small_b.High;
                Low = small_b.Low;
                Close = small_b.Close;
                Volume = small_b.Volume;
            }
            else
                throw new Exception("Source Bar has to be at smaller time frame!");
        }

        public Bar(BarTable bt, DateTime time, double last, double volume)
        {
            Table = bt;
            Period = Frequency.AlignPeriod(time);
            DataSourcePeriod = new Period(time);
            Source = DataSourceType.Tick;
            Open = High = Low = Close = last;
            Volume = volume;
        }

        public Bar(BarTable bt, DateTime time, double open, double high, double low, double close, double volume, DataSourceType source = DataSourceType.Tick)
        {
            Table = bt;
            Period = Frequency.AlignPeriod(time);
            DataSourcePeriod = new Period(time);
            Source = source;

            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }

        /// <summary>
        /// BarTable this Bar belongs to. And unable to change through the entire life cycle of the Bar.
        /// </summary>
        public BarTable Table { get; }

        /// <summary>
        /// BarSize of the Bar
        /// </summary>
        public BarFreq BarFreq => Table.BarFreq; // { get; private set; }

        /// <summary>
        /// Attached to the Table's frequency.
        /// </summary>
        public Frequency Frequency => Table.Frequency; //BarFreq.GetAttribute<BarFreqInfo>().Result.Frequency;

        #endregion Ctor

        #region Time and Period Info

        public int Index { get; set; } = 0;

        /// <summary>
        /// Start Time of the Bar
        /// </summary>
        // Need to get Period depending on the time and BarSize
        public DateTime Time => Period.Start; // { get; private set; } // -- Shall we used Period to reduce the confusion???

        /// <summary>
        /// The time period of this Bar (Every OHLC is from a period of time)
        /// </summary>
        public Period Period { get; }

        /// <summary>
        /// The actual period according from the datasource
        /// For identifing partial bar (If the Period "Contains" and "Wider" than DataSourcePeriod)
        /// </summary>
        public Period DataSourcePeriod { get; }

        #endregion

        #region Original

        public bool IsValid => Source != DataSourceType.Invalid;

        /// <summary>
        /// 
        /// </summary>
        public DataSourceType Source { get; set; } = DataSourceType.Invalid;

        public double Open { get; set; } = -1;

        public double High { get; set; } = -1;

        public double Low { get; set; } = -1;

        public double Close { get; set; } = -1;

        public double Volume { get; set; } = -1;

        public static NumericColumn Column_Open { get; } = new NumericColumn("OPEN", "O");
        public static NumericColumn Column_High { get; } = new NumericColumn("HIGH", "H");
        public static NumericColumn Column_Low { get; } = new NumericColumn("LOW", "L");
        public static NumericColumn Column_Close { get; } = new NumericColumn("CLOSE", "CLOSE");
        public static NumericColumn Column_Volume { get; } = new NumericColumn("VOLUME", string.Empty);

        #endregion Original

        #region Smaller Bars

        public bool MergeFromSmallerBar(Bar b)
        {
            bool isModified = false;

            if (b.BarFreq <= BarFreq)
            {
                if (b.High > High) // New High
                {
                    High = b.High;
                    isModified = true;
                }

                if (b.Low < Low) // New Low
                {
                    Low = b.Low;
                    isModified = true;
                }

                if (b.Period.Stop <= DataSourcePeriod.Start) // Eariler Open
                {
                    Open = b.Open;
                    Volume += b.Volume;
                    DataSourcePeriod.Insert(b.Period.Start);
                    isModified = true;
                }

                if (b.Period.Start >= DataSourcePeriod.Stop) // Later Close
                {
                    Close = b.Close;
                    Volume += b.Volume;
                    DataSourcePeriod.Insert(b.Period.Stop);
                    isModified = true;
                }

                if (Source < b.Source) Source = b.Source; // Worse Source
            }
            else
            {
                throw new Exception("Can't merge from larger Bar!!");
            }

            return isModified;
        }

        #endregion Smaller Bars

        #region Intrinsic Indicators

        public List<CandleStickType> CandleStickList { get; } = new();

        public double Gain { get; set; } = 0;

        public double GainPercent { get; set; } = 0;

        public double Gap { get; set; } = 0;

        public double GapPercent { get; set; } = 0;

        public double Typical { get; set; }

        public double Range { get; set; } = 0;

        public double TrueRange { get; set; }

        public int NarrowRange { get; set; } = 0;

        public int TrendStrength { get; set; } = 0;

        public int Pivot { get; set; }

        public static NumericColumn Column_Gain { get; } = new NumericColumn("GAIN", "GAIN");
        public static NumericColumn Column_GainPercent { get; } = new NumericColumn("GAINPERCENT", "G%");
        public static NumericColumn Column_Gap { get; } = new NumericColumn("GAP", "GAP");
        public static NumericColumn Column_GapPercent { get; } = new NumericColumn("GAPPERCENT", "GAP%");
        public static NumericColumn Column_Typical { get; } = new NumericColumn("TYPICAL", "TYPICAL");
        public static NumericColumn Column_Range { get; } = new NumericColumn("RANGE", "RANGE");
        public static NumericColumn Column_TrueRange { get; } = new NumericColumn("TRUERANGE", "TR");
        public static NumericColumn Column_NarrowRange { get; } = new NumericColumn("NARROWRANGE", "NR");
        public static NumericColumn Column_TrendStrength { get; } = new NumericColumn("TrendStrength", "TS");
        public static NumericColumn Column_Pivot { get; } = new NumericColumn("PIVOT", "PVT");

        #endregion Intrinsic Indicators

        #region Numeric Column

        /// <summary>
        /// BarAnalysis Data Line
        /// </summary>
        // set is not allowed// One column only has one data
        private Dictionary<NumericColumn, double> NumericColumnsLUT { get; } = new Dictionary<NumericColumn, double>();

        public double this[NumericColumn column]
        {
            get
            {
                return column switch
                {
                    NumericColumn dc when dc == Column_Open => Open,
                    NumericColumn dc when dc == Column_High => High,
                    NumericColumn dc when dc == Column_Low => Low,
                    NumericColumn dc when dc == Column_Close => Close,
                    NumericColumn dc when dc == Column_Volume => Volume,

                    NumericColumn dc when dc == Column_Gain => Gain,
                    NumericColumn dc when dc == Column_GainPercent => GainPercent,
                    NumericColumn dc when dc == Column_Gap => Gap,
                    NumericColumn dc when dc == Column_GapPercent => GapPercent,

                    NumericColumn dc when dc == Column_Typical => Typical,
                    NumericColumn dc when dc == Column_Range => Range,
                    NumericColumn dc when dc == Column_TrueRange => TrueRange,
                    NumericColumn dc when dc == Column_NarrowRange => NarrowRange,
                    NumericColumn dc when dc == Column_TrendStrength => TrendStrength,
                    NumericColumn dc when dc == Column_Pivot => Pivot,

                    NumericColumn ic when NumericColumnsLUT.ContainsKey(ic) => NumericColumnsLUT[ic],

                    _ => double.NaN,
                };
            }
            set
            {
                if (double.IsNaN(value) && NumericColumnsLUT.ContainsKey(column))
                    NumericColumnsLUT.Remove(column);
                else
                    switch (column)
                    {
                        case NumericColumn dc when dc == Column_Open: Open = value; break;
                        case NumericColumn dc when dc == Column_High: High = value; break;
                        case NumericColumn dc when dc == Column_Low: Low = value; break;
                        case NumericColumn dc when dc == Column_Close: Close = value; break;
                        case NumericColumn dc when dc == Column_Volume: Volume = value; break;

                        case NumericColumn dc when dc == Column_Gain: Gain = value; break;
                        case NumericColumn dc when dc == Column_GainPercent: GainPercent = value; break;
                        case NumericColumn dc when dc == Column_Gap: Gap = value; break;
                        case NumericColumn dc when dc == Column_GapPercent: GapPercent = value; break;

                        case NumericColumn dc when dc == Column_Typical: Typical = value; break;
                        case NumericColumn dc when dc == Column_Range: Range = value; break;
                        case NumericColumn dc when dc == Column_TrueRange: TrueRange = value; break;
                        case NumericColumn dc when dc == Column_NarrowRange: NarrowRange = value.ToInt32(); break;
                        case NumericColumn dc when dc == Column_TrendStrength: TrendStrength = value.ToInt32(); break;
                        case NumericColumn dc when dc == Column_Pivot: Pivot = value.ToInt32(); break;

                        default:
                            if (!NumericColumnsLUT.ContainsKey(column))
                                NumericColumnsLUT.Add(column, value);
                            else
                                NumericColumnsLUT[column] = value;
                            break;
                    }
            }
        }



        #endregion Numeric Column

        #region Datum Column

        private Dictionary<DatumColumn, IDatum> DatumColumnsLUT { get; } = new Dictionary<DatumColumn, IDatum>();

        public IDatum this[DatumColumn dc]
        {
            get
            {
                if (DatumColumnsLUT.ContainsKey(dc))
                {
                    return DatumColumnsLUT[dc];
                }

                return null;
            }

            set
            {
                if (value is IDatum dat && value.GetType() == dc.DatumType)
                {
                    DatumColumnsLUT[dc] = dat;
                }
                else if (value is null && DatumColumnsLUT.ContainsKey(dc))
                {
                    DatumColumnsLUT.Remove(dc);
                }
                else
                {
                    throw new("Invalid data type assigned");
                }
            }
        }

        #endregion Datum Column

        #region Trailing Pattern Point

        public void ResetPatternPointList() 
        {
            PatternPointsRange.Reset(double.MaxValue, double.MinValue);
            PositivePatternPointList.Clear();
            NegativePatternPointList.Clear();
        }

        public object PatternPointLockObject = new object();

        public Range<double> PatternPointsRange { get; set; } = new Range<double>(double.MaxValue, double.MinValue);

        public Dictionary<int, PatternPoint> PositivePatternPointList { get; } = new Dictionary<int, PatternPoint>();

        public Dictionary<int, PatternPoint> NegativePatternPointList { get; } = new Dictionary<int, PatternPoint>();

        public KeyValuePair<int, PatternPoint>[] PatternPoints
        {
            get
            {
                lock (PatternPointLockObject)
                {
                    return PositivePatternPointList.Concat(NegativePatternPointList).OrderBy(n => n.Key).ToArray();
                }
            }
        }

        #endregion Trailing Pattern Point

        #region Pattern

        public IEnumerable<IPatternObject> Pivots => DatumColumnsLUT.Values.Where(n => n is PatternDatum).SelectMany(n => n as PatternDatum);

        #endregion Pattern

        #region Range Bound

        private Dictionary<string, RangeBoundDatum> RangeBoundDatums { get; } = new Dictionary<string, RangeBoundDatum>();

        public void CalculateRangeBoundDatums()
        {
            RangeBoundDatums.Clear();
            foreach (var p in Pivots)
            {
                string areaName = p.Source.AreaName;
                if (!RangeBoundDatums.ContainsKey(areaName))
                    RangeBoundDatums[areaName] = new RangeBoundDatum();

                RangeBoundDatum prd = RangeBoundDatums[areaName];// = new PivotRangeDatum();
                prd.Insert(p);
            }
        }

        public RangeBoundDatum GetRangeBoundDatum() => GetRangeBoundDatum(MainBarChartArea.DefaultName);

        public RangeBoundDatum GetRangeBoundDatum(string areaName)
        {
            if (RangeBoundDatums.ContainsKey(areaName))
                return RangeBoundDatums[areaName];
            else
                return null;
        }

        public RangeBoundDatum this[IBarChartArea column]
        {
            get
            {
                if (RangeBoundDatums.ContainsKey(column.Name))
                    return RangeBoundDatums[column.Name];
                else
                    return null;
            }
        }

        public RangeBoundDatum this[IChartPattern column]
        {
            get
            {
                if (RangeBoundDatums.ContainsKey(column.AreaName))
                    return RangeBoundDatums[column.AreaName];
                else
                    return null;
            }
        }

        #endregion Range Bound

        #region Signal Information Tools

        private Dictionary<SignalColumn, SignalDatum> SignalLUT { get; } = new Dictionary<SignalColumn, SignalDatum>();

        public SignalDatum this[SignalColumn column]
        {
            get
            {
                if (!SignalLUT.ContainsKey(column))
                    SignalLUT[column] = new SignalDatum();

                return SignalLUT[column];
            }
        }

        public void SetSignal(SignalColumn column, double[] score, string message) 
        {
            SignalDatum sd = this[column];
            if (Index > 0)
            {
                SignalDatum sd_1 = Table[Index - 1][column];
                sd.Set(score, message, sd_1);
            }
            else
            {
                sd.Set(score, message);
            }
        }

        public (double bullish, double bearish) SignalScore(IEnumerable<SignalColumn> scs)
        {
            double bull = 0, bear = 0;
            foreach (SignalColumn sc in scs)
            {
                if (this[sc] is SignalDatum sd)
                {
                    double score = sd.Score;
                    if (score > 0) bull += score;
                    else if (score < 0) bear += score;
                }
            }
            return (bull, bear);
        }

        public (double bullish, double bearish) SignalScore(BarAnalysisSet bas) => SignalScore(bas.SignalColumns);

        #endregion Signal Information Tools

        //#region Position / Simulation Information

        /// <summary>
        /// Data sets for simulation analysis, virtualization
        /// </summary>
        
        /*
        private Dictionary<Strategy, BarPositionData> PositionDatums { get; } = new Dictionary<Strategy, BarPositionData>();

        public BarPositionData this[Strategy s]
        {
            get
            {
                if (!PositionDatums.ContainsKey(s))
                    PositionDatums.Add(s, new BarPositionData(this, s));

                return PositionDatums[s];
            }
        }

        public void RemoveTradeDatum(Strategy s)
        {
            if (PositionDatums.ContainsKey(s))
            {
                PositionDatums.Remove(s);
            }
        }*/

        //#endregion Position / Simulation Information

        public void ClearAllCalculationData()
        {
            NumericColumnsLUT.Clear();
            DatumColumnsLUT.Clear();
            PositivePatternPointList.Clear();
            NegativePatternPointList.Clear();
            RangeBoundDatums.Clear();
            //PositionDatums.Clear();
        }

        public override int GetHashCode() => Table.GetHashCode() ^ Time.GetHashCode();
    }
}
