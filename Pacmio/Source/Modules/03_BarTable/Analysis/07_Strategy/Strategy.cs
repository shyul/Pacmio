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
            double minRiskRewardRatio,
            TimeSpan holdingMaxSpan,
            TimePeriod holdingPeriod,
            TimePeriod tif,
            BarFreq barFreq,
            PriceType priceType)
            : base(tif, barFreq, priceType)
        {
            MinimumRiskRewardRatio = minRiskRewardRatio;
            HoldingPeriod = holdingPeriod;
            HoldingMaxSpan = holdingMaxSpan;
        }

        #region Filter Signals

        /// <summary>
        /// The first simple filter to narrow down the list before any complex BarAnalysis.
        /// </summary>
        public FilterAnalysis Filter { get; protected set; }

        #endregion Filter Signals

        #region Entry Signals

        public BarAnalysisSet AnalysisSet { get; protected set; }

        #endregion Entry Signals

        #region Exit Signals

        public TimePeriod HoldingPeriod { get; }

        public TimeSpan HoldingMaxSpan { get; }

        public double MinimumRiskRewardRatio { get; }

        #endregion Exit Signals

        #region Draw Chart Overlay

        public int DrawOrder { get; set; } = 0;

        public bool ChartEnabled { get; set; } = true;

        public string AreaName { get; protected set; } = MainBarChartArea.DefaultName;

        public virtual void DrawOverlay(Graphics g, BarChart bc)
        {


        }

        #endregion Draw Chart Overlay
    }
}
