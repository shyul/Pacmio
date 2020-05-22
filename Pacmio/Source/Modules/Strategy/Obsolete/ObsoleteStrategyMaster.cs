/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.IO;
using Xu;
using System.Threading.Tasks;

namespace Pacmio
{
    public static class ObsoleteStrategyMaster
    {
        public static bool Enabled { get; set; } = false;
        /// <summary>
        /// Activated Strategies
        /// </summary>
        public static readonly Dictionary<ObsoleteStrategy, List<Contract>> WatchList = new Dictionary<ObsoleteStrategy, List<Contract>>();

        public static readonly Dictionary<Contract, List<ObsoleteStrategy>> TradeContract = new Dictionary<Contract, List<ObsoleteStrategy>>();

        /// <summary>
        /// Has to stop the strategy
        /// </summary>
        public static void BuildList()
        {
            Enabled = false;

            // Make sure all existing positions are off.

            TradeContract.Clear();
            
            WatchList.ToList().ForEach(s => {
                WatchList[s.Key] = s.Key.GetWatchList().ToList(); // This shall kill the old listings
                foreach (Contract c in WatchList[s.Key])
                {
                    if (!TradeContract.ContainsKey(c))
                        TradeContract.Add(c, new List<ObsoleteStrategy>());

                    TradeContract[c].CheckAdd(s.Key);
                }
            });
            
            /*
            // Parallel is not neccessary for this one because it takes a very short amount of effort anyway
            Parallel.ForEach(WatchList.ToList(), s => {
                WatchList[s.Key] = s.Key.GetWatchList().ToList(); // This shall kill the old listings
                foreach (Contract c in WatchList[s.Key])
                {
                    if (!TradeContract.ContainsKey(c))
                        TradeContract.Add(c, new List<Strategy>());

                    TradeContract[c].CheckAdd(s.Key);
                }
            });
            */

            /*
            TradeContract.ToList().ForEach(s => {
                s.Value.ForEach(n => {
                    n.Setup(s.Key);

                });
            });
            */

            Parallel.ForEach(TradeContract, s => {
                Parallel.ForEach(s.Value, n => {
                    n.Setup(s.Key);
                });
            });
            
            //Enabled = true;
        }

        /// <summary>
        /// 0.5% of the total account is the total
        /// </summary>
        public static double MaximumRiskPercentPerContract { get; set; } = 0.002;

        public static double MaximumPositionSizePercent { get; set; } = 0.125;

        public static Period SimulatePeriod { get; set; } = new Period(DateTime.Now.AddDays(-30), DateTime.Now);

        public static void Simulate(double initialValue)
        {
            SimulationAccount sac = new SimulationAccount(initialValue);

            Console.WriteLine("Initial Account = " + sac.BuyingPower);

            //Parallel.ForEach(WatchList.Keys, s => Simulate(s, sac));
            
            foreach (ObsoleteStrategy s in WatchList.Keys)
            {
                Simulate(s, sac);
            }
            
            double TotalProfit = 0;
            double TradeCount = 0;

            // Apply the trade log back to BarTable

            var poslist = sac.Positions.OrderBy(n => n.Value.Accumulation);

            foreach (var item in poslist)
            {
                Console.WriteLine("\n" + item.Key.Name + " ====================== ");
                PositionStatus ps = item.Value;
                TotalProfit += ps.Accumulation;
                TradeCount += ps.TradeCount;
                Console.WriteLine(" Total Trading Count = " + ps.TradeCount + " | Win rate = " + (100 * ps.WinRate).ToString("0.###") + "%" + " | Max Trade Value = " + ps.MaxTradeValue.ToString("0.###"));
                Console.WriteLine(" Total compound profit = " + ps.Accumulation.ToString("0.###") + " (per trade: " + ps.AverageGain.ToString("0.###") + ")");
                Console.WriteLine(" Total Win = " + ps.WinAccumulation.ToString("0.###") + " | Average Win = " + ps.AverageWin.ToString("0.###") + " | Max Single Win = " + ps.MaxSingleWin.ToString("0.###") + " | Min Single Win = " + ps.MinSingleWin.ToString("0.###"));
                Console.WriteLine(" Total Loss = " + ps.LossAccumulation.ToString("0.###") + " | Average Loss = " + ps.AverageLoss.ToString("0.###") + " | Max Single Loss = " + ps.MaxSingleLoss.ToString("0.###") + " | Min Single Loss = " + ps.MinSingleLoss.ToString("0.###"));
                Console.WriteLine(" Long Side PnL = " + ps.LongPnL.ToString("0.###") + " | Short Side PnL = " + ps.ShortPnL.ToString("0.###"));
                Console.WriteLine(item.Key.Name + " ======================\n");
            }

            Console.WriteLine("\n\nTotal Gain = " + TotalProfit);
            Console.WriteLine("Gain per Trade = " + TotalProfit / TradeCount);
            Console.WriteLine("Final Account = " + sac.TotalValue);
            Console.WriteLine("Total PnL = " + sac.TradeLog.TotalPnL());
    
            sac.TradeLog.ExportTradeLog("D:\\sim.tlg");
        }

        public static void Simulate(ObsoleteStrategy s, SimulationAccount sac)
        {
            //Parallel.ForEach(WatchList[s], c => s.Simulate(c, sac));
            
            foreach (Contract c in WatchList[s]) 
            {
                s.Simulate(c, sac);
            }
        }
    }
}
