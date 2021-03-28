/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using Xu;

namespace Pacmio.Analysis
{
    public sealed class DualDataSignal : SignalAnalysis
    {
        public DualDataSignal(IDualData analysis)
        {
            Fast_Column = analysis.Column_High;
            Slow_Column = analysis.Column_Low;

            string label = "(" + analysis.Name + ")";
            GroupName = Name = GetType().Name + label;
            Column_Result = new(this, typeof(DualDataSignalDatum));

            analysis.AddChild(this);
        }

        public DualDataSignal(ISingleData fast_analysis, ISingleData slow_analysis)
        {
            Fast_Column = fast_analysis.Column_Result;
            Slow_Column = slow_analysis.Column_Result;

            string label = "(" + fast_analysis.Name + "," + slow_analysis.Name + ")";
            GroupName = Name = GetType().Name + label;
            Column_Result = new(this, typeof(DualDataSignalDatum));

            fast_analysis.AddChild(this);
            slow_analysis.AddChild(this);
        }

        public DualDataSignal(NumericColumn fast_column, NumericColumn slow_column)
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
        };

        public void AddType(DualDataSignalDatum d, DualDataSignalType type)
        {
            List<DualDataSignalType> list = d.List;

            if (!list.Contains(type))
            {
                list.Add(type);
                d.SetPoints(TypeToTrailPoints[type]);
            }
        }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            if (bap.StartPt < 1)
                bap.StartPt = 1;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                double value_fast = b[Fast_Column];
                double value_slow = b[Slow_Column];

                DualDataSignalDatum d = new(b, Column_Result);
                //b[Column_Result] = d;

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

                    Bar b_1 = bt[i - 1];

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
                }


            }
        }

        #endregion Calculation
    }
}
