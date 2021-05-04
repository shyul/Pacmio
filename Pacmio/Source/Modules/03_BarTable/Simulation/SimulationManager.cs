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
    public static class SimulationManager
    {
        /// <summary>
        /// This function is for narrowing down the group of contracts for actually simulation.
        /// </summary>
        /// <param name="cList"></param>
        /// <param name="inds"></param>
        /// <param name="evaluateTimeRange"></param>
        /// <param name="cts"></param>
        /// <param name="Progress"></param>
        /// <returns></returns>
        public static Dictionary<Contract, FilterScanResult> Evaluate(IEnumerable<Contract> cList, BarAnalysisSet inds, Period evaluateTimeRange, CancellationTokenSource cts, IProgress<float> Progress)
        {
            Dictionary<Contract, FilterScanResult> result = new();

            double totalseconds = 0;
            float total_num = cList.Count();
            float i = 0;

            //BarTableSet bts =
            Parallel.ForEach(cList, new ParallelOptions { MaxDegreeOfParallelism = 8 }, c =>
            {
                DateTime startTime = DateTime.Now;
                BarTableSet bts = new BarTableSet(c, false);
                bts.SetPeriod(evaluateTimeRange, cts);

                //Dictionary<>

                // So far we are only getting results from one time frame!




                DateTime endTime = DateTime.Now;
                double seconds = (endTime - startTime).TotalSeconds;
                totalseconds += seconds;
                i++;
                Progress.Report(i * 100.0f / total_num);
            });


            return result;
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


        public static void PrintResult(Dictionary<Contract, FilterScanResult> result)
        {
            var r = result.OrderByDescending(n => n.Value.Percent).Select(n => n.Value).Take(100);

            foreach (var ier in r)
            {
                Console.WriteLine(ier.Contract + ": " + (ier.BullishPercent * 100).ToString("0.###") + "% | " + (ier.BearishPercent * 100).ToString("0.###") + "%");
            }
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
