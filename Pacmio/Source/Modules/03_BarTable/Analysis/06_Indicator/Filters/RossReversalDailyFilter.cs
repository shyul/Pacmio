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
    public class RossReversalDailyFilter : Indicator, IPriceRangFilter
    {
        public RossReversalDailyFilter() 
        {
            SignalColumns = new SignalColumn[] { };
            SignalSeries = new(this);
            BarAnalysisSet = new(this);
        }

        public Range<double> PriceRange { get; } = new Range<double>(15, 250);

        public Range<double> VolumeRange { get; } = new Range<double>(5e5, double.MaxValue);

        // 5 dailys Volume 3e5

        public override IEnumerable<SignalColumn> SignalColumns { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {

        }
    }
}
