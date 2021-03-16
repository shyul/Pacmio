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
    public class SingleColumnAnalysis : Indicator
    {
        protected SingleColumnAnalysis() { }

        public SingleColumnAnalysis(IOscillator iosc, double range_percent = 0.05)
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

        public SingleColumnAnalysis(NumericColumn column, Range<double> range)
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

        public Dictionary<SingleColumnType, double[]> TypeToScore { get; } = new Dictionary<SingleColumnType, double[]>
        {
            { SingleColumnType.Above, new double[] { 2 } },
            { SingleColumnType.CrossDown, new double[] { -4, -3.5, -3, -2 } },
            { SingleColumnType.EnterFromAbove, new double[] { -1 } },
            { SingleColumnType.ExitAbove, new double[] { 3, 2, 1 } },
            { SingleColumnType.ExitBelow, new double[] { -3, -2, -1 } },
            { SingleColumnType.Within, new double[] { 0 } },
            { SingleColumnType.CrossUp, new double[] { 4, 3.5, 3, 2 } },
            { SingleColumnType.Below, new double[] { -2 } },
            { SingleColumnType.EnterFromBelow, new double[] { 1 } },
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

        public static (double[] points, string description) ConstantDataSignal(BarTable bt, int i, NumericColumn column, Range<double> range, Dictionary<SingleColumnType, double[]> typeToScore)
        {
            double value = bt[i, column];
            double last_value = bt[i - 1, column];

            if (!double.IsNaN(value) && !double.IsNaN(last_value))
            {
                if (last_value > range.Max)
                {
                    if (value > range.Max)
                    {
                        return (typeToScore[SingleColumnType.Above], "Above");
                    }
                    else if (value < range.Min)
                    {
                        return (typeToScore[SingleColumnType.CrossDown], "Cross Down");
                    }
                    else if (value <= range.Max && value >= range.Min)
                    {
                        return (typeToScore[SingleColumnType.EnterFromAbove], "Enter From Above");
                    }
                }
                else if (last_value >= range.Min && last_value <= range.Max)
                {
                    if (value > range.Max)
                    {
                        return (typeToScore[SingleColumnType.ExitAbove], "Exit Above");
                    }
                    else if (value < range.Min)
                    {
                        return (typeToScore[SingleColumnType.ExitBelow], "Exit Below");
                    }
                    else if (value <= range.Max && value >= range.Min)
                    {
                        return (typeToScore[SingleColumnType.Within], "Within");
                    }
                }
                else if (last_value < range.Min)
                {
                    if (value > range.Max)
                    {
                        return (typeToScore[SingleColumnType.CrossUp], "Cross Up");
                    }
                    else if (value < range.Min)
                    {
                        return (typeToScore[SingleColumnType.Below], "Below");
                    }
                    else if (value <= range.Max && value >= range.Min)
                    {
                        return (typeToScore[SingleColumnType.EnterFromBelow], "Enter From Below");
                    }
                }
            }

            return (new double[] { 0 }, "");
        }

        public static SingleColumnType ConstantDataSignal(BarTable bt, int i, ISingleData analysis, double constant)
            => ConstantDataSignal(bt, i, analysis.Column_Result, constant);

        public static SingleColumnType ConstantDataSignal(BarTable bt, int i, NumericColumn column, double constant)
        {
            double value = bt[i, column];
            double last_value = bt[i - 1, column];

            if (!double.IsNaN(value) && !double.IsNaN(last_value))
            {
                if (last_value > constant)
                {
                    if (value > constant)
                    {
                        return SingleColumnType.Above;
                    }
                    else if (value < constant)
                    {
                        return SingleColumnType.CrossDown;
                    }
                    else if (value == constant)
                    {
                        return SingleColumnType.EnterFromAbove;
                    }
                }
                else if (last_value == constant)
                {
                    if (value > constant)
                    {
                        return SingleColumnType.ExitAbove;
                    }
                    else if (value < constant)
                    {
                        return SingleColumnType.ExitBelow;
                    }
                    else if (value == constant)
                    {
                        return SingleColumnType.Within;
                    }
                }
                else if (last_value < constant)
                {
                    if (value > constant)
                    {
                        return SingleColumnType.CrossUp;
                    }
                    else if (value < constant)
                    {
                        return SingleColumnType.Below;
                    }
                    else if (value == constant)
                    {
                        return SingleColumnType.EnterFromBelow;
                    }
                }
            }

            return SingleColumnType.None;
        }

        public static SingleColumnType ConstantDataSignal(BarTable bt, int i, ISingleData analysis, Range<double> range)
            => ConstantDataSignal(bt, i, analysis.Column_Result, range);

        public static SingleColumnType ConstantDataSignal(BarTable bt, int i, NumericColumn column, Range<double> range)
        {
            double value = bt[i, column];
            double last_value = bt[i - 1, column];

            if (!double.IsNaN(value) && !double.IsNaN(last_value))
            {
                if (last_value > range.Max)
                {
                    if (value > range.Max)
                    {
                        return SingleColumnType.Above;
                    }
                    else if (value < range.Min)
                    {
                        return SingleColumnType.CrossDown;
                    }
                    else if (value <= range.Max && value >= range.Min)
                    {
                        return SingleColumnType.EnterFromAbove;
                    }
                }
                else if (last_value >= range.Min && last_value <= range.Max)
                {
                    if (value > range.Max)
                    {
                        return SingleColumnType.ExitAbove;
                    }
                    else if (value < range.Min)
                    {
                        return SingleColumnType.ExitBelow;
                    }
                    else if (value <= range.Max && value >= range.Min)
                    {
                        return SingleColumnType.Within;
                    }
                }
                else if (last_value < range.Min)
                {
                    if (value > range.Max)
                    {
                        return SingleColumnType.CrossUp;
                    }
                    else if (value < range.Min)
                    {
                        return SingleColumnType.Below;
                    }
                    else if (value <= range.Max && value >= range.Min)
                    {
                        return SingleColumnType.EnterFromBelow;
                    }
                }
            }

            return SingleColumnType.None;
        }

        #endregion Constant Data Tools
    }
}
