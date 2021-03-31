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
    public class FlagDatum : PatternDatum
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

        /// <summary>
        ///         (P2)
        ///         ||-----____(P3)
        ///        P2_B--___  ||
        ///       ||        --P3_B
        ///        |
        ///       ||
        ///       |
        ///      ||
        ///      |
        ///    (P1)         
        /// </summary>
        public ApexPt P1 { get; set; }

        public ApexPt P2 { get; set; }

        public ApexPt P2_B { get; set; }

        public ApexPt P3 { get; set; }

        public ApexPt P3_B { get; set; }

        public override IEnumerable<IPatternObject> PatternObjects => new IPatternObject[] { };
    }
}


