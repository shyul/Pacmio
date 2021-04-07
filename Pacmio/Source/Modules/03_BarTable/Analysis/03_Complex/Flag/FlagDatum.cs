/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio
{
    public class FlagDatum : IDatum
    {
        public FlagType Type { get; set; } = FlagType.None;

        public Range<double> TotalRange { get; } = new Range<double>(double.MaxValue, double.MinValue);

        public double LastFlagBarRangeLocation { get; set; } = double.NaN;

        public TrendLine UpperFlag { get; set; }

        public TrendLine LowerFlag { get; set; }

        public TrendLine RunUp { get; set; }
    }
}
