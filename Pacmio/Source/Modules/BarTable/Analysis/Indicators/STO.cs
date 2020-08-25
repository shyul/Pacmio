/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// https://school.stockcharts.com/doku.php?id=technical_indicators:stochastic_oscillator_fast_slow_and_full
/// https://docs.anychart.com/Stock_Charts/Technical_Indicators/Mathematical_Description#kdj
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
    public sealed class STO : BarAnalysis, IOscillator, IChartSeries
    {
        public STO(int k, int d, int d2)
        {
            K = k;
            D = d;
            D2 = d2;

            string label = "(" + K.ToString() + "," + D.ToString() + "," + D2.ToString() + ")";
            Description = "Stochastic Oscillator " + label;
            AreaName = GroupName = Name = GetType().Name + label;

            BoxRange = new PriceChannel(K) { ChartEnabled = false };
            BoxRange.AddChild(this);

            OSC_Column = new NumericColumn(Name + "_OSC");

            SL = new SMA(OSC_Column, D) { ChartEnabled = false };
            this.AddChild(SL);

            LineSeries_SL = new LineSeries(SL.Column_Result)
            {
                Name = Name + "_STO",
                Label = string.Empty,
                LegendName = GroupName,
                Importance = Importance.Major,
                DrawLimitShade = true,
                IsAntialiasing = true,
                Color = Color.FromArgb(255, 96, 96, 96)
            };

            SL_FULL = new SMA(Column_Result, D2) { ChartEnabled = false };
            SL.AddChild(SL_FULL);

            LineSeries_FULL = new LineSeries(SL_FULL.Column_Result)
            {
                Name = Name + "_SL",
                Label = "SL",
                LegendName = GroupName,
                Importance = Importance.Minor,
                DrawLimitShade = false,
                IsAntialiasing = true,
                Color = Color.DodgerBlue
            };
        }

        #region Parameters

        public override int GetHashCode() => GetType().GetHashCode() ^ K ^ D ^ D2; // Name.GetHashCode();

        public int K { get; } = 14;

        public int D { get; } = 3;

        public int D2 { get; } = 3;

        #endregion Parameters

        #region Calculation

        public double Reference { get; set; } = 50;

        public double UpperLimit { get; set; } = 80;

        public double LowerLimit { get; set; } = 20;

        public PriceChannel BoxRange { get; }

        public NumericColumn OSC_Column { get; }

        public NumericColumn Column_Result => SL.Column_Result;

        public SMA SL { get; }

        public SMA SL_FULL { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                double high = b[BoxRange.Column_High];
                double low = b[BoxRange.Column_Low];
                b[OSC_Column] = (high == low) ? 100.0 : 100.0 * (b.Close - low) / (high - low);
            }
        }

        #endregion Calculation

        #region Series

        public Color Color { get => LineSeries_SL.Color; set => LineSeries_SL.Color = value; }

        public Color UpperColor { get; set; } = Color.CornflowerBlue;

        public Color LowerColor { get; set; } = Color.Tomato;

        public bool ChartEnabled { get => Enabled && LineSeries_SL.Enabled; set => LineSeries_SL.Enabled = LineSeries_FULL.Enabled = value; }

        public int SeriesOrder { get => LineSeries_SL.Order; set => LineSeries_SL.Order = value; }

        public bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; }

        public float AreaRatio { get; set; } = 10;

        public int AreaOrder { get; set; } = 0;

        public Series MainSeries => LineSeries_SL;

        public LineSeries LineSeries_SL { get; }

        public LineSeries LineSeries_FULL { get; }

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
                        FixedTickStep_Right = 15,
                    });

                a.AddSeries(LineSeries_SL);
                a.AddSeries(LineSeries_FULL);
            }
        }

        #endregion Series
    }
}
