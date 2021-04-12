using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;

namespace Pacmio.Analysis
{
    public interface IFilter { }

    public interface IPriceRangFilter : IFilter
    {
        Range<double> PriceRange { get; }
    }

    public interface IGapPercentFilter : IFilter
    {
        double BullishGapPercent { get; }

        double BearishGapPercent { get; }
    }

    public class RossGapGoDailyFilter : Indicator, IPriceRangFilter, IGapPercentFilter
    {
        public RossGapGoDailyFilter()
        {
            GapPercentSignalColumn = new SignalColumn(this, "Gap Percent")
            {
                BullishColor = Color.BlueViolet,
                BearishColor = Color.DarkOrange
            };

            VolumeSignalColumn = new SignalColumn(this, "Volume")
            {
                BullishColor = Color.Pink,
                BearishColor = Color.Yellow
            };

            SignalColumns = new SignalColumn[] { GapPercentSignalColumn, VolumeSignalColumn };
            SignalSeries = new SignalSeries(this);
        }

        public Range<double> PriceRange { get; } = new Range<double>(1, 20);

        public Range<double> VolumeRange { get; } = new Range<double>(5e5, double.MaxValue);

        public double BullishGapPercent { get; } = 4;

        public double BearishGapPercent { get; } = -4;

        public SignalColumn GapPercentSignalColumn { get; protected set; }

        public SignalColumn VolumeSignalColumn { get; protected set; }

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
                        new SignalDatum(b, GapPercentSignalColumn, new double[] { b.GapPercent });
                        // new SignalDatum(b, VolumeSignalColumn, new double[] { VolumeRange.Minimum > 0 ? b.Volume / VolumeRange.Minimum : 0 });
                    }
                }
            }
        }
    }
}
