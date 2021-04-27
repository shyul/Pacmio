/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Filter 1) Signal Set; 2) Signal Threshold Range<double>(), within? / outside?; 3) Ranking Column?
/// 
/// ***************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio
{
    public sealed class Filter
    {
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

        public FilterScanResult RunScan(BarTableSet bts, Period pd)
        {
            bts.CalculateRefresh(SignalAnalysisSet);
            BarTable bt = bts[SignalAnalysisSet.TimeFrameList.Last()];
            var allbars = bt.Bars.Where(b => pd.Contains(b.Time));
            var bars = allbars.Where(n => Calculate(n).pass);
            return new(bts.Contract, bars, allbars.Count());
        }
    }

    public class FilterScanResult
    {
        public FilterScanResult(Contract c, IEnumerable<Bar> bars, int totalCount)
        {
            Contract = c;
            Count = bars.Count();
            TotalCount = totalCount;
            Percent = TotalCount > 0 ? (Count * 100 / TotalCount) : 0;

            bars.RunEach(n =>
            {
                var pd_bull = ToDailyPeriod(n.Period);
                Periods.Add(pd_bull);
            });
        }

        public Contract Contract { get; }

        public MultiPeriod Periods { get; } = new MultiPeriod();

        public int TotalCount { get; }

        public int Count { get; }

        public double Percent { get; }

        public static Period ToDailyPeriod(Period pd) => new Period(pd.Start.Date, pd.Stop.AddDays(1).Date);
    }
}
