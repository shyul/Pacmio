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
    public enum SingleDataType : int
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

    public class SingleDataIndicator : Indicator
    {
        protected SingleDataIndicator() { }

        public SingleDataIndicator(IOscillator iosc, double range_percent = 0.05)
        {
            Column = iosc.Column_Result;
            double range = iosc.Reference * range_percent;
            Range = new Range<double>(iosc.Reference - range, iosc.Reference + range);

            string label = "(" + Column.Name + "," + Range.ToStringShort() + ")";
            GroupName = Name = GetType().Name + label;

            SignalColumn = new SignalColumn(Name, label) { BullishColor = iosc.UpperColor, BearishColor = iosc.LowerColor };
            SignalColumns = new SignalColumn[] { SignalColumn };

            //Order = iosc.Order + 1;
            iosc.AddChild(this);
        }

        public SingleDataIndicator(NumericColumn column, Range<double> range)
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

        public Dictionary<SingleDataType, double[]> TypeToScore { get; } = new Dictionary<SingleDataType, double[]>
        {
            { SingleDataType.Above, new double[] { 2 } },
            { SingleDataType.CrossDown, new double[] { -4, -3.5, -3, -2 } },
            { SingleDataType.EnterFromAbove, new double[] { -1 } },
            { SingleDataType.ExitAbove, new double[] { 3, 2, 1 } },
            { SingleDataType.ExitBelow, new double[] { -3, -2, -1 } },
            { SingleDataType.Within, new double[] { 0 } },
            { SingleDataType.CrossUp, new double[] { 4, 3.5, 3, 2 } },
            { SingleDataType.Below, new double[] { -2 } },
            { SingleDataType.EnterFromBelow, new double[] { 1 } },
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

                var (points, description) = ConstantDataSignal(bt, i, Column, Range, TypeToScore);

                if (i > 0)
                {
                    SignalDatum sd_1 = bt[i - 1][SignalColumn] as SignalDatum;
                    sd.Set(points, description, sd_1);
                }
                else
                    sd.Set(points, description);
            }
        }

        #endregion Calculation

        #region Constant Data Tools

        public static (double[] points, string description) ConstantDataSignal(BarTable bt, int i, NumericColumn column, Range<double> range, Dictionary<SingleDataType, double[]> typeToScore)
        {
            double value = bt[i, column];
            double last_value = bt[i - 1, column];

            if (!double.IsNaN(value) && !double.IsNaN(last_value))
            {
                if (last_value > range.Max)
                {
                    if (value > range.Max)
                    {
                        return (typeToScore[SingleDataType.Above], "Above");
                    }
                    else if (value < range.Min)
                    {
                        return (typeToScore[SingleDataType.CrossDown], "Cross Down");
                    }
                    else if (value <= range.Max && value >= range.Min)
                    {
                        return (typeToScore[SingleDataType.EnterFromAbove], "Enter From Above");
                    }
                }
                else if (last_value >= range.Min && last_value <= range.Max)
                {
                    if (value > range.Max)
                    {
                        return (typeToScore[SingleDataType.ExitAbove], "Exit Above");
                    }
                    else if (value < range.Min)
                    {
                        return (typeToScore[SingleDataType.ExitBelow], "Exit Below");
                    }
                    else if (value <= range.Max && value >= range.Min)
                    {
                        return (typeToScore[SingleDataType.Within], "Within");
                    }
                }
                else if (last_value < range.Min)
                {
                    if (value > range.Max)
                    {
                        return (typeToScore[SingleDataType.CrossUp], "Cross Up");
                    }
                    else if (value < range.Min)
                    {
                        return (typeToScore[SingleDataType.Below], "Below");
                    }
                    else if (value <= range.Max && value >= range.Min)
                    {
                        return (typeToScore[SingleDataType.EnterFromBelow], "Enter From Below");
                    }
                }
            }

            return (new double[] { 0 }, "");
        }

        public static SingleDataType ConstantDataSignal(BarTable bt, int i, ISingleData analysis, double constant)
            => ConstantDataSignal(bt, i, analysis.Column_Result, constant);

        public static SingleDataType ConstantDataSignal(BarTable bt, int i, NumericColumn column, double constant)
        {
            double value = bt[i, column];
            double last_value = bt[i - 1, column];

            if (!double.IsNaN(value) && !double.IsNaN(last_value))
            {
                if (last_value > constant)
                {
                    if (value > constant)
                    {
                        return SingleDataType.Above;
                    }
                    else if (value < constant)
                    {
                        return SingleDataType.CrossDown;
                    }
                    else if (value == constant)
                    {
                        return SingleDataType.EnterFromAbove;
                    }
                }
                else if (last_value == constant)
                {
                    if (value > constant)
                    {
                        return SingleDataType.ExitAbove;
                    }
                    else if (value < constant)
                    {
                        return SingleDataType.ExitBelow;
                    }
                    else if (value == constant)
                    {
                        return SingleDataType.Within;
                    }
                }
                else if (last_value < constant)
                {
                    if (value > constant)
                    {
                        return SingleDataType.CrossUp;
                    }
                    else if (value < constant)
                    {
                        return SingleDataType.Below;
                    }
                    else if (value == constant)
                    {
                        return SingleDataType.EnterFromBelow;
                    }
                }
            }

            return SingleDataType.None;
        }

        public static SingleDataType ConstantDataSignal(BarTable bt, int i, ISingleData analysis, Range<double> range)
            => ConstantDataSignal(bt, i, analysis.Column_Result, range);

        public static SingleDataType ConstantDataSignal(BarTable bt, int i, NumericColumn column, Range<double> range)
        {
            double value = bt[i, column];
            double last_value = bt[i - 1, column];

            if (!double.IsNaN(value) && !double.IsNaN(last_value))
            {
                if (last_value > range.Max)
                {
                    if (value > range.Max)
                    {
                        return SingleDataType.Above;
                    }
                    else if (value < range.Min)
                    {
                        return SingleDataType.CrossDown;
                    }
                    else if (value <= range.Max && value >= range.Min)
                    {
                        return SingleDataType.EnterFromAbove;
                    }
                }
                else if (last_value >= range.Min && last_value <= range.Max)
                {
                    if (value > range.Max)
                    {
                        return SingleDataType.ExitAbove;
                    }
                    else if (value < range.Min)
                    {
                        return SingleDataType.ExitBelow;
                    }
                    else if (value <= range.Max && value >= range.Min)
                    {
                        return SingleDataType.Within;
                    }
                }
                else if (last_value < range.Min)
                {
                    if (value > range.Max)
                    {
                        return SingleDataType.CrossUp;
                    }
                    else if (value < range.Min)
                    {
                        return SingleDataType.Below;
                    }
                    else if (value <= range.Max && value >= range.Min)
                    {
                        return SingleDataType.EnterFromBelow;
                    }
                }
            }

            return SingleDataType.None;
        }

        #endregion Constant Data Tools
    }
}
