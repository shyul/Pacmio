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
    public class TaskSetting 
    {
        public IProgress<float> Progress { get; }

        public CancellationTokenSource Cts { get; }

        public ParallelOptions ParallelOptions { get; } = new ParallelOptions();
    }

    public static class StrategyUtility
    { 
        public static Dictionary<Contract, Dictionary<Strategy, StrategyEvaluationResult>> Evaluate(this IEnumerable<Contract> contracts, IEnumerable<Strategy> strategies, Period evaluateTimeRange, int maxDegreeOfParallelism = 8, CancellationTokenSource cts = null, IProgress<float> progress = null)
        {
            double totalseconds = 0;
            float total_num = contracts.Count();
            float i = 0;

            var filterResult = contracts.Screen(strategies.Select(n => n.Filter), evaluateTimeRange, maxDegreeOfParallelism, cts, progress);

            Console.WriteLine("Start Strategy Analysis ");
            cts = new CancellationTokenSource();
            ParallelOptions po = new ParallelOptions()
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism,
                CancellationToken = cts.Token
            };

            try
            {
                Parallel.ForEach(filterResult, po, item =>
                {
                    DateTime startTime = DateTime.Now;
                    BarTableSet bts = new BarTableSet(item.Key, false);
                    bts.SetPeriod(evaluateTimeRange, cts);

                    Dictionary<Strategy, StrategyEvaluationResult> results = new();

                    foreach (var s in strategies)
                    {
                        bts.CalculateRefresh(s.AnalysisSet);



                    }


                    // So far we are only getting results from one time frame!



                    bts.Dispose();
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

            Console.WriteLine("Strategy Analysis Finished!");

            return null; // filterResult;
        }


        public static void BackTest(this Strategy s, BarTableSet bts, Period periodLimit, CancellationTokenSource cts = null)
        {
            var result = s.Filter.RunScan(bts, periodLimit);

            MultiPeriod mps = bts.MultiPeriod is not null ? new MultiPeriod(bts.MultiPeriod) : new MultiPeriod();

            foreach (var pd in result.Periods)
            {
                if (periodLimit.Contains(pd))
                {
                    mps.Add(pd);
                }
            }

            bts.SetPeriod(mps, cts);
            bts.CalculateRefresh(s.AnalysisSet);

            //BarTable bt = bts[s.BarFreq, s.PriceType];
            //bt.CalculateRefresh(BarAnalysisSet);
        }





        // BarTable bt = bts[BarFreq.Minute];

        // Then run order / trade / position analysis, using Typical price of the next bar as entry or matching limit / stop...
        // This analysis shall not be BarAnalysis  

        // Effectiveness Result -> 

        //Dictionary<IndicatorSet, TrainingResultDatum> TrainingResults = new Dictionary<IndicatorSet, TrainingResultDatum>();

        // Overlapping Backtest N days => trade 1 days Result ->

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
