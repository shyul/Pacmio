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

namespace Pacmio.Analysis
{
    public class IndicatorEvaluationResult
    {
        public IndicatorEvaluationResult(BarTable bt, IndicatorSet inds) 
        {
            int totalBarCount = bt.Count;

            if (totalBarCount > 0)
            {
                var bas = inds[bt];
                Indicator ind = inds[bt.BarFreq, bt.Type];

                bt.CalculateRefresh(bas);

                BullishBars = bt.Bars.Where(n => n.GetSignalScore(ind).Bullish > ind.BullishPointLimit);
                BearishBars = bt.Bars.Where(n => n.GetSignalScore(ind).Bearish > ind.BearishPointLimit);

                BullishPercent = BullishBars.Count() * 100 / totalBarCount;
                BearishPercent = BearishBars.Count() * 100 / totalBarCount;

                BullishBars.RunEach(n => BullishPeriods.Add(n.Period));
                BearishBars.RunEach(n => BearishPeriods.Add(n.Period));
            }
        }

        public IEnumerable<Bar> BullishBars { get; }

        public IEnumerable<Bar> BearishBars { get; }

        public MultiPeriod BullishPeriods { get; } = new MultiPeriod();

        public MultiPeriod BearishPeriods { get; } = new MultiPeriod();

        public double BullishPercent { get; } = 0;

        public double BearishPercent { get; } = 0;
    }


    public static class IndicatorManager 
    {
        public static void Evaluate(IEnumerable<Contract> cList,  CancellationTokenSource cts, IProgress<float> Progress) 
        {
            double totalseconds = 0;
            float total_num = cList.Count();
            float i = 0;

            //BarTableSet bts =
            Parallel.ForEach(cList, new ParallelOptions { MaxDegreeOfParallelism = 8 },
                c => {
                    DateTime startTime = DateTime.Now;

                    BarTableSet bts = new BarTableSet(c, false);
                    bts.SetPeriod(pd, Cts);
                    BarTable bt = bts[freq, type];

                    bt.CalculateRefresh(bas);
                    DateTime endTime = DateTime.Now;
                    double seconds = (endTime - startTime).TotalSeconds;
                    totalseconds += seconds;
                    i++;
                    Progress.Report(i * 100.0f / total_num);
                });

        }
    }
}
