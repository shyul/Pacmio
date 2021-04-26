/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xu;

namespace Pacmio
{
    public sealed class DualDataSignal : SignalAnalysis
    {
        public DualDataSignal(BarFreq barFreq, IDualData analysis) : base(barFreq)
        {
            Fast_Column = analysis.Column_High;
            Slow_Column = analysis.Column_Low;

            string label = "(" + analysis.Name + ")";
            GroupName = Name = GetType().Name + label;
            Column_Result = new(this, typeof(DualDataSignalDatum));

            BullishColor = analysis.UpperColor;
            BearishColor = analysis.LowerColor;

            analysis.AddChild(this);
        }

        public DualDataSignal(BarFreq barFreq, ISingleData fast_analysis, ISingleData slow_analysis) : base(barFreq)
        {
            Fast_Column = fast_analysis.Column_Result;
            Slow_Column = slow_analysis.Column_Result;

            string label = "(" + fast_analysis.Name + "," + slow_analysis.Name + ")";
            GroupName = Name = GetType().Name + label;
            Column_Result = new(this, typeof(DualDataSignalDatum));

            if (fast_analysis is IChartSeries fast_ics) BullishColor = fast_ics.Color;
            if (slow_analysis is IChartSeries slow_ics) BearishColor = slow_ics.Color;

            fast_analysis.AddChild(this);
            slow_analysis.AddChild(this);
        }

        public DualDataSignal(BarFreq barFreq, NumericColumn fast_column, NumericColumn slow_column) : base(barFreq)
        {
            Fast_Column = fast_column;
            Slow_Column = slow_column;

            string label = "(" + Fast_Column.Name + "," + Slow_Column.Name + ")";
            GroupName = Name = GetType().Name + label;
            Column_Result = new(this, typeof(DualDataSignalDatum));
        }

        #region Parameters

        public override int GetHashCode() => GetType().GetHashCode() ^ Fast_Column.GetHashCode() ^ Slow_Column.GetHashCode();

        public NumericColumn Fast_Column { get; }

        public NumericColumn Slow_Column { get; }

        #endregion Parameters

        #region Calculation

        public Dictionary<DualDataSignalType, double[]> TypeToTrailPoints { get; set; } = new()
        {
            { DualDataSignalType.Above, new double[] { 0.5 } },
            { DualDataSignalType.Below, new double[] { -0.5 } },
            { DualDataSignalType.Expansion, new double[] { 1 } },
            { DualDataSignalType.Contraction, new double[] { 1 } },
            { DualDataSignalType.CrossUp, new double[] { 4, 3.5, 3, 2.5 } },
            { DualDataSignalType.CrossDown, new double[] { -4, -3.5, -3, -2.5 } },
            { DualDataSignalType.TrendUp, new double[] { 0.5 } },
            { DualDataSignalType.TrendDown, new double[] { -0.5 } },
            { DualDataSignalType.BounceUp, new double[] { 10, 7, 2 } },
            { DualDataSignalType.BounceDown, new double[] { -10, -7, -2 } },
        };

        public void AddType(DualDataSignalDatum d, DualDataSignalType type)
        {
            if (!d.List.Contains(type))
            {
                d.List.Add(type);
                d.SetPoints(TypeToTrailPoints[type]);
            }
        }

        public override SignalColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            int bounce_range = 5;
            int bounce_middle_point = Math.Floor(bounce_range / 2f).ToInt32(bounce_range / 2);


            if (bap.StartPt < 1)
                bap.StartPt = 1;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                var bars = bt[i, bounce_range];
                Bar b = bars.Last();

                double value_fast = b[Fast_Column];
                double value_slow = b[Slow_Column];

                DualDataSignalDatum d = new(b, Column_Result);

                if (!double.IsNaN(value_fast) && !double.IsNaN(value_slow))
                {
                    double delta = d.Difference = value_fast - value_slow;

                    if (delta == value_slow) d.DifferenceRatio = 1;
                    else if (value_slow == 0) d.DifferenceRatio = 0;
                    else d.DifferenceRatio = delta / value_slow;

                    if (value_fast == value_slow) d.Ratio = 1;
                    else if (value_slow == 0) d.Ratio = 0;
                    else d.Ratio = value_fast / value_slow;

                    double delta_abs = Math.Abs(delta);

                    if (delta > 0)
                    {
                        AddType(d, DualDataSignalType.Above);
                    }
                    else if (delta < 0)
                    {
                        AddType(d, DualDataSignalType.Below);
                    }

                    if (bars.Count > 1)
                    {
                        Bar b_1 = bars[bars.Count - 2];

                        double last_value_fast = b_1[Fast_Column];
                        double last_value_slow = b_1[Slow_Column];

                        if (!double.IsNaN(last_value_fast) && !double.IsNaN(last_value_slow))
                        {
                            double last_delta = last_value_fast - last_value_slow;

                            if (value_fast > last_value_fast && value_slow > last_value_slow)
                            {
                                AddType(d, DualDataSignalType.TrendUp);
                            }
                            else if (value_fast < last_value_fast && value_slow < last_value_slow)
                            {
                                AddType(d, DualDataSignalType.TrendDown);
                            }

                            if (delta >= 0 && last_delta < 0)
                            {
                                AddType(d, DualDataSignalType.CrossUp);
                            }
                            else if (delta <= 0 && last_delta > 0)
                            {
                                AddType(d, DualDataSignalType.CrossDown);
                            }

                            double last_delta_abs = Math.Abs(last_delta);

                            if (delta_abs > last_delta_abs)
                            {
                                AddType(d, DualDataSignalType.Expansion);
                            }
                            else
                            {
                                AddType(d, DualDataSignalType.Contraction);
                            }
                        }

                        if (bars.Count() == bounce_range)
                        {
                            Bar b_first = bars.First();
                            Bar b_middle = bars.ElementAt(bounce_middle_point);

                            double first_fast_value = b_first[Fast_Column];
                            double first_slow_value = b_first[Slow_Column];
                            double middle_fast_value = b_middle[Fast_Column];
                            double middle_slow_value = b_middle[Slow_Column];

                            if (first_fast_value > first_slow_value && value_fast > value_slow && middle_fast_value <= middle_slow_value)
                            {
                                AddType(d, DualDataSignalType.BounceUp);
                            }
                            else if (first_fast_value < first_slow_value && value_fast < value_slow && middle_fast_value >= middle_slow_value)
                            {
                                AddType(d, DualDataSignalType.BounceDown);
                            }
                        }
                    }
                }


            }
        }

        #endregion Calculation
    }
}
