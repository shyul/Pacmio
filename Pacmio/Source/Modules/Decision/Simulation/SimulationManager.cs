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
        public static double InitialFund { get; set; } = 50000;

        

        public static readonly List<SimulationSet> List = new List<SimulationSet>();

        // Per Contract and Per TradeRule 

        public static int MaxDegreeOfParallelism => Root.DegreeOfParallelism;

        /// <summary>
        /// List of all available Contracts
        /// </summary>
        public static List<Contract> ContractList { get; } = new List<Contract>();

        //public static BarTableSet BarTableSet { get; } = new BarTableSet();

        public static void Simulate(Period pd, CancellationTokenSource cts)
        {

        }

        public static void Train(Contract c)
        {


        }

        public static void Simulate()
        {


        }

        public static TimeSpan Range { get; set; }
    }
}
