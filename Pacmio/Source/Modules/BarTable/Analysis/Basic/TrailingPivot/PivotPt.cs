/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using Xu;

namespace Pacmio.Analysis
{
    public class PivotPt : IEquatable<PivotPt>
    {
        public PivotPt(int index, DateTime time, double level, double strength) // double prominence, double trendStrength)
        {
            Index = index;
            Time = time;
            Level = level;
            Strength = strength;
        }

        // X
        public int Index { get; }

        // X Time
        public DateTime Time { get; }

        // Y
        public double Level { get; }

        // Z
        public double Strength { get; }

        public bool Equals(PivotPt other) => Index == other.Index && Level == other.Level;

        public static bool operator !=(PivotPt s1, PivotPt s2) => !s1.Equals(s2);

        public static bool operator ==(PivotPt s1, PivotPt s2) => s1.Equals(s2);

        public override int GetHashCode() => (Index, Level).GetHashCode();

        public override bool Equals(object other) => other is PivotPt s1 && Equals(s1);
    }
}
