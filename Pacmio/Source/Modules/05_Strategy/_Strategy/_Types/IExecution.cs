/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;

namespace Pacmio
{
    public class Execution
    {
        public Execution(double price, double qty) 
        {
            ExecutePrice = price;
            Quantity = qty;
        }

        public double ExecutePrice { get; private set; }

        public double Quantity { get; private set; }

        public double Proceeds => Math.Abs(ExecutePrice * Quantity);

        public LiquidityType LiquidityType { get; set; } = LiquidityType.None;

        public double RealizedPnL { get; set; } = 0;
    }
}
