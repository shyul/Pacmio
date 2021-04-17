/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// Filter:
/// Price $1 to $10
/// 1M in volume, Relative Volume of 2
/// Low Float, less than 2e7 shares
/// 5 min Volume Surge 2000% (20x)
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;

namespace Pacmio.Analysis
{
    public class MomentumDailyFilter : Indicator
    {
        public MomentumDailyFilter() : base(BarFreq.Daily, PriceType.Trades)
        {
            SignalColumns = new SignalColumn[] { };
            BarAnalysisSet = new(this);
            SignalSeries = new(this);
        }

        public Range<double> PriceRange { get; } = new Range<double>(1, 10);

        public Range<double> VolumeRange { get; } = new Range<double>(1e6, double.MaxValue);

        public Range<double> FloatRange { get; } = new Range<double>(0, 2e7);

        public Relative RelativeVolume { get; } = new Relative(Bar.Column_Volume, 5);

        public override IEnumerable<SignalColumn> SignalColumns { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {

        }
    }
}
