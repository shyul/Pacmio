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

namespace Pacmio
{
    public class EvaluationResult
    {
        public EvaluationResult(Contract c, Strategy inds)
        {
            Contract = c;
            IndicatorSet = inds;
        }

        public Contract Contract { get; }

        public Strategy IndicatorSet { get; }

        public MultiPeriod BullishPeriods { get; } = new MultiPeriod();

        public MultiPeriod BearishPeriods { get; } = new MultiPeriod();

        public double BullishPercent { get; set; } = 0;

        public double BearishPercent { get; set; } = 0;

        // Total Wins

        // Total Loss


    }
}
