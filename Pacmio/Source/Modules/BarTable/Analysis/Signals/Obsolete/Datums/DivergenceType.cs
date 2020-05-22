/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public enum DivergenceType : int
    {
        None = 0,
        /// <summary>
        /// 1. Price downtrend, lower lows
        /// 2. Indicator up trend, higher lows
        /// </summary>
        Positive, // a.k.a: Bullish, exit short positions, enter long position

        /// <summary>
        /// 1. Price uptrend, higher highs
        /// 2. Indicator lower highs
        /// </summary>
        Negative, // aka Bearish: Exit long position, or start short position 

        HiddenPositive,

        HiddenNegative
    }
}
