/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio
{
    public class FilterScanResult
    {
        public FilterScanResult(Contract c, IEnumerable<Bar> bars, IEnumerable<Bar> bullishBars, IEnumerable<Bar> bearishBars)
        {
            Contract = c;

            TotalCount = bars.Count();
            BullishCount = bullishBars.Count();
            BearishCount = bearishBars.Count();

            Percent = TotalCount > 0 ? ((BullishCount + BearishCount) * 100 / TotalCount) : 0;
            BullishPercent = TotalCount > 0 ? (BullishCount * 100 / TotalCount) : 0;
            BearishPercent = TotalCount > 0 ? (BearishCount * 100 / TotalCount) : 0;

            bullishBars.RunEach(n =>
            {
                var pd_bull = ToDailyPeriod(n.Period);
                Periods.Add(pd_bull);
                BullishPeriods.Add(pd_bull);
            });

            bearishBars.RunEach(n =>
            {
                var pd_bull = ToDailyPeriod(n.Period);
                Periods.Add(pd_bull);
                BearishPeriods.Add(pd_bull);
            });
        }

        public Contract Contract { get; }

        public MultiPeriod Periods { get; } = new MultiPeriod();

        public MultiPeriod BullishPeriods { get; } = new MultiPeriod();

        public MultiPeriod BearishPeriods { get; } = new MultiPeriod();

        public int TotalCount { get; }

        public int BullishCount { get; }

        public int BearishCount { get; }

        public double Percent { get; }

        public double BullishPercent { get; }

        public double BearishPercent { get; }

        public static Period ToDailyPeriod(Period pd) => new Period(pd.Start.Date, pd.Stop.AddDays(1).Date);
    }
}
