/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
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
    public enum ConstantDataType : int
    {
        None = 0,

        Above,

        Below,

        Within,

        //Expansion,

        //Contraction,

        CrossUp,

        CrossDown,

        ExitBelow,

        EnterFromBelow,

        ExitAbove,

        EnterFromAbove,

        //TrendUp,

        //TrendDown,
    }

    public class ConstantIndicator : Indicator
    {
        protected ConstantIndicator() { }

        protected override void Calculate(BarAnalysisPointer bap)
        {

        }

        #region Constant Data

        public static ConstantDataType ConstantDataSignal(BarTable bt, int i, ISingleData analysis, double constant)
            => ConstantDataSignal(bt, i, analysis.Result_Column, constant);

        public static ConstantDataType ConstantDataSignal(BarTable bt, int i, NumericColumn column, double constant)
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

        public static ConstantDataType ConstantDataSignal(BarTable bt, int i, ISingleData analysis, Range<double> range)
            => ConstantDataSignal(bt, i, analysis.Result_Column, range);

        public static ConstantDataType ConstantDataSignal(BarTable bt, int i, NumericColumn column, Range<double> range)
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
    }
}
