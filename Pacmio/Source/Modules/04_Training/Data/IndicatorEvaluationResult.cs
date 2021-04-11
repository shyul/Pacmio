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
    public class IndicatorEvaluationResult
    {
        public IndicatorEvaluationResult(Contract c, IndicatorSet inds)
        {
            Contract = c;
            IndicatorSet = inds;
        }

        public Contract Contract { get; }

        public IndicatorSet IndicatorSet { get; }

        public MultiPeriod BullishPeriods { get; } = new MultiPeriod();

        public MultiPeriod BearishPeriods { get; } = new MultiPeriod();

        public double BullishPercent { get; set; } = 0;

        public double BearishPercent { get; set; } = 0;
    }

    public static class IndicatorManager
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
        public static Dictionary<Contract, IndicatorEvaluationResult> Evaluate(IEnumerable<Contract> cList, IndicatorSet inds, Period evaluateTimeRange, CancellationTokenSource cts, IProgress<float> Progress)
        {
            Dictionary<Contract, IndicatorEvaluationResult> result = new();

            double totalseconds = 0;
            float total_num = cList.Count();
            float i = 0;

            //BarTableSet bts =
            Parallel.ForEach(cList, new ParallelOptions { MaxDegreeOfParallelism = 8 }, c =>
            {

                IndicatorEvaluationResult ier = new(c, inds);

                DateTime startTime = DateTime.Now;
                BarTableSet bts = new BarTableSet(c, false);
                bts.SetPeriod(evaluateTimeRange, cts);

                //Dictionary<>

                // So far we are only getting results from one time frame!

                foreach (var item in inds.Where(n => n.freq >= BarFreq.Daily).OrderByDescending(n => n.freq))
                {
                    BarTable bt = bts[item.freq, item.type];

                    Indicator ind = inds[item.freq, item.type];

                    int totalBarCount = bt.Count;
                    bt.CalculateRefresh(item.bas);

                    var BullishBars = bt.Bars.Where(n => n.GetSignalScore(ind).Bullish > ind.BullishPointLimit);
                    var BearishBars = bt.Bars.Where(n => n.GetSignalScore(ind).Bearish > ind.BearishPointLimit);

                    ier.BullishPercent = BullishBars.Count() * 100 / totalBarCount;
                    ier.BearishPercent = BearishBars.Count() * 100 / totalBarCount;

                    BullishBars.RunEach(n => ier.BullishPeriods.Add(n.Period));
                    BearishBars.RunEach(n => ier.BearishPeriods.Add(n.Period));

                }

                DateTime endTime = DateTime.Now;
                double seconds = (endTime - startTime).TotalSeconds;
                totalseconds += seconds;
                i++;
                Progress.Report(i * 100.0f / total_num);
            });


            return result;
        }

        public static void PrintResult(Dictionary<Contract, IndicatorEvaluationResult> result)
        {
            var r = result.OrderByDescending(n => n.Value.BullishPercent).Select(n => n.Value).Take(100);

            foreach (var ier in r)
            {
                Console.WriteLine(ier.Contract + ": " + ier.BullishPercent);
            }
        }


    }
}
