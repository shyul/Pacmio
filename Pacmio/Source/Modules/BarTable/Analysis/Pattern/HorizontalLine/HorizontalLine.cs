/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;

namespace Pacmio.Analysis
{
    public class HorizontalLine : IPatternObject
    {
        public HorizontalLine(PivotPt pt1)
        {
            P1 = pt1;
            Strength = pt1.Strength * 2;
        }

        public PivotPt P1 { get; }

        public int X1 => P1.Index;

        public double Y1 => P1.Level;

        public double Strength { get; set; } = 1;

        public double Level(int _) => P1.Level;

        #region Equality

        public bool Equals(HorizontalLine other) => other.P1.Equals(other.P1);

        public static bool operator !=(HorizontalLine s1, HorizontalLine s2) => !s1.Equals(s2);

        public static bool operator ==(HorizontalLine s1, HorizontalLine s2) => s1.Equals(s2);

        public override int GetHashCode() => P1.GetHashCode();

        public override bool Equals(object other) => other is HorizontalLine s1 && Equals(s1);

        #endregion Equality
    }
}
