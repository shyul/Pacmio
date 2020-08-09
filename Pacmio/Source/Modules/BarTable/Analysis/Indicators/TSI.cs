/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Ref 1: https://school.stockcharts.com/doku.php?id=technical_indicators:true_strength_index
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
    public sealed class TSI : BarAnalysis, ISingleData, IOscillator, IChartSeries
    {
        public TSI(int interval = 25, int interval_2nd = 13, int interval_sl = 7, MovingAverageType avgType = MovingAverageType.Exponential)
        {
            Interval = interval;
            Interval_2nd = interval_2nd;
            Interval_Signal = interval_sl;

            AverageType = avgType;

            string label = AverageType == MovingAverageType.Exponential ?
                "(" + Interval.ToString() + "," + Interval_2nd.ToString() + "," + Interval_Signal.ToString() + ")" :
                "(" + Interval.ToString() + "," + Interval_2nd.ToString() + "," + Interval_Signal.ToString() + "," + AverageType + ")";

            AreaName = GroupName = Name = GetType().Name + label;
            Description = "True Strength Index " + label;

            APC_Column = new NumericColumn(Name + "_APC");
            Column_Result = new NumericColumn(Name);
            HIST_Column = new NumericColumn(Name + "_HIST");

            switch (AverageType)
            {
                case MovingAverageType.Simple:
                    MA_1_PC = new SMA(BarTable.GainAnalysis.Column_Gain, Interval);
                    MA_2_PC = new SMA(MA_1_PC.Column_Result, Interval_2nd);
                    MA_1_APC = new SMA(APC_Column, Interval);
                    MA_2_APC = new SMA(MA_1_APC.Column_Result, Interval_2nd);
                    MA_SL = new SMA(Column_Result, Interval_Signal);
                    break;
                case MovingAverageType.Smoothed:
                    MA_1_PC = new SMMA(BarTable.GainAnalysis.Column_Gain, Interval);
                    MA_2_PC = new SMMA(MA_1_PC.Column_Result, Interval_2nd);
                    MA_1_APC = new SMMA(APC_Column, Interval);
                    MA_2_APC = new SMMA(MA_1_APC.Column_Result, Interval_2nd);
                    MA_SL = new SMMA(Column_Result, Interval_Signal);
                    break;
                case MovingAverageType.Exponential:
                    MA_1_PC = new EMA(BarTable.GainAnalysis.Column_Gain, Interval);
                    MA_2_PC = new EMA(MA_1_PC.Column_Result, Interval_2nd);
                    MA_1_APC = new EMA(APC_Column, Interval);
                    MA_2_APC = new EMA(MA_1_APC.Column_Result, Interval_2nd);
                    MA_SL = new EMA(Column_Result, Interval_Signal);
                    break;
                case MovingAverageType.Weighted:
                    MA_1_PC = new WMA(BarTable.GainAnalysis.Column_Gain, Interval);
                    MA_2_PC = new WMA(MA_1_PC.Column_Result, Interval_2nd);
                    MA_1_APC = new WMA(APC_Column, Interval);
                    MA_2_APC = new WMA(MA_1_APC.Column_Result, Interval_2nd);
                    MA_SL = new WMA(Column_Result, Interval_Signal);
                    break;
                case MovingAverageType.Hull:
                    MA_1_PC = new HMA(BarTable.GainAnalysis.Column_Gain, Interval);
                    MA_2_PC = new HMA(MA_1_PC.Column_Result, Interval_2nd);
                    MA_1_APC = new HMA(APC_Column, Interval);
                    MA_2_APC = new HMA(MA_1_APC.Column_Result, Interval_2nd);
                    MA_SL = new HMA(Column_Result, Interval_Signal);
                    break;
            }

            MA_1_PC.ChartEnabled = MA_2_PC.ChartEnabled = MA_1_APC.ChartEnabled = MA_2_APC.ChartEnabled = false; // = MA_SL.ChartEnabled = false;

            MA_1_PC.AddChild(this);
            MA_2_PC.AddChild(this);

            LineSeries = new LineSeries(Column_Result, Color.FromArgb(255, 96, 96, 96), LineType.Default, 2)
            {
                Name = Name,
                LegendName = GroupName,
                DrawLimitShade = false,
                Importance = Importance.Major,
                IsAntialiasing = true,
                Label = " "
            };

            MA_SL.Color = Color.Red;
            MA_SL.LineSeries.Importance = Importance.Minor;
            MA_SL.LineSeries.Name = Name + "_SL";
            MA_SL.LineSeries.Label = "SL";
            MA_SL.LineSeries.LegendName = GroupName;
            MA_SL.LineSeries.IsAntialiasing = true;
            MA_SL.LineSeries.Order = 100;

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

        public override int GetHashCode() => GetType().GetHashCode() ^ Interval ^ Interval_2nd ^ AverageType.GetHashCode();

        public int Interval { get; }

        public int Interval_2nd { get; }

        public int Interval_Signal { get; }

        public MovingAverageType AverageType { get; } = MovingAverageType.Exponential;

        #endregion Parameters

        #region Calculation

        public double Reference { get; set; } = 0;

        public double UpperLimit { get; set; } = double.NaN;

        public double LowerLimit { get; set; } = double.NaN;

        public SMA MA_1_PC { get; }

        public SMA MA_2_PC { get; }

        public NumericColumn APC_Column { get; }

        public SMA MA_1_APC { get; }

        public SMA MA_2_APC { get; }

        public NumericColumn Column_Result { get; }

        public SMA MA_SL { get; }

        public NumericColumn HIST_Column { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            int startPt = bap.StartPt;

            for (int i = startPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                b[APC_Column] = Math.Abs(b.Gain);
            }

            bap.StartPt = startPt;
            MA_1_APC.Update(bap);

            bap.StartPt = startPt;
            MA_2_APC.Update(bap);

            for (int i = startPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                double ma_2_apc = b[MA_2_APC.Column_Result];
                b[Column_Result] = ma_2_apc == 0 ? 100 : 100 * b[MA_2_PC.Column_Result] / ma_2_apc;
            }

            bap.StartPt = startPt;
            MA_SL.Update(bap);

            for (int i = startPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                b[HIST_Column] = b[Column_Result] - b[MA_SL.Column_Result];
            }
        }

        #endregion Calculation

        #region Series

        public Color Color { get => UpperColor; set => UpperColor = value; }

        public Color UpperColor { get; set; } = Color.Green;

        public Color LowerColor { get; set; } = Color.OrangeRed;

        public float LineWidth { get => LineSeries.Width; set => LineSeries.Width = value; }

        public LineType LineType { get => LineSeries.LineType; set => LineSeries.LineType = value; }

        public Series MainSeries => LineSeries;

        public LineSeries LineSeries { get; }

        public ColumnSeries ColumnSeries { get; }

        public bool ChartEnabled { get => Enabled && LineSeries.Enabled; set => ColumnSeries.Enabled = LineSeries.Enabled = MA_SL.LineSeries.Enabled = value; }

        public int SeriesOrder { get => LineSeries.Order; set => LineSeries.Order = value; }

        public bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; }

        public float AreaRatio { get; set; } = 8;

        public int AreaOrder { get; set; } = 0;

        public void ConfigChart(BarChart bc)
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
                        //FixedTickStep_Right = 20,
                    });
                a.AddSeries(ColumnSeries);
                a.AddSeries(LineSeries);
                a.AddSeries(MA_SL.LineSeries);
            }
        }
        #endregion Series
    }
}