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
    public sealed class BarPosition
    {
        public BarPosition(Bar b, ITradeSetting tr)
        {
            Bar = b;
            if (Bar.Table.LastBar_1 is Bar b_1)
                Snapshot(b_1[tr]);
            else
                Reset();
        }

        public void Reset()
        {
            ActionType = TradeActionType.None;
            Quantity = 0;
            AveragePrice = double.NaN;
        }

        private void Snapshot(BarPosition bp)
        {
            Quantity = bp.Quantity;
            AveragePrice = bp.AveragePrice;

            if (Quantity == 0)
                ActionType = TradeActionType.None;
            else if (Quantity > 0)
                ActionType = TradeActionType.LongHold;
            else if (Quantity < 0)
                ActionType = TradeActionType.ShortHold;
        }

        public void Snapshot(PositionStatus ps)
        {
            double qty_1 = ps.Quantity;
            if (Quantity > qty_1)
            {
                ActionType = (Quantity <= 0) ? TradeActionType.Cover : TradeActionType.Long;
            }
            else if (Quantity < qty_1)
            {
                ActionType = (Quantity >= 0) ? TradeActionType.Sell : TradeActionType.Short;
            }
            else // (Quantity == qty_1)
            {
                if (Quantity == 0)
                    ActionType = TradeActionType.None;
                else if (Quantity > 0)
                    ActionType = TradeActionType.LongHold;
                else if (Quantity < 0)
                    ActionType = TradeActionType.ShortHold;
            }

            Quantity = qty_1;
            AveragePrice = ps.AveragePrice;
        }

        public void Copy(PositionStatus ps)
        {
            ActionType = ps.ActionType;
            Quantity = ps.Quantity;
            AveragePrice = ps.AveragePrice;
        }

        private readonly Bar Bar; 

        public TradeActionType ActionType { get; private set; } = TradeActionType.None;

        public double Quantity { get; private set; } = 0;

        public double AveragePrice { get; private set; } = double.NaN;

        public double Value => double.IsNaN(AveragePrice) ? 0 : Math.Abs(Quantity * AveragePrice);

        public double PnL => Value == 0 ? 0 : Quantity * (Bar.Close - AveragePrice);

        public double PnL_Percent => Value == 0 ? 0 : 100 * (Bar.Close - AveragePrice) / AveragePrice;
    }
}
