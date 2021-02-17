/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

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

    public class DivergenceIndicator : Indicator
    {
        protected override void Calculate(BarAnalysisPointer bap)
        {

        }

        #region Divergence Tools

        public static void Divergence(BarTable bt, int i, int minimumPeakMargin)
        {

        }

        #endregion Divergence Tools
    }


}
