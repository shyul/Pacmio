/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

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
