/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// ***************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio.Analysis
{
    public abstract class PatternDatum : IDatum
    {
        public Range<double> TotalRange { get; set; }

        public Range<double> StrengthRange { get; } = new(double.MaxValue, double.MinValue);

        public abstract IEnumerable<IPatternObject> PatternObjects { get; }
    }
}
