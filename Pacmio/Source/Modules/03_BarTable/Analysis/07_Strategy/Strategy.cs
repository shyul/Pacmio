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
        protected Strategy(
            TimeSpan holdingMaxSpan,
            TimePeriod holdingPeriod,
            TimePeriod tif,
            BarFreq barFreq,
            PriceType priceType)
            : base(tif, barFreq, priceType)
        {
            HoldingPeriod = holdingPeriod;
            HoldingMaxSpan = holdingMaxSpan;
        }

        /// <summary>
        /// The first simple filter to narrow down the list before any complex BarAnalysis.
        /// </summary>
        public BarAnalysisFilter Filter { get; protected set; }

        public BarAnalysisSet BarAnalysisSet { get; protected set; }

        public TimePeriod HoldingPeriod { get; } = TimePeriod.Full;

        public TimeSpan HoldingMaxSpan { get; } = new TimeSpan(1000, 1, 1, 1, 1);

        public double MinimumRiskRewardRatio { get; set; } = 2;

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

        #region Draw Chart Overlay

        public int DrawOrder { get; set; } = 0;

        public bool ChartEnabled { get; set; } = true;

        public string AreaName { get; }

        public virtual void DrawOverlay(Graphics g, BarChart bc)
        {


        }

        #endregion Draw Chart Overlay
    }
}
