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
    public class StrategyDatum : SignalDatum
    {
        public StrategyDatum(Bar b, Strategy s) : base(b, s.Column_Result)
        {
            Bar = b;
            Strategy = s;
            IsLive = b.Table.CalculateTickRequested;

            if (Bar.Bar_1 is Bar b_1 && b_1[Strategy] is StrategyDatum sd_1)
            {
                Datum_1 = sd_1;
                ProfitTakePrice = sd_1.ProfitTakePrice;
                StopLossPrice = sd_1.StopLossPrice;

                if (sd_1.EntryBarIndex >= 0)
                    EntryBarIndex = sd_1.EntryBarIndex + 1;
                else
                    EntryBarIndex = -1;

                if (IsLive && PositionInfo is PositionInfo pi)
                {
                    Quantity = pi.Quantity;
                    AveragePrice = pi.AverageEntryPrice;
                    if (sd_1.IsLive)
                    {
                        sd_1.Quantity = Quantity;
                        sd_1.AveragePrice = AveragePrice;
                    }
                }
                else if (!IsLive && !sd_1.IsLive)
                {
                    Quantity = sd_1.Quantity;
                    AveragePrice = sd_1.AveragePrice;
                }
            }
        }

        public Strategy Strategy { get; }

        public Contract Contract => Bar.Table.Contract;

        public MarketData MarketData => Contract.MarketData;

        public AccountInfo Account => TradeManager.GetAccount(Strategy);

        public PositionInfo PositionInfo => Account is AccountInfo ac ? ac[Contract] : null;

        public bool IsLive { get; private set; } = false;

        #region Decision

        public StrategyDatum Datum_1 { get; } = null;

        /// <summary>
        /// Positive means adding scale amount of position
        /// Negative means removing scale amount of position
        /// This is a ratio data: 1 means initial entry of the maximum riskable position, 2 means add double
        /// 0.5 (Remove Liq) means remove half, 1 (Remove) means empty the position.
        /// The actual "quantity" will be calculated with R/R and WinRate of the backtesting result.
        /// </summary>

        public double ProfitTakePrice { get; set; } = double.NaN;

        public double StopLossPrice { get; set; } = double.NaN;

        public double RiskPart { get; set; } = double.NaN;

        public string Message { get; set; } = string.Empty;

        public int EntryBarIndex { get; set; } = -1;

        public DateTime LastEntryTime { get; set; }

        #endregion Decision

        #region Order

        public void StopLossOrTakeProfit(double ratio = 0.5)
        {
            if (ratio > 1) ratio = 1; else if (ratio < 0) return;

            //Console.WriteLine(Bar.Time + " | Current Quantity = " + Quantity);

            if (Quantity > 0) // || sd.Datum_1.Message == RangeBarBullishMessage)
            {
                if (Bar.Contains(StopLossPrice))
                {
                    SendOrder(ActionDirection.Remove, StopLossPrice, 1, OrderType.MidPrice);
                }
                else if (Bar.High < StopLossPrice)
                {
                    SendOrder(ActionDirection.Remove, Bar.Open, 1, OrderType.MidPrice);
                }
                else if (Bar.BarFreq < BarFreq.Daily && !Strategy.HoldingPeriod.Contains(Bar.Time))
                {
                    SendOrder(ActionDirection.Remove, Bar.Open, 1, OrderType.MidPrice);
                }

                if (Bar.Contains(ProfitTakePrice))
                {
                    SendOrder(ActionDirection.Remove, ProfitTakePrice, -ratio, OrderType.Limit);
                    ProfitTakePrice += RiskPart;
                }
                else if (Bar.Low > ProfitTakePrice)
                {
                    SendOrder(ActionDirection.Remove, Bar.Low, 0.5, OrderType.Limit);
                    ProfitTakePrice = Bar.Low + RiskPart;
                }

                if (ratio < 1)
                    StopLossPrice = Math.Max(AveragePrice, StopLossPrice + RiskPart);
            }
            else if (Quantity < 0) // || sd.Datum_1.Message == RangeBarBearishMessage)
            {
                if (Bar.Contains(StopLossPrice))
                {
                    SendOrder(ActionDirection.Remove, StopLossPrice, 1, OrderType.MidPrice);
                }
                else if (Bar.Low > StopLossPrice)
                {
                    SendOrder(ActionDirection.Remove, Bar.Open, 1, OrderType.MidPrice);
                }
                else if (Bar.BarFreq < BarFreq.Daily && !Strategy.HoldingPeriod.Contains(Bar.Time))
                {
                    SendOrder(ActionDirection.Remove, Bar.Open, 1, OrderType.MidPrice);
                }

                // Profit Taking
                if (Bar.Contains(ProfitTakePrice))
                {
                    SendOrder(ActionDirection.Remove, ProfitTakePrice, ratio, OrderType.Stop);
                    ProfitTakePrice -= RiskPart;
                }
                else if (Bar.High < ProfitTakePrice)
                {
                    SendOrder(ActionDirection.Remove, Bar.High, 0.5, OrderType.Limit);
                    ProfitTakePrice = Bar.High - RiskPart;
                }

                if (ratio < 1)
                    StopLossPrice = Math.Min(AveragePrice, StopLossPrice - RiskPart);
            }
        }

        public void ClosePosition()
        {
            //SendOrder()
        }

        public void SendOrder(ActionDirection direction, double executionPrice, double scale, OrderType orderType)
        {
            if (scale != 0)
            {
                //double orderQty = Quantity == 0 ? 1 : Math.Abs(Quantity * scale);

                /*
                if (scale * Quantity < 0 && scale == 1) // Identify if it closes the position entirely.
                {
                    AveragePrice = double.NaN;
                }*/

                if (IsLive) // && the same direction execution is not sent ?// Check the bar see how it is filled
                {
                    AccountInfo ac = Account;

                    double riskableAmount = ac.NetLiquidation * 0.02;
                    double qty = riskableAmount / RiskPart;

                    if (qty / 100 > 1)
                    {
                        qty = Math.Round(qty / 100, MidpointRounding.AwayFromZero) * 100;
                    }

                    if (ac.BuyingPower > qty * MarketData.LastPrice)
                    {


                    }

                    // f = ((P * b) - (1 - P))/b   // Pb is WinRate, b is the reward ratio, and f is the part of the whole account

                    // example: if b is three times (4x), b = 3, and 30% possible, the result should be 6.7%

                    // Get the quantity from scale and RR ratio, win rate, and account size.


                    // Send this Execution!

                    // get actual price and quantity

                    // Wait until it returns 

                }
                else // If it not alive, just add the execution directly.
                {
                    // Add splipage and commission here??

                    double commission = 0; // Hard to estimate, since I am using unity quantity here.
                    double slippage = 0; // Get it from simulation setting!
                    Console.WriteLine(Bar.Time + " | Order: " + direction + " | price = " + executionPrice + " | scale = " + scale + " | orderType = " + orderType);

                    double quantity = 0;

                    if (direction == ActionDirection.Add)
                    {
                        if (FullQuantity == 0)
                            quantity = scale;
                        else
                            quantity = scale * Math.Abs(FullQuantity);
                    }
                    else if(direction == ActionDirection.Remove) 
                    {
                        scale = Math.Abs(scale);

                        if (scale == 1 ||  Math.Abs(scale * 1.01) > Math.Abs(QuantityRatio))
                            quantity = -Quantity;
                        else
                            quantity = scale * FullQuantity;

                    }



                    AddExecutionRecord(new ExecutionRecord(executionPrice + slippage, quantity, commission, orderType));
                }

                // Modify the decision:
                // 1. Null, no decsion, example the position just got exit
                // 2. Entry Decision, when there is an oppotunity
                // 3. Hold Decision, check the changes of the price target and stop loss.
            }
        }

        public void CancelOrder()
        {
            if (ActiveOrder is OrderInfo od && od.IsEditable)
            {
                od.Cancel();
            }
        }

        public OrderInfo ActiveOrder { get; set; } = null;

        public bool HasActiveOrder => ActiveOrder is OrderInfo od && od.IsEditable;

        #endregion Order

        #region Position Track

        public double PendingQuantity => ActiveOrder is OrderInfo od && od.IsEditable ? od.RemainingQuantity : 0;

        public double FullQuantity { get; private set; } = 0;

        public double Quantity { get; private set; } = 0; //=> PendingQuantity + FilledQuantity;

        public double QuantityRatio => FullQuantity == 0 ? 0 : (Quantity / FullQuantity);

        public double AveragePrice { get; private set; } = double.NaN;
        //public double AveragePrice { get; set; } = double.NaN;

        public double CurrentPrice => IsLive ? MarketData.LastPrice : Bar.Close;

        public double Cost => double.IsNaN(AveragePrice) ? 0 : Math.Abs(Quantity * AveragePrice);

        public double UnrealizedPnL => Cost == 0 ? 0 : Quantity * (CurrentPrice - AveragePrice);

        public double UnrealizedPnLPercent => Cost == 0 ? 0 : 100 * (CurrentPrice - AveragePrice) / AveragePrice;

        #endregion Position Track

        #region Execution Record

        private void AddExecutionRecord(ExecutionRecord exec)
        {
            if (exec.Quantity != 0)
            {
                Console.WriteLine(Bar.Time + " | exec.Quantity = " + exec.Quantity + " | exec.Proceeds = " + exec.Proceeds + " | Quantity = " + Quantity);
                if (exec.Quantity * Quantity >= 0) // Same direction or initial entry (when Qty == 0)
                {
                    exec.LiquidityType = LiquidityType.Added;
                    AveragePrice = Math.Abs((exec.Proceeds + Cost) / (exec.Quantity + Quantity));
                    //AveragePrice = Math.Abs((exec.Proceeds ) / (exec.Quantity));
                    //Console.WriteLine("Added LiquidityType | AveragePrice =" + AveragePrice);
                    if (exec.Quantity > 0)
                        exec.Action = ActionType.Long;
                    else if (exec.Quantity < 0)
                        exec.Action = ActionType.Short;

                    Quantity += exec.Quantity;
                    FullQuantity = Quantity;
                    exec.RealizedPnL = 0;
                }
                else // Opposite direction in this case.
                {
                    exec.LiquidityType = LiquidityType.Removed;

                    if (exec.Quantity > 0)
                        exec.Action = ActionType.Cover;
                    else if (exec.Quantity < 0)
                        exec.Action = ActionType.Sell;

                    double old_qty = Quantity;
                    Quantity += exec.Quantity;

                    if (old_qty * Quantity >= 0)
                    {
                        exec.RealizedPnL = exec.Quantity * (AveragePrice - exec.ExecutionPrice);
                        if (Quantity == 0)
                        {
                            FullQuantity = 0;
                            ProfitTakePrice = double.NaN;
                            StopLossPrice = double.NaN;
                            AveragePrice = double.NaN;
                        }
                    }
                    else if (old_qty * Quantity < 0) // The position is flippe
                    {
                        FullQuantity = Quantity;
                        exec.RealizedPnL = -old_qty * (AveragePrice - exec.ExecutionPrice);
                        AveragePrice = exec.ExecutionPrice;
                    }
                }

                ExecutionRecordList.Add(exec);
            }
        }

        private List<ExecutionRecord> ExecutionRecordList { get; } = new();

        public ExecutionRecord LatestExecutionRecord => ExecutionRecordList.Count > 0 ? ExecutionRecordList.Last() : null;

        public ActionType LatestAction => LatestExecutionRecord is ExecutionRecord exec ? exec.Action : ActionType.None;

        public double RealizedPnL => ExecutionRecordList.Select(n => n.RealizedPnL).Sum();

        #endregion Execution Record
    }
}