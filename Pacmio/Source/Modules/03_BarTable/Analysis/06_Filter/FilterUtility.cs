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
        public static IEnumerable<Contract> Screen(this IEnumerable<Contract> contracts, FilterAnalysis filter, Period pd, int maxDegreeOfParallelism = 8, CancellationTokenSource cts = null, IProgress<float> progress = null)
        {
            if (cts is null) cts = new CancellationTokenSource();
            double totalseconds = 0;
            int total_num = contracts.Count();
            int i = 0;

            List<(Contract c, double percent)> clist = new();

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

                    var res = filter.RunScan(bts, pd);

                    foreach (var pd in res.BullishPeriods) { Console.WriteLine("Bull: " + pd); }
                    foreach (var pd in res.BearishPeriods) { Console.WriteLine("Bear: " + pd); }

                    if (res.Percent > 0)
                    {
                        clist.Add((c, res.Percent));
                    }

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

            foreach (var item in clist.OrderBy(n => n.percent))
            {
                Console.WriteLine(item.c + " | " + item.percent.ToString("0.##") + "%");

            }

            Console.WriteLine("Total " + clist.Count + " contracts found!");

            return clist.Select(n => n.c);
        }

    }
}
