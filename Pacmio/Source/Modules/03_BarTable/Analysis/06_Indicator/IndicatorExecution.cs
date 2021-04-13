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
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    /// <summary>
    /// Indication: Move into Either Enter or Exit
    /// Active Indicator, yield score, and check other time frame's scores
    /// </summary>
    public abstract class IndicatorExecution : Indicator, ISingleDatum
    {
        public Strategy IndicatorSet { get; }

        public DatumColumn Column_Result { get; }


        // Assure Tables from other time frame has the same or later ticker time...

        // Get existing Position...

        // Determine trade direction, add liquidity or remove  liquidity

        // Fetch all results from other filtes.

        // Construct the score...

        /// <summary>
        /// Example: Only trade 9:30 AM to 10 AM
        /// </summary>
        public MultiTimePeriod TradeTimeOfDay { get; set; }



        #region Order

        public double MinimumRiskRewardRatio { get; set; }

        public double MinimumTradeSize { get; set; }

        #endregion Order



        // Step 1: Define WatchList (Filters) Group sort by time frame -> Filter has B.A.S 

        // Step 1a: optionally manually defined [[[[ Daily ]]]] Scanner for faster live trading

        // Step 2: Define Signal Group



        #region Training Settings

        //   public double SlippageRatio { get; set; } = 0.0001;

        /// <summary>
        /// The unit for training time frames
        /// </summary>
        public virtual BarFreq TrainingUnitFreq { get; set; } = BarFreq.Daily;

        /// <summary>
        /// The number of days for getting the bench mark: RR ratio, win rate, volatility, max win, max loss, and so on.
        /// The commission model shall be defined by Simulate Engine.
        /// </summary>
        public virtual int TrainingLength { get; set; } = 5;

        /// <summary>
        /// The number of days enters the actual trade or tradelog for simulation | final bench mark.
        /// Only when the SimulationResult is positive (or above a threshold), does the trading start log, and this time, it logs the trades.
        /// </summary>
        public virtual int TradingLength { get; set; } = 1;

        #endregion Training Settings
    }
}
