/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
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
            DataSourcePeriod = Period = Frequency.AlignPeriod(time); // TODO: Can't be ticks here!!!

            //DataSourcePeriod = new Period(time);
        }

        public Bar(BarTable bt, Bar small_b)
        {
            Table = bt;

            if (BarFreq >= small_b.BarFreq) // Merge From A Smaller Bar
            {
                Period = Frequency.AlignPeriod(small_b.Time);
                DataSourcePeriod = new Period(small_b.Period);
                Source = small_b.Source;

                Actual_Open = small_b.Actual_Open;
                Actual_High = small_b.Actual_High;
                Actual_Low = small_b.Actual_Low;
                Actual_Close = small_b.Actual_Close;
                Actual_Volume = small_b.Actual_Volume;

                Open = small_b.Open;
                High = small_b.High;
                Low = small_b.Low;
                Close = small_b.Close;
                Volume = small_b.Volume;
            }
            else
                throw new Exception("Source Bar has to be at smaller time frame!");
        }

        public Bar(BarTable bt, DateTime tickTime, double last, double volume)
        {
            Table = bt;
            Period = Frequency.AlignPeriod(tickTime);
            DataSourcePeriod = new Period(tickTime);
            Source = DataSource.Tick;

            Open = High = Low = Close = Actual_Open = Actual_High = Actual_Low = Actual_Close = last;
            Volume = Actual_Volume = volume;
        }

        public Bar(BarTable bt, DateTime time, double open, double high, double low, double close, double volume)
        {
            Table = bt;
            Period = Frequency.AlignPeriod(time);
            DataSourcePeriod = new Period(time);
            Source = DataSource.Tick;
            Open = Actual_Open = open;
            High = Actual_High = high;
            Low = Actual_Low = low;
            Close = Actual_Close = close;
            Volume = Actual_Volume = volume;
        }


        /*
        public Bar(BarTable bt, DateTime time, DataSource source,
            double actual_open, double actual_high, double actual_low, double actual_close, double actual_volume,
            double open, double high, double low, double close, double volume)
        {
            Table = bt;
            Period = Frequency.AlignPeriod(time);
            Source = source;

            if (actual_open > 0 && actual_high > 0 && actual_low > 0 && actual_close > 0 && actual_volume >= 0)
            {
                Actual_Open = actual_open;
                Actual_High = actual_high;
                Actual_Low = actual_low;
                Actual_Close = actual_close;
                Actual_Volume = actual_volume;
            }

            if (open > 0 && high > 0 && low > 0 && close > 0 && volume >= 0)
            {
                Open = open;
                High = high;
                Low = low;
                Close = close;
                Volume = volume;
            }
        }*/

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

        #region Original / Actual Values

        public bool IsValid => Source != DataSource.Invalid;

        /// <summary>
        /// 
        /// </summary>
        public DataSource Source { get; set; } = DataSource.Invalid;

        public double Actual_Open { get; set; } = -1;

        public double Actual_High { get; set; } = -1;

        public double Actual_Low { get; set; } = -1;

        public double Actual_Close { get; set; } = -1;

        public double Actual_Volume { get; set; } = -1;

        #endregion

        #region Adjusted Values

        public double Open { get; set; } = -1;

        public double High { get; set; } = -1;

        public double Low { get; set; } = -1;

        public double Close { get; set; } = -1;

        public double Volume { get; set; } = -1;

        #region Adjusted Calculation

        public void Adjust(double adj_price, double adj_vol, bool forwardAdjust = true)
        {
            if (forwardAdjust) // Adjust Part
            {
                if (Actual_Open > 0 && Actual_High > 0 && Actual_Low > 0 && Actual_Close > 0 && Actual_Volume >= 0)
                {
                    Open = Actual_Open * adj_price;
                    High = Actual_High * adj_price;
                    Low = Actual_Low * adj_price;
                    Close = Actual_Close * adj_price;
                    Volume = Actual_Volume / adj_vol;
                }
            }
            else // CounterAdjust Part
            {
                if (Open > 0 && High > 0 && Low > 0 && Close > 0 && Volume >= 0)
                {
                    Actual_Open = Open / adj_price;
                    Actual_High = High / adj_price;
                    Actual_Low = Low / adj_price;
                    Actual_Close = Close / adj_price;
                    Actual_Volume = Volume * adj_vol;
                }
            }
        }

        #endregion Adjusted Calculation

        #endregion Adjusted Values

        #region Intrinsic Indicators

        public List<CandleStickType> CandleStickTypes { get; } = new List<CandleStickType>();

        public object this[Column column]
        {
            get
            {
                return column switch
                {
                    NumericColumn nc => this[nc],
                    TagColumn tc => this[tc],
                    TrailingPivotsColumn tpc => this[tpc],
                    PatternColumn pc => this[pc],
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
        private Dictionary<NumericColumn, double> NumericDatums { get; } = new Dictionary<NumericColumn, double>();

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

                    NumericColumn ic when NumericDatums.ContainsKey(ic) => NumericDatums[ic],

                    _ => double.NaN,
                };
            }
            set
            {
                if (double.IsNaN(value) && NumericDatums.ContainsKey(column))
                    NumericDatums.Remove(column);
                else
                    switch (column)
                    {
                        case NumericColumn dc when dc == Column_Open: Open = value; break;
                        case NumericColumn dc when dc == Column_High: High = value; break;
                        case NumericColumn dc when dc == Column_Low: Low = value; break;
                        case NumericColumn dc when dc == Column_Close: Close = value; break;
                        case NumericColumn dc when dc == Column_Volume: Volume = value; break;

                        default:
                            if (!NumericDatums.ContainsKey(column))
                                NumericDatums.Add(column, value);
                            else
                                NumericDatums[column] = value;
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

        #region Tag Column

        private Dictionary<TagColumn, TagInfo> TagDatums { get; } = new Dictionary<TagColumn, TagInfo>();

        public TagInfo this[TagColumn column]
        {
            get => TagDatums.ContainsKey(column) ? TagDatums[column] : null;

            set
            {
                if (value is TagInfo tag)
                    if (!TagDatums.ContainsKey(column))
                        TagDatums.Add(column, tag);
                    else
                        TagDatums[column] = tag;
                else if (value is null && TagDatums.ContainsKey(column))
                    TagDatums.Remove(column);
            }
        }

        #endregion Tag Column

        #region Trailing Pivot Points

        private Dictionary<TrailingPivotsColumn, TrailingPivotsDatum> TrailingPivotPoints { get; } = new Dictionary<TrailingPivotsColumn, TrailingPivotsDatum>();

        public TrailingPivotsDatum this[TrailingPivotsColumn column]
        {
            get => TrailingPivotPoints.ContainsKey(column) ? TrailingPivotPoints[column] : null;

            set
            {
                if (value is TrailingPivotsDatum gpd)
                    if (!TrailingPivotPoints.ContainsKey(column))
                        TrailingPivotPoints.Add(column, gpd);
                    else
                        TrailingPivotPoints[column] = gpd;
                else if (value is null && TrailingPivotPoints.ContainsKey(column))
                    TrailingPivotPoints.Remove(column);
            }
        }

        #endregion Trailing Pivot Points

        #region Patterns

        public Dictionary<PatternColumn, PatternDatum> Patterns { get; } = new Dictionary<PatternColumn, PatternDatum>();

        public IEnumerable<IPivot> Pivots => Patterns.Values.SelectMany(n => n); //.Pivots);

        public PatternDatum this[PatternColumn column]
        {
            get => Patterns.ContainsKey(column) ? Patterns[column] : null;

            set
            {
                if (value is PatternDatum pd)
                    Patterns[column] = pd;
                else if (value is null && Patterns.ContainsKey(column))
                    Patterns.Remove(column);
            }
        }

        #endregion Patterns

        #region PivotRangeSet

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
            NumericDatums.Clear();
            TagDatums.Clear();
            TrailingPivotPoints.Clear();
            PivotRangeDatums.Clear();
            Patterns.Clear();
            PositionDatums.Clear();
            SignalDatums.Clear();
        }

        public override int GetHashCode() => Table.GetHashCode() ^ Time.GetHashCode();
    }
}
