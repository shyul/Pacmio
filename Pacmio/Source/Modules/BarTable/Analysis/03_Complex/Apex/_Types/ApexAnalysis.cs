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
    public abstract class ApexAnalysis : BarAnalysis, ITagAnalysis//, IDualData
    {
        public int MaximumPeakProminence { get; protected set; }

        /// <summary>
        /// Mainly for tag display
        /// </summary>
        public int MinimumPeakProminence { get; protected set; } = 5;

        /// <summary>
        /// Will use as a reference for pattern momo / reversal
        /// </summary>
        public abstract NumericColumn Column_High { get; }

        /// <summary>
        /// Will use as a reference for pattern momo / reversal
        /// </summary>
        public abstract NumericColumn Column_Low { get; }

        public DatumColumn Column_PeakTags { get; protected set; }

        public override void Update(BarAnalysisPointer bap) // Cancellation Token should be used
        {
            if (!bap.IsUpToDate)
            {
                bap.StopPt = bap.Count;

                int min_peak_start = bap.Count - MaximumPeakProminence * 2 - 1;
                if (bap.StartPt > min_peak_start)
                    bap.StartPt = min_peak_start;

                if (bap.StartPt < 0)
                    bap.StartPt = 0;
                else if (bap.StartPt > bap.StopPt)
                    bap.StartPt = bap.StopPt - 1;

                Calculate(bap);
                bap.StartPt = bap.Count;
            }
        }

        public Color UpperColor
        {
            get => UpperTagTheme.ForeColor;

            set
            {
                Color c = value;
                UpperTagTheme.FillColor = c.GetBrightness() < 0.6 ? c.Brightness(0.85f) : c.Brightness(-0.85f);
                UpperTagTheme.EdgeColor = UpperTagTheme.ForeColor = c;
            }
        }

        public Color LowerColor
        {
            get => LowerTagTheme.ForeColor;

            set
            {
                Color c = value;
                LowerTagTheme.FillColor = c.GetBrightness() < 0.6 ? c.Brightness(0.85f) : c.Brightness(-0.85f);
                LowerTagTheme.EdgeColor = LowerTagTheme.ForeColor = c;
            }
        }

        public ColorTheme UpperTagTheme { get; } = new ColorTheme();

        public ColorTheme LowerTagTheme { get; } = new ColorTheme();

        public virtual string AreaName { get; protected set; } = MainBarChartArea.DefaultName;

        public virtual void ConfigChart(BarChart bc) { }
    }
}
