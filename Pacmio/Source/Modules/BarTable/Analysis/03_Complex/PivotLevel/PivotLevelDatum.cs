/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// BarTable Data Types
/// 
/// ***************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public class PivotLevelDatum : ILevelDatum
    {
        public double P { get; set; }

        public double S1 { get; set; }

        public double S2 { get; set; }

        public double R1 { get; set; }

        public double R2 { get; set; }

        public List<Level> Levels { get; } = new();

        //public IEnumerator<Level> GetEnumerator() => Levels.GetEnumerator();

        //IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
