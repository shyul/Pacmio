/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
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
    public class PivotLine : PivotLevel
    {
        public PivotLine(Pivot startPoint, Pivot stopPoint) : base(startPoint)
        {
            P2 = stopPoint;
            Distance = Math.Abs(X2 - X1);
            TrendRate = (Y2 - Y1) / (X2 - X1);
        }

        public Pivot P2 { get; }

        public int X2 => P2.Index;

        public double Y2 => P2.Level;

        public int Distance { get; }

        /// <summary>
        /// Increase or decresse per each Bar.... at BarFreq...
        /// </summary>
        public double TrendRate { get; }

    }
}