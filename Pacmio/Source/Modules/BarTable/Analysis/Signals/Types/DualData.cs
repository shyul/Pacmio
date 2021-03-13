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
    public enum DualDataType : int
    {
        Above,

        Below,

        Expansion,

        Contraction,

        CrossUp,

        CrossDown,

        TrendUp,

        TrendDown,
    }

    public class DualDataIndicator : Indicator
    {
        protected DualDataIndicator() { }

        public DualDataIndicator(NumericColumn fast_column, NumericColumn slow_column)
        {
            Fast_Column = fast_column;
            Slow_Column = slow_column;

            string label = "(" + Fast_Column.Name + "," + Slow_Column.Name + ")";
            GroupName = Name = GetType().Name + label;

            SignalColumn = new SignalColumn(Name, label);
            SignalColumns = new SignalColumn[] { SignalColumn };
        }

        #region Parameters

        public override int GetHashCode() => GetType().GetHashCode() ^ Fast_Column.GetHashCode() ^ Slow_Column.GetHashCode();

        public NumericColumn Fast_Column { get; protected set; }

        public NumericColumn Slow_Column { get; protected set; }

        #endregion Parameters

        #region Calculation

        public SignalColumn SignalColumn { get; protected set; }

        public virtual Dictionary<DualDataType, double[]> TypeToScore { get; } = new Dictionary<DualDataType, double[]>
        {
            { DualDataType.Above, new double[] { 0.5 } },
            { DualDataType.Below, new double[] { -0.5 } },
            { DualDataType.Expansion, new double[] { 1 } },
            { DualDataType.Contraction, new double[] { 1 } },
            { DualDataType.CrossUp, new double[] { 4, 3.5, 3, 2.5 } },
            { DualDataType.CrossDown, new double[] { -4, -3.5, -3, -2.5 } },
            { DualDataType.TrendUp, new double[] { 0.5 } },
            { DualDataType.TrendDown, new double[] { -0.5 } },
        };

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            if (bap.StartPt < 1)
            {
                bap.StartPt = 1;
            }

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                SignalDatum sd = bt[i][SignalColumn] as SignalDatum;

                var (points, description) = DualDataSignal(bt, i, Fast_Column, Slow_Column, TypeToScore);

                if (i > 0)
                {
                    SignalDatum sd_1 = bt[i - 1][SignalColumn] as SignalDatum;
                    sd.Set(points, description, sd_1);
                }
                else
                    sd.Set(points, description);

                //Console.WriteLine("Score: " + sd.Score);
            }

        }

        #endregion Calculation

        #region Dual Data Tools

        public static List<DualDataType> DualDataSignal(BarTable bt, int i, ISingleData fast_analysis, ISingleData slow_analysis)
            => DualDataSignal(bt, i, fast_analysis.Column_Result, slow_analysis.Column_Result);

        public static List<DualDataType> DualDataSignal(BarTable bt, int i, IDualData analysis)
            => DualDataSignal(bt, i, analysis.Column_High, analysis.Column_Low);

        public static List<DualDataType> DualDataSignal(BarTable bt, int i, NumericColumn fast_Column, NumericColumn slow_Column)
        {
            double value_fast = bt[i, fast_Column];
            double value_slow = bt[i, slow_Column];

            List<DualDataType> dualDataTypes = new List<DualDataType>();

            if (!double.IsNaN(value_fast) && !double.IsNaN(value_slow))
            {
                double delta = value_fast - value_slow;
                double delta_abs = Math.Abs(delta);

                if (delta > 0)
                {
                    dualDataTypes.Add(DualDataType.Above);
                }
                else if (delta < 0)
                {
                    dualDataTypes.Add(DualDataType.Below);
                }

                double last_value_fast = bt[i - 1, fast_Column];
                double last_value_slow = bt[i - 1, slow_Column];

                if (!double.IsNaN(last_value_fast) && !double.IsNaN(last_value_slow))
                {
                    double last_delta = last_value_fast - last_value_slow;

                    if (value_fast > last_value_fast && value_slow > last_value_slow)
                    {
                        dualDataTypes.Add(DualDataType.TrendUp);
                    }
                    else if (value_fast < last_value_fast && value_slow < last_value_slow)
                    {
                        dualDataTypes.Add(DualDataType.TrendDown);
                    }

                    if (delta >= 0 && last_delta < 0)
                    {
                        dualDataTypes.Add(DualDataType.CrossUp);
                    }
                    else if (delta <= 0 && last_delta > 0)
                    {
                        dualDataTypes.Add(DualDataType.CrossDown);
                    }

                    double last_delta_abs = Math.Abs(last_delta);

                    if (delta_abs > last_delta_abs)
                    {
                        dualDataTypes.Add(DualDataType.Expansion);
                    }
                    else
                    {
                        dualDataTypes.Add(DualDataType.Contraction);
                    }
                }
            }

            return dualDataTypes;
        }

        public static (double[] points, string description) DualDataSignal(BarTable bt, int i, NumericColumn fast_Column, NumericColumn slow_Column, Dictionary<DualDataType, double[]> typeToScore)
        {
            List<DualDataType> dualDataTypes = new List<DualDataType>();
            List<double> point_list = new List<double>();

            double value_fast = bt[i, fast_Column];
            double value_slow = bt[i, slow_Column];

            if (!double.IsNaN(value_fast) && !double.IsNaN(value_slow))
            {
                double delta = value_fast - value_slow;
                double delta_abs = Math.Abs(delta);

                if (delta > 0)
                {
                    dualDataTypes.Add(DualDataType.Above);
                    SignalDatum.MergePoints(point_list, typeToScore[DualDataType.Above]);
                }
                else if (delta < 0)
                {
                    dualDataTypes.Add(DualDataType.Below);
                    SignalDatum.MergePoints(point_list, typeToScore[DualDataType.Below]);
                }

                double last_value_fast = bt[i - 1, fast_Column];
                double last_value_slow = bt[i - 1, slow_Column];

                if (!double.IsNaN(last_value_fast) && !double.IsNaN(last_value_slow))
                {
                    double last_delta = last_value_fast - last_value_slow;

                    if (value_fast > last_value_fast && value_slow > last_value_slow)
                    {
                        dualDataTypes.Add(DualDataType.TrendUp);
                        SignalDatum.MergePoints(point_list, typeToScore[DualDataType.TrendUp]);
                    }
                    else if (value_fast < last_value_fast && value_slow < last_value_slow)
                    {
                        dualDataTypes.Add(DualDataType.TrendDown);
                        SignalDatum.MergePoints(point_list, typeToScore[DualDataType.TrendDown]);
                    }

                    if (delta >= 0 && last_delta < 0)
                    {
                        dualDataTypes.Add(DualDataType.CrossUp);
                        SignalDatum.MergePoints(point_list, typeToScore[DualDataType.CrossUp]);
                    }
                    else if (delta <= 0 && last_delta > 0)
                    {
                        dualDataTypes.Add(DualDataType.CrossDown);
                        SignalDatum.MergePoints(point_list, typeToScore[DualDataType.CrossDown]);
                    }

                    double last_delta_abs = Math.Abs(last_delta);

                    if (delta_abs > last_delta_abs)
                    {
                        dualDataTypes.Add(DualDataType.Expansion);
                        if (delta > 0)
                            SignalDatum.MergePoints(point_list, typeToScore[DualDataType.Expansion]);
                        else if (delta < 0)
                            SignalDatum.MergePointsNegative(point_list, typeToScore[DualDataType.Expansion]);
                    }
                    else
                    {
                        dualDataTypes.Add(DualDataType.Contraction);
                        if (delta > 0)
                            SignalDatum.MergePointsNegative(point_list, typeToScore[DualDataType.Contraction]);
                        else if (delta < 0)
                            SignalDatum.MergePoints(point_list, typeToScore[DualDataType.Contraction]);
                    }
                }
            }

            return (point_list.ToArray(), dualDataTypes.ToString(","));
        }

        #endregion Dual Data Tools
    }


}
