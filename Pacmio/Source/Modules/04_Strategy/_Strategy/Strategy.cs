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
using System.Drawing;

namespace Pacmio
{
    /// <summary>
    /// Indication: Move into Either Enter or Exit
    /// Active Indicator, yield score, and check other time frame's scores
    /// </summary>
    public abstract class Strategy : Indicator, ISingleDatum, IChartOverlay
    {
        /// <summary>
        /// The first simple filter to narrow down the list before any complex BarAnalysis.
        /// </summary>
        public abstract Filter Filter { get; }

        public IndicatorSet IndicatorSet { get; } = new();

        /// <summary>
        /// Example: Only trade 9:30 AM to 10 AM
        /// </summary>
        public MultiTimePeriod TradeTimeOfDay { get; set; }



        public void BackTest(BarTableSet bts, Period periodLimit, CancellationTokenSource cts = null)
        {
            var result = Filter.RunScanResult(bts, periodLimit);

            MultiPeriod mps = bts.MultiPeriod is not null ? new MultiPeriod(bts.MultiPeriod) : new MultiPeriod();

            foreach (var pd in result.Periods)
            {
                if (periodLimit.Contains(pd))
                {
                    mps.Add(pd);
                }
            }

            bts.SetPeriod(mps, cts);

            foreach (var ind in IndicatorSet)
            {
                bts[ind.BarFreq, ind.DataType].CalculateRefresh(ind);
            }

            BarTable bt = bts[BarFreq, DataType];
            bt.CalculateRefresh(this);
        }



        public DatumColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            BarTableSet bts = bt.BarTableSet;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];

                Bar time_frame_b = bts[b.Time, Filter];
                
                // Assure Tables from other time frame has the same or later ticker time...

                // Get existing Position...

                // Determine trade direction, add liquidity or remove  liquidity

                // Fetch all results from other filtes.

                // Construct the score...
            }
        }

        public void DrawOverlay(Graphics g, BarChart bc)
        {




        }








        #region Order

        public double MinimumRiskRewardRatio { get; set; }

        public double MinimumTradeSize { get; set; }

        #endregion Order

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

        // Step 1: Define WatchList (Filters) Group sort by time frame -> Filter has B.A.S 

        // Step 1a: optionally manually defined [[[[ Daily ]]]] Scanner for faster live trading

        // Step 2: Define Signal Group



        #region Training Settings

        //   public double SlippageRatio { get; set; } = 0.0001;

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
