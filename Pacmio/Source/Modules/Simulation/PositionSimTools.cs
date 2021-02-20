/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;

namespace Pacmio
{
    public static class PositionSimTools
    {
        /// <summary>
        /// Commission Calculator based on IB Tiered Fee Structure.
        /// Calculated when adding Liquidity.
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public static double EstimatedCommission(double quantity, double price)
        {
            quantity = Math.Abs(quantity);
            double value = quantity * price;
            double comms = quantity * 0.0035;
            if (comms < 0.35) comms = 0.35;
            else if (comms > 0.01 * value) comms = 0.01 * value;

            double exchange_fee = 0.00045 * quantity;
            double transaction_fee = 0.0000221 * value;
            double finra_fee = 0.00056 * comms;
            double nyse_pass_fee = 0.000175 * comms;

            return comms + exchange_fee + transaction_fee + finra_fee + nyse_pass_fee;
        }

        public static TradeInfo AddLiquidity(this Position ps, double price) => ps.Trade(-ps.Quantity, price);

        public static TradeInfo Close(this Position ps, double price) => ps.Trade(-ps.Quantity, price);

        public static TradeInfo Trade(this Position ps, double quantity, double price)
        {
            Count++;
            double proceeds = 0; // Cash flow: positive means cash to stock (add liquidity), negative means stock to cash (removing liquidity)
            double pnl = 0;
            LiquidityType liquidityType = LiquidityType.None;

            double commission = EstimatedCommission(Math.Abs(quantity), price);

            if (price <= 0) throw new Exception("Price can't be less than or equial to zero. It is not possible to have free or money giving positions");

            if (quantity != 0)
            {
                double order_value = price * quantity;

                if (ps.Quantity == 0)
                {
                    liquidityType = LiquidityType.Added;
                    ps.Quantity = quantity;
                    ps.AveragePrice = price;
                    proceeds = Math.Abs(order_value); // A position add proceeds is always positive, money out of cash.
                }
                else if (ps.Quantity > 0) // Existing long position here.
                {
                    double new_qty = ps.Quantity + quantity;
                    if (quantity > 0) // Long Action
                    {
                        liquidityType = LiquidityType.Added;
                        ps.AveragePrice = (ps.Value + order_value) / new_qty;
                        proceeds = order_value; // Positive proceeds
                    }
                    else if (quantity < 0) // Selling / Short Action
                    {
                        if (new_qty >= 0) // TradeActionType.Sell: Selling long position here
                        {
                            liquidityType = LiquidityType.Removed;
                            pnl = (ps.BreakEvenPrice - price) * quantity; // qty is negative
                            proceeds = order_value; // Negative Proceeds. Since qty < 0, so the cost is negative here, and proceeds is negative
                        }
                        else // TradeActionType.Short; Sell long into short position
                        {
                            liquidityType = LiquidityType.Added;
                            pnl = (price - ps.BreakEvenPrice) * ps.Quantity; // Sell long will generate pnl here
                            proceeds = -(ps.Quantity + new_qty) * price;
                            ps.AveragePrice = price;
                        }
                    }
                    ps.Quantity = new_qty;
                }
                else if (ps.Quantity < 0) // Existing short position here.
                {
                    double new_qty = ps.Quantity + quantity;
                    if (quantity < 0) // Short Action here: ActionType = TradeActionType.Short;
                    {
                        liquidityType = LiquidityType.Added;
                        ps.AveragePrice = (order_value - ps.Value) / new_qty;
                        proceeds = -order_value; // Positive proceeds
                    }
                    else if (quantity > 0) // Buying / Long / Cover Action
                    {
                        if (new_qty <= 0) // ActionType = TradeActionType.Cover;
                        {
                            liquidityType = LiquidityType.Removed;
                            pnl = (ps.BreakEvenPrice - price) * quantity;
                            proceeds = -order_value; // Negative Proceeds. Since qty > 0, so the cost is positive here, so we need to add negative to it.
                        }
                        else // TradeActionType.Long; Cover short and buy into long position
                        {
                            liquidityType = LiquidityType.Added; // Positive proceeds
                            pnl = (ps.BreakEvenPrice - price) * ps.Quantity; // Sell will generate pnl here
                            proceeds = (ps.Quantity + new_qty) * price; // Negative if new_qty is smaller than abs Quantity, which means new_qty value is smaller, so we have money flow back to the pool.
                            ps.AveragePrice = price;
                        }
                    }
                    ps.Quantity = new_qty;
                }
            }

            DateTime time = DateTime.Now;

            return new TradeInfo(ps.Contract.ToString() + "|" + time)
            {
                Contract = ps.Contract,
                OrderId = Count,
                PermId = Count,
                ClientId = -1,
                AccountId = "Simulation",
                FillExchangeCode = "PacmioSim",
                TotalQuantity = quantity,
                AveragePrice = price,
                Quantity = quantity,
                Price = price,
                Commissions = commission,
                LastLiquidity = liquidityType,
                ExecuteTime = time,
                Liquidation = 0,
                RealizedPnL = pnl,
            };
        }

        public static int Count { get; private set; } = 0;
    }
}
