/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public sealed class TimeFrameCumulativeVolume : BarAnalysis, ISingleData
    {
        public TimeFrameCumulativeVolume(BarFreq barFreq = BarFreq.Daily)
        {
            TimeFrameBarFreq = barFreq;
            Frequency = barFreq.GetAttribute<BarFreqInfo>().Frequency;

            string label = "(" + barFreq.ToString() + ")";
            Name = GetType().Name + label;
            GroupName = GetType().Name;
            Description = "Cumulative Volume within Time Frame " + label;

            Column_Result = new NumericColumn(Name, label);
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ TimeFrameBarFreq.GetHashCode();

        public BarFreq TimeFrameBarFreq { get; }

        public Frequency Frequency { get; }

        public NumericColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            if (bt.Frequency.Span < Frequency.Span)
            {
                double cumulativeVolume = 0;
                DateTime base_time = DateTime.MinValue;

                for (int i = bap.StartPt; i < bap.StopPt; i++)
                {
                    Bar b = bt[i];
                    DateTime time = Frequency.Align(b.Time);

                    if (time != base_time)
                    {
                        cumulativeVolume = 0;
                        base_time = time;
                    }

                    cumulativeVolume += b.Volume;
                    b[Column_Result] = cumulativeVolume;
                }
            }
        }
    }
}
