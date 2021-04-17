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
using System.Drawing;
using Pacmio.Analysis;

namespace Pacmio
{
    public class MomentumDailyIndicator : Indicator
    {
        public MomentumDailyIndicator(double minPrice = 1, double maxPrice = 12, double minimumRelativeVolume = 2, double minimumVolume = 1e6) : base(BarFreq.Daily, PriceType.Trades)
        {
            PriceRange = new Range<double>(minPrice, maxPrice);
            MinimumRelativeVolume = minimumRelativeVolume;
            MinimumVolume = minimumVolume;
            string label = "(" + PriceRange.ToString() + "," + MinimumVolume + "," + MinimumRelativeVolume + ")";
            GroupName = Name = GetType().Name + label;

            RelativeVolume = new Relative(Bar.Column_Volume, 20);
            SignalColumn = new SignalColumn(this, Name + "_Signal")
            {
                BullishColor = Color.Green,
                BearishColor = Color.DarkOrange
            };

            SignalColumns = new SignalColumn[] { SignalColumn };
            SignalSeries = new(this);
            BarAnalysisSet = new(this);
        }

        public Range<double> PriceRange { get; }

        public double MinimumRelativeVolume { get; }

        public double MinimumVolume { get; }

        public Relative RelativeVolume { get; }

        public SignalColumn SignalColumn { get; protected set; }

        public override IEnumerable<SignalColumn> SignalColumns { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                double relativeVolume = b[RelativeVolume];
                if (PriceRange.Contains(b.Typical) && b.Volume > MinimumVolume && relativeVolume > MinimumRelativeVolume)
                {
                    new SignalDatum(b, SignalColumn, new double[] { relativeVolume });
                }
            }
        }
    }
}
