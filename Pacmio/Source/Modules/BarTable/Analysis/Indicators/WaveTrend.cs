/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// https://www.tradingview.com/script/2KE8wTuF-Indicator-WaveTrend-Oscillator-WT/
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio // Can be derived from SMA
{
    public sealed class WaveTrend : BarAnalysis, ISingleData, IOscillator, IChartSeries
    {
        public WaveTrend(int interval_ch = 10, int interval_avg = 21, int interval_sl = 4, double weight = 0.015)
        {
            Interval_Channel = interval_ch;
            Interval_Average = interval_avg;
            Interval_Slow = interval_sl;
            Weight = weight;
            Interval_Channel_Multiplier = 2D / (Interval_Channel + 1D);

            string label = "(" + Interval_Channel.ToString() + "," + Interval_Average.ToString() + "," + Interval_Slow.ToString() + "," + Weight.ToString() + ")";
            Description = "Wave Trend Oscillator " + label;
            AreaName = GroupName = Name = GetType().Name + " " + label;

            ESA_Column = new NumericColumn(Name + "_ESA");
            D_Column = new NumericColumn(Name + "_D");
            DE_Column = new NumericColumn(Name + "_DE");
            HIST_Column = new NumericColumn(Name + "_HIST");

            EMA_DE = new EMA(D_Column, Interval_Channel);

            EMA_TCI = new EMA(DE_Column, Interval_Average);
            EMA_TCI.LineSeries.LegendName = GroupName;
            EMA_TCI.LineSeries.Name = Name + "_WT";
            EMA_TCI.LineSeries.Label = string.Empty;
            EMA_TCI.LineSeries.Importance = Importance.Major;
            EMA_TCI.LineSeries.DrawLimitShade = true;
            EMA_TCI.Color = Color.DarkSlateGray;

            SMA_WT2 = new SMA(EMA_TCI.Column_Result, Interval_Slow);

            Column_Result.Name = Name;
            Column_Result.Label = string.Empty;

            DotSeries = new DotSeries(SMA_WT2.Column_Result, Color.DarkOrange, 1)
            {
                Importance = Importance.Major,
                Name = Name + "_SIGNAL",
                LegendName = GroupName,
                Label = "SL",
                IsAntialiasing = true
            };

            ColumnSeries = new ColumnSeries(HIST_Column, Color.CadetBlue, Color.Teal, 50)
            {
                Importance = Importance.Minor,
                Name = Name + "_HIST",
                LegendName = GroupName,
                Label = "HIST",
                IsAntialiasing = true
            };
        }

        #region Parameters

        public override int GetHashCode() => GetType().GetHashCode() ^ Interval_Channel ^ Interval_Average ^ Interval_Slow ^ Weight.GetHashCode();

        public int Interval_Channel { get; }

        private double Interval_Channel_Multiplier { get; }

        public int Interval_Average { get; }

        public int Interval_Slow { get; }

        public double Weight { get; }

        #endregion Parameters

        #region Calculation

        public double Reference { get; set; } = 0;

        public double UpperLimit { get; set; } = 53;

        public double LowerLimit { get; set; } = -53;

        public NumericColumn ESA_Column { get; }

        public NumericColumn D_Column { get; }

        public EMA EMA_DE { get; }

        public NumericColumn DE_Column { get; }

        public EMA EMA_TCI { get; }

        public SMA SMA_WT2 { get; }

        public NumericColumn Column_Result => SMA_WT2.Column_Result;

        public NumericColumn HIST_Column { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            int startPt = bap.StartPt;
            for (int i = startPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                double typical = b.Typical;
                double typical_esa;

                if (i > 0)
                {
                    double y0 = bt[i - 1][ESA_Column];
                    b[ESA_Column] = typical_esa = (typical - y0) * Interval_Channel_Multiplier + y0;
                }
                else
                    b[ESA_Column] = typical_esa = typical;

                b[D_Column] = Math.Abs(typical - typical_esa);

                //Console.WriteLine("b[D_Column] = " + b[D_Column]);
            }

            bap.StartPt = startPt;
            EMA_DE.Update(bap);

            for (int i = startPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                double de = b[EMA_DE.Column_Result];
                if (de == 0) de = 0.001;
                //Console.WriteLine("de = " + de);

                b[DE_Column] = (b.Typical - b[ESA_Column]) / (Weight * de);
                //Console.WriteLine("b[DE_Column] = " + b[DE_Column]);
            }

            bap.StartPt = startPt;
            EMA_TCI.Update(bap);

            bap.StartPt = startPt;
            SMA_WT2.Update(bap);

            for (int i = startPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                b[HIST_Column] = b[EMA_TCI.Column_Result] - b[SMA_WT2.Column_Result];
            }
        }

        #endregion Calculation

        #region Series

        public ColumnSeries ColumnSeries { get; }

        public DotSeries DotSeries { get; }

        public Color Color { get => SMA_WT2.Color; set => SMA_WT2.Color = value; }

        public Color UpperColor { get; set; } = Color.YellowGreen;

        public Color LowerColor { get; set; } = Color.Crimson;

        public bool ChartEnabled { get => Enabled && EMA_TCI.ChartEnabled; set => EMA_TCI.ChartEnabled = ColumnSeries.Enabled = DotSeries.Enabled = value; }

        public int SeriesOrder { get => EMA_TCI.LineSeries.Order; set => EMA_TCI.LineSeries.Order = value; }

        public bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; }

        public float AreaRatio { get; set; } = 10;

        public int AreaOrder { get; set; } = 0;

        public void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                OscillatorArea a = bc.AddArea(new OscillatorArea(bc, AreaName, AreaRatio)
                {
                    Order = AreaOrder,
                    HasXAxisBar = HasXAxisBar,
                    Reference = Reference,
                    UpperLimit = UpperLimit,
                    LowerLimit = LowerLimit,
                    UpperColor = UpperColor,
                    LowerColor = LowerColor,
                    FixedTickStep_Right = 20,
                });

                a.AddSeries(ColumnSeries);
                a.AddSeries(EMA_TCI.LineSeries);
                a.AddSeries(DotSeries);
            }
        }

        #endregion Series
    }
}
