/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
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
    public class Strategy
    {
        public Strategy() 
        {
        
        }

        public string Name { get; }

        // Condition
        // --------------------
        // Result: List of symbol matching the condition

        // Indication
        // --------------------
        // Elevation Factors
        // Deprication Factors
        // Signals
        // Result: Signals list, sum of all signals at the close of a Bar

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
