/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;

namespace Pacmio
{
    public class PatternPoint
    {
        public PatternPoint(int index, DateTime time, double level, double prominence, double trendStrength)
        {
            Index = index;
            Time = time;
            Level = level;
            Prominece = prominence;
            TrendStrength = trendStrength;
        }

        // X
        public int Index { get; }

        // X Time
        public DateTime Time { get; }

        // Y
        public double Level { get; }

        // Potency 1
        public double Prominece { get; }

        // Potency 2
        public double TrendStrength { get; }
    }
}
