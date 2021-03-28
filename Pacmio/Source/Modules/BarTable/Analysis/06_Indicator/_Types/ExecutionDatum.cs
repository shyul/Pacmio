/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio.Analysis
{
    public class ExecutionDatum : IDatum
    {
        public TradeExecutionType Type { get; } = TradeExecutionType.None;

        public double Size { get; } = 1;

        public double LimitPrice { get; } = double.NaN;

        public double AuxPrice { get; } = double.NaN;

        // Use to calculate simulation result... position vs time, price, so on...

        public int SameDecisionCount { get; set; } = 0;
    }
}