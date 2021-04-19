/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;

namespace Pacmio
{
    public class HoldDecision : IDecision
    {
        public HoldDecision(Bar b)
        {
            DecisionBar = b;
        }

        public Bar DecisionBar { get; }

        public double ProfitTakePrice { get; set; }

        public double StopLossPrice { get; set; }

    }
}
