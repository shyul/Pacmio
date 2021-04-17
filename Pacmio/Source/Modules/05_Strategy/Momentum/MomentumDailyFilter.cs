/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// Filter:
/// Price $1 to $10
/// 1M in volume, Relative Volume of 2
/// Low Float, less than 2e7 shares
/// 5 min Volume Surge 2000% (20x)
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;

namespace Pacmio.Analysis
{
    public class MomentumDailyFilter : Filter
    {
        public MomentumDailyFilter() : base(BarFreq.Daily, PriceType.Trades)
        {
            BullishPointLimit = 1;
            BearishPointLimit = -1;

            GroupName = Name = GetType().Name;

            SignalColumn = new SignalColumn(this, Name + "_Signal")
            {
                BullishColor = Color.Green,
                BearishColor = Color.DarkOrange
            };

            SignalColumns = new SignalColumn[] { SignalColumn };
            BarAnalysisSet = new(this);
            SignalSeries = new(this);
        }

        public override Range<double> PriceRange { get; } = new Range<double>(1, 10);

        public override Range<double> VolumeRange { get; } = new Range<double>(1e6, double.MaxValue);

        public override SignalColumn SignalColumn { get; }

        public override IEnumerable<SignalColumn> SignalColumns { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                if (PriceRange.Contains(b.Typical) && VolumeRange.Contains(b.Volume))
                {
                    if (b.Gain > 0)
                    {
                        new SignalDatum(b, SignalColumn, new double[] { b.Volume / VolumeRange.Minimum });
                    }
                    else if (b.Gain < 0)
                    {
                        new SignalDatum(b, SignalColumn, new double[] { -b.Volume / VolumeRange.Minimum });
                    }
                }
            }
        }
    }
}
