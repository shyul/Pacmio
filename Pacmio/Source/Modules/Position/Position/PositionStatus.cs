/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using Xu;

namespace Pacmio
{
    public class PositionStatus
    {
        public PositionStatus(Contract c)
        {
            Contract = c;
        }

        public Contract Contract { get; }

        #region Position Information

        public TradeActionType ActionType
        {
            get
            {
                if (Quantity > 0)
                    return TradeActionType.LongHold;
                else if (Quantity < 0)
                    return TradeActionType.ShortHold;
                else
                    return TradeActionType.None;
            }
        }

        public double Quantity { get; set; } = 0;

        public double AveragePrice { get; set; } = double.NaN;

        public double BreakEvenPrice
        {
            get
            {
                double commission_for_current_position = 2 * PositionSimTools.EstimatedCommission(Math.Abs(Quantity), AveragePrice);
                return (Quantity * AveragePrice + commission_for_current_position) / Quantity;
            }
        }

        public double Value => double.IsNaN(AveragePrice) ? 0 : Math.Abs(Quantity * AveragePrice);

        #endregion  Position Information

        #region Risk Management

        public double Stop { get; set; } = double.NaN;

        public double Limit { get; set; } = double.NaN;


        // Scale Setting


        // All trend line and level (support-resistance) lines from higher time frames are buffered here.

        #endregion Risk Management
    }
}
