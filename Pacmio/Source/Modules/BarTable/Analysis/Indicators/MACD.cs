/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public sealed class MACD : SMA, IOscillator
    {
        public MACD(int interval_fast, int interval_slow, int interval_sl,
            MovingAverageType avgType = MovingAverageType.Exponential) :
            this(Bar.Column_Close, interval_fast, interval_slow, interval_sl, avgType)
        { }

        public MACD(NumericColumn column, int interval_fast, int interval_slow, int interval_sl,
            MovingAverageType avgType = MovingAverageType.Exponential)
            : base(column, interval_fast)
        {
            Interval = interval_fast;
            Interval_Slow = interval_slow;
            Interval_Signal = interval_sl;
            AverageType = avgType;
            Column = column;

            string label = Column is null ? "(error)" : Column == Bar.Column_Close ?

                            (AverageType == MovingAverageType.Exponential ? "(" + Interval.ToString() + "," + Interval_Slow.ToString() + "," + Interval_Signal.ToString() + ")" :
                            "(" + Interval.ToString() + "," + Interval_Slow.ToString() + "," + Interval_Signal.ToString() + "," + AverageType + ")") :

                            (AverageType == MovingAverageType.Exponential ? "(" + Column.Name + "," + Interval.ToString() + "," + Interval_Slow.ToString() + "," + Interval_Signal.ToString() + ")" :
                            "(" + Column.Name + "," + Interval.ToString() + "," + Interval_Slow.ToString() + "," + Interval_Signal.ToString() + AverageType + ")");

            AreaName = GroupName = Name = GetType().Name + label;
            Description = "Moving Average Convergence Divergence " + label;

            Column_Result = new NumericColumn(Name);
            HIST_Column = new NumericColumn(Name + "_HIST");

            switch (AverageType)
            {
                case MovingAverageType.Simple:
                    Fast_MA = new SMA(Column, Interval) { Order = Order - 1 };
                    Slow_MA = new SMA(Column, Interval_Slow) { Order = Fast_MA.Order + 1 };
                    Order = Slow_MA.Order + 1;
                    MACD_SL = new SMA(Column_Result, Interval_Signal);
                    break;
                case MovingAverageType.Smoothed:
                    Fast_MA = new SMMA(Column, Interval) { Order = Order - 1 };
                    Slow_MA = new SMMA(Column, Interval_Slow) { Order = Fast_MA.Order + 1 };
                    Order = Slow_MA.Order + 1;
                    MACD_SL = new SMMA(Column_Result, Interval_Signal);
                    break;
                case MovingAverageType.Exponential:
                    Fast_MA = new EMA(Column, Interval) { Order = Order - 1 };
                    Slow_MA = new EMA(Column, Interval_Slow) { Order = Fast_MA.Order + 1 };
                    Order = Slow_MA.Order + 1;
                    MACD_SL = new EMA(Column_Result, Interval_Signal);
                    break;
                case MovingAverageType.Weighted:
                    Fast_MA = new WMA(Column, Interval) { Order = Order - 1 };
                    Slow_MA = new WMA(Column, Interval_Slow) { Order = Fast_MA.Order + 1 };
                    Order = Slow_MA.Order + 1;
                    MACD_SL = new WMA(Column_Result, Interval_Signal);
                    break;
                case MovingAverageType.Hull:
                    Fast_MA = new HMA(Column, Interval) { Order = Order - 1 };
                    Slow_MA = new HMA(Column, Interval_Slow) { Order = Fast_MA.Order + 1 };
                    Order = Slow_MA.Order + 1;
                    MACD_SL = new HMA(Column_Result, Interval_Signal);
                    break;
            }

            Fast_MA.AddChild(this);
            Slow_MA.AddChild(this);

            LineSeries = new LineSeries(Column_Result, Color.FromArgb(255, 96, 96, 96), LineType.Default, 2)
            {
                Name = Name,
                LegendName = GroupName,
                DrawLimitShade = false,
                Importance = Importance.Major,
                IsAntialiasing = true,
                Label = " "
            };

            MACD_SL.Color = Color.Red;
            MACD_SL.LineSeries.Importance = Importance.Minor;
            MACD_SL.LineSeries.Name = Name + "_SL";
            MACD_SL.LineSeries.Label = "SL";
            MACD_SL.LineSeries.LegendName = GroupName;
            MACD_SL.LineSeries.IsAntialiasing = true;
            MACD_SL.LineSeries.Order = 100;

            ColumnSeries = new ColumnSeries(HIST_Column, Color.FromArgb(88, 168, 208), Color.FromArgb(32, 104, 136), 50)
            {
                Name = Name + "_HIST",
                LegendName = GroupName,
                Label = "HIST",
                Importance = Importance.Minor,
                Order = 200
            };
        }

        #region Parameters

        public override int GetHashCode() => GetType().GetHashCode() ^ Column.GetHashCode() ^ Interval ^ Interval_Slow ^ Interval_Signal ^ AverageType.GetHashCode();

        public int Interval_Slow { get; } = 26;

        public int Interval_Signal { get; } = 9;

        public MovingAverageType AverageType { get; } = MovingAverageType.Exponential;

        #endregion Parameters

        #region Calculation

        public double Reference { get; set; } = 0;

        public double UpperLimit { get; set; } = double.NaN;

        public double LowerLimit { get; set; } = double.NaN;

        public SMA Fast_MA { get; }

        public SMA Slow_MA { get; }

        public SMA MACD_SL { get; }

        public NumericColumn HIST_Column { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            int startPt = bap.StartPt;
            for (int i = startPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                b[Column_Result] = b[Fast_MA.Column_Result] - b[Slow_MA.Column_Result];
            }

            bap.StartPt = startPt;
            MACD_SL.Update(bap);

            for (int i = startPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                b[HIST_Column] = b[Column_Result] - b[MACD_SL.Column_Result];
            }
        }

        #endregion Calculation

        #region Series

        public Color UpperColor { get; set; } = Color.Green;

        public Color LowerColor { get; set; } = Color.OrangeRed;

        public ColumnSeries ColumnSeries { get; }

        public override bool ChartEnabled { get => Enabled && LineSeries.Enabled; set => ColumnSeries.Enabled = LineSeries.Enabled = value; }

        public int AreaOrder { get; set; } = 0;

        public override void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                OscillatorArea a = bc[AreaName] is OscillatorArea oa ? oa :
                    bc.AddArea(new OscillatorArea(bc, AreaName, AreaRatio)
                    {
                        Order = AreaOrder,
                        Reference = Reference,
                        UpperLimit = UpperLimit,
                        LowerLimit = LowerLimit,
                        UpperColor = UpperColor,
                        LowerColor = LowerColor,
                        //FixedTickStep_Right = 10,
                    });
                a.AddSeries(ColumnSeries);
                a.AddSeries(LineSeries);
                a.AddSeries(MACD_SL.LineSeries);
            }
        }

        #endregion Series
    }
}
