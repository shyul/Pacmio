/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xu;

namespace Pacmio
{
    public static class ResearchTool
    {
        // IndicatorEvaluationResult
        public static IEnumerable<Contract> RunScreener(IEnumerable<Contract> contracts, Filter filter, Period pd, int maxDegreeOfParallelism = 8, CancellationTokenSource cts = null, IProgress<float> progress = null)
        {
            if (cts is null) cts = new CancellationTokenSource();
            double totalseconds = 0;
            int total_num = contracts.Count();
            int i = 0;

            List<(Contract c, double bull, double bear)> clist = new();

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

                    var res = filter.RunScanResult(bts, pd);

                    foreach (var pd in res.BullishPeriods) { Console.WriteLine("Bull: " + pd); }
                    foreach (var pd in res.BearishPeriods) { Console.WriteLine("Bear: " + pd); }

                    if (res.BullishPercent > 0 || res.BearishPercent > 0)
                    {
                        clist.Add((c, res.BullishPercent, res.BearishPercent));
                    }

                    //BarTable bt = bts[BarFreq.Minute];

                    // Then run order / trade / position analysis, using Typical price of the next bar as entry or matching limit / stop...
                    // This analysis shall not be BarAnalysis  

                    // Effectiveness Result -> 

                    //Dictionary<IndicatorSet, TrainingResultDatum> TrainingResults = new Dictionary<IndicatorSet, TrainingResultDatum>();

                    // Overlapping Backtest N days => trade 1 days Result ->


                    bts.Dispose();

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

            foreach (var item in clist.OrderBy(n => (n.bull + n.bear)))
            {
                Console.WriteLine(item.c + " | " + item.bull.ToString("0.##") + "%" + " | " + item.bear.ToString("0.##") + "%");
            }

            return clist.Select(n => n.c);
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

    }
}
