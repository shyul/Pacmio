/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Filter 1) Signal Set; 2) Signal Threshold Range<double>(), within? / outside?; 3) Ranking Column?
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
    public static class FilterUtility
    {
        public static Dictionary<Contract, Dictionary<FilterAnalysis, FilterScreenResult>> Screen(this IEnumerable<Contract> contracts, IEnumerable<FilterAnalysis> filters, Period evaluateTimeRange, int maxDegreeOfParallelism = 8, CancellationTokenSource cts = null, IProgress<float> progress = null)
        {
            if (cts is null) cts = new CancellationTokenSource();
            double totalseconds = 0;
            int total_num = contracts.Count();
            int i = 0;

            Dictionary<Contract, Dictionary<FilterAnalysis, FilterScreenResult>> clist = new();

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
                    MultiPeriod mp = new MultiPeriod();
                    mp.Add(evaluateTimeRange);
                    BarTableSet bts = new BarTableSet(c, mp, false);

                    Dictionary<FilterAnalysis, FilterScreenResult> results = new();

                    foreach (var filter in filters)
                    {
                        var res = filter.RunScan(bts, evaluateTimeRange);
                        foreach (var pd in res.BullishPeriods) { Console.WriteLine("Bull: " + pd); }
                        foreach (var pd in res.BearishPeriods) { Console.WriteLine("Bear: " + pd); }
                        if (res.Percent > 0)
                        {
                            results[filter] = res;
                        }
                    }

                    if (results.Count > 0) clist[c] = results;


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

            foreach (var filter in filters)
            {
                foreach (var item in clist.Select(n => n.Value[filter]).OrderBy(n => n.Percent))
                {
                    Console.WriteLine(item.Contract + " | " + item.FilterAnalysis + " | " + item.Percent.ToString("0.##") + "%");

                }
            }

            Console.WriteLine("Total " + clist.Count + " contracts found!");

            return clist;
        }

        public static void PrintResult(Dictionary<Contract, FilterScreenResult> result)
        {
            var r = result.OrderByDescending(n => n.Value.Percent).Select(n => n.Value).Take(100);

            foreach (var ier in r)
            {
                Console.WriteLine(ier.Contract + ": " + (ier.BullishPercent * 100).ToString("0.###") + "% | " + (ier.BearishPercent * 100).ToString("0.###") + "%");
            }
        }

    }
}
