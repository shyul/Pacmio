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
    public class TrendLineDatum
    {
        public bool IsLogarithmic { get; }

        public (DateTime time, double value) Point1 { get; }

        public (DateTime time, double value) Point2 { get; }

        public double Weight { get; }

        public double Tolerance { get; }

        public string AreaName { get; }
    }
}
