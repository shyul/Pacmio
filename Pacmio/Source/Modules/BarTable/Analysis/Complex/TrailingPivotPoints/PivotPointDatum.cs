/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;

namespace Pacmio
{
    public class PivotPointDatum
    {
        public PivotPointDatum(int index, DateTime time, double level, double prominence, double trendStrength)
        {
            Index = index;
            Time = time;
            Level = level;
            Prominece = prominence;
            TrendStrength = trendStrength;
        }

        public int Index { get; }

        public DateTime Time { get; }

        public double Level { get; }

        public double Prominece { get; }

        public double TrendStrength { get; }
    }
}
