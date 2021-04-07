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
using System.Linq;
using Xu;

namespace Pacmio.Analysis
{
    public class LevelRangeDatum : IDatum, IEnumerable<LevelRange>
    {
        public LevelRangeDatum(ILevelDatum ild, double tolerancePercent) 
        {
            ild.Levels.ForEach(n => Insert(n, tolerancePercent));
        }

        private List<LevelRange> List { get; } = new List<LevelRange>();

        public IEnumerator<LevelRange> GetEnumerator() => List.OrderBy(n => n.Strength).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Insert(Level lvl, double tolerancePercent)
        {
            double level = lvl.LevelValue;

            if (List.Where(n => Math.Abs(n.DistancePercent(level)) < tolerancePercent).OrderBy(n => Math.Abs(n.Distance(level))).FirstOrDefault() is LevelRange lr)
            {
                lr.Insert(lvl);
            }
            else
            {
                List.Add(new LevelRange(lvl, tolerancePercent));
            }
        }

        public double Strength(Range<double> r) => List.Where(n => n.Intersects(r)).Select(n => n.Strength).Sum();
    }
}
