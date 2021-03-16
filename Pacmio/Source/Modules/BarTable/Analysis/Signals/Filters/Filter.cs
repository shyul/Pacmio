/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// 1. Pre-Market Gapper (10%, total pre-market volume, social media sentiment)
/// 2. Halted and Resumed
/// 3. Low Share out-standing / Small-cap / and high *** volume spike ***
/// 4. Reversal??
/// 5. Social Mention
/// 
/// ***************************************************************************

using Pacmio.IB;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    /// <summary>
    /// Merge Filter to Indicator
    /// </summary>
    public abstract class Filter
    {
        /// <summary>
        /// Time Frame
        /// </summary>
        public BarFreq BarFreq { get; } 

        // We only run the Filter 
        public TimePeriod TimeInForce { get;}

        public List<DayOfWeek> DayOfWeekInForce { get; } = new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };

        public List<int> DayOfMonthInForce { get; }

        public List<int> MonthOfYearInForce { get; }

        public bool ValidTime(DateTime time) 
        {

            DayOfWeekInForce.Contains(time.DayOfWeek);

            return true;
        }

        // Get time zone information from the contract
        //public bool ValidTime() => ValidTime(DateTime.Now);

        public TIProData.TopListHandler TopWatchList { get; }

        // Output: name/source of the value, and value itself
        public (bool pushToLowerTimeFrame, List<HorizontalLine> importantLevels) Calculate(Contract c, List<HorizontalLine> Levels) 
        {
            return (false, new List<HorizontalLine>());
        }





        public SignalColumn[] SignalColumns { get; protected set; }
    }

   
}
