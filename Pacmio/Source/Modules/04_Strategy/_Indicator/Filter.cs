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
    public sealed class Filter : Indicator
    {
        public Filter()
        {
            SignalColumns = new SignalColumn[] { };
            SignalSeries = new(this);
            BarAnalysisSet = new(this);
        }

        public Range<double> PriceRange { get; } = new Range<double>(1, 10);

        public Range<double> VolumeRange { get; } = new Range<double>(1e6, double.MaxValue);

        public Relative RelativeVolume { get; } = new Relative(Bar.Column_Volume, 5);

        public override IEnumerable<SignalColumn> SignalColumns { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {

        }

        public (IEnumerable<Bar> BullishBars, IEnumerable<Bar> BearishBars) RunFilter(BarTableSet bts, Period pd) => RunFilter(bts, FilterIndicator, pd, FilterTimeFrame.freq, FilterTimeFrame.type);

        public (MultiPeriod bullish, MultiPeriod bearish) RunFilterMultiPeriod(BarTableSet bts, Period pd) => RunFilterMultiPeriod(bts, FilterIndicator, pd, FilterTimeFrame.freq, FilterTimeFrame.type);

        public static (IEnumerable<Bar> BullishBars, IEnumerable<Bar> BearishBars) RunFilter(BarTableSet bts, Indicator filter, Period pd, BarFreq freq = BarFreq.Daily, DataType type = DataType.Trades)
        {
            BarTable bt = bts[freq, type];

            BarAnalysisSet bas = filter.BarAnalysisSet;
            bt.CalculateRefresh(bas);

            Indicator ind = filter;
            var BullishBars = bt.Bars.Where(b => pd.Contains(b.Time) && b.GetSignalScore(ind).Bullish >= ind.BullishPointLimit);
            var BearishBars = bt.Bars.Where(b => pd.Contains(b.Time) && b.GetSignalScore(ind).Bearish <= ind.BearishPointLimit);

            return (BullishBars, BearishBars);
        }

        public static (MultiPeriod bullish, MultiPeriod bearish) RunFilterMultiPeriod(BarTableSet bts, Indicator filter, Period pd, BarFreq freq = BarFreq.Daily, DataType type = DataType.Trades)
        {
            var (BullishBars, BearishBars) = RunFilter(bts, filter, pd, freq, type);

            MultiPeriod bullish = new MultiPeriod();
            MultiPeriod bearish = new MultiPeriod();
            BullishBars.RunEach(n => bullish.Add(ToDailyPeriod(n.Period)));
            BearishBars.RunEach(n => bearish.Add(ToDailyPeriod(n.Period)));

            return (bullish, bearish);
        }

        public static Period ToDailyPeriod(Period pd) => new Period(pd.Start.Date, pd.Stop.AddDays(1).Date);
    }
}
