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
        public PatternDatum(double min, double max)
        {
            LevelRange = new Range<double>(double.MaxValue, double.MinValue);
            LevelRange.Insert(min);
            LevelRange.Insert(max);
        }
        /*
        public PatternDatum() 
        {
            LevelRange = new Range<double>(double.MaxValue, double.MinValue);
        }
        */
        public void Add(IPivot p) 
        {
            Pivots.Add(p);
            WeightRange.Insert(p.Weight);
        }
        
        public List<IPivot> Pivots { get; } = new List<IPivot>();

        public Range<double> LevelRange { get; }

        public Range<double> WeightRange { get; } = new Range<double>(double.MaxValue, double.MinValue);
    }
}
