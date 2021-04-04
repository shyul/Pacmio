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
    public class DebugDualDataOsc : DebugSeries
    {
        public DebugDualDataOsc(IDualData idd)
        {
            Column_High = idd.Column_High;
            Column_Low = idd.Column_Low;

            string label = "(" + Column_High.Name + "," + Column_Low.Name + ")";
            Name = GetType().Name + label;
            GroupName = GetType().Name;
            Description = label;

            ColumnSeries_High = new(Column_High, idd.UpperColor, 50, 0)
            {
                Name = Column_High.Name,
                LegendName = Column_High.Name,
                Label = "",
                Importance = Importance.Major,
                Side = AlignType.Right,
                IsAntialiasing = false
            };

            ColumnSeries_Low = new(Column_Low, idd.LowerColor, 50, 0)
            {
                Name = Column_Low.Name,
                LegendName = Column_Low.Name,
                Label = "",
                Importance = Importance.Major,
                Side = AlignType.Right,
                IsAntialiasing = false
            };

            AreaName = Name + "_Area";
            AreaRatio = 10;
            ChartEnabled = true;

            idd.AddChild(this);
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Column_High.GetHashCode() ^ Column_Low.GetHashCode();

        public NumericColumn Column_High { get; }

        public NumericColumn Column_Low { get; }

        public ColumnSeries ColumnSeries_High { get; }

        public ColumnSeries ColumnSeries_Low { get; }

        public override Series MainSeries => ColumnSeries_High;

        public override Color Color { get => UpperColor; set => UpperColor = value; }

        public Color UpperColor
        {
            get => ColumnSeries_High.Color;

            set
            {
                Color c = value;
                ColumnSeries_High.Color = c.GetBrightness() < 0.6 ? c.Brightness(0.85f) : c.Brightness(-0.85f);
                ColumnSeries_High.EdgeColor = ColumnSeries_High.TextTheme.ForeColor = c;
            }
        }

        public Color LowerColor
        {
            get => ColumnSeries_High.Color;

            set
            {
                Color c = value;
                ColumnSeries_Low.Color = c.GetBrightness() < 0.6 ? c.Brightness(0.85f) : c.Brightness(-0.85f);
                ColumnSeries_Low.EdgeColor = ColumnSeries_Low.TextTheme.ForeColor = c;
            }
        }

        public override void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                BarChartOscillatorArea area =
                    bc[AreaName] is BarChartOscillatorArea oa ?
                    oa :
                    bc.AddArea(new BarChartOscillatorArea(bc, AreaName, AreaRatio)
                    {
                        Reference = 0,
                        HasXAxisBar = false,
                    });

                area.AddSeries(ColumnSeries_High);
                area.AddSeries(ColumnSeries_Low);
            }
        }
    }
}
