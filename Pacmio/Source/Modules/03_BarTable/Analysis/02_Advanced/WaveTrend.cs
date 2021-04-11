/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
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

namespace Pacmio.Analysis
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
            Interval_Average_Multiplier = 2D / (Interval_Average + 1D);

            string label = "(" + Interval_Channel.ToString() + "," + Interval_Average.ToString() + "," + Interval_Slow.ToString() + "," + Weight.ToString() + ")";
            Description = "Wave Trend Oscillator " + label;
            AreaName = GroupName = Name = GetType().Name + " " + label;

            ESA_Column = new NumericColumn(Name + "_ESA");
            D_Column = new NumericColumn(Name + "_D");
            DE_Column = new NumericColumn(Name + "_DE");
            HIST_Column = new NumericColumn(Name + "_HIST");
            EMA_DE = new NumericColumn(Name + "_EMA_DE");
            EMA_TCI = new NumericColumn(Name + "_EMA_TCI");
            SMA_WT2 = new NumericColumn(Name, string.Empty);


            EMA_TCI_LineSeries = new LineSeries(EMA_TCI, Color.DarkSlateGray, LineType.Default, 1.5f)
            {
                Name = Name + "_WT",
                Label = string.Empty,
                LegendName = GroupName,
                Importance = Importance.Major,
                IsAntialiasing = true,
                DrawLimitShade = true,
            };

            DotSeries = new DotSeries(SMA_WT2, Color.DarkOrange, 1)
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

        private double Interval_Average_Multiplier { get; }

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

        public NumericColumn EMA_DE { get; }

        public NumericColumn DE_Column { get; }

        public NumericColumn EMA_TCI { get; }

        public NumericColumn SMA_WT2 { get; }

        public NumericColumn Column_Result => SMA_WT2;

        public NumericColumn HIST_Column { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
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
            }

            EMA.Calculate(bt, D_Column, EMA_DE, bap.StartPt, bap.StopPt, Interval_Channel_Multiplier);

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                double de = b[EMA_DE];
                if (de == 0) de = 0.001;
                b[DE_Column] = (b.Typical - b[ESA_Column]) / (Weight * de);
            }

            EMA.Calculate(bt, DE_Column, EMA_TCI, bap.StartPt, bap.StopPt, Interval_Average_Multiplier);
            SMA.Calculate(bt, EMA_TCI, SMA_WT2, bap.StartPt, bap.StopPt, Interval_Slow);

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                b[HIST_Column] = b[EMA_TCI] - b[SMA_WT2];
            }
        }

        #endregion Calculation

        #region Series

        public Series MainSeries => EMA_TCI_LineSeries;

        public LineSeries EMA_TCI_LineSeries { get; }

        public ColumnSeries ColumnSeries { get; }

        public DotSeries DotSeries { get; }

        public Color Color { get => DotSeries.Color; set => DotSeries.Color = value; }

        public Color UpperColor { get; set; } = Color.YellowGreen;

        public Color LowerColor { get; set; } = Color.Crimson;

        public bool ChartEnabled { get => Enabled && EMA_TCI_LineSeries.Enabled; set => Enabled = EMA_TCI_LineSeries.Enabled = ColumnSeries.Enabled = DotSeries.Enabled = value; }

        public int DrawOrder { get => MainSeries.Order; set => MainSeries.Order = value; }

        public bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; }

        public float AreaRatio { get; set; } = 10;

        public int AreaOrder { get; set; } = 0;

        public void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                BarChartOscillatorArea a = bc[AreaName] is BarChartOscillatorArea oa ? oa :
                    bc.AddArea(new BarChartOscillatorArea(bc, AreaName, AreaRatio)
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
                a.AddSeries(MainSeries);
                a.AddSeries(DotSeries);
            }
        }

        #endregion Series
    }
}
