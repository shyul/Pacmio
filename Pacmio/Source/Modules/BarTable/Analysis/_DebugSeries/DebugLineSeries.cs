/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public class DebugLineSeries : DebugSeries
    {
        public DebugLineSeries(NumericColumn column)
        {
            Column = column;
            //SingleDataAnalysis.AddChild(this);

            string label = "(" + column.Name + ")";
            Name = GetType().Name + label;
            GroupName = GetType().Name;
            Description = column.Name;

            LineSeries = new LineSeries(Column)
            {
                Name = Name,
                LegendName = GroupName,
                Label = label,
                IsAntialiasing = true,
                DrawLimitShade = false
            };

            AreaName = Name + "_Area";
            AreaRatio = 10;
            ChartEnabled = true;
        }

        public DebugLineSeries(ISingleData isd) : this(isd.Column_Result)
        {
            isd.AddChild(this);
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Column.GetHashCode();

        public NumericColumn Column { get; }

        public override Series MainSeries => LineSeries;

        public LineSeries LineSeries { get; }
    }
}
