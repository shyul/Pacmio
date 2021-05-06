/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;

namespace Pacmio
{
    public class ExecutionRecord
    {
        public ExecutionRecord(double price, double qty, double commissions, OrderType orderType)
        {
            ExecutionPrice = price;
            Quantity = qty;
            Commissions = commissions;
            OrderType = orderType;
        }

        public double Commissions { get; private set; }

        public double ExecutionPrice { get; }

        public double Quantity { get; }

        public double Proceeds => Math.Abs(ExecutionPrice * Quantity);

        public LiquidityType LiquidityType { get; set; } = LiquidityType.None;

        public ActionType Action { get; set; } = ActionType.None;

        public double RealizedPnL { get; set; } = 0;

        public OrderType OrderType { get; }
    }
}
