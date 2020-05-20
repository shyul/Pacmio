/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// 
/// ***************************************************************************

using IbXmlScannerParameter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public sealed class ConstantData : BarAnalysis, ISignalAnalysis
    {
        public ConstantData(ISingleData analysis, Range<double> range)
        {
            Column = analysis.Result_Column;
            Range = range;

            if (analysis is IOscillator iosc)
            {
                BullishTheme = new ColorTheme(iosc.UpperColor, iosc.UpperColor.Opaque(64));
                BearishTheme = new ColorTheme(iosc.LowerColor, iosc.LowerColor.Opaque(64));
            }

            string label = "[" + Column.Name + "," + Range.ToString() + "]";

            Name = GetType().Name + label;
            Description = GetType().Name + " " + label;
            GroupName = Description + ": ";

            Signal_Column = new ObjectColumn(Name, typeof(DualDataSignalDatum)) { Label = label };
        }

        public ConstantData(NumericColumn column, Range<double> range)
        {
            Column = column;
            Range = range;

            string label = "[" + Column.Name + "," + Range.ToString() + "]";

            Name = GetType().Name + label;
            Description = GetType().Name + " " + label;
            GroupName = Description + ": ";

            Signal_Column = new ObjectColumn(Name, typeof(DualDataSignalDatum)) { Label = label };
        }

        #region Parameters

        public override int GetHashCode() => GetType().GetHashCode() ^ Column.GetHashCode() ^ Range.GetHashCode();

        public NumericColumn Column { get; private set; }

        public Range<double> Range { get; private set; }

        #endregion Parameters

        #region Calculation

        public ObjectColumn Signal_Column { get; private set; }

        public readonly Dictionary<ConstantDataType, double> TypeToScore = new Dictionary<ConstantDataType, double>
        {
            { ConstantDataType.Above, 0.25 },
            { ConstantDataType.CrossDown, 3 },
            { ConstantDataType.EnterFromAbove, 0.5 },
            { ConstantDataType.ExitAbove, 1 },
            { ConstantDataType.ExitBelow, 1 },
            { ConstantDataType.Within, 0 },
            { ConstantDataType.CrossUp, 3 },
            { ConstantDataType.Below, 0.25 },
            { ConstantDataType.EnterFromBelow, 0.5 },
        };

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            if (bap.StartPt < 1)
            {
                bap.StartPt = 1;
            }
            else
            {
                bt[0][Signal_Column] = new ConstantSignalDatum();
            }

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                ConstantSignalDatum dsd = new ConstantSignalDatum();
                b[Signal_Column] = dsd;

                double value = bt[i, Column];
                double last_value = bt[i - 1, Column];

                if (!double.IsNaN(value) && !double.IsNaN(last_value))
                {
                    if (last_value > Range.Max)
                    {
                        if (value > Range.Max)
                        {
                            dsd.Type = ConstantDataType.Above;
                            dsd.BullishScore += TypeToScore[ConstantDataType.Above];
                        }
                        else if (value < Range.Min)
                        {
                            dsd.Type = ConstantDataType.CrossDown;
                            dsd.BearishScore += TypeToScore[ConstantDataType.CrossDown];
                        }
                        else if (value <= Range.Max && value >= Range.Min)
                        {
                            dsd.Type = ConstantDataType.EnterFromAbove;
                            dsd.BearishScore += TypeToScore[ConstantDataType.EnterFromAbove];
                        }
                    }
                    else if (last_value >= Range.Min && last_value <= Range.Max)
                    {
                        if (value > Range.Max)
                        {
                            dsd.Type = ConstantDataType.ExitAbove;
                            dsd.BullishScore += TypeToScore[ConstantDataType.ExitAbove];
                        }
                        else if (value < Range.Min)
                        {
                            dsd.Type = ConstantDataType.ExitBelow;
                            dsd.BearishScore += TypeToScore[ConstantDataType.ExitBelow];
                        }
                        else if (value <= Range.Max && value >= Range.Min)
                        {
                            dsd.Type = ConstantDataType.Within;
                        }
                    }
                    else if (last_value < Range.Min)
                    {
                        if (value > Range.Max)
                        {
                            dsd.Type = ConstantDataType.CrossUp;
                            dsd.BullishScore += TypeToScore[ConstantDataType.CrossUp];
                        }
                        else if (value < Range.Min)
                        {
                            dsd.Type = ConstantDataType.Below;
                            dsd.BearishScore += TypeToScore[ConstantDataType.Below];
                        }
                        else if (value <= Range.Max && value >= Range.Min)
                        {
                            dsd.Type = ConstantDataType.EnterFromBelow;
                            dsd.BullishScore += TypeToScore[ConstantDataType.EnterFromBelow];
                        }
                    }
                }
            }
        }

        #endregion Calculation

        #region Series

        public Color Color { get => BullishColor; set => BullishColor = value; }

        public Color BullishColor
        {
            get => BullishTheme.ForeColor;
            set
            {
                BullishTheme.ForeColor = BullishTheme.EdgeColor = value;
                BullishTheme.FillColor = value.Opaque(64);
            }
        }

        public Color BearishColor
        {
            get => BearishTheme.ForeColor;
            set
            {
                BearishTheme.ForeColor = BearishTheme.EdgeColor = value;
                BearishTheme.FillColor = value.Opaque(64);
            }
        }

        public ColorTheme BullishTheme { get; } = new ColorTheme(Color.Teal, Color.Teal.Opaque(64));

        public ColorTheme BearishTheme { get; } = new ColorTheme(Color.Orange, Color.Peru.Opaque(64));

        #endregion Series
    }
}
