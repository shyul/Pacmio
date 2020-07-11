/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// https://school.stockcharts.com/doku.php?id=technical_indicators:stochastic_oscillator_fast_slow_and_full
/// https://docs.anychart.com/Stock_Charts/Technical_Indicators/Mathematical_Description#kdj
/// 
/// ***************************************************************************

using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio
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

            BoxRange = new PriceChannel(K);
            BoxRange.AddChild(this);

            OSC_Column = new NumericColumn(Name + "_OSC");

            SL = new SMA(OSC_Column, D);
            SL.LineSeries.LegendName = GroupName;
            SL.LineSeries.Name = Name + "_STO";
            SL.LineSeries.Label = string.Empty;
            SL.LineSeries.Importance = Importance.Major;
            SL.LineSeries.DrawLimitShade = true;
            SL.Color = Color.FromArgb(255, 96, 96, 96);
            this.AddChild(SL);

            SL_FULL = new SMA(Result_Column, D2);
            SL_FULL.LineSeries.LegendName = GroupName;
            SL_FULL.LineSeries.Name = Name + "_SL";
            SL_FULL.LineSeries.Label = "SL";
            SL_FULL.LineSeries.Importance = Importance.Minor;
            SL_FULL.LineSeries.DrawLimitShade = false;
            SL_FULL.Color = Color.DodgerBlue;
            SL.AddChild(SL_FULL);
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

        public NumericColumn Result_Column => SL.Result_Column;

        public SMA SL { get; }

        public SMA SL_FULL { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                double high = b[BoxRange.High_Column];
                double low = b[BoxRange.Low_Column];
                b[OSC_Column] = (high == low) ? 100.0 : 100.0 * (b.Close - low) / (high - low);
            }
        }

        #endregion Calculation

        #region Series

        public Color Color { get => SL.Color; set => SL.Color = value; }

        public Color UpperColor { get; set; } = Color.CornflowerBlue;

        public Color LowerColor { get; set; } = Color.Tomato;

        public bool ChartEnabled { get => Enabled && SL.ChartEnabled; set => SL.ChartEnabled = SL_FULL.ChartEnabled = value; }

        public int SeriesOrder { get => SL.LineSeries.Order; set => SL.LineSeries.Order = value; }

        public bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; }

        public float AreaRatio { get; set; } = 10;

        public int AreaOrder { get; set; } = 0;

        public void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                OscillatorArea a = bc[AreaName] is OscillatorArea oa ? oa :
                    bc.AddArea(new OscillatorArea(bc, AreaName, AreaRatio)
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

                a.AddSeries(SL.LineSeries);
                a.AddSeries(SL_FULL.LineSeries);
                /*
                Area a2 = bc.AddArea(new Area(bc, MainArea.DefaultName, 10));
                a2.AddSeries(LineSeries_H);
                a2.AddSeries(LineSeries_L);*/
            }
        }

        #endregion Series
    }
}
