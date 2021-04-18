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
    public abstract class IntervalAnalysis : SingleDataAnalysis
    {
        protected IntervalAnalysis(int interval)
        {
            Interval = interval;

            Label = "(" + Interval.ToString() + ")";
            Name = GetType().Name + Label;
            AreaName = GroupName = GetType().Name;

            Column_Result = new NumericColumn(Name, Label);
            LineSeries = new LineSeries(Column_Result, Color.DarkSlateGray, LineType.Default, 1.5f)
            {
                Name = Name,
                Label = Label,
                LegendName = GroupName,
                Importance = Importance.Major,
                IsAntialiasing = true,
                DrawLimitShade = false,
            };
        }

        protected IntervalAnalysis() { }

        public override string Label { get; }

        #region Parameters

        public override int GetHashCode() => GetType().GetHashCode() ^ Interval;

        public virtual int Interval { get; protected set; }

        #endregion Parameters
    }
}
