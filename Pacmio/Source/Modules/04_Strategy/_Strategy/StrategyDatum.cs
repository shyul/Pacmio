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
    public class StrategyDatum : IDatum
    {
        public StrategyDatum(Bar b, Strategy s)
        {
            Bar = b;
            Strategy = s;
            b[s] = this;
        }

        public Strategy Strategy { get; }

        public Bar Bar { get; }

        public Contract Contract => Bar.Table.Contract;

        public MarketData MarketData => Contract.MarketData;

        #region Execution

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

        #endregion Execution

        #region Position

        // A. Simulation Mode.
        // B. Actual Trade Mode. -- Keep update the last Bar's Datum Info
        // -- Run Snapshot first during the simulation
        public void Snapshot(bool isLive = false)
        {
            if (isLive)
            {
                // Get Account Numner from Strategy
                // Find PositionInfo
                // Copy it from there...
            }
            else if (Bar.Bar_1 is Bar b_1 && b_1[Strategy] is StrategyDatum sd_1)
            {
                Quantity = sd_1.Quantity;
                AveragePrice = sd_1.AveragePrice;
            }
        }

        public double Commission { get; set; } = 0;

        public double Quantity { get; set; } = 0;

        public double AveragePrice { get; set; } = double.NaN;

        public double Cost => double.IsNaN(AveragePrice) ? 0 : Math.Abs(Quantity * AveragePrice);

        public double PnL => Cost == 0 ? 0 : Quantity * (Bar.Close - AveragePrice);

        public double PnL_Percent => Cost == 0 ? 0 : 100 * (Bar.Close - AveragePrice) / AveragePrice;

        #endregion Position
    }
}