/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// Filter:
/// Price $1 to $10
/// 1M in volume, Relative Volume of 2
/// Low Float, less than 2e7 shares
/// 5 min Volume Surge 2000% (20x)
/// 
/// Entry
/// 1. Ideally Next to 9 EMA, or 20 EMA
/// 2. Float under 50 million shares, and prefer under 10 Million shares OR trade with VWAP
/// 3. Bid and ask with in "5 cents" tight spread
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;

namespace Pacmio.Analysis
{
    public class MomentumStrategy : Strategy
    {
        public MomentumStrategy(BarFreq barFreq, PriceType type) : base(barFreq, type)
        {

            DailyIndicator = new();

            IndicatorSet.Add(DailyIndicator);

            SignalColumns = new SignalColumn[] { };
            SignalSeries = new(this);
            BarAnalysisSet = new(this);
        }

        public override IEnumerable<SignalColumn> SignalColumns { get; }

        public override Filter Filter { get; } // = new MomentumDailyFilter();

        public double MinimumVolume { get; } = 1e6;

        public MomentumDailyIndicator DailyIndicator { get; }

        /// <summary>
        /// Ideally the entry point shall close to the EMA9 or EMA20 support level.
        /// </summary>
        public MovingAverageAnalysis EMA9 { get; } = new EMA(9);

        public MovingAverageAnalysis EMA20 { get; } = new EMA(20);

        public TimeFramePricePosition TimeFramePricePosition { get; } = new TimeFramePricePosition(BarFreq.Daily);

        public TimeFrameRelativeVolume TimeFrameRelativeVolume { get; } = new TimeFrameRelativeVolume(5, BarFreq.Daily);

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            BarTableSet bts = bt.BarTableSet;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                Bar daily_b = bts[b.Time, DailyIndicator];
                double daily_relative_volume = daily_b[DailyIndicator.RelativeVolume];

                if (daily_b.Volume > 1e6 && daily_relative_volume > 2)
                {



                }
            }
        }
    }
}
