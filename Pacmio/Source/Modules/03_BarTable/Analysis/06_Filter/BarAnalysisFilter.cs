/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Filter 1) Signal Set; 2) Signal Threshold Range<double>(), within? / outside?; 3) Ranking Column?
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio
{
    /// <summary>
    /// Flawed, direction -> no score, but ranking!!!
    /// </summary>
    public sealed class BarAnalysisFilter : BarAnalysisSet
    {
        public BarAnalysisFilter(IEnumerable<SignalAnalysis> SignalList, double bullishLimit, double bearishLimit, NumericColumn rankColumn) : base(SignalList)
        {
            BullishLimit = bullishLimit;
            BearishLimit = bearishLimit;
            Column_Rank = rankColumn;
        }

        public NumericColumn Column_Rank { get; }

        public double BullishLimit { get; }

        public double BearishLimit { get; }

        public (bool bullish, bool bearish, double rank) Calculate(Bar b)
        {
            bool bullish = b.GetSignalScore(SignalList).Bullish >= BullishLimit;
            bool bearish = b.GetSignalScore(SignalList).Bearish <= BearishLimit;
            return (bullish, bearish, b[Column_Rank]);
        }

        public FilterScanResult RunScan(BarTableSet bts, Period pd)
        {
            bts.CalculateRefresh(this);
            BarTable bt = bts[TimeFrameList.Last()];
            var allbars = bt.Bars.Where(b => pd.Contains(b.Time));

            List<Bar> bullishBars = new();
            List<Bar> bearishBars = new();

            foreach (Bar b in allbars)
            {
                var (bullish, bearish, _) = Calculate(b);
                if (bullish) bullishBars.Add(b);
                if (bearish) bearishBars.Add(b);
            }

            return new(bts.Contract, allbars, bullishBars, bearishBars);
        }
    }
}
