/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// 3 Criteria for Finding Swing Stocks
/// https://www.youtube.com/watch?v=GHG3Kf-FYvw
/// 
/// 3 (Powerful) Swing Trading Strategies
/// https://www.youtube.com/watch?v=MK2V6GKBmf0
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xu;

namespace Pacmio.Analysis
{
    public enum FilterType : int
    {
        Bearish = -1,
        None = 0,
        Bullish = 1,
    }

    /// <summary>
    /// Indication: Move into Either Enter or Exit
    /// Active Indicator, yield score, and check other time frame's scores
    /// </summary>
    public abstract class IndicatorFilter : Indicator
    {
        public abstract double HighScoreLimit { get; }

        public abstract double LowScoreLimit { get; }

        public static (double BullishPercent, double BearishPercent) Evaluate(IndicatorFilter indf, BarTable bt)
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

        public static (double HighScorePercent, double LowScorePercent) Evaluate2(IndicatorFilter indf, BarTable bt)
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
    }
}
