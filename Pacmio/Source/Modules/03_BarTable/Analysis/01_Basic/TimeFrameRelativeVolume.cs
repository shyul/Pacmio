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
    public sealed class TimeFrameRelativeVolume : BarAnalysis, ISingleData
    {
        public TimeFrameRelativeVolume(int interval = 5, BarFreq barFreq = BarFreq.Daily)
        {
            TimeFrameBarFreq = barFreq;
            Frequency = barFreq.GetAttribute<BarFreqInfo>().Frequency;
            Interval = interval;
            Multiplier = 2D / (Interval + 1D);

            TimeFrameCumulativeVolume = new TimeFrameCumulativeVolume(barFreq);

            string label = "(" + Interval + "," + barFreq.ToString() + ")";
            Name = GetType().Name + label;
            GroupName = GetType().Name;
            Description = "Relative Volume within Time Frame " + label;

            Column_EMA = new NumericColumn(Name + "_EMA", label);
            Column_Result = new NumericColumn(Name, label);

            TimeFrameCumulativeVolume.AddChild(this);
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Interval ^ TimeFrameBarFreq.GetHashCode();

        public BarFreq TimeFrameBarFreq { get; }

        public Frequency Frequency { get; }

        public int Interval { get; }

        public double Multiplier { get; }

        public TimeFrameCumulativeVolume TimeFrameCumulativeVolume { get; }

        public NumericColumn Column_EMA { get; }

        public NumericColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            BarTableSet bts = bt.BarTableSet;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                DateTime time = b.Time;
                if (bts[TimeFrameBarFreq][time].Bar_1 is Bar timeFrame_b)
                {
                    DateTime lastTime = time - (Frequency.Align(time) - timeFrame_b.Time);

                    if (bt.GetCurrentOrFormerByTime(lastTime) is Bar b_1 && Frequency.Align(b_1.Time) == timeFrame_b.Time)
                    {
                        Console.WriteLine("B_1 Time = " + b_1.Time + " | B Time = " + b.Time);

                        double y0 = b_1[Column_EMA];
                        double value = b[TimeFrameCumulativeVolume];
                        double ma = b[Column_EMA] = (value - y0) * Multiplier + y0;
                        b[Column_Result] = value / ma;
                    }
                    else // Time Frame penetration...
                    {
                        b[Column_Result] = 0;
                        b[Column_EMA] = b[TimeFrameCumulativeVolume];
                    }
                }
                else // Beginning of the table
                {
                    b[Column_Result] = 0;
                    b[Column_EMA] = b[TimeFrameCumulativeVolume];
                }
            }
        }
    }
}
