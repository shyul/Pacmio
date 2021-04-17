/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;

namespace Pacmio.Analysis
{
    public class ReversalDailyFilter : Indicator
    {
        public ReversalDailyFilter()
        {
            SignalColumns = new SignalColumn[] { };
            BarAnalysisSet = new(this);
            SignalSeries = new(this);
        }

        public Range<double> PriceRange { get; } = new Range<double>(15, 250);

        public Range<double> VolumeRange { get; } = new Range<double>(5e5, double.MaxValue);

        /// <summary>
        /// Or we can use Relative Volume of 1 ~ 1.5
        /// </summary>
        public Range<double> Volume5DAverageRange { get; } = new Range<double>(3e5, double.MaxValue);

        // 1 ~ 1.5
        public Relative RelativeVolume { get; } = new Relative(Bar.Column_Volume, 5);

        // 3e5
        public MovingAverage AverageVolume => RelativeVolume.MovingAverage;

        // 4 Consecutive Candles 5 minutes



        public override IEnumerable<SignalColumn> SignalColumns { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {

        }
    }
}
