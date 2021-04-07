/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// BarTable Data Types
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public class LevelRange : Range<double>
    {
        public LevelRange(Level lvl, double tolerancePercent)
            : base(lvl.LevelValue * (1 - tolerancePercent), lvl.LevelValue * (1 + tolerancePercent))
        {
            Strength = lvl.Strength;
        }

        public LevelRange() : base(double.MaxValue, double.MinValue) { }

        public void Insert(Level lvl)
        {
            Insert(lvl.LevelValue);
            Strength += lvl.Strength;
        }

        public double Distance(double level)
        {
            if (level > Maximum)
                return level - Maximum;
            else if (level < Minimum)
                return level - Minimum;
            else
                return 0;
        }

        public double DistancePercent(double level) => Distance(level) / level;

        public double LevelValue => (Maximum + Minimum) / 2;

        public double Strength { get; private set; } = 0;
    }
}
