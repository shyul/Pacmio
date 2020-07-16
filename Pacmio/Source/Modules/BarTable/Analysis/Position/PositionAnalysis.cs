/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
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

namespace Pacmio
{
    /// <summary>
    /// Confirmation and Validation: yield enter position
    /// Settings: Sizing strategy, Account #, Time in effect, OrderType, Order Expiry, and so on...
    /// Does not yield signal score
    /// </summary>
    public abstract class PositionAnalysis
    {
        /// <summary>
        /// BarFreq for Trading || Evaluate Positions
        /// All other BarFreq are filters and should be calculated from high to low
        /// 
        /// How Filter works
        /// 1. Get the filter event
        /// 2. Download 300 units before the event, and one unit worth of data after the event for calculation and evaluations
        /// </summary>
        public BarFreq BarFreq { get; }

        public IEnumerable<SignalColumn> EntrySignals { get; }

        public IEnumerable<SignalColumn> LongExitSignals { get; }

        public IEnumerable<SignalColumn> ShortExitSignals { get; }

        public MultiTimePeriod EntryPeriods { get; }


    }
}
