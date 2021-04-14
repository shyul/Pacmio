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

namespace Pacmio
{
    public class StrategyResultDatum : IDatum
    {
        public Bar Bar { get; }

        public ExecutionType Type
        {
            get => m_Type;

            set
            {
                ExecutionType type = value;

                if (type != ExecutionType.None)
                    if (type == m_Type)
                        DecisionCount++;
                    else
                        DecisionCount = 1;
                else
                    DecisionCount = 0;

                m_Type = type;
            }
        }

        private ExecutionType m_Type = ExecutionType.None;

        public int DecisionCount { get; private set; } = 0;

        /// <summary>
        /// This is a ratio data: 1 means initial entry of the maximum riskable position, 2 means add double
        /// 0.5 (Remove Liq) means remove half, 1 (Remove) means empty the position.
        /// The actual "quantity" will be calculated with R/R and WinRate of the backtesting result.
        /// </summary>
        public double SizeRatio { get; } = 1;

        public double LimitPrice { get; } = double.NaN;

        public double AuxPrice { get; } = double.NaN;


        // ---------- Position Tracking -------------

        // A. Simulation Mode.
        // B. Actual Trade Mode. -- Keep update the last Bar's Datum Info

        public double Commission { get; set; }

        public double Quantity { get; set; }

        public double AveragePrice { get; set; }

        public double Cost => double.IsNaN(AveragePrice) ? 0 : Math.Abs(Quantity * AveragePrice);

        public double PnL => Cost == 0 ? 0 : Quantity * (Bar.Close - AveragePrice);

        public double PnL_Percent => Cost == 0 ? 0 : 100 * (Bar.Close - AveragePrice) / AveragePrice;
    }







    public sealed class PositionDatum : IDatum
    {
        public PositionDatum(Bar b)
        {
            Bar = b;

            //Snapshot();
            Reset();
        }

        public void Reset()
        {
            ActionType = ExecutionType.None;
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
                ActionType = (Quantity < 0) ? ExecutionType.Cover : ExecutionType.Long;
            }
            else if (Quantity > new_qty)
            {
                ActionType = (Quantity > 0) ? ExecutionType.Sell : ExecutionType.Short;
            }
            else
            {
                if (Quantity > 0)
                    ActionType = ExecutionType.LongHold;
                else if (Quantity < 0)
                    ActionType = ExecutionType.ShortHold;
                else
                    ActionType = ExecutionType.None;
            }

            Quantity = new_qty;
            AveragePrice = ps.AverageEntryPrice;
        }

        public Bar Bar { get; }






        public ExecutionType ActionType { get; private set; } = ExecutionType.None;

        public double Quantity { get; private set; } = 0;

        public double AveragePrice { get; private set; } = double.NaN;

        public double Value => double.IsNaN(AveragePrice) ? 0 : Math.Abs(Quantity * AveragePrice);

        public double PnL => Value == 0 ? 0 : Quantity * (Bar.Close - AveragePrice);

        public double PnL_Percent => Value == 0 ? 0 : 100 * (Bar.Close - AveragePrice) / AveragePrice;
    }
}