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
    public class TrendLine : HorizontalLine
    {
        public TrendLine(IChartPattern source, PatternPoint pt1, PatternPoint pt2, int x3, double tolerance) : base(source, pt1, tolerance)
        {
            P2 = pt2;
            DeltaX = Math.Abs(X2 - X1);
            TrendRate = (Y2 - Y1) / (X2 - X1);
            Level = Y1 + (TrendRate * (x3 - X1));
        }

        public PatternPoint P2 { get; }

        public int X2 => P2.Index;

        public double Y2 => P2.Level;

        public int DeltaX { get; }

        public double TrendRate { get; }
    }
}