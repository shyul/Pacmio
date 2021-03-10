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
                // Get list of contracts at the particular date
                List<Contract> list = new List<Contract>();

                Period pd = new Period(t, t.AddDays(2)); // Length...

                foreach (Contract c in list)
                {
                    // Create Intraday Bar Table Set

                    // Run

                    // Trade, with assumptions.

                    // Keep (yield) the Trade log...
                }
            }
        }
    }
}
