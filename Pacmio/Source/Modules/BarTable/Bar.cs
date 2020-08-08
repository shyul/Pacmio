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
using Xu;

namespace Pacmio
{
    /// <summary>
    /// One Single Bar is not as simple as it sounds.
    /// </summary>
    public class Bar : IRow
    {
        #region Ctor

        public Bar(BarTable bt, DateTime time)
        {
            Table = bt;
            Period = BarFreq.GetAttribute<BarFreqInfo>().Result.Frequency.AlignPeriod(time);
        }

        public Bar(BarTable bt, DateTime time, DataSource source,
            double actual_open, double actual_high, double actual_low, double actual_close, double actual_volume,
            double open, double high, double low, double close, double volume)
        {
            Table = bt;
            Period = BarFreq.GetAttribute<BarFreqInfo>().Result.Frequency.AlignPeriod(time);
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
        }

        /// <summary>
        /// BarTable this Bar belongs to. And unable to change through the entire life cycle of the Bar.
        /// </summary>
        public readonly BarTable Table; // { get; set; }

        public override int GetHashCode() => Time.GetHashCode();

        #endregion Ctor

        #region Time and Period Info

        /// <summary>
        /// BarSize of the Bar
        /// </summary>
        public BarFreq BarFreq => Table.BarFreq; // { get; private set; }

        /// <summary>
        /// Attached to the Table's frequency.
        /// </summary>
        public Frequency Frequency => Table.Frequency; //BarFreq.GetAttribute<BarFreqInfo>().Result.Frequency;

        public int Index { get; set; } = 0;

        /// <summary>
        /// Start Time of the Bar
        /// </summary>
        // Need to get Period depending on the time and BarSize
        public DateTime Time => Period.Start; // { get; private set; } // -- Shall we used Period to reduce the confusion???

        /// <summary>
        /// The time period of this Bar (Every OHLC is from a period of time)
        /// </summary>
        public Period Period { get; private set; }

        /// <summary>
        /// The actual period according from the datasource
        /// For identifing partial bar (If the Period "Contains" and "Wider" than DataSourcePeriod)
        /// </summary>
        public Period DataSourcePeriod { get; set; } = new Period();

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

        public double Gain { get; set; } = double.NaN;

        public double Percent { get; set; } = double.NaN;

        public double Gap { get; set; } = double.NaN;

        public double GapPercent { get; set; } = double.NaN;

        public double TrueRange { get; set; } = double.NaN;

        public double Typical { get; set; } = double.NaN;

        public double TrendStrength { get; set; } = double.NaN;

        public double Peak { get; set; } = double.NaN;

        public TagInfo PeakTag { get; set; }

        public List<CandleStickType> CandleStickTypes { get; } = new List<CandleStickType>();

        public object this[Column column]
        {
            get
            {
                return column switch
                {
                    NumericColumn nc => this[nc],
                    TagColumn tc => this[tc],
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

                    NumericColumn dc when dc == Column_Gain => Gain,
                    NumericColumn dc when dc == Column_Percent => Percent,
                    NumericColumn dc when dc == Column_Gap => Gap,
                    NumericColumn dc when dc == Column_GapPercent => GapPercent,
                    NumericColumn dc when dc == Column_TrueRange => TrueRange,
                    NumericColumn dc when dc == Column_Typical => Typical,
                    NumericColumn dc when dc == Column_Peak => Peak,
                    NumericColumn dc when dc == Column_TrendStrength => TrendStrength,

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

                        case NumericColumn dc when dc == Column_Gain: Gain = value; break;
                        case NumericColumn dc when dc == Column_Percent: Percent = value; break;
                        case NumericColumn dc when dc == Column_Gap: Gap = value; break;
                        case NumericColumn dc when dc == Column_GapPercent: GapPercent = value; break;
                        case NumericColumn dc when dc == Column_TrueRange: TrueRange = value; break;
                        case NumericColumn dc when dc == Column_Typical: Typical = value; break;
                        case NumericColumn dc when dc == Column_Peak: Peak = value; break;
                        case NumericColumn dc when dc == Column_TrendStrength: TrendStrength = value; break;

                        default:
                            if (!NumericDatums.ContainsKey(column))
                                NumericDatums.Add(column, value);
                            else
                                NumericDatums[column] = value;
                            break;
                    }
            }
        }

        public static readonly NumericColumn Column_Open = new NumericColumn("OPEN", "O");
        public static readonly NumericColumn Column_High = new NumericColumn("HIGH", "H");
        public static readonly NumericColumn Column_Low = new NumericColumn("LOW", "L");
        public static readonly NumericColumn Column_Close = new NumericColumn("CLOSE", "CLOSE");
        public static readonly NumericColumn Column_Volume = new NumericColumn("VOLUME", string.Empty);

        //public static readonly NumericColumn Column_Gain = new NumericColumn("GAIN");
        //public static readonly NumericColumn Column_Percent = new NumericColumn("PERCENT");
        public static NumericColumn Column_Gain => BarTable.GainAnalysis.Column_Gain;
        public static NumericColumn Column_Percent => BarTable.GainAnalysis.Column_Percent;

        public static readonly NumericColumn Column_Gap = new NumericColumn("GAP");
        public static readonly NumericColumn Column_GapPercent = new NumericColumn("GAPPERCENT");
        public static readonly NumericColumn Column_TrueRange = new NumericColumn("TRUERANGE");
        public static readonly NumericColumn Column_Typical = new NumericColumn("TYPICAL");

        public static readonly NumericColumn Column_TrendStrength = new NumericColumn("TREND");
        public static readonly NumericColumn Column_Peak = new NumericColumn("PEAK");

        #endregion Numeric Column

        #region Tag Column

        private Dictionary<TagColumn, TagInfo> TagDatums { get; } = new Dictionary<TagColumn, TagInfo>();

        public TagInfo this[TagColumn column]
        {
            get
            {
                return column switch
                {
                    TagColumn tagc when tagc == Column_PeakTags => PeakTag,
                    TagColumn tagc when TagDatums.ContainsKey(tagc) => TagDatums[tagc],
                    _ => null,
                };
            }
            set
            {
                if (value is TagInfo tag)
                    switch (column)
                    {
                        case TagColumn oc when oc == Column_PeakTags: PeakTag = tag; break;
                        default:
                            if (!TagDatums.ContainsKey(column))
                                TagDatums.Add(column, tag);
                            else
                                TagDatums[column] = tag;
                            break;
                    }
                else if (value is null && TagDatums.ContainsKey(column))
                    TagDatums.Remove(column);
            }
        }

        public static readonly TagColumn Column_PeakTags = new TagColumn("PEAKTAG", "PEAK");

        #endregion Tag Column

        #region Gain Point Analysis

        private Dictionary<GainPointColumn, GainPointDatum> GainPoints { get; } = new Dictionary<GainPointColumn, GainPointDatum>();

        public GainPointDatum this[GainPointColumn column]
        {
            get => GainPoints.ContainsKey(column) ? GainPoints[column] : null;
            set
            {
                if (value is GainPointDatum gpd)
                    if (!GainPoints.ContainsKey(column))
                        GainPoints.Add(column, gpd);
                    else
                        GainPoints[column] = gpd;
                else if (value is null && GainPoints.ContainsKey(column))
                    GainPoints.Remove(column);
            }
        }

        #endregion Gain Point Analysis

        #region Patterns

        private Dictionary<PatternColumn, PatternDatum> Patterns { get; } = new Dictionary<PatternColumn, PatternDatum>();

        public PatternDatum this[PatternColumn column]
        {
            get => Patterns.ContainsKey(column) ? Patterns[column] : null;
            set
            {
                if (value is PatternDatum pattern)
                    if (!Patterns.ContainsKey(column))
                        Patterns.Add(column, pattern);
                    else
                        Patterns[column] = pattern;
                else if (value is null && Patterns.ContainsKey(column))
                    Patterns.Remove(column);
            }
        }

        /// <summary>
        /// helps BarTable to extract all trendlines for trend levels at the last Bar!!
        /// helps BarChart to draw every thing...
        /// </summary>
        public IEnumerable<TrendLine> TrendLines => Patterns.Values.SelectMany(n => n.TrendLines);

        #endregion Patterns

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
            PositionDatums.Clear();
            SignalDatums.Clear();
        }
    }
}
