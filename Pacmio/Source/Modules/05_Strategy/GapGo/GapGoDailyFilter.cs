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
                BullishColor = Color.BlueViolet,
                BearishColor = Color.DarkOrange
            };

            VolumeSignalColumn = new SignalColumn(this, "Volume")
            {
                BullishColor = Color.Pink,
                BearishColor = Color.Yellow
            };

            SignalColumns = new[] { SignalColumn, VolumeSignalColumn };
            BarAnalysisSet = new(this);
            SignalSeries = new(this);
        }

        public override Range<double> PriceRange { get; } = new Range<double>(1, 300);

        public override Range<double> VolumeRange { get; } = new Range<double>(5e5, double.MaxValue);

        public double BullishGapPercent { get; } = 4;

        public double BearishGapPercent { get; } = -4;

        public override SignalColumn SignalColumn { get; }

        public SignalColumn VolumeSignalColumn { get; }

        public override IEnumerable<SignalColumn> SignalColumns { get; }

        // Move these complex analysis to other daily Indicator

        /*
        public NativeApexAnalysis ApexAnalysis { get; } = new(250);

        public TrailingApexPtAnalysis TrailingApexPtAnalysis { get; }

        public TrendLineAnalysis TrendLine { get; }

        public TrailingTrendStrengthAnalysis TrendLineStrength { get; }
        */



        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            //if ((DateTime.Now - bt.LastTime).TotalDays < 4)
            //{
            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                if (PriceRange.Contains(b.Typical) && VolumeRange.Contains(b.Volume))
                {
                    if (b.GapPercent >= BullishGapPercent || b.GapPercent <= BearishGapPercent)
                    {
                        new SignalDatum(b, SignalColumn, new double[] { b.GainPercent });
                        //Console.WriteLine("Gain = " + b.GainPercent);
                        //SignalDatum signal = new SignalDatum(b, GapPercentSignalColumn); //, new double[] { b.GainPercent });
                        //signal.SetPoints(new double[] { b.GapPercent });
                        //Console.WriteLine(b.Time + " | Point = " + b[GapPercentSignalColumn].Points);
                        // new SignalDatum(b, VolumeSignalColumn, new double[] { VolumeRange.Minimum > 0 ? b.Volume / VolumeRange.Minimum : 0 });
                    }
                }
            }

            //}

        }
    }
}
