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

namespace Pacmio.Analysis
{
    public sealed class SingleDataSignal : BarAnalysis, ISingleComplex
    {
        // new SingleColumnAnalysis(rsi),
        public SingleDataSignal(IOscillator analysis, double range_percent = 0.05)
        {
            Column = analysis.Column_Result;
            analysis.AddChild(this);

            double range = analysis.Reference * range_percent;
            Range = new Range<double>(analysis.Reference - range, analysis.Reference + range);
            string label = "(" + Column.Name + "," + Range.ToStringShort() + ")";
            GroupName = Name = GetType().Name + label;
            //SignalColumn = new SignalColumn(Name, label) { BullishColor = iosc.UpperColor, BearishColor = iosc.LowerColor };
            //SignalColumns = new SignalColumn[] { SignalColumn };
            Column_Result = new(Name, typeof(SingleDataSignalDatum));
            //Order = iosc.Order + 1;
        }

        public SingleDataSignal(ISingleData analysis, Range<double> range)
        {
            Column = analysis.Column_Result;
            analysis.AddChild(this);

            Range = range;
            string label = "(" + Column.Name + "," + Range.ToStringShort() + ")";
            GroupName = Name = GetType().Name + label;
            Column_Result = new(Name, typeof(SingleDataSignalDatum));
        }

        public SingleDataSignal(NumericColumn column, Range<double> range)
        {
            Column = column;

            Range = range;
            string label = "(" + Column.Name + "," + Range.ToStringShort() + ")";
            GroupName = Name = GetType().Name + label;
            Column_Result = new(Name, typeof(SingleDataSignalDatum));
        }

        #region Parameters

        public override int GetHashCode() => GetType().GetHashCode() ^ Column.GetHashCode() ^ Range.GetHashCode();

        public Range<double> Range { get; set; }

        public Dictionary<Range<double>, double[]> LevelScores { get; }

        public NumericColumn Column { get; set; }

        public DatumColumn Column_Result { get; }

        public SignalColumn Column_Signal { get; }

        #endregion Parameters

        #region Calculation

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            if (bap.StartPt < 1)
            {
                bap.StartPt = 1;
            }

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                double value = b[Column];
                double last_value = bt[i - 1][Column];

                SingleDataSignalDatum d = new();
                b[Column_Result] = d;

                if (!double.IsNaN(value) && !double.IsNaN(last_value))
                {
                    if (last_value > Range)
                    {
                        if (value > Range)
                        {
                            d.Type = SingleDataSignalType.Above;
                        }
                        else if (value < Range)
                        {
                            d.Type = SingleDataSignalType.CrossDown;
                        }
                        else if (value == Range)
                        {
                            d.Type = SingleDataSignalType.EnterFromAbove;
                        }
                    }
                    else if (last_value == Range)
                    {
                        if (value > Range)
                        {
                            d.Type = SingleDataSignalType.ExitAbove;
                        }
                        else if (value < Range)
                        {
                            d.Type = SingleDataSignalType.ExitBelow;
                        }
                        else if (value == Range)
                        {
                            d.Type = SingleDataSignalType.Within;
                        }
                    }
                    else if (last_value < Range)
                    {
                        if (value > Range)
                        {
                            d.Type = SingleDataSignalType.CrossUp;
                        }
                        else if (value < Range)
                        {
                            d.Type = SingleDataSignalType.Below;
                        }
                        else if (value == Range)
                        {
                            d.Type = SingleDataSignalType.EnterFromBelow;
                        }
                    }
                }



            }
        }

        #endregion Calculation

        #region Constant Data Tools

        /*
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
        };*/

        /*
        SignalDatum sd = bt[i][SignalColumn] as SignalDatum;

        var (points, description) = ConstantDataSignal(bt, i, Column, Range, TypeToScore);

        if (i > 0)
        {
            SignalDatum sd_1 = bt[i - 1][SignalColumn] as SignalDatum;
            sd.Set(points, description, sd_1);
        }
        else
        sd.Set(points, description);*/

        public static (double[] points, string description) ConstantDataSignal(BarTable bt, int i, NumericColumn column, Range<double> range, Dictionary<SingleDataSignalType, double[]> typeToScore)
        {
            double value = bt[i, column];
            double last_value = bt[i - 1, column];

            if (!double.IsNaN(value) && !double.IsNaN(last_value))
            {
                if (last_value > range.Max)
                {
                    if (value > range.Max)
                    {
                        return (typeToScore[SingleDataSignalType.Above], "Above");
                    }
                    else if (value < range.Min)
                    {
                        return (typeToScore[SingleDataSignalType.CrossDown], "Cross Down");
                    }
                    else if (value <= range.Max && value >= range.Min)
                    {
                        return (typeToScore[SingleDataSignalType.EnterFromAbove], "Enter From Above");
                    }
                }
                else if (last_value >= range.Min && last_value <= range.Max)
                {
                    if (value > range.Max)
                    {
                        return (typeToScore[SingleDataSignalType.ExitAbove], "Exit Above");
                    }
                    else if (value < range.Min)
                    {
                        return (typeToScore[SingleDataSignalType.ExitBelow], "Exit Below");
                    }
                    else if (value <= range.Max && value >= range.Min)
                    {
                        return (typeToScore[SingleDataSignalType.Within], "Within");
                    }
                }
                else if (last_value < range.Min)
                {
                    if (value > range.Max)
                    {
                        return (typeToScore[SingleDataSignalType.CrossUp], "Cross Up");
                    }
                    else if (value < range.Min)
                    {
                        return (typeToScore[SingleDataSignalType.Below], "Below");
                    }
                    else if (value <= range.Max && value >= range.Min)
                    {
                        return (typeToScore[SingleDataSignalType.EnterFromBelow], "Enter From Below");
                    }
                }
            }

            return (new double[] { 0 }, "");
        }

        public static SingleDataSignalType ConstantDataSignal(BarTable bt, int i, ISingleData analysis, double constant)
            => ConstantDataSignal(bt, i, analysis.Column_Result, constant);

        public static SingleDataSignalType ConstantDataSignal(BarTable bt, int i, NumericColumn column, double constant)
        {
            double value = bt[i, column];
            double last_value = bt[i - 1, column];

            if (!double.IsNaN(value) && !double.IsNaN(last_value))
            {
                if (last_value > constant)
                {
                    if (value > constant)
                    {
                        return SingleDataSignalType.Above;
                    }
                    else if (value < constant)
                    {
                        return SingleDataSignalType.CrossDown;
                    }
                    else if (value == constant)
                    {
                        return SingleDataSignalType.EnterFromAbove;
                    }
                }
                else if (last_value == constant)
                {
                    if (value > constant)
                    {
                        return SingleDataSignalType.ExitAbove;
                    }
                    else if (value < constant)
                    {
                        return SingleDataSignalType.ExitBelow;
                    }
                    else if (value == constant)
                    {
                        return SingleDataSignalType.Within;
                    }
                }
                else if (last_value < constant)
                {
                    if (value > constant)
                    {
                        return SingleDataSignalType.CrossUp;
                    }
                    else if (value < constant)
                    {
                        return SingleDataSignalType.Below;
                    }
                    else if (value == constant)
                    {
                        return SingleDataSignalType.EnterFromBelow;
                    }
                }
            }

            return SingleDataSignalType.None;
        }

        public static SingleDataSignalType ConstantDataSignal(BarTable bt, int i, ISingleData analysis, Range<double> range)
            => ConstantDataSignal(bt, i, analysis.Column_Result, range);

        public static SingleDataSignalType ConstantDataSignal(BarTable bt, int i, NumericColumn column, Range<double> range)
        {
            double value = bt[i, column];
            double last_value = bt[i - 1, column];

            if (!double.IsNaN(value) && !double.IsNaN(last_value))
            {
                if (last_value > range.Max)
                {
                    if (value > range.Max)
                    {
                        return SingleDataSignalType.Above;
                    }
                    else if (value < range.Min)
                    {
                        return SingleDataSignalType.CrossDown;
                    }
                    else if (value <= range.Max && value >= range.Min)
                    {
                        return SingleDataSignalType.EnterFromAbove;
                    }
                }
                else if (last_value >= range.Min && last_value <= range.Max)
                {
                    if (value > range.Max)
                    {
                        return SingleDataSignalType.ExitAbove;
                    }
                    else if (value < range.Min)
                    {
                        return SingleDataSignalType.ExitBelow;
                    }
                    else if (value <= range.Max && value >= range.Min)
                    {
                        return SingleDataSignalType.Within;
                    }
                }
                else if (last_value < range.Min)
                {
                    if (value > range.Max)
                    {
                        return SingleDataSignalType.CrossUp;
                    }
                    else if (value < range.Min)
                    {
                        return SingleDataSignalType.Below;
                    }
                    else if (value <= range.Max && value >= range.Min)
                    {
                        return SingleDataSignalType.EnterFromBelow;
                    }
                }
            }

            return SingleDataSignalType.None;
        }

        #endregion Constant Data Tools
    }
}
