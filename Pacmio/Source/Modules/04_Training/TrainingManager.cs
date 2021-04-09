/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xu;
using Pacmio.Analysis;


namespace Pacmio
{
    public static class TrainingManager
    {
        public static void RunScreener(IEnumerable<Contract> contracts, IndicatorSet iset, Period pd, int maxDegreeOfParallelism = 8, CancellationTokenSource cts = null, IProgress<float> progress = null) 
        {
            if (cts is null) cts = new CancellationTokenSource();
            double totalseconds = 0;
            int total_num = contracts.Count();
            int i = 0;

            ParallelOptions po = new ParallelOptions()
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism,
                CancellationToken = cts.Token
            };

            try
            {
                Parallel.ForEach(contracts, po, c =>
                {
                    DateTime startTime = DateTime.Now;
                    BarTableSet bts = new BarTableSet(c, false);

                    bts.SetPeriod(pd, cts);

                    var (bullish, bearish) = iset.RunScreener(bts);

                    foreach (var mp in bullish) { Console.WriteLine("Bull: " + mp); }
                    foreach (var mp in bearish) { Console.WriteLine("Bear: " + mp); }

                    bts.SetPeriod(bullish, cts);

                    
                    
                    BarTable bt = bts[BarFreq.Minute];




                    //BarChart bc = bt.GetChart(TestTrend.BarAnalysisSet);
                    DateTime endTime = DateTime.Now;
                    double seconds = (endTime - startTime).TotalSeconds;
                    totalseconds += seconds;
                    i++;
                    progress.Report(i * 100.0f / total_num);
                    po.CancellationToken.ThrowIfCancellationRequested();
                });
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine("Parallel task is cancelled: " + e.Message);
            }
            finally
            {
                cts.Dispose();
            }

        }






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

        /*
        public static TradeInfo AddLiquidity(this PositionInfo ps, double price) => ps.Trade(-ps.Quantity, price);

        public static TradeInfo Close(this PositionInfo ps, double price) => ps.Trade(-ps.Quantity, price);

        public static TradeInfo Trade(this PositionInfo ps, double quantity, double price)
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
        */
    }

    public class OrderRule
    {


    }


    /// public void Simulate(BarTableSet bts, IndicatorSet inds, OrderRule)
    /// {
    ///      1) Run Indicators (includeded in the Strategy) ->
    ///      1.5) Run higher timeframe indicator first
    ///      1.6) Yield MultiPeriod for intested intraday bars
    ///      2) Run Intraday Indicators
    ///      3) Yield all scores ->
    ///      4) Run OrderRules identify all of the scores
    ///      5) Yield OrderDatum Only
    ///
    /// }
    /// 


    public static class DecisionDataManager
    {
        public static Dictionary<Contract, BarTableSet> BarTableSetLUT { get; } = new();




        /*
#region Watch List

/// <summary>
/// Acquire from WatchListManager
/// </summary>
public WatchList WatchList
{
    get => m_WatchList;

    set
    {
        if (m_WatchList is WatchList w) w.RemoveDataConsumer(this);
        m_WatchList = value;
        m_WatchList.AddDataConsumer(this);
    }
}

private WatchList m_WatchList = null;

public List<Contract> ContractList { get; private set; }

public void DataIsUpdated(IDataProvider provider)
{
    if (provider is WatchList w)
    {
        if (ContractList is List<Contract>)
        {
            var list_to_remove = ContractList.Where(n => !w.Contracts.Contains(n));
            list_to_remove.RunEach(n => n.MarketData.RemoveDataConsumer(this));
        }

        ContractList = w.Contracts.ToList();
        ContractList.ForEach(n => n.MarketData.AddDataConsumer(this));
    }
}

#endregion Watch List
*/

    }
}
