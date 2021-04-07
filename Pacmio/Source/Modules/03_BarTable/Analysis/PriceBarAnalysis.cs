/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public sealed class PriceBarAnalysis : BarAnalysis, ISingleData, IDualData, IChartSeries
    {
        public PriceBarAnalysis() 
        {

            VolumeSeries = new AdColumnSeries(Bar.Column_Volume, Bar.Column_GainPercent, 50)
            {
                Order = int.MinValue,
                Side = AlignType.Left,
                Name = Bar.Column_Volume.Name,
                LegendName = "VOLUME",
                LegendLabelFormat = "0.##"
            };
        }

        #region Calculation

        public NumericColumn Column_Result => Bar.Column_Close;

        public NumericColumn Column_High => Bar.Column_High;

        public NumericColumn Column_Low => Bar.Column_Low;

        public Color UpperColor => Color.Green;

        public Color LowerColor => Color.Red;

        protected override void Calculate(BarAnalysisPointer bap)
        {

        }

        #endregion Calculation

        #region Series

        public void ConfigChart(BarChart bc)
        {

        }

        public CandleStickSeries PriceSeries { get; }

        public AdColumnSeries VolumeSeries { get; }

        public Color Color { get => PriceSeries.Color; set => PriceSeries.Color = value; }

        public Series MainSeries => PriceSeries;

        public string AreaName => MainBarChartArea.DefaultName;

        public bool ChartEnabled { get => Enabled && MainSeries.Enabled; set => MainSeries.Enabled = value; }

        public int DrawOrder { get => MainSeries.Order; set => MainSeries.Order = value; }


        #endregion Series
    }
}
