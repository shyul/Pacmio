/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// BarTable Data Types
/// 
/// ***************************************************************************

using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Text;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public class BarPosition
    {
        public BarPosition(Bar b) 
        {
            Bar = b;
            ActionType = TradeActionType.None;
            Quantity = 0;
            AveragePrice = double.NaN;
        }

        public readonly Bar Bar; 

        public TradeActionType ActionType { get; set; } = TradeActionType.None;

        public double Quantity { get; private set; } = 0;

        public double AveragePrice { get; private set; } = double.NaN;

        public double Value => double.IsNaN(AveragePrice) ? 0 : Math.Abs(Quantity * AveragePrice);

        public double PnL => Value == 0 ? 0 : Quantity * (Bar.Close - AveragePrice);

        public double PnL_Percent => Value == 0 ? 0 : 100 * (Bar.Close - AveragePrice) / AveragePrice;

        public void Reset()
        {
            ActionType = TradeActionType.None;
            Quantity = 0;
            AveragePrice = double.NaN;
        }

        public void Snapshot(PositionStatus ps) 
        {
            ActionType = ps.ActionType;
            Quantity = ps.Quantity;
            AveragePrice = ps.AveragePrice;
        }
    }
}
