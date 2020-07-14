﻿/// ***************************************************************************
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
    }

    public class ConstantIndicator : Indicator
    {
        protected ConstantIndicator() { }

        public ConstantIndicator(IOscillator iosc, double range_percent = 0.05)
        {
            Column = iosc.Result_Column;
            double range = iosc.Reference * range_percent;
            Range = new Range<double>(iosc.Reference - range, iosc.Reference + range);

            string label = "(" + Column.Name + "," + Range.ToStringShort() + ")";
            GroupName = Name = GetType().Name + label;

            SignalColumn = new SignalColumn(Name, label) { BullishColor = iosc.UpperColor, BearishColor = iosc.LowerColor };
            SignalColumns = new SignalColumn[] { SignalColumn };
        }

        public ConstantIndicator(NumericColumn column, Range<double> range)
        {
            Column = column;
            Range = range;

            string label = "(" + Column.Name + "," + Range.ToStringShort() + ")";
            GroupName = Name = GetType().Name + label;

            SignalColumn = new SignalColumn(Name, label);
            SignalColumns = new SignalColumn[] { SignalColumn };
        }

        #region Parameters

        public override int GetHashCode() => GetType().GetHashCode() ^ Column.GetHashCode() ^ Range.GetHashCode();

        public NumericColumn Column { get; protected set; }

        public Range<double> Range { get; protected set; }

        #endregion Parameters

        #region Calculation

        public SignalColumn SignalColumn { get; protected set; }

        public readonly Dictionary<ConstantDataType, double[]> TypeToScore = new Dictionary<ConstantDataType, double[]>
        {
            { ConstantDataType.Above, new double[] { 0.25 } },
            { ConstantDataType.CrossDown, new double[] { -3.5, -3, -2 } },
            { ConstantDataType.EnterFromAbove, new double[] { -1 } },
            { ConstantDataType.ExitAbove, new double[] { 3, 2 } },
            { ConstantDataType.ExitBelow, new double[] { -3, -2 } },
            { ConstantDataType.Within, new double[] { 0 } },
            { ConstantDataType.CrossUp, new double[] { 3.5, 3, 2 } },
            { ConstantDataType.Below, new double[] { -0.25 } },
            { ConstantDataType.EnterFromBelow, new double[] { 1 } },
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
                SignalDatum sd = bt[i][SignalColumn];

                var (points, description) = ConstantDataSignal(bt, i, Column, Range, TypeToScore);

                if (i > 0)
                {
                    SignalDatum sd_1 = bt[i - 1][SignalColumn];
                    sd.Set(points, description, sd_1);
                }
                else
                    sd.Set(points, description);
            }
        }

        #endregion Calculation

        #region Constant Data Tools

        public static (double[] points, string description) ConstantDataSignal(BarTable bt, int i, NumericColumn column, Range<double> range, Dictionary<ConstantDataType, double[]> typeToScore)
        {
            double value = bt[i, column];
            double last_value = bt[i - 1, column];

            if (!double.IsNaN(value) && !double.IsNaN(last_value))
            {
                if (last_value > range.Max)
                {
                    if (value > range.Max)
                    {
                        return (typeToScore[ConstantDataType.Above], "Above");
                    }
                    else if (value < range.Min)
                    {
                        return (typeToScore[ConstantDataType.CrossDown], "Cross Down");
                    }
                    else if (value <= range.Max && value >= range.Min)
                    {
                        return (typeToScore[ConstantDataType.EnterFromAbove], "Enter From Above");
                    }
                }
                else if (last_value >= range.Min && last_value <= range.Max)
                {
                    if (value > range.Max)
                    {
                        return (typeToScore[ConstantDataType.ExitAbove], "Exit Above");
                    }
                    else if (value < range.Min)
                    {
                        return (typeToScore[ConstantDataType.ExitBelow], "Exit Below");
                    }
                    else if (value <= range.Max && value >= range.Min)
                    {
                        return (typeToScore[ConstantDataType.Within], "Within");
                    }
                }
                else if (last_value < range.Min)
                {
                    if (value > range.Max)
                    {
                        return (typeToScore[ConstantDataType.CrossUp], "Cross Up");
                    }
                    else if (value < range.Min)
                    {
                        return (typeToScore[ConstantDataType.Below], "Below");
                    }
                    else if (value <= range.Max && value >= range.Min)
                    {
                        return (typeToScore[ConstantDataType.EnterFromBelow], "Enter From Below");
                    }
                }
            }

            return (new double[] { 0 }, "");
        }

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

        #endregion Constant Data Tools
    }
}
