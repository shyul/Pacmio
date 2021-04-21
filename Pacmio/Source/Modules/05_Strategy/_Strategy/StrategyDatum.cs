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

            if (IsLive && PositionInfo is PositionInfo pi)
            {
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

        public PositionInfo PositionInfo => Strategy.AccountInfo is AccountInfo ac ? ac[Contract] : null;

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
            if (HasDecision)
            {
                if (Decision is EquityDecision entry)
                {
                    if (entry.Scale > 0) // Buy Side 
                    {
                        double qty = Quantity == 0 ? 1 : Math.Abs(Quantity * entry.Scale);

                        if (entry.Type == EntryType.Limit && Bar.High < entry.EntryPrice && Bar.Open > entry.StopLossPrice)
                        {
                            SendOrder(EntryType.None, Bar.Open, qty);
                        }
                        else if (entry.Type == EntryType.Stop && Bar.Low > entry.EntryPrice && Bar.Open < entry.ProfitTakePrice)
                        {
                            SendOrder(EntryType.None, Bar.Open, qty);
                        }
                        else if (Bar.Contains(entry.EntryPrice))
                        {
                            SendOrder(entry.Type, entry.EntryPrice, qty);
                        }

                    }
                    else if (entry.Scale < 0) // Sell Side
                    {
                        double qty = Quantity == 0 ? -1 : -Math.Abs(Quantity * entry.Scale);

                        if (entry.Type == EntryType.Limit && Bar.Low > entry.EntryPrice && Bar.Open < entry.StopLossPrice)
                        {
                            SendOrder(EntryType.None, Bar.Open, qty);
                        }
                        else if (entry.Type == EntryType.Stop && Bar.High < entry.EntryPrice && Bar.Open > entry.ProfitTakePrice)
                        {
                            SendOrder(EntryType.None, Bar.Open, qty);
                        }
                        else if (Bar.Contains(entry.EntryPrice))
                        {
                            SendOrder(entry.Type, entry.EntryPrice, qty);
                        }
                    }
                }

                if (Decision is IDecision dec)
                {
                    // Pessimistic by check stop loss first.
                    if (Bar.Contains(dec.StopLossPrice))
                    {
                        SendOrder(EntryType.Stop, dec.StopLossPrice, -Quantity);
                    }
                    else if (Bar.Contains(dec.ProfitTakePrice))
                    {
                        SendOrder(EntryType.Limit, dec.ProfitTakePrice, -Quantity);
                    }
                    else if (Quantity > 0 && (Bar.High < dec.StopLossPrice || Bar.Low > dec.ProfitTakePrice)) // Long position
                    {
                        SendOrder(EntryType.None, Bar.Open, -Quantity);
                    }
                    else if (Quantity < 0 && (Bar.Low > dec.StopLossPrice || Bar.High < dec.ProfitTakePrice)) // Short Position
                    {
                        SendOrder(EntryType.None, Bar.Open, -Quantity);
                    }
                }
            }
        }

        private void SendOrder(EntryType entryType, double refPrice, double qty)
        {
            if (qty != 0)
            {
                if (qty == -Quantity) // Identify if it closes the position entirely.
                {
                    AveragePrice = double.NaN;
                    Decision = null;
                }

                if (IsLive) // && the same direction execution is not sent ?// Check the bar see how it is filled
                {
                    // Send this Execution!

                    // get actual price and quantity

                    // Wait until it returns 

                }
                else // If it not alive, just add the execution directly.
                {
                    // Add splipage and commission here??

                    double commission = 0; // Hard to estimate, since I am using unity quantity here.
                    double slippage = 0; // Get it from simulation setting!

                    AddExecutionRecord(new ExecutionRecord(refPrice + slippage, qty, commission));
                }

                // Modify the decision:
                // 1. Null, no decsion, example the position just got exit
                // 2. Entry Decision, when there is an oppotunity
                // 3. Hold Decision, check the changes of the price target and stop loss.
            }
        }

        private void AddExecutionRecord(ExecutionRecord exec)
        {
            if (exec.Quantity != 0)
            {
                if (exec.Quantity * Quantity >= 0) // Same direction or initial entry (when Qty == 0)
                {
                    exec.LiquidityType = LiquidityType.Added;
                    AveragePrice = Math.Abs((exec.Proceeds + Cost) / (exec.Quantity + Quantity));

                    if (exec.Quantity > 0)
                        exec.Action = ActionType.Long;
                    else if (exec.Quantity < 0)
                        exec.Action = ActionType.Short;

                    Quantity += exec.Quantity;
                    exec.RealizedPnL = 0;
                }
                else // Opposite direction in this case.
                {
                    exec.LiquidityType = LiquidityType.Removed;

                    if (exec.Quantity > 0)
                        exec.Action = ActionType.Cover;
                    else if (exec.Quantity < 0)
                        exec.Action = ActionType.Sell;

                    exec.RealizedPnL = exec.Quantity * (AveragePrice - exec.ExecutePrice);
                    Quantity -= exec.Quantity;
                }

                ExecutionRecordList.Add(exec);
            }
        }

        private List<ExecutionRecord> ExecutionRecordList = new();

        public ExecutionRecord LatestExecutionRecord => ExecutionRecordList.Count > 0 ? ExecutionRecordList.Last() : null;

        public ActionType LatestAction => LatestExecutionRecord is ExecutionRecord exr ? exr.Action : ActionType.None;

        public double RealizedPnL => ExecutionRecordList.Select(n => n.RealizedPnL).Sum();

        #endregion Execution

        #region Position Track

        public double Quantity { get; private set; } = 0;

        public double AveragePrice { get; private set; } = double.NaN;

        public double CurrentPrice => IsLive ? MarketData.LastPrice : Bar.Close;

        public double Cost => double.IsNaN(AveragePrice) ? 0 : Math.Abs(Quantity * AveragePrice);

        public double UnrealizedPnL => Cost == 0 ? 0 : Quantity * (CurrentPrice - AveragePrice);

        public double UnrealizedPnLPercent => Cost == 0 ? 0 : 100 * (CurrentPrice - AveragePrice) / AveragePrice;

        #endregion Position Track
    }
}