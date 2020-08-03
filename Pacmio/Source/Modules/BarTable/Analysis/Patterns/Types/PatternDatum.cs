/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
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
    public class PatternDatum
    {
        public string AreaName { get; }

        public List<TrendLine> TrendLines { get; } = new List<TrendLine>();


        // Vectors of the pattern
    }
}
