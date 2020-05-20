/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// BarTable Data Types
/// 
/// ***************************************************************************

using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Text;
using Xu;
using Xu.Chart;
using System.Security.Permissions;

namespace Pacmio
{
    /// <summary>
    /// One Single Bar is not as simple as it sounds.
    /// </summary>
    public class Bar // : IDisposable
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

        #endregion Intrinsic Indicators

        #region Position Tracking Information

        public void ResetPositionTrackingInfo()
        {
            PositionHasAnalyzed = false;
            PositionHasExited = false;
            ActionType = TradeActionType.None;
            PositionQuantity = 0;
            PositionCostBasis = double.NaN;
        }

        public bool PositionHasAnalyzed { get; set; } = false;

        public bool PositionHasExited { get; set; } = false;

        /// <summary>
        /// The ActionType of current Bar
        /// </summary>
        public TradeActionType ActionType { get; set; } = TradeActionType.None;

        public double PositionQuantity { get; set; } = 0;

        public double PositionCostBasis { get; set; } = double.NaN;

        public double PositionValue => double.IsNaN(PositionCostBasis) ? 0 : Math.Abs(PositionQuantity * PositionCostBasis);

        public bool HasNoPosition => PositionQuantity == 0 || double.IsNaN(PositionCostBasis);

        /// <summary>
        /// For chart visual analysis only!!
        /// Please use RealizedPnL from TradeLogDatum for performance calculation
        /// </summary>
        public double UnrealizedProfit => HasNoPosition ? 0 : PositionQuantity * (Close - PositionCostBasis);

        /// <summary>
        /// Profit Change in Percent
        /// </summary>
        public double ProfitChangePercent => HasNoPosition ? 0 : 100 * (Close - PositionCostBasis) / PositionCostBasis;

        public void SnapShotPosition(PositionStatus ps)
        {
            if (ActionType == TradeActionType.None || (int)ps.ActionType > 10)
            {
                ActionType = ps.ActionType;
            }
            PositionQuantity = ps.Quantity;
            PositionCostBasis = ps.CostBasis;
        }

        #endregion Position Tracking Information

        #region Numeric Column

        /// <summary>
        /// BarAnalysis Data Line
        /// </summary>
        // set is not allowed// One column only has one data
        private readonly Dictionary<NumericColumn, double> NumericDatums = new Dictionary<NumericColumn, double>();

        public double this[NumericColumn column]
        {
            get
            {
                return column switch
                {
                    NumericColumn dc when dc == TableList.Column_Open => Open,
                    NumericColumn dc when dc == TableList.Column_High => High,
                    NumericColumn dc when dc == TableList.Column_Low => Low,
                    NumericColumn dc when dc == TableList.Column_Close => Close,
                    NumericColumn dc when dc == TableList.Column_Volume => Volume,

                    NumericColumn dc when dc == TableList.Column_Gain => Gain,
                    NumericColumn dc when dc == TableList.Column_Percent => Percent,
                    NumericColumn dc when dc == TableList.Column_Gap => Gap,
                    NumericColumn dc when dc == TableList.Column_GapPercent => GapPercent,
                    NumericColumn dc when dc == TableList.Column_TrueRange => TrueRange,
                    NumericColumn dc when dc == TableList.Column_Typical => Typical,
                    NumericColumn dc when dc == TableList.Column_Peak => Peak,
                    NumericColumn dc when dc == TableList.Column_TrendStrength => TrendStrength,

                    NumericColumn dc when dc == TableList.Column_ProfitChange => ProfitChangePercent,

                    NumericColumn ic when NumericDatums.ContainsKey(ic) => NumericDatums[ic],
                    _ => double.NaN,
                };
            }
            set
            {
                switch (column)
                {
                    case NumericColumn dc when dc == TableList.Column_Open: Open = value; break;
                    case NumericColumn dc when dc == TableList.Column_High: High = value; break;
                    case NumericColumn dc when dc == TableList.Column_Low: Low = value; break;
                    case NumericColumn dc when dc == TableList.Column_Close: Close = value; break;
                    case NumericColumn dc when dc == TableList.Column_Volume: Volume = value; break;

                    case NumericColumn dc when dc == TableList.Column_Gain: Gain = value; break;
                    case NumericColumn dc when dc == TableList.Column_Percent: Percent = value; break;
                    case NumericColumn dc when dc == TableList.Column_Gap: Gap = value; break;
                    case NumericColumn dc when dc == TableList.Column_GapPercent: GapPercent = value; break;
                    case NumericColumn dc when dc == TableList.Column_TrueRange: TrueRange = value; break;
                    case NumericColumn dc when dc == TableList.Column_Typical: Typical = value; break;
                    case NumericColumn dc when dc == TableList.Column_Peak: Peak = value; break;
                    case NumericColumn dc when dc == TableList.Column_TrendStrength: TrendStrength = value; break;

                    case NumericColumn dc when dc == TableList.Column_ProfitChange: break;

                    default:
                        if (!NumericDatums.ContainsKey(column))
                            NumericDatums.Add(column, value);
                        else
                            NumericDatums[column] = value;
                        break;
                }
            }
        }

        #endregion Numeric Column

        #region Object Column

        private readonly Dictionary<ObjectColumn, object> ObjectDatums = new Dictionary<ObjectColumn, object>();

        public object this[ObjectColumn column]
        {
            get
            {
                return column switch
                {
                    ObjectColumn oc when oc == TableList.Column_PeakTags => PeakTag,
                    ObjectColumn oc when oc == TableList.Column_CandleStickTypes => CandleStickTypes,

                    ObjectColumn oc when ObjectDatums.ContainsKey(oc) => ObjectDatums[column],
                    _ => null,
                };
            }
            set
            {
                if (!(value is null))
                {
                    switch (column)
                    {
                        case ObjectColumn oc when oc == TableList.Column_PeakTags: PeakTag = (TagInfo)value; break;
                        case ObjectColumn oc when oc == TableList.Column_CandleStickTypes: break;

                        default:
                            if (!ObjectDatums.ContainsKey(column))
                                ObjectDatums.Add(column, value);
                            else
                                ObjectDatums[column] = value;
                            break;
                    }
                }
            }
        }

        public double BullishScore => ObjectDatums.Values.Where(n => n is ISignalDatum).Select(n => ((ISignalDatum)n).BullishScore).Sum();

        public double BearishScore => ObjectDatums.Values.Where(n => n is ISignalDatum).Select(n => ((ISignalDatum)n).BearishScore).Sum();

        #endregion Object Column


















        /*
        public bool RemoveAnalysisDatum(BarAnalysis da)
        {
            return da switch
            {
                NumericAnalysis dc => NumericDatums.Remove(dc),
                ConditionAnalysis dsc => ConditionDatums.Remove(dsc),
                PatternAnalysis pa => PatternDatums.Remove(pa),
                _ => false,
            };
        }

        public void CleanUp()
        {
            foreach (NumericAnalysis dc in NumericDatums.Keys)
            {
                if (!Table.Contains(dc)) NumericDatums.Remove(dc);
            }

            foreach (ConditionAnalysis dsc in ConditionDatums.Keys)
            {
                if (!Table.Contains(dsc)) ConditionDatums.Remove(dsc);
            }

            foreach (PatternAnalysis pa in PatternDatums.Keys)
            {
                if (!Table.Contains(pa)) PatternDatums.Remove(pa);
            }
        }*/
    }
}
