/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public abstract class CustomIntervalAnalysis : IntervalAnalysis
    {
        protected CustomIntervalAnalysis(NumericColumn column, int interval)
        {
            Interval = interval;
            Column = column;

            Label = (Column is null) ? "(error)" : ((Column == Bar.Column_Close) ? "(" + Interval.ToString() + ")" : "(" + Column.Name + "," + Interval.ToString() + ")");
            Name = GetType().Name + Label;
            GroupName = (Column == Bar.Column_Close) ? GetType().Name : GetType().Name + " (" + Column.Name + ")";
            Description = "Custom Interval Analysis " + Label;

            Column_Result = new NumericColumn(Name, Label);
            LineSeries = new LineSeries(Column_Result)
            {
                Name = Name,
                LegendName = GroupName,
                Label = Label,
                IsAntialiasing = true,
                DrawLimitShade = false
            };
        }

        protected CustomIntervalAnalysis() { }

        public override int GetHashCode() => GetType().GetHashCode() ^ Column.GetHashCode() ^ Interval;

        public NumericColumn Column { get; protected set; }
    }
}