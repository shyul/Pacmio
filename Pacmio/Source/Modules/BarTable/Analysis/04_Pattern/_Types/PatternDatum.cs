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

namespace Pacmio
{
    public abstract class PatternDatum : ILevelDatum
    {
        public Range<double> TotalLevelRange { get; set; }

        public Range<double> TotalStrengthRange { get; } = new(double.MaxValue, double.MinValue);

        public List<Level> Levels { get; } = new();
    }
}
