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
    public sealed class TimeFramePricePosition : BarAnalysis, ISingleData
    {
        public TimeFramePricePosition(BarFreq barFreq = BarFreq.Daily)
        {
            TimeFrameBarFreq = barFreq;
            Frequency = barFreq.GetAttribute<BarFreqInfo>().Frequency;
            Column = Bar.Column_Typical;

            string label = "(" + Column.Name + "," + barFreq.ToString() + ")";
            Name = GetType().Name + label;
            GroupName = GetType().Name;
            Description = "Price Position within Time Frame " + label;

            Column_Result = new NumericColumn(Name, label);
        }

        public TimeFramePricePosition(ISingleData isd, BarFreq barFreq = BarFreq.Daily)
        {
            TimeFrameBarFreq = barFreq;
            Frequency = barFreq.GetAttribute<BarFreqInfo>().Frequency;
            Column = isd.Column_Result;

            string label = "(" + Column.Name + "," + barFreq.ToString() + ")";
            Name = GetType().Name + label;
            GroupName = GetType().Name;
            Description = "Price Position within Time Frame " + label;

            Column_Result = new NumericColumn(Name, label);
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ TimeFrameBarFreq.GetHashCode() ^ Column.GetHashCode();

        public BarFreq TimeFrameBarFreq { get; }

        /// <summary>
        /// The time period for each average frame
        /// </summary>
        public Frequency Frequency { get; }

        public NumericColumn Column { get; }

        public NumericColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            if (bt.Frequency.Span < Frequency.Span)
            {
                Range<double> cumulative_range = new(double.MaxValue, double.MinValue);
                DateTime base_time = DateTime.MinValue;

                for (int i = bap.StartPt; i < bap.StopPt; i++)
                {
                    Bar b = bt[i];
                    DateTime time = Frequency.Align(b.Time);

                    if (time != base_time)
                    {
                        cumulative_range = new Range<double>(double.MaxValue, double.MinValue);
                        base_time = time;
                    }

                    double value = b[Column];
                    cumulative_range.Insert(value);
                    double range = cumulative_range.Maximum - cumulative_range.Minimum;

                    b[Column_Result] = range > 0 ? (value - cumulative_range.Minimum) / range : 0;
                }
            }
        }
    }
}
