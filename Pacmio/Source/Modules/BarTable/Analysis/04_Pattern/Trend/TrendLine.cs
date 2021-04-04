/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;

namespace Pacmio
{
    /// <summary>
    /// Weight:
    /// 1. Confluence Points Count
    /// 2. Distance between Points
    /// 3. Tolerance
    /// </summary>
    public class TrendLine : IPatternObject, IEquatable<TrendLine>
    {
        public TrendLine((ApexPt pt1, ApexPt pt2) pt) : this(pt.pt1, pt.pt2) { }

        public TrendLine(ApexPt pt1, ApexPt pt2)
        {
            P1 = pt1;
            P2 = pt2;

            DeltaX = Math.Abs(X2 - X1);
            TrendRate = (Y2 - Y1) / (X2 - X1);
            Strength = DeltaX * (pt1.Strength + pt2.Strength);
        }

        public ApexPt P1 { get; }

        public int X1 => P1.Index;

        public double Y1 => P1.Level;

        public ApexPt P2 { get; }

        public int X2 => P2.Index;

        public double Y2 => P2.Level;

        public int DeltaX { get; }

        public double TrendRate { get; }

        public double Strength { get; }

        public double Level(int x3) => Y1 + (TrendRate * (x3 - X1));

        #region Equality

        public bool Equals(TrendLine other) => (other.P1.Equals(other.P1) && other.P2.Equals(other.P2)) || (other.P1.Equals(other.P2) && other.P2.Equals(other.P1));

        public static bool operator !=(TrendLine s1, TrendLine s2) => !s1.Equals(s2);

        public static bool operator ==(TrendLine s1, TrendLine s2) => s1.Equals(s2);

        public override int GetHashCode() => (P1, P2).GetHashCode();

        public override bool Equals(object other) => other is TrendLine s1 && Equals(s1);

        #endregion Equality
    }
}