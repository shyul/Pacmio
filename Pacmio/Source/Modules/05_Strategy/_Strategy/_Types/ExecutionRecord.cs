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
        public ExecutionRecord(double price, double qty, double commission)
        {
            ExecutePrice = price;
            Quantity = qty;
            Commission = commission;
        }

        public double Commission { get; private set; }

        public double ExecutePrice { get; private set; }

        public double Quantity { get; private set; }

        public double Proceeds => Math.Abs(ExecutePrice * Quantity);

        public LiquidityType LiquidityType { get; set; } = LiquidityType.None;

        public ActionType Action { get; set; } = ActionType.None;

        public double RealizedPnL { get; set; } = 0;
    }
}
