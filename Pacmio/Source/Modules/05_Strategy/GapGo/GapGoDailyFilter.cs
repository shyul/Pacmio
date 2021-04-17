/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;

namespace Pacmio.Analysis
{
    public class GapGoDailyFilter : Filter
    {
        public GapGoDailyFilter(double gappercent = 4) : base(BarFreq.Daily, PriceType.Trades)
        {
            BullishPointLimit = BullishGapPercent = gappercent;
            BearishPointLimit = BearishGapPercent = -gappercent;

            string label = "(" + gappercent + "%)";
            GroupName = Name = GetType().Name + label;

            SignalColumn = new SignalColumn(this, "Gap Percent")
            {
                BullishColor = Color.Green,
                BearishColor = Color.DarkOrange
            };

            SignalColumns = new SignalColumn[] { SignalColumn };
            BarAnalysisSet = new(this);
            SignalSeries = new(this);
        }

        public override Range<double> PriceRange { get; } = new Range<double>(1, 300);

        public override Range<double> VolumeRange { get; } = new Range<double>(5e5, double.MaxValue);

        public double BullishGapPercent { get; } = 4;

        public double BearishGapPercent { get; } = -4;

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
                    if (b.GapPercent >= BullishGapPercent || b.GapPercent <= BearishGapPercent)
                    {
                        new SignalDatum(b, SignalColumn, new double[] { b.GainPercent });
                    }
                }
            }
        }
    }
}
