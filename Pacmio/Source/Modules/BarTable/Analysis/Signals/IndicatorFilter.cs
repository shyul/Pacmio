/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xu;

namespace Pacmio.Analysis
{
    public enum IndicatorFilterResultType : int
    {
        Bearish = -1,
        None = 0,
        Bullish = 1,
    }

    /// <summary>
    /// Indication: Move into Either Enter or Exit
    /// Active Indicator, yield score, and check other time frame's scores
    /// </summary>
    public abstract class IndicatorFilter : Indicator
    {
        public double HighScoreLimit { get; protected set; }

        public double LowScoreLimit { get; protected set; }
    }
}
