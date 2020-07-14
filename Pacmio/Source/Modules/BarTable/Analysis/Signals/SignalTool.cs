/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Xu;

namespace Pacmio
{
    public static class SignalTool
    {
        public static void MergePoints(this List<double> list, double[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (i < list.Count)
                    list[i] += points[i];
                else
                    list.Add(points[i]);
            }
        }

        public static void MergePointsNegative(this List<double> list, double[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (i < list.Count)
                    list[i] -= points[i];
                else
                    list.Add(-points[i]);
            }
        }

        public static void MergePoints(this List<double> list, double point)
        {
            if (list.Count > 0)
                list[0] += point;
            else
                list.Add(point);
        }

        #region Constant Data

        public static ConstantDataType ConstantDataSignal(this BarTable bt, int i, ISingleData analysis, double constant)
            => ConstantDataSignal(bt, i, analysis.Result_Column, constant);

        public static ConstantDataType ConstantDataSignal(this BarTable bt, int i, NumericColumn column, double constant)
        {
            double value = bt[i, column];
            double last_value = bt[i - 1, column];

            if (!double.IsNaN(value) && !double.IsNaN(last_value))
            {
                if (last_value > constant)
                {
                    if (value > constant)
                    {
                        return ConstantDataType.Above;
                    }
                    else if (value < constant)
                    {
                        return ConstantDataType.CrossDown;
                    }
                    else if (value == constant)
                    {
                        return ConstantDataType.EnterFromAbove;
                    }
                }
                else if (last_value == constant)
                {
                    if (value > constant)
                    {
                        return ConstantDataType.ExitAbove;
                    }
                    else if (value < constant)
                    {
                        return ConstantDataType.ExitBelow;
                    }
                    else if (value == constant)
                    {
                        return ConstantDataType.Within;
                    }
                }
                else if (last_value < constant)
                {
                    if (value > constant)
                    {
                        return ConstantDataType.CrossUp;
                    }
                    else if (value < constant)
                    {
                        return ConstantDataType.Below;
                    }
                    else if (value == constant)
                    {
                        return ConstantDataType.EnterFromBelow;
                    }
                }
            }

            return ConstantDataType.None;
        }

        public static ConstantDataType ConstantDataSignal(this BarTable bt, int i, ISingleData analysis, Range<double> range)
            => ConstantDataSignal(bt, i, analysis.Result_Column, range);

        public static ConstantDataType ConstantDataSignal(this BarTable bt, int i, NumericColumn column, Range<double> range)
        {
            double value = bt[i, column];
            double last_value = bt[i - 1, column];

            if (!double.IsNaN(value) && !double.IsNaN(last_value))
            {
                if (last_value > range.Max)
                {
                    if (value > range.Max)
                    {
                        return ConstantDataType.Above;
                    }
                    else if (value < range.Min)
                    {
                        return ConstantDataType.CrossDown;
                    }
                    else if (value <= range.Max && value >= range.Min)
                    {
                        return ConstantDataType.EnterFromAbove;
                    }
                }
                else if (last_value >= range.Min && last_value <= range.Max)
                {
                    if (value > range.Max)
                    {
                        return ConstantDataType.ExitAbove;
                    }
                    else if (value < range.Min)
                    {
                        return ConstantDataType.ExitBelow;
                    }
                    else if (value <= range.Max && value >= range.Min)
                    {
                        return ConstantDataType.Within;
                    }
                }
                else if (last_value < range.Min)
                {
                    if (value > range.Max)
                    {
                        return ConstantDataType.CrossUp;
                    }
                    else if (value < range.Min)
                    {
                        return ConstantDataType.Below;
                    }
                    else if (value <= range.Max && value >= range.Min)
                    {
                        return ConstantDataType.EnterFromBelow;
                    }
                }
            }

            return ConstantDataType.None;
        }

        #endregion Constant Data

        #region Dual Data

        public static List<DualDataType> DualDataSignal(this BarTable bt, int i, ISingleData fast_analysis, ISingleData slow_analysis)
            => DualDataSignal(bt, i, fast_analysis.Result_Column, slow_analysis.Result_Column);

        public static List<DualDataType> DualDataSignal(this BarTable bt, int i, IDualData analysis)
            => DualDataSignal(bt, i, analysis.High_Column, analysis.Low_Column);

        public static List<DualDataType> DualDataSignal(this BarTable bt, int i, NumericColumn fast_Column, NumericColumn slow_Column)
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

        public static (double[] points, string description) DualDataSignal(this BarTable bt, int i, NumericColumn fast_Column, NumericColumn slow_Column, Dictionary<DualDataType, double[]> typeToScore)
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
                    point_list.MergePoints(typeToScore[DualDataType.Above]);
                }
                else if (delta < 0)
                {
                    dualDataTypes.Add(DualDataType.Below);
                    point_list.MergePoints(typeToScore[DualDataType.Below]);
                }

                double last_value_fast = bt[i - 1, fast_Column];
                double last_value_slow = bt[i - 1, slow_Column];

                if (!double.IsNaN(last_value_fast) && !double.IsNaN(last_value_slow))
                {
                    double last_delta = last_value_fast - last_value_slow;

                    if (value_fast > last_value_fast && value_slow > last_value_slow)
                    {
                        dualDataTypes.Add(DualDataType.TrendUp);
                        point_list.MergePoints(typeToScore[DualDataType.TrendUp]);
                    }
                    else if (value_fast < last_value_fast && value_slow < last_value_slow)
                    {
                        dualDataTypes.Add(DualDataType.TrendDown);
                        point_list.MergePoints(typeToScore[DualDataType.TrendDown]);
                    }

                    if (delta >= 0 && last_delta < 0)
                    {
                        dualDataTypes.Add(DualDataType.CrossUp);
                        point_list.MergePoints(typeToScore[DualDataType.CrossUp]);
                    }
                    else if (delta <= 0 && last_delta > 0)
                    {
                        dualDataTypes.Add(DualDataType.CrossDown);
                        point_list.MergePoints(typeToScore[DualDataType.CrossDown]);
                    }

                    double last_delta_abs = Math.Abs(last_delta);

                    if (delta_abs > last_delta_abs)
                    {
                        dualDataTypes.Add(DualDataType.Expansion);
                        if (delta > 0)
                            point_list.MergePoints(typeToScore[DualDataType.Expansion]);
                        else if (delta < 0)
                            point_list.MergePointsNegative(typeToScore[DualDataType.Expansion]);
                    }
                    else
                    {
                        dualDataTypes.Add(DualDataType.Contraction);
                        if (delta > 0)
                            point_list.MergePoints(typeToScore[DualDataType.Contraction]);
                        else if (delta < 0)
                            point_list.MergePointsNegative(typeToScore[DualDataType.Contraction]);
                    }
                }
            }

            return (point_list.ToArray(), dualDataTypes.ToString(","));
        }

        #endregion Dual Data

        #region CandleStick

        public static void CandleStickDojiMarubozuSignal(this BarTable bt, int i, double doji_ratio = 0.12, double marubozu_ratio = 0.92)
        {
            Bar b = bt[i];
            double high = b.High;
            double low = b.Low;
            double hl_Length = Math.Abs(high - low);

            if (hl_Length > 0)
            {
                double open = b.Open;
                double close = b.Close;
                double oc_Length = Math.Abs(open - close);
                double body_shadow_ratio = oc_Length / hl_Length;

                if (body_shadow_ratio > marubozu_ratio) // Marubozu
                {
                    b.CandleStickTypes.Add(CandleStickType.Marubozu);
                }
                else if (body_shadow_ratio < doji_ratio)
                {
                    b.CandleStickTypes.Add(CandleStickType.Doji);

                    double avg_oc = (open + close) / 2;
                    double oc_position_ratio = (high - avg_oc) / hl_Length;

                    if (oc_position_ratio > 0.88)
                        b.CandleStickTypes.Add(CandleStickType.GravestoneDoji);
                    else if (oc_position_ratio < 0.12)
                        b.CandleStickTypes.Add(CandleStickType.DragonflyDoji);
                    else if (oc_position_ratio > 0.45 && oc_position_ratio < 0.55)
                        b.CandleStickTypes.Add(CandleStickType.LongLeggedDoji);
                }
            }
            else
            {
                b.CandleStickTypes.Add(CandleStickType.Doji);
            }
        }

        public static void CandleStickShadowStarSignal(this BarTable bt, int i)
        {
            Bar b = bt[i];
            double high = b.High;
            double low = b.Low;
            double hl_Length = Math.Abs(high - low);

            if (hl_Length > 0)
            {
                double open = b.Open;
                double close = b.Close;
                double oc_Length = Math.Abs(open - close);

                double body_shadow_ratio = oc_Length / hl_Length;

                if (body_shadow_ratio < 0.33)
                {
                    double trend_1 = (i > 0) ? bt[i - 1].TrendStrength : 0;

                    double top_shadow = Math.Abs(high - Math.Max(open, close));
                    double buttom_shadow = Math.Abs(Math.Min(open, close) - low);

                    if (buttom_shadow > 0)
                    {
                        double shadow_ratio = top_shadow / buttom_shadow;
                        if (shadow_ratio > 0.9 && shadow_ratio < 1.1)
                        {
                            b.CandleStickTypes.Add(CandleStickType.SpinningTop);
                        }
                        else if (shadow_ratio < 0.2 && oc_Length / buttom_shadow < 0.3)
                        {
                            b.CandleStickTypes.Add(CandleStickType.LongButtomShadows);

                            if (trend_1 > 2)
                                b.CandleStickTypes.Add(CandleStickType.HangingMan);
                            else if (trend_1 < -2)
                                b.CandleStickTypes.Add(CandleStickType.Hammer);
                        }
                    }

                    if (top_shadow > 0)
                    {
                        double shadow_ratio = buttom_shadow / top_shadow;
                        if (shadow_ratio < 0.2 && oc_Length / top_shadow < 0.3)
                        {
                            b.CandleStickTypes.Add(CandleStickType.LongTopShadows);
                            if (trend_1 > 2)
                                b.CandleStickTypes.Add(CandleStickType.ShootingStar);
                            else if (trend_1 < -2)
                                b.CandleStickTypes.Add(CandleStickType.InvertedHammer);
                        }
                    }
                }
            }
        }

        #endregion CandleStick

        #region Divergence

        public static void Divergence(this BarTable bt, int i, int minimumPeakMargin)
        {

        }

        #endregion Divergence
    }
}
