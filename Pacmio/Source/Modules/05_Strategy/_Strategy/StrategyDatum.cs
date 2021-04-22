﻿/// ***************************************************************************
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

            if (Bar.Bar_1 is Bar b_1 && b_1[Strategy] is StrategyDatum sd_1)
            {
                Datum_1 = sd_1;
                ProfitTakePrice = sd_1.ProfitTakePrice;
                StopLossPrice = sd_1.StopLossPrice;
                //ExecutionPrice = sd_1.ExecutionPrice;

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

        public Bar Bar { get; }

        public Contract Contract => Bar.Table.Contract;

        public MarketData MarketData => Contract.MarketData;

        public PositionInfo PositionInfo => Strategy.AccountInfo is AccountInfo ac ? ac[Contract] : null;

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


        public double RiskPart => double.IsNaN(AveragePrice) ? double.NaN : Math.Abs(AveragePrice - StopLossPrice);

        public double RewardPart => double.IsNaN(AveragePrice) ? double.NaN : Math.Abs(ProfitTakePrice - AveragePrice);

        public double RewardRiskRatio => double.IsNaN(AveragePrice) ? double.NaN : (RewardPart / RiskPart);


        public string Message { get; set; } = string.Empty;

        public int EntryBarIndex { get; set; } = -1;

        #endregion Decision



        public void CheckStopLoss()
        {
            if (Quantity > 0) // || sd.Datum_1.Message == RangeBarBullishMessage)
            {
                if (Bar.Contains(StopLossPrice))
                {
                    SendOrder(StopLossPrice, -1, OrderType.MidPrice);
                }
                else if (Bar.High < StopLossPrice)
                {
                    SendOrder(Bar.Open, -1, OrderType.MidPrice);
                }
            }
            else if (Quantity < 0) // || sd.Datum_1.Message == RangeBarBearishMessage)
            {
                if (Bar.Contains(StopLossPrice))
                {
                    SendOrder(StopLossPrice, 1, OrderType.MidPrice);
                }
                else if (Bar.Low > StopLossPrice)
                {
                    SendOrder(Bar.Open, 1, OrderType.MidPrice);
                }
            }
        }

        public void SendOrder(double executionPrice, double scale, OrderType orderType)
        {
            if (scale != 0)
            {

                double orderQty = Quantity == 0 ? 1 : Math.Abs(Quantity * scale);


                if (scale * Quantity < 0 && scale == 1) // Identify if it closes the position entirely.
                {
                    AveragePrice = double.NaN;
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

                    AddExecutionRecord(new ExecutionRecord(executionPrice + slippage, scale, commission, orderType));
                }

                // Modify the decision:
                // 1. Null, no decsion, example the position just got exit
                // 2. Entry Decision, when there is an oppotunity
                // 3. Hold Decision, check the changes of the price target and stop loss.
            }
        }


        #region Position Track

        public double PendingQuantity { get; set; } = 0;

        public double Quantity { get; private set; } = 0;

        public double AveragePrice { get; private set; } = double.NaN;

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

                    exec.RealizedPnL = exec.Quantity * (AveragePrice - exec.ExecutionPrice);
                    Quantity -= exec.Quantity;
                }

                ExecutionRecordList.Add(exec);
            }
        }

        private List<ExecutionRecord> ExecutionRecordList = new();

        public ExecutionRecord LatestExecutionRecord => ExecutionRecordList.Count > 0 ? ExecutionRecordList.Last() : null;

        public ActionType LatestAction => LatestExecutionRecord is ExecutionRecord exr ? exr.Action : ActionType.None;

        public double RealizedPnL => ExecutionRecordList.Select(n => n.RealizedPnL).Sum();

        #endregion Execution Record
    }
}