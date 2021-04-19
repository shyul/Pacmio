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
            IsLive = b.Table.CalculateTickRequested;

            if (IsLive && s.AccountInfo is AccountInfo ac)
            {
                PositionInfo pi = ac[Contract];
                Quantity = pi.Quantity;
                AveragePrice = pi.AverageEntryPrice;

                if (Bar.Bar_1 is Bar b_1 && b_1[Strategy] is StrategyDatum sd_1 && sd_1.IsLive)
                {
                    sd_1.Quantity = Quantity;
                    sd_1.AveragePrice = AveragePrice;
                    Decision = sd_1.Decision;
                }
            }
            else if (Bar.Bar_1 is Bar b_1 && b_1[Strategy] is StrategyDatum sd_1 && !sd_1.IsLive)
            {
                Quantity = sd_1.Quantity;
                AveragePrice = sd_1.AveragePrice;
                Decision = sd_1.Decision; // keep the record of "current decision"
            }
        }

        public Strategy Strategy { get; }

        public Bar Bar { get; }

        public Contract Contract => Bar.Table.Contract;

        public MarketData MarketData => Contract.MarketData;

        public bool IsLive { get; private set; } = false;

        #region Decision

        public IDecision Decision { get; set; } = null;

        public int DecisionBarDistance => Decision is not null ? Bar.Index - Decision.DecisionBar.Index : 0;

        public bool HasDecision => DecisionBarDistance == 1;

        #endregion Decision

        #region Execution

        // A. Simulation Mode.
        // B. Actual Trade Mode. -- Keep update the last Bar's Datum Info
        // -- Run Snapshot first during the simulation

        public void ExecuteDecision()
        {
            // Entry Exec is proactive
            // Exit Exec is passive.
            if (HasDecision)
            {
                if (IsLive) 
                {
                    // Check current price is fillable

                    // send the order and quantity registered.
                }
                else
                {
                    // Check the bar see how it is filled



                }

                // Modify the decision:
                // 1. Null, no decsion, example the position just got exit
                // 2. Entry Decision, when there is an oppotunity
                // 3. Hold Decision, check the changes of the price target and stop loss.
            }
        }

        public List<ActionType> Actions = new();

        public ActionType Action => Actions.Count > 0 ? Actions.Last() : ActionType.None;

        #endregion Execution

        #region Position Track

        public double Quantity { get; private set; } = 0;

        public double AveragePrice { get; private set; } = double.NaN;

        public double CurrentPrice => IsLive ? MarketData.LastPrice : Bar.Close;

        public double Cost => double.IsNaN(AveragePrice) ? 0 : Math.Abs(Quantity * AveragePrice);

        public double PnL => Cost == 0 ? 0 : Quantity * (CurrentPrice - AveragePrice);

        public double PnL_Percent => Cost == 0 ? 0 : 100 * (CurrentPrice - AveragePrice) / AveragePrice;

        #endregion Position Track
    }



}