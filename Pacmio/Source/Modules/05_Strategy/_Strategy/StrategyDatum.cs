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
                if (IsLive) // Check the bar see how it is filled
                {
                    if (Decision is EquityDecision entry)
                    {
                        // Check current price is fillable
                        if (entry.Scale > 0 && Bar.Close > entry.StopLossPrice && Bar.Close < entry.ProfitTakePrice) // Buy Side 
                        {
                            double qty = Quantity == 0 ? 1 : Math.Abs(Quantity * entry.Scale);

                            if (entry.Type == EntryType.Limit && Bar.Close < entry.EntryPrice)
                            {
                                // send the order and quantity registered.
                                RegisterLiveExecution(qty);
                            }
                            else if (entry.Type == EntryType.Stop && Bar.Close > entry.EntryPrice)
                            {
                                RegisterLiveExecution(qty);
                            }
                            else if (Bar.Contains(entry.EntryPrice))
                            {
                                RegisterLiveExecution(qty);
                            }
                        }
                        else if (entry.Scale < 0) // Sell Side
                        {
                            double qty = Quantity == 0 ? -1 : -Math.Abs(Quantity * entry.Scale);

                            if (entry.Type == EntryType.Limit && Bar.Close > entry.EntryPrice)
                            {
                                RegisterLiveExecution(qty);
                            }
                            else if (entry.Type == EntryType.Stop && Bar.Close < entry.EntryPrice)
                            {
                                RegisterLiveExecution(qty);
                            }
                            else if (Bar.Contains(entry.EntryPrice))
                            {
                                RegisterLiveExecution(qty);
                            }
                        }
                    }

                    if (Decision is IDecision dec)
                    {
                        if (Quantity > 0) // Long position
                        {
                            if (Bar.Close < dec.StopLossPrice)
                            {
                                RegisterLiveExecution(-Quantity);
                            }
                            else if (Bar.Close > dec.ProfitTakePrice)
                            {
                                RegisterLiveExecution(-Quantity);
                            }
                        }
                        else if (Quantity < 0) // Short Position
                        {
                            if (Bar.Close > dec.StopLossPrice)
                            {
                                RegisterLiveExecution(-Quantity);
                            }
                            else if (Bar.Close < dec.ProfitTakePrice)
                            {
                                RegisterLiveExecution(-Quantity);
                            }
                        }




                    }
                }
                else
                {
                    if (Decision is EquityDecision entry)
                    {
                        if (entry.Scale > 0) // Buy Side 
                        {
                            double qty = Quantity == 0 ? 1 : Math.Abs(Quantity * entry.Scale);

                            if (entry.Type == EntryType.Limit && Bar.High < entry.EntryPrice)
                            {
                                AddExecution(new Execution(Bar.Open, qty));
                            }
                            else if (entry.Type == EntryType.Stop && Bar.Low > entry.EntryPrice)
                            {
                                AddExecution(new Execution(Bar.Open, qty));
                            }
                            else if (Bar.Contains(entry.EntryPrice))
                            {
                                AddExecution(new Execution(entry.EntryPrice, qty));
                            }

                        }
                        else if (entry.Scale < 0) // Sell Side
                        {
                            double qty = Quantity == 0 ? -1 : -Math.Abs(Quantity * entry.Scale);

                            if (entry.Type == EntryType.Limit && Bar.Low > entry.EntryPrice)
                            {
                                AddExecution(new Execution(Bar.Open, qty));
                            }
                            else if (entry.Type == EntryType.Stop && Bar.High < entry.EntryPrice)
                            {
                                AddExecution(new Execution(Bar.Open, qty));
                            }
                            else if (Bar.Contains(entry.EntryPrice))
                            {
                                AddExecution(new Execution(entry.EntryPrice, qty));
                            }
                        }

                        if (Decision is IDecision dec)
                        {
                            // Pessimistic by check stop loss first.
                            if (Bar.Contains(dec.StopLossPrice))
                            {
                                AddExecution(new Execution(dec.StopLossPrice, -Quantity));
                            }
                            else if (Bar.Contains(dec.ProfitTakePrice))
                            {
                                AddExecution(new Execution(dec.ProfitTakePrice, -Quantity));
                            }
                        }
                    }
                    // Modify the decision:
                    // 1. Null, no decsion, example the position just got exit
                    // 2. Entry Decision, when there is an oppotunity
                    // 3. Hold Decision, check the changes of the price target and stop loss.
                }
            }
        }

        public void RegisterLiveExecution(double qty)
        {
            if (qty != 0)
            {
                // Send this Execution!

                // get actual price and quantity



            }
        }

        public void AddExecution(Execution exec)
        {
            if (exec.Quantity != 0)
            {
                if (exec.Quantity * Quantity >= 0) // Same direction or initial entry (when Qty == 0)
                {
                    exec.LiquidityType = LiquidityType.Added;
                    AveragePrice = Math.Abs((exec.Proceeds + Cost) / (exec.Quantity + Quantity));
                    Quantity += exec.Quantity;
                    exec.RealizedPnL = 0;
                }
                else // Opposite direction in this case.
                {
                    exec.LiquidityType = LiquidityType.Removed;
                    exec.RealizedPnL = exec.Quantity * (AveragePrice - exec.ExecutePrice);
                    Quantity -= exec.Quantity;

                    if (Quantity == 0)
                    {
                        AveragePrice = double.NaN;
                        Decision = null;
                    }
                }

                Executions.Add(exec);
            }
        }

        private List<Execution> Executions = new();

        public Execution LatestExecution => Executions.Count > 0 ? Executions.Last() : null;

        public double RealizedPnL => Executions.Select(n => n.RealizedPnL).Sum();

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