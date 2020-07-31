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
    public class TrendLine
    {
        public TrendLine(DateTime startTime, double startLevel, double trendRate, double tolerance, double weight, bool isLogarithmic) 
        {
            StartTime = startTime;
            StartLevel = startLevel;
            TrendRate = trendRate;
            IsLogarithmic = isLogarithmic;
            Weight = weight;
            Tolerance = tolerance;
        }


        public DateTime StartTime { get; }

        public double StartLevel { get; }

        /// <summary>
        /// Increase or decresse per each Bar.... at BarFreq...
        /// </summary>
        public double TrendRate { get; }

        public double Tolerance { get; }

        public double Weight { get; }

        public bool IsLogarithmic { get; }

        // Utilities for Trend Line crossing, approaching, *** fake breakout, estimate the price target (distance) signals...
        /// Up Trend is the indication, a pattern is the varlidation
    }
}
