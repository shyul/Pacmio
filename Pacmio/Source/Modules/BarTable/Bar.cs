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

namespace Pacmio
{
    /// <summary>
    /// One Single Bar is not as simple as it sounds.
    /// </summary>
    public class Bar : IRow, ICandleStick
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

        /*
        public double Actual_Open { get; set; } = -1;

        public double Actual_High { get; set; } = -1;

        public double Actual_Low { get; set; } = -1;

        public double Actual_Close { get; set; } = -1;

        public double Actual_Volume { get; set; } = -1;
       
        #endregion

        #region Adjusted Values
         */

        public double Open { get; set; } = -1;

        public double High { get; set; } = -1;

        public double Low { get; set; } = -1;

        public double Close { get; set; } = -1;

        public double Volume { get; set; } = -1;

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

        public List<CandleStickType> CandleStickTypes { get; } = new List<CandleStickType>();

        public object this[Column column]
        {
            get
            {
                return column switch
                {
                    NumericColumn nc => this[nc],
                    DatumColumn tc => this[tc],
                    SignalColumn sc => this[sc],
                    _ => null,
                };
            }
        }

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

                        default:
                            if (!NumericColumnsLUT.ContainsKey(column))
                                NumericColumnsLUT.Add(column, value);
                            else
                                NumericColumnsLUT[column] = value;
                            break;
                    }
            }
        }

        public static NumericColumn Column_Open { get; } = new NumericColumn("OPEN", "O");
        public static NumericColumn Column_High { get; } = new NumericColumn("HIGH", "H");
        public static NumericColumn Column_Low { get; } = new NumericColumn("LOW", "L");
        public static NumericColumn Column_Close { get; } = new NumericColumn("CLOSE", "CLOSE");
        public static NumericColumn Column_Volume { get; } = new NumericColumn("VOLUME", string.Empty);

        #endregion Numeric Column

        #region Datum Column

        private Dictionary<DatumColumn, IDatum> DatumColumnsLUT { get; } = new Dictionary<DatumColumn, IDatum>();

        public IDatum this[DatumColumn dc]
        {
            get => DatumColumnsLUT.ContainsKey(dc) ? DatumColumnsLUT[dc] : null;

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

        #region PivotRangeSet

        public IEnumerable<IPivot> Pivots => DatumColumnsLUT.Values.Where(n => n is PatternDatum).SelectMany(n => n as PatternDatum);

        private Dictionary<string, PivotRangeDatum> PivotRangeDatums { get; } = new Dictionary<string, PivotRangeDatum>();

        public void CalculatePivotRangeDatums()
        {
            PivotRangeDatums.Clear();
            foreach (var p in Pivots)
            {
                string areaName = p.Source.AreaName;
                if (!PivotRangeDatums.ContainsKey(areaName))
                    PivotRangeDatums[areaName] = new PivotRangeDatum();

                PivotRangeDatum prd = PivotRangeDatums[areaName];// = new PivotRangeDatum();
                prd.Insert(p);
            }
        }

        public PivotRangeDatum GetPivotRangeDatum() => GetPivotRangeDatum(MainBarChartArea.DefaultName);

        public PivotRangeDatum GetPivotRangeDatum(string areaName)
        {
            if (PivotRangeDatums.ContainsKey(areaName))
                return PivotRangeDatums[areaName];
            else
                return null;
        }

        public PivotRangeDatum this[IBarChartArea column]
        {
            get
            {
                if (PivotRangeDatums.ContainsKey(column.Name))
                    return PivotRangeDatums[column.Name];
                else
                    return null;
            }
        }

        public PivotRangeDatum this[IChartPattern column]
        {
            get
            {
                if (PivotRangeDatums.ContainsKey(column.AreaName))
                    return PivotRangeDatums[column.AreaName];
                else
                    return null;
            }
        }

        #endregion PivotRangeSet

        #region Signal Information

        public Dictionary<SignalColumn, SignalDatum> SignalDatums { get; } = new Dictionary<SignalColumn, SignalDatum>();

        public SignalDatum this[SignalColumn column]
        {
            get
            {
                if (!SignalDatums.ContainsKey(column))
                    SignalDatums.Add(column, new SignalDatum());

                return SignalDatums[column];
            }
            set
            {
                if (!SignalDatums.ContainsKey(column))
                    SignalDatums.Add(column, value);
                else
                {
                    if (value is null)
                        SignalDatums.Remove(column);
                    else
                        SignalDatums[column] = value;
                }
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

        #endregion Signal Information

        #region Position / Simulation Information

        /// <summary>
        /// Data sets for simulation analysis, virtualization
        /// </summary>
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
        }

        #endregion Position / Simulation Information

        public void ClearAllCalculationData()
        {
            NumericColumnsLUT.Clear();
            DatumColumnsLUT.Clear();
            //TrailingPivotPoints.Clear();
            PivotRangeDatums.Clear();
            //Patterns.Clear();
            PositionDatums.Clear();
            SignalDatums.Clear();
        }

        public override int GetHashCode() => Table.GetHashCode() ^ Time.GetHashCode();
    }
}
