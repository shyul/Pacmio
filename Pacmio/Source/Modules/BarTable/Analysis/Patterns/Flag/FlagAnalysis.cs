/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio.Analysis
{
    public class FlagAnalysis : BarAnalysis
    {

        public int MaximumInterval { get; } = 20;

        public int MaximumRunUpInterval { get; } = 5;

        public double MaximumFlagRatio { get; } = 0.2;

        protected override void Calculate(BarAnalysisPointer bap)
        {       
            // Yield Pattern
                // Yield Critical Levels


        }
    }
}
