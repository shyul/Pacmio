/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public static class Signals
    {
        #region Singal Data

        public static int SingalDataSignal(this BarTable bt, int i, NumericColumn column, int maxTestCount) 
        {
            return 0;
        }

        #endregion Singal Data

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

        #endregion Dual Data

        #region Divergence


        #endregion Divergence


    }



}
