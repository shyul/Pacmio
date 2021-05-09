/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// For example, calculate relative volume
/// https://www.warriortrading.com/relative-volume-day-trading-terminology/
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
    public sealed class RelativeToAverage : BarAnalysis, ISingleData
    {
        public RelativeToAverage(NumericColumn column, int interval = 20) :
            this(column, new SMA(column, interval) { ChartEnabled = false })
        { }

        public RelativeToAverage(NumericColumn column, MovingAverageAnalysis average_isd)
        {
            Column = column;
            MovingAverage = average_isd;

            string label = "(" + Column.Name + "," + MovingAverage.GetType().Name + "," + MovingAverage.Interval + ")";
            GroupName = Name = GetType().Name + label;
            Description = "Relative To Average " + label;

            Column_Result = new NumericColumn(Name) { Label = label };
            average_isd.AddChild(this);
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Column.GetHashCode() ^ MovingAverage.GetHashCode();

        public NumericColumn Column { get; }

        public MovingAverageAnalysis MovingAverage { get; }

        public NumericColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            int i = bap.StartPt;
            if (i == 0) i = 1;

            for (; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                double v = b[Column];
                double v_normal = bt[i - 1][MovingAverage.Column_Result];
                b[Column_Result] = v_normal != 0 ? v / v_normal : 0;
            }
        }
    }
}
