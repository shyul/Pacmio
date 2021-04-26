/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xu;
using Pacmio.Analysis;

namespace Pacmio
{
    public sealed class Filter
    {
        // Filter 1) Signal Set; 2) Signal Threshold Range<double>(), within? / outside?; 3) Ranking Column?
        public Filter(IEnumerable<SignalAnalysis> SignalList, Range<double> signalRange, bool isPointWithin, NumericColumn rankColumn)
        {
            SignalAnalysisSet = new(SignalList);
            SignalPointRange = signalRange;
            IsPointWithin = isPointWithin;
            Column_Rank = rankColumn;
        }

        public SignalAnalysisSet SignalAnalysisSet { get; }

        public Range<double> SignalPointRange { get; }

        public bool IsPointWithin { get; }

        public NumericColumn Column_Rank { get; }

        public (bool pass, double rank) Calculate(Bar b)
        {
            bool isPass = IsPointWithin ?
            SignalPointRange.Contains(b.GetSignalScore(SignalAnalysisSet).Bullish) || SignalPointRange.Contains(-b.GetSignalScore(SignalAnalysisSet).Bearish) :
            (!SignalPointRange.Contains(b.GetSignalScore(SignalAnalysisSet).Bullish)) || (!SignalPointRange.Contains(-b.GetSignalScore(SignalAnalysisSet).Bearish));

            return (isPass, b[Column_Rank]);
        }

        public IEnumerable<Bar> RunScan(BarTableSet bts, Period pd)
        {
            bts.CalculateRefresh(SignalAnalysisSet);
            BarTable bt = bts[SignalAnalysisSet.TimeFrameList.Last()];
            var bars = bt.Bars.Where(b => pd.Contains(b.Time)).Where(n => Calculate(n).pass);
            return bars;
        }
    }
}
