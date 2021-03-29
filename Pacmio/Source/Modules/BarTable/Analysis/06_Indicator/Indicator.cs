/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// https://support.stockcharts.com/doku.php?id=scans:library:sample_scans
/// 
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
    /// <summary>
    /// Indication: Move into Either Enter or Exit
    /// Passive: Only yield signal score
    /// </summary>
    public abstract class Indicator : BarAnalysis, IChartSeries
    {
        protected Indicator()
        {
            SignalSeries = new(this);
        }

        public abstract IEnumerable<SignalColumn> SignalColumns { get; }

        public TimePeriod TimeInForce { get; set; } = TimePeriod.Full;

        public double HighScoreLimit { get; set; }

        public double LowScoreLimit { get; set; }

        public static (double BullishPercent, double BearishPercent) Evaluate(Indicator indf, BarTable bt)
        {
            int countTotal = bt.Count;
            if (countTotal > 0)
            {
                var bas = BarAnalysisSet.Create(indf);
                bt.CalculateRefresh(bas);
                var scores = bt.Bars.Select(n => n.GetSignalScore(indf));
                int countBullish = scores.Where(n => n.Bullish >= indf.HighScoreLimit).Count();
                int countBearish = scores.Where(n => n.Bearish >= indf.LowScoreLimit).Count();
                return (countBullish / countTotal, countBearish / countTotal);
            }
            else
                return (0, 0);
        }

        public static (double HighScorePercent, double LowScorePercent) Evaluate2(Indicator indf, BarTable bt)
        {
            int countTotal = bt.Count;
            if (countTotal > 0)
            {
                var bas = BarAnalysisSet.Create(indf);
                bt.CalculateRefresh(bas);
                var scores = bt.Bars.Select(n => n.GetSignalScore(indf));
                int countBullish = scores.Where(n => n.Bullish >= indf.HighScoreLimit).Count();
                int countBearish = scores.Where(n => n.Bearish >= indf.LowScoreLimit).Count();
                return (countBullish / countTotal, countBearish / countTotal);
            }
            else
                return (0, 0);
        }

        #region Series

        public Color Color { get => SignalSeries.Color; set => SignalSeries.Color = value; }

        public Series MainSeries => SignalSeries;

        public SignalSeries SignalSeries { get; protected set; }

        public virtual bool ChartEnabled { get => Enabled && SignalSeries.Enabled; set => SignalSeries.Enabled = value; }

        public int DrawOrder { get => SignalSeries.Order; set => SignalSeries.Order = value; }

        public virtual bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; protected set; }

        public float AreaRatio { get; set; } = 8;

        public int AreaOrder { get; set; } = 0;

        public virtual void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                SignalArea a = bc.AddArea(new SignalArea(bc, SignalSeries)
                {
                    Order = AreaOrder,
                    HasXAxisBar = HasXAxisBar,
                });
                a.AddSeries(SignalSeries);
            }
        }

        #endregion Series
    }

    public enum FilterType : int
    {
        Bearish = -1,
        None = 0,
        Bullish = 1,
    }
}
