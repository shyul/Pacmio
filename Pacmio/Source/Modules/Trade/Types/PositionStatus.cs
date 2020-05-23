/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public class PositionStatus
    {
        #region Order
        public TradeActionType ActionType
        {
            get
            {
                return m_ActionType;
            }

            private set
            {
                if (m_ActionType != value)
                {
                    m_ActionType = value;
                    // emit Position Status has changed
                }
            }
        }

        private TradeActionType m_ActionType = TradeActionType.None;

        public bool PendingEntry => (EntryOrder is OrderInfo od) && (int)od.Status < 5;

        public bool PendingExit { get; set; } = false;

        public OrderInfo EntryOrder { get; set; } = null;

        #endregion

        #region Position Information

        public double Quantity { get; private set; } = 0;

        public double CostBasis { get; private set; } = double.NaN;

        public double Commission { get; private set; } = 0;

        public double BreakEvenPrice => (Quantity * CostBasis + Commission) / Quantity;

        public double Value => double.IsNaN(CostBasis) ? 0 : Math.Abs(Quantity * CostBasis);

        public void Reset() 
        {
            Quantity = 0;
            CostBasis = double.NaN;
            PendingExit = false;
        }

        public void Set(double qty, double price)
        {
            if (Quantity == qty)
            {
                if (Quantity > 0) ActionType = TradeActionType.LongHold;
                else if (Quantity < 0) ActionType = TradeActionType.ShortHold;
                else
                { 
                    ActionType = TradeActionType.None;
                    PendingExit = false;
                }
            }

            Quantity = qty;
            CostBasis = (Quantity == 0) ? double.NaN : price;
        }

        public (double Proceeds, double PnL, LiquidityType LiquidityType) Close(double price) => Add(-Quantity, price);

        public (double Proceeds, double PnL, LiquidityType LiquidityType) Add(double qty, double price)
        {
            double proceeds = 0; // Cash out flows
            double pnl = 0;
            LiquidityType liquidityType = LiquidityType.None;

            //if (price <= 0) throw new Exception("Price can't be less than or equial to zero. It is not possible to have free or money giving positions");

            if (qty != 0)
            {
                double order_value = price * qty;

                if (Quantity == 0)
                {
                    ActionType = (qty > 0) ? TradeActionType.Long : TradeActionType.Short;
                    liquidityType = LiquidityType.Added;
                    Set(qty, price);
                    proceeds = Math.Abs(order_value); // A position add proceeds is always positive, money out of cash.
                }
                else if (Quantity > 0) // Long position here.
                {
                    double new_qty = Quantity + qty;
                    if (qty > 0)
                    {
                        ActionType = TradeActionType.Long;
                        liquidityType = LiquidityType.Added;
                        CostBasis = (Value + order_value) / new_qty;
                        proceeds = order_value; // Positive proceeds
                        throw new Exception("Add Position over existing long position is not supported so far.");
                    }
                    else if (qty < 0)
                    {
                        if (new_qty >= 0) // Selling long position here
                        {
                            ActionType = TradeActionType.Sell;
                            liquidityType = LiquidityType.Removed;
                            pnl = (CostBasis - price) * qty; // qty is negative
                            proceeds = order_value; // Negative Proceeds. Since qty < 0, so the cost is negative here, and proceeds is negative
                        }
                        else
                        {
                            ActionType = TradeActionType.Short;
                            liquidityType = LiquidityType.Added;
                            pnl = (price - CostBasis) * Quantity; // Sell will generate pnl here
                            proceeds = -(Quantity + new_qty) * price;
                            CostBasis = price;
                            throw new Exception("Sell long into short is not supported so far");
                        }
                    }
                    Quantity = new_qty;
                }
                else if (Quantity < 0)
                {
                    double new_qty = Quantity + qty;
                    if (qty < 0)
                    {
                        ActionType = TradeActionType.Short;
                        liquidityType = LiquidityType.Added;
                        CostBasis = (order_value - Value) / new_qty;
                        proceeds = -order_value; // Positive proceeds
                        throw new Exception("Add Position over existing short position is not supported so far.");
                    }
                    else if (qty > 0)
                    {
                        if (new_qty <= 0)
                        {
                            ActionType = TradeActionType.Cover;
                            liquidityType = LiquidityType.Removed;
                            pnl = (CostBasis - price) * qty;
                            proceeds = -Value - pnl;// -order_value; // Negative Proceeds. Since qty > 0, so the cost is positive here, so we need to add negative to it.
                        }
                        else
                        {
                            ActionType = TradeActionType.Long;
                            liquidityType = LiquidityType.Added; // Positive proceeds
                            pnl = (CostBasis - price) * Quantity; // Sell will generate pnl here
                            proceeds = new_qty * price - Value - pnl;//   (Quantity + new_qty) * price; // Negative if new_qty is smaller than abs Quantity, which means new_qty value is smaller, so we have money flow back to the pool.
                            CostBasis = price;
                            throw new Exception("Cover short into long is not supported so far");
                        }
                    }
                    Quantity = new_qty;
                }

                if (Quantity == 0) CostBasis = double.NaN;
            }
            else
            {
                if (Quantity > 0) ActionType = TradeActionType.LongHold;
                else if (Quantity < 0) ActionType = TradeActionType.ShortHold;
                else
                {
                    CostBasis = double.NaN;
                    ActionType = TradeActionType.None;
                }
            }

            if (ActionType == TradeActionType.Sell)
                LongPnL += pnl;
            else if (ActionType == TradeActionType.Cover)
                ShortPnL += pnl;

            if (Quantity == 0) 
            {
                CostBasis = double.NaN;
                PendingExit = false; 
            }

            return (proceeds, pnl, liquidityType);
        }

        #endregion  Position Information

        //public double RiskDelta { get; set; } = double.NaN;

        public double Stop { get; set; } = double.NaN;

        public double Limit { get; set; } = double.NaN;

        public double PnL
        {
            get
            {
                return m_PnL;
            }
            set
            {
                TradeCount++;
                Accumulation += PnL;
                m_PnL = value;

                if (m_PnL > 0)
                {
                    WinCount++;
                    WinAccumulation += m_PnL;
                    if (MaxSingleWin == 0) MaxSingleWin = m_PnL;
                    if (MinSingleWin == 0) MinSingleWin = m_PnL;
                    MaxSingleWin = Math.Max(MaxSingleWin, m_PnL);
                    MinSingleWin = Math.Min(MinSingleWin, m_PnL);
                }
                else if (m_PnL < 0)
                {
                    LossCount++;
                    LossAccumulation += m_PnL;
                    if (MaxSingleLoss == 0) MaxSingleLoss = m_PnL;
                    if (MinSingleLoss == 0) MinSingleLoss = m_PnL;
                    MaxSingleLoss = Math.Min(MaxSingleLoss, m_PnL);
                    MinSingleLoss = Math.Max(MinSingleLoss, m_PnL);
                }
            }
        }

        private double m_PnL = 0;

        #region Statistics

        public double MaxTradeValue { get; set; } = 0;

        public double TradeCount { get; private set; } = 0;

        public double WinCount { get; private set; } = 0;

        public double LossCount { get; private set; } = 0;

        public double WinRate
        {
            get
            {
                if (WinCount > 0)
                    return WinCount / (WinCount + LossCount);
                else
                    return 0;
            }
        }

        public double Accumulation { get; private set; } = 0;

        public double AverageGain => (TradeCount > 0) ? Accumulation / TradeCount : 0;

        public double WinAccumulation { get; private set; } = 0;

        public double AverageWin => (WinCount > 0) ? WinAccumulation / WinCount : 0;

        public double LossAccumulation { get; private set; } = 0;

        public double AverageLoss => (LossCount > 0) ? LossAccumulation / LossCount : 0;

        public double MaxSingleWin { get; private set; } = 0;

        public double MinSingleWin { get; private set; } = 0;

        public double MaxSingleLoss { get; private set; } = 0;

        public double MinSingleLoss { get; private set; } = 0;

        public double LongPnL { get; private set; } = 0;

        public double ShortPnL { get; private set; } = 0;

        #endregion Statistics
    }
}
