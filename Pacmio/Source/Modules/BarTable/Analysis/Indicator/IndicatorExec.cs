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
    public enum ExecutionType : int
    {
        Cover = 1,
        Short = -2,
        None = 0,
        Long = 2,
        Sell = -1,
    }

    public class ExecutionDatum : IDatum
    {
        public ExecutionType Type { get; } = ExecutionType.None;

        public double Size { get; } = 1;

        public double LimitPrice { get; } = double.NaN;

        public double AuxPrice { get; } = double.NaN;

        // Use to calculate simulation result... position vs time, price, so on...
    }

    /// <summary>
    /// Indication: Move into Either Enter or Exit
    /// Active Indicator, yield score, and check other time frame's scores
    /// </summary>
    public abstract class IndicatorExec : Indicator
    {
        protected IndicatorExec(Strategy s)
        {
            Strategy = s;
            SignalSeries = new(this);
        }

        public Strategy Strategy { get; }




        public double MaximumHoldingTimeInMs { get; set; }


        public void GenerateOrder(ExecutionDatum ed) => Strategy.GenerateOrder(ed);



        /// <summary>
        /// Example: Only trade 9:30 AM to 10 AM
        /// </summary>
        public MultiTimePeriod TradeTimeOfDay { get; set; }

        /// <summary>
        /// Wait 1000 ms, and cancel the rest of the unfiled order if there is any.
        /// </summary>
        public double WaitMsForOutstandingOrder { get; }

        /// <summary>
        /// If the price goes 1% to the upper side of the triggering level, then cancel the rest of the order.
        /// Can use wait Ms and set limit price.
        /// </summary>
        public double MaximumPriceGoingPositionFromDecisionPointPrecent { get; } = double.NaN;

        /// <summary>
        /// If the price goes ?? % to the down side of the triggering price, then cancel the unfiled order.
        /// </summary>
        public double MaximumPriceGoinNegativeFromDecisionPointPrecent { get; }
    }
}
