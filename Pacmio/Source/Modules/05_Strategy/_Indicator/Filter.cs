/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xu;
using Pacmio.Analysis;

namespace Pacmio
{
    public abstract class Filter : Indicator
    {
        protected Filter(BarFreq barFreq, PriceType type) : base(barFreq, type)
        {

        }

        /*
        public Filter(Range<double> priceRange, Range<double> volumeRange, Range<double> gapExcluded, Range<double> gainExcluded)
        {
            PriceRange = priceRange;
            VolumeRange = volumeRange;
            GapPercentExcluded = gapExcluded;
            GainPercentExcluded = gainExcluded;

            RelativeVolume.AddChild(this);

            SignalColumns = new SignalColumn[] { };
            SignalSeries = new(this);
            BarAnalysisSet = new(this);
        }
        */

        public virtual Range<double> PriceRange { get; } //= new Range<double>(1, 10);

        public virtual Range<double> VolumeRange { get; } // = new Range<double>(1e6, double.MaxValue);

        public abstract SignalColumn SignalColumn { get; }



        /*
        public virtual Range<double> RelativeVolumeRange { get; } //= new Range<double>(1.5, double.MaxValue);

        public virtual Range<double> GainPercentExcluded { get; } //= new Range<double>(-1, 1);

        public virtual Range<double> GapPercentExcluded { get; } // = new Range<double>(-4, 4);

        public virtual Relative RelativeVolume { get; } = new Relative(Bar.Column_Volume, 5);

       
        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                if (PriceRange.Contains(b.Typical) && VolumeRange.Contains(b.Volume))
                {
                    //new SignalDatum(b, PriceVolumeSignalColumn, new double[] { 1 });
                    if ((!GainPercentExcluded.Contains(b.GainPercent)) && (!GapPercentExcluded.Contains(b.GapPercent)))
                    {
                        //new SignalDatum(b, GainSignalColumn, new double[] { 1 });
                    }
                }
            }
        }*/
    }
}
