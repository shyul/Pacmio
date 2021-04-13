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

namespace Pacmio.Analysis
{
    public abstract class PositionAnalysis : BarAnalysis
    {



        public double InitialFunds { get; }

        public double AverageCommission { get; }

        public IndicatorExecution IndicatorExec { get; }



    }

    public sealed class BarPositionData
    {
        public BarPositionData(Bar b)
        {
            Bar = b;

            //Snapshot();
            Reset();
        }

        public void Reset()
        {
            ActionType = TradeExecutionType.None;
            Quantity = 0;
            AveragePrice = double.NaN;

        }

        public void Snapshot()
        {
            /*
            if (Bar.Table.LastBar_1 is Bar b_1 && b_1[Strategy] is BarPositionData bp_1)
            {
                Quantity = bp_1.Quantity;
                AveragePrice = bp_1.AveragePrice;

                if (Quantity == 0)
                    ActionType = TradeActionType.None;
                else if (Quantity > 0)
                    ActionType = TradeActionType.LongHold;
                else if (Quantity < 0)
                    ActionType = TradeActionType.ShortHold;
            }
            else
                Reset();*/
        }

        public void Snapshot(PositionInfo ps)
        {
            double new_qty = ps.Quantity;
            if (Quantity < new_qty)
            {
                ActionType = (Quantity < 0) ? TradeExecutionType.Cover : TradeExecutionType.Long;
            }
            else if (Quantity > new_qty)
            {
                ActionType = (Quantity > 0) ? TradeExecutionType.Sell : TradeExecutionType.Short;
            }
            else
            {
                if (Quantity > 0)
                    ActionType = TradeExecutionType.LongHold;
                else if (Quantity < 0)
                    ActionType = TradeExecutionType.ShortHold;
                else
                    ActionType = TradeExecutionType.None;
            }

            Quantity = new_qty;
            AveragePrice = ps.AverageEntryPrice;
        }

        public Bar Bar { get; }






        public TradeExecutionType ActionType { get; private set; } = TradeExecutionType.None;

        public double Quantity { get; private set; } = 0;

        public double AveragePrice { get; private set; } = double.NaN;

        public double Value => double.IsNaN(AveragePrice) ? 0 : Math.Abs(Quantity * AveragePrice);

        public double PnL => Value == 0 ? 0 : Quantity * (Bar.Close - AveragePrice);

        public double PnL_Percent => Value == 0 ? 0 : 100 * (Bar.Close - AveragePrice) / AveragePrice;
    }
}
