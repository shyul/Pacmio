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
    public sealed class PositionOfTimeframe : BarAnalysis, ISingleData
    {
        public PositionOfTimeframe(BarFreq barFreq = BarFreq.Daily)
        {
            Column = Bar.Column_Typical;
            Frequency = barFreq.GetAttribute<BarFreqInfo>().Frequency;

            string label = "(" + Column.Name + "," + barFreq.ToString() + ")";
            Name = GetType().Name + label;
            GroupName = GetType().Name;
            Description = "Position Of Timeframe " + label;

            Column_Result = new NumericColumn(Name, label);
        }

        public PositionOfTimeframe(ISingleData isd, BarFreq barFreq = BarFreq.Daily)
        {
            Column = isd.Column_Result;
            Frequency = barFreq.GetAttribute<BarFreqInfo>().Frequency;

            string label = "(" + Column.Name + "," + barFreq.ToString() + ")";
            Name = GetType().Name + label;
            GroupName = GetType().Name;
            Description = "Position Of Timeframe " + label;

            Column_Result = new NumericColumn(Name, label);
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Frequency.GetHashCode() ^ Column.GetHashCode();

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
                Range<double> cumulative_range = new Range<double>(double.MaxValue, double.MinValue);
                DateTime base_time = DateTime.MinValue;

                for (int i = bap.StartPt; i < bap.StopPt; i++)
                {
                    Bar b = bt[i];
                    DateTime time = Frequency.Align(b.Time);

                    if (time != base_time)
                    {
                        cumulative_range = new Range<double>(double.MaxValue, double.MinValue);
                    }

                    double value = b[Column];
                    cumulative_range.Insert(value);
                    double range = cumulative_range.Maximum - cumulative_range.Minimum;

                    b[Column_Result] = range > 0 ? (value - cumulative_range.Minimum) / range : 0;
                    base_time = time;
                }
            }
        }
    }
}
