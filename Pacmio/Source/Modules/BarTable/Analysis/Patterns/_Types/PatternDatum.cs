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
    public class PatternDatum : IDatum, IEnumerable<IPatternObject>
    {
        public PatternDatum(double min, double max)
        {
            LevelRange = new Range<double>(min);
            LevelRange.Insert(max);
        }

        public void Add(IPatternObject p)
        {
            PatternObjects.Add(p);
            WeightRange.Insert(p.Weight);
        }

        public Range<double> LevelRange { get; }

        public Range<double> WeightRange { get; } = new(double.MaxValue, double.MinValue);

        private List<IPatternObject> PatternObjects { get; } = new();

        public IEnumerator<IPatternObject> GetEnumerator() => PatternObjects.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => PatternObjects.GetEnumerator();

        /// <summary>
        /// Acompany with the MaximumTrailingIndex, this time limits the perticular datum's effective time boundary
        /// </summary>
        public DateTime MaximumEffectiveTime { get; set; } = DateTime.MaxValue;
    }
}
