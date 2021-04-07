/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public class DebugColumnSeries : DebugSeries
    {
        public DebugColumnSeries(NumericColumn column)
        {
            Column = column;
            //SingleDataAnalysis.AddChild(this);

            string label = "(" + column.Name + ")";
            Name = GetType().Name + label;
            GroupName = GetType().Name;
            Description = column.Name;// "Narrow Range " + label;

            ColumnSeries = new(Column, 50, 0, 0)
            {
                Name = Column.Name,
                LegendName = Column.Name,
                Label = "",
                Importance = Importance.Major,
                Side = AlignType.Right,
                IsAntialiasing = false
            };

            AreaName = Name + "_Area";
            AreaRatio = 10;
            ChartEnabled = true;
        }

        public DebugColumnSeries(ISingleData isd) : this(isd.Column_Result)
        {
            isd.AddChild(this);
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Column.GetHashCode();

        public NumericColumn Column { get; }

        public override Color Color { get => UpperColor; set => UpperColor = value; }

        public Color UpperColor
        {
            get => ColumnSeries.Color;

            set
            {
                Color c = value;
                ColumnSeries.Color = c.GetBrightness() < 0.6 ? c.Brightness(0.85f) : c.Brightness(-0.85f);
                ColumnSeries.EdgeColor = ColumnSeries.TextTheme.ForeColor = c;
            }
        }

        public Color LowerColor
        {
            get => ColumnSeries.LowerColor;

            set
            {
                Color c = value;
                ColumnSeries.LowerColor = c.GetBrightness() < 0.6 ? c.Brightness(0.85f) : c.Brightness(-0.85f);
                ColumnSeries.LowerEdgeColor = ColumnSeries.LowerTextTheme.ForeColor = c;
            }
        }

        public override Series MainSeries => ColumnSeries;

        public AdColumnSeries ColumnSeries { get; }
    }
}