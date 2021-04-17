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
        public static Dictionary<Contract, IndicatorScanResult> Evaluate(IEnumerable<Contract> cList, IndicatorSet inds, Period evaluateTimeRange, CancellationTokenSource cts, IProgress<float> Progress)
        {
            Dictionary<Contract, IndicatorScanResult> result = new();

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

                foreach (var ind in inds.Where(n => n.BarFreq >= BarFreq.Daily).OrderByDescending(n => n.BarFreq))
                {

                }


                DateTime endTime = DateTime.Now;
                double seconds = (endTime - startTime).TotalSeconds;
                totalseconds += seconds;
                i++;
                Progress.Report(i * 100.0f / total_num);
            });


            return result;
        }

        public static void PrintResult(Dictionary<Contract, IndicatorScanResult> result)
        {
            var r = result.OrderByDescending(n => n.Value.BullishPercent).Select(n => n.Value).Take(100);

            foreach (var ier in r)
            {
                Console.WriteLine(ier.Contract + ": " + ier.BullishPercent);
            }
        }
    }
}
