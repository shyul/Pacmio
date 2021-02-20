/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xu;

namespace Pacmio
{
    public static class TradeTestPoC 
    {
        public static int DaysOfTradeHistoryToLoad { get; } = 365;

        public static Dictionary<Contract, PositionInfo> Positions { get; }
    
    }



    public class EntryMethod
    {
        public string Name { get; set; }

        public double MinimumRiskRewardRatio { get; set; }

        public double MinimumTradeSize { get; set; }

        /// <summary>
        /// Bar Analysis vs Multi Time Frame
        /// </summary>
        protected Dictionary<(BarFreq BarFreq, BarType BarType), BarAnalysisSet> BarAnalysisSets { get; } = new Dictionary<(BarFreq BarFreq, BarType BarType), BarAnalysisSet>();

        public virtual void ClearBarAnalysisSet() => BarAnalysisSets.Clear();

        public virtual BarAnalysisSet this[BarFreq BarFreq, BarType BarType = BarType.Trades]
        {
            get
            {
                if (BarAnalysisSets.ContainsKey((BarFreq, BarType)))
                    return BarAnalysisSets[(BarFreq, BarType)];
                else
                    return null;
            }
            set
            {
                if (value is BarAnalysisSet bas)
                    BarAnalysisSets[(BarFreq, BarType)] = new BarAnalysisSet(bas);
                else if (BarAnalysisSets.ContainsKey((BarFreq, BarType)))
                    BarAnalysisSets.Remove((BarFreq, BarType));
            }
        }

        #region Order Settings

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

        #endregion Order Settings
    }


}
