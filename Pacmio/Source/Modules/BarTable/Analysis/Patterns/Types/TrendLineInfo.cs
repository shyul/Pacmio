/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using Xu;

namespace Pacmio
{
    public class TrendLineInfo : IEquatable<TrendLineInfo>
    {
        public IPatternAnalysis Analysis { get; set; }

        public string Name => Analysis.Name;

        public Period Length { get; set; }

        public double Weight { get; set; }

        public double Tolerance { get; set; }

        public (DateTime time, double value) Point1 { get; set; }

        public (DateTime time, double value) Point2 { get; set; }

        public string Label { get; set; }

        public ColorTheme Theme { get; set; }

        public override int GetHashCode() => Name.GetHashCode() ^ Length.GetHashCode() ^ Point1.GetHashCode() ^ Point2.GetHashCode(); // ^ Tolerance.GetHashCode();

        public bool Equals(TrendLineInfo other) => Name == other.Name && Length == other.Length && Point1 == other.Point1 && Point2 == other.Point2; // && Tolerance == other.Tolerance;

        public override bool Equals(object obj)
        {
            if (obj is TrendLineInfo tli)
                return Equals(tli);
            else
                return false;
        }

        public static bool operator ==(TrendLineInfo left, TrendLineInfo right) => left.Equals(right);
        public static bool operator !=(TrendLineInfo left, TrendLineInfo right) => !left.Equals(right);
    }
}
