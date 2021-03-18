/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;
using System.Windows.Forms;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public abstract class PeakAnalysis : BarAnalysis
    {
        public int MaximumPeakProminence { get; protected set; }

        /// <summary>
        /// Mainly for tag display
        /// </summary>
        public int MinimumPeakProminence { get; protected set; } = 5;

        public DatumColumn Column_PeakTags { get; protected set; }

        public override void Update(BarAnalysisPointer bap) // Cancellation Token should be used
        {
            int count = bap.Count;
            if (!bap.IsUpToDate && count > 0)
            {
                bap.StopPt = count - 1;  // bap.StopPt = count - MinimumPeakProminenceForAnalysis + 1;
                int min_peak_start = count - MaximumPeakProminence * 2 - 1;

                if (bap.StartPt > min_peak_start)
                    bap.StartPt = min_peak_start;

                if (bap.StartPt < 0)
                    bap.StartPt = 0;

                Calculate(bap);
                bap.StartPt = bap.StopPt; //bap.StartPt = bap.StopPt = count;
                bap.StopPt++;
            }
        }

        public Color Color { get => UpperColor; set => UpperColor = value; }

        public Color UpperColor
        {
            get => ColumnSeries.Color;

            set
            {
                Color c = value;
                ColumnSeries.Color = c.GetBrightness() < 0.6 ? c.Brightness(0.85f) : c.Brightness(-0.85f);
                ColumnSeries.EdgeColor = ColumnSeries.TextTheme.ForeColor = c;
            }
        }

        public Color LowerColor
        {
            get => ColumnSeries.LowerColor;

            set
            {
                Color c = value;
                ColumnSeries.LowerColor = c.GetBrightness() < 0.6 ? c.Brightness(0.85f) : c.Brightness(-0.85f);
                ColumnSeries.LowerEdgeColor = ColumnSeries.LowerTextTheme.ForeColor = c;
            }
        }

        public Series MainSeries => ColumnSeries;

        public AdColumnSeries ColumnSeries { get; protected set; }

        public bool ChartEnabled { get => Enabled && ColumnSeries.Enabled; set => ColumnSeries.Enabled = value; }

        public int SeriesOrder { get => ColumnSeries.Order; set => ColumnSeries.Order = value; }

        public bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; protected set; }

        public float AreaRatio { get; set; } = 8;

        public void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                BarChartArea a_gain = bc.AddArea(new BarChartArea(bc, AreaName, AreaRatio)
                {
                    HasXAxisBar = HasXAxisBar,
                });
                a_gain.AddSeries(ColumnSeries);
            }
        }
    }
}
