/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public static class SimulationManager
    {
        public static double InitialFund { get; set; } = 50000;


        public static readonly List<SimulationSet> List = new List<SimulationSet>();

        // Per Contract and Per TradeRule 



        public static void Train(Contract c)
        {


        }

        public static void Simulate()
        {


        }

        public static TimeSpan Range { get; set; }
    }
}
