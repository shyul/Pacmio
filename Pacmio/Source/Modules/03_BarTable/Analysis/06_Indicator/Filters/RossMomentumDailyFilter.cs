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
    public class RossMomentumDailyFilter : Indicator, IPriceRangFilter
    {
        public RossMomentumDailyFilter() 
        {
            SignalColumns = new SignalColumn[] { };
            SignalSeries = new(this);
            BarAnalysisSet = new(this);
        }

        public Range<double> PriceRange { get; } = new Range<double>(1, 10);

        public Range<double> VolumeRange { get; } = new Range<double>(1e6, double.MaxValue);

        public Range<double> FloatRange { get; } = new Range<double>(0, 2e7);

        public override IEnumerable<SignalColumn> SignalColumns { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {

        }
    }
}
