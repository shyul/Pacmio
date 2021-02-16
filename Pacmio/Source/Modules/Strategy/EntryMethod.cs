/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
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

        public static Dictionary<Contract, PositionStatus> Positions { get; }
    
    }

    /// <summary>
    /// Only used in live trades, and emulations
    /// </summary>
    public class PositionStatus
    {
        public PositionStatus(MarketData md)
        {
            MarketData = md;
        }

        public MarketData MarketData { get; }

        public Contract Contract => MarketData.Contract;

        public double DecisionPrice { get; set; }

        public DateTime PositionOpenTime { get; set; }

        public double MarkPrice => MarketData.MarkPrice; 

        public double AverageEntryPrice { get; set; }

        public double Quantity { get; set; } = 0;

        public Strategy AssignedStrategy { get; set; }

        public OrderInfo CurrentOrder { get; private set; }

        public OrderStatus OrderStatus => CurrentOrder is OrderInfo od ? od.Status : OrderStatus.Inactive;

        

        /// <summary>
        /// When the order if filled
        /// </summary>
        public void CloseCurrentOrder()
        {
            // If this order is still alive,
            // Cancel it here!

            if (CurrentOrder is OrderInfo od && !OrderHistory.Contains(od))
            {
                OrderHistory.Add(od);
            }

            CurrentOrder = null;
        }

        public List<OrderInfo> OrderHistory { get; } = new List<OrderInfo>();

        public OrderInfo LastOrder => OrderHistory.OrderBy(n => n.OrderTime).LastOrDefault();

        public string Account => LastOrder.AccountCode;

        public DateTime LastTradeTime => LastOrder.OrderTime;
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
