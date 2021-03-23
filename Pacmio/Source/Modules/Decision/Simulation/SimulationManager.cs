/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Threading;
using Xu;

namespace Pacmio
{
    public static class SimulationManager
    {
        public static Dictionary<Contract, IntraDayBarTableSet> BarTableSetLUT { get; } = new Dictionary<Contract, IntraDayBarTableSet>();

        public static void Run(Strategy s, Period period)
        {
            for (DateTime t = period.Start.Date; t < period.Stop; t = t.AddDays(1))
            {
                // Set the time for the WatchList;

                // Get the list of contract
                var ContractList = s.WatchList.Contracts;

                // run highest time frame first, 

                //BarTable daily_bt = new BarTable(...);, monthly, weekdly and so on...

                // get a list of Period




            }
        }
    }
}
