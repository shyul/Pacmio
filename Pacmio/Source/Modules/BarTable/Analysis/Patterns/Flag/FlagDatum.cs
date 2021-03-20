/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio.Analysis
{
    public class FlagDatum //: PatternDatum
    {
        public FlagDatum(FlagType type)
        {
            Type = type;
        }

        public FlagType Type { get; }

        public Range<double> RunUpRange { get; set; }

        public Range<double> PullBackRange { get; set; }

        public double PullBackRatio { get; set; }

        public double BreakOutLevel { get; set; }





        public Range<double> TotalRange { get; set; }

        public List<IPatternObject> PatternObjects { get; } = new();

        //public Dictionary<double, double> KeyLevels { get; } = new();
        public List<TrendLine> TrendLines { get; } = new();
    }
}
