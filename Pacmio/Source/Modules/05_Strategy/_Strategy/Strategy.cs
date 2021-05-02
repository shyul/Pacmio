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
    public abstract class Strategy : SignalAnalysis, IChartOverlay
    {
        protected Strategy(TimePeriod tif, BarFreq barFreq, PriceType priceType) : base(tif, barFreq, priceType) { }

        /// <summary>
        /// The first simple filter to narrow down the list before any complex BarAnalysis.
        /// </summary>
        public abstract BarAnalysisFilter Filter { get; }

        public abstract BarAnalysisSet BarAnalysisSet { get; }

        /// <summary>
        /// Example: Only trade 9:30 AM to 10 AM
        /// </summary>
        //public TimePeriod TimeInForce { get; set; } = TimePeriod.Full;

        public TimePeriod PositionHoldingPeriod { get; set; } = TimePeriod.Full;

        public TimeSpan HoldingTime { get; } = new TimeSpan(1000, 1, 1, 1, 1);


        #region Order

        public double MinimumRiskRewardRatio { get; set; }

        public double MinimumTradeSize { get; set; }



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
        #endregion Order

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            BarTableSet bts = bt.BarTableSet;
            MultiPeriod testPeriods = bts.MultiPeriod;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                StrategyDatum sd = b[this];

                #region Identify the indicators

                if (testPeriods.Contains(b.Time) && TimeInForce.Contains(b.Time))
                {

                }

                #endregion Identify the indicators

                #region Check profit and stop

                sd.StopLossOrTakeProfit(0.5);

                if (sd.Quantity > 0) // || sd.Datum_1.Message == RangeBarBullishMessage)
                {

                }
                else if (sd.Quantity < 0) // || sd.Datum_1.Message == RangeBarBearishMessage)
                {

                }

                #endregion Check profit and stop
            }
        }





        public virtual void DrawOverlay(Graphics g, BarChart bc)
        {


        }

        public int DrawOrder { get; set; } = 0;

        public bool ChartEnabled { get; set; } = true;

        public string AreaName { get; }
    }




    public class StrategyBacktestSetting
    {
        #region Training Settings

        //   public double SlippageRatio { get; set; } = 0.0001;

        /// <summary>
        /// The number of days for getting the bench mark: RR ratio, win rate, volatility, max win, max loss, and so on.
        /// The commission model shall be defined by Simulate Engine.
        /// </summary>
        //public virtual int TrainingLength { get; set; } = 5;

        /// <summary>
        /// The number of days enters the actual trade or tradelog for simulation | final bench mark.
        /// Only when the SimulationResult is positive (or above a threshold), does the trading start log, and this time, it logs the trades.
        /// </summary>
        //public virtual int TradingLength { get; set; } = 1;

        #endregion Training Settings

    }
}
