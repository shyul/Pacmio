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

namespace Pacmio.Analysis
{
    public class LevelRange : Range<double>, ILevel
    {
        public LevelRange(ILevel lvl, double tolerancePercent)
            : base(lvl.Value * (1 - tolerancePercent), lvl.Value * (1 + tolerancePercent))
        {
            Strength = lvl.Strength;
        }

        public LevelRange() : base(double.MaxValue, double.MinValue) { }

        public void Insert(ILevel lvl)
        {
            Insert(lvl.Value);
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

        public double Value => (Maximum + Minimum) / 2;

        public double Strength { get; private set; } = 0;
    }
}
