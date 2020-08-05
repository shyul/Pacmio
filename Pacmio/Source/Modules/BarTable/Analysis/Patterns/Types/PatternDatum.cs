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
        public PatternDatum(BarFreq barFreq, string areaName) 
        {
            BarFreq = barFreq;
            AreaName = areaName;
        }

        public BarFreq BarFreq { get; }
        // Vectors of the pattern

        public string AreaName { get; }



        public List<TrendLine> TrendLines { get; } = new List<TrendLine>();

        public List<(double Weight, double Level)> Levels { get; } = new List<(double Weight, double Level)>();

        public double MaxTrendLineWeight { get; set; }
    }
}
