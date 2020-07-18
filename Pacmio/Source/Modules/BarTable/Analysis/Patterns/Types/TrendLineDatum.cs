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
        // public identify the source of this TrendLine... 

        public bool IsLogarithmic { get; }

        public (DateTime time, double value) Start { get; }

        /// <summary>
        /// Increase or decresse per each Bar.... at BarFreq...
        /// </summary>
        public double Rate { get; }

        //public (DateTime time, double value) Point2 { get; }

        public double Weight { get; }

        public double Tolerance { get; }

        public string AreaName { get; }
    }
}
