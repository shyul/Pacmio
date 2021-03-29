/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xu;
using Pacmio.Analysis;

namespace Pacmio
{
    public static class SimulationManager
    {
        // Find Contracts!!

        public static void Run(IndicatorSet s, Period period)
        {
            for (DateTime t = period.Start.Date; t < period.Stop; t = t.AddDays(1))
            {
                // Set the time for the WatchList;

                // Get the list of contract
                //var ContractList = s.WatchList.Contracts;

                // run highest time frame first, 

                //BarTable daily_bt = new BarTable(...);, monthly, weekdly and so on...

                // get a list of Period




            }
        }


        public static void Simulate(IEnumerable<Contract> cList, Indicator ind, Period pd, CancellationTokenSource cts = null, IProgress<float> progress = null)
        {
            foreach(Contract c in cList)
            {
                //BarTableSet bts = new BarTableSet(c);

                // Run daily first,

                // 

            }
        }


        #region Search for tradeable contracts

        public static (double BullishPercent, double BearishPercent) Evaluate(this BarTable bt, Indicator indf)
        {
            int countTotal = bt.Count;
            if (countTotal > 0)
            {
                var bas = BarAnalysisSet.Create(indf);
                bt.CalculateRefresh(bas);
                var scores = bt.Bars.Select(n => n.GetSignalScore(indf));
                int countBullish = scores.Where(n => n.Bullish >= indf.HighScoreLimit).Count();
                int countBearish = scores.Where(n => n.Bearish >= indf.LowScoreLimit).Count();
                return (countBullish / countTotal, countBearish / countTotal);
            }
            else
                return (0, 0);
        }

        public static (double HighScorePercent, double LowScorePercent) Evaluate2(Indicator indf, BarTable bt)
        {
            int countTotal = bt.Count;
            if (countTotal > 0)
            {
                var bas = BarAnalysisSet.Create(indf);
                bt.CalculateRefresh(bas);
                var scores = bt.Bars.Select(n => n.GetSignalScore(indf));
                int countBullish = scores.Where(n => n.Bullish >= indf.HighScoreLimit).Count();
                int countBearish = scores.Where(n => n.Bearish >= indf.LowScoreLimit).Count();
                return (countBullish / countTotal, countBearish / countTotal);
            }
            else
                return (0, 0);
        }

        public static IEnumerable<Contract> Search(Indicator ind, Period pd)
        {
            return Search(ContractManager.Values.Where(n => n is Stock s && s.Country == "US" && s.Status == ContractStatus.Alive).OrderBy(n => n.Name).Select(n => n as Stock).ToList(), ind, pd);
        }

        public static IEnumerable<Contract> Search(IEnumerable<Contract> clist, Indicator ind, Period pd) 
        {

            return new List<Contract>();
        }

        #endregion Search for tradeable contracts
    }
}
