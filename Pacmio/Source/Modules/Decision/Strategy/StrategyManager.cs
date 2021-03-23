/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Pacmio.Analysis;
using Xu;

namespace Pacmio
{
    public static class StrategyManager
    {
        public static Dictionary<Contract, IntraDayBarTableSet> BarTableSetLUT { get; } = new();





        // New Types for Bar
        // ===================
        // 1. TrendLine Type (Will be derived)
        // 2. Signal Type
        // 3. Pattern Type
        // 4. Position Status
        // 5. Better Tags
        // 
        // <<<<<<<<<< Get them from the Technical Anlysis (TA)'s purpose >>>>>>>>>>
        // 1. For example, RSI: see how the overbought and oversold are causing price reversals
        // 2. Moving average crossings. Test difference time periods

        // Condition
        // --------------------
        // Result: List of symbol matching the condition
        // 1. Volume, 
        // 2. Price Range
        // 3. Indicators + Elevation Factors

        //public Dictionary<Contract, int> Parameters = new Dictionary<Contract, int>();

        // Indication
        // --------------------
        // Elevation Factors (Bullish)
        // Deprication Factors (Bearish)
        // Signals
        // Result: Signals list, sum of all signals at the close of a Bar
        // --------------------
        // 1. A pool of bullish and bearish factor-tets: <<<<<<<<<< Get them from the Technical Anlysis (TA)'s purpose >>>>>>>>>>
        // 2. Test each of them individually for each contract. Signal clarity -> bullish signal to bullish upside rate / Bearish to downside rate. Delay, strength, winrate
        // 3. Classic combination: Moving Average Cross (s) + Oscillator (s)
        // 4. Combined Score to simulated trades: KPI: winrate, per trade profit

        // Confirmation
        // --------------------
        // 1. Signal reseaches a certain level for the last Bar
        // 2. Match time frame
        // 3. One technical situation is met for current Bar.
        // Buy Limit, Buy Stop Limit
        // Result: Long / Short. Scale in??? Most unlikely.

        // Validation
        // --------------------
        // 1. Entry Stop is met
        // 2. Breakeven Stop is met
        // 3. Profit taking limit is met
        // Result: Remove liquidation, scale out, Sell / Cover
    }


}
