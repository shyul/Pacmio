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
    public class FilterTestResult
    {
        public FilterTestResult(Contract c)
        {
            Contract = c;
        }

        public Contract Contract { get; }

        public MultiPeriod Periods { get; } = new MultiPeriod();

        public MultiPeriod BullishPeriods { get; } = new MultiPeriod();

        public MultiPeriod BearishPeriods { get; } = new MultiPeriod();

        public int TotalCount { get; set; } = 0;

        public int BullishCount { get; set; } = 0;

        public int BearishCount { get; set; } = 0;

        public double BullishPercent => TotalCount > 0 ? (BullishCount * 100 / TotalCount) : 0;

        public double BearishPercent => TotalCount > 0 ? (BearishCount * 100 / TotalCount) : 0;
    }
}
