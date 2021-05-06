/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio
{
    public sealed class SingleDataSignal : SignalAnalysis
    {
        public SingleDataSignal(TimePeriod tif, BarFreq barFreq, IOscillator analysis, double range_percent = 0.05, PriceType priceType = PriceType.Trades)
            : base(tif, barFreq, priceType)
        {
            Column = analysis.Column_Result;
            analysis.AddChild(this);

            double range = analysis.Reference * range_percent;
            Range = new Range<double>(analysis.Reference - range, analysis.Reference + range);
            Label = "(" + Column.Name + "," + Range.ToStringShort() + "," + tif + "," + barFreq + "," + priceType + ")";
            GroupName = Name = GetType().Name + Label;
            Column_Result = new(this, typeof(SingleDataSignalDatum));

            BullishColor = analysis.UpperColor;
            BearishColor = analysis.LowerColor;
        }

        public SingleDataSignal(TimePeriod tif, BarFreq barFreq, ISingleData analysis, Range<double> range, PriceType priceType = PriceType.Trades)
            : base(tif, barFreq, priceType)
        {
            Column = analysis.Column_Result;
            analysis.AddChild(this);

            Range = range;
            Label = "(" + Column.Name + "," + Range.ToStringShort() + "," + tif + "," + barFreq + "," + priceType + ")";
            GroupName = Name = GetType().Name + Label;
            Column_Result = new(this, typeof(SingleDataSignalDatum));

            if (analysis is IChartSeries ics)
            {
                BullishColor = ics.Color;
                BearishColor = ics.Color;
            }
        }

        public SingleDataSignal(TimePeriod tif, BarFreq barFreq, NumericColumn column, Range<double> range, PriceType priceType = PriceType.Trades)
            : base(tif, barFreq, priceType)
        {
            Column = column;

            Range = range;
            Label = "(" + Column.Name + "," + Range.ToStringShort() + "," + tif + "," + barFreq + "," + priceType + ")";
            GroupName = Name = GetType().Name + Label;
            Column_Result = new(this, typeof(SingleDataSignalDatum));
        }

        #region Parameters

        public override int GetHashCode() => GetType().GetHashCode() ^ Column.GetHashCode() ^ Range.GetHashCode();

        public Range<double> Range { get; set; }

        public Dictionary<Range<double>, double[]> LevelScores { get; }

        public NumericColumn Column { get; set; }

        #endregion Parameters

        #region Calculation

        public Dictionary<SingleDataSignalType, double[]> TypeToTrailPoints { get; set; } = new()
        {
            { SingleDataSignalType.None, new double[] { 0 } },
            { SingleDataSignalType.Within, new double[] { 0 } },
            { SingleDataSignalType.Above, new double[] { 1 } },
            { SingleDataSignalType.Below, new double[] { -1 } },
            { SingleDataSignalType.EnterFromBelow, new double[] { 2 } },
            { SingleDataSignalType.EnterFromAbove, new double[] { -2 } },
            { SingleDataSignalType.ExitAbove, new double[] { 5, 5 } },
            { SingleDataSignalType.ExitBelow, new double[] { -5, -5 } },
            { SingleDataSignalType.CrossUp, new double[] { 7, 5, 2 } },
            { SingleDataSignalType.CrossDown, new double[] { -7, -5, -2 } },
            { SingleDataSignalType.BounceUp, new double[] { 10, 5, 2 } },
            { SingleDataSignalType.BounceDown, new double[] { -10, -5, -2 } },
        };

        public void AddType(SingleDataSignalDatum d, SingleDataSignalType type)
        {
            if (!d.List.Contains(type))
            {
                d.List.Add(type);

                if (TypeToTrailPoints.ContainsKey(type))
                    d.SetPoints(TypeToTrailPoints[type]);
            }
        }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            int bounce_range = 5;
            int bounce_middle_point = Math.Floor(bounce_range / 2f).ToInt32(bounce_range / 2);

            if (bap.StartPt < 1)
            {
                bap.StartPt = 1;
            }

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                var bars = bt[i, bounce_range];

                Bar b = bars.Last();
                double value = b[Column];

                if ((BarFreq >= BarFreq.Daily || TimeInForce.Contains(b.Time)) && !double.IsNaN(value))
                {
                    SingleDataSignalDatum d = new(b, Column_Result);

                    if (value > Range)
                        AddType(d, SingleDataSignalType.Above);
                    else if (value < Range)
                        AddType(d, SingleDataSignalType.Below);

                    if (bars.Count > 1)
                    {
                        Bar b_1 = bars[bars.Count - 2];
                        double last_value = b_1[Column];

                        if (!double.IsNaN(last_value))
                        {
                            if (last_value > Range)
                            {
                                if (value < Range)
                                {
                                    AddType(d, SingleDataSignalType.CrossDown);
                                }
                                else if (value == Range)
                                {
                                    AddType(d, SingleDataSignalType.EnterFromAbove);
                                }
                            }
                            else if (last_value == Range)
                            {
                                if (value > Range)
                                {
                                    AddType(d, SingleDataSignalType.ExitAbove);
                                }
                                else if (value < Range)
                                {
                                    AddType(d, SingleDataSignalType.ExitBelow);
                                }
                                else if (value == Range)
                                {
                                    AddType(d, SingleDataSignalType.Within);
                                }
                            }
                            else if (last_value < Range)
                            {
                                if (value > Range)
                                {
                                    AddType(d, SingleDataSignalType.CrossUp);
                                }
                                else if (value == Range)
                                {
                                    AddType(d, SingleDataSignalType.EnterFromBelow);
                                }
                            }
                        }

                        if (bars.Count() == bounce_range)
                        {
                            double first_value = bars.First()[Column];
                            double middle_value = bars.ElementAt(bounce_middle_point)[Column];

                            if (first_value > Range && value > Range && (middle_value < Range || middle_value == Range))
                            {
                                AddType(d, SingleDataSignalType.BounceUp);
                            }
                            else if (first_value < Range && value < Range && (middle_value > Range || middle_value == Range))
                            {
                                AddType(d, SingleDataSignalType.BounceDown);
                            }
                        }
                    }
                }
            }
        }

        #endregion Calculation
    }
}

