/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public sealed class PositionSeries : Series
    {
        public PositionSeries(BarChart chart)
        {
            BarChart = chart;
            LegendName = "Position";
            Width = 40;
        }

        public BarChart BarChart { get; }

        public BarTable Table => BarChart.BarTable;

        public BarAnalysisSet BarAnalysisSet => BarChart.BarAnalysisSet;

        public override List<(string text, Font font, Brush brush)> ValueLabels(ITable table, int pt)
        {
            List<(string text, Font font, Brush brush)> labels = new List<(string text, Font font, Brush brush)>();



            return labels;
        }

        public override void Draw(Graphics g, IIndexArea area, ITable table)
        {
            var (pointList, pt, _, _) = GetPixel(area, Side);

            // Don't even bother if the data column has no data.
            if (pointList.Count > 0 && pt > 0)
            {
                // Turn on the antialiasing if it is required and always turn it off in the end.
                g.SmoothingMode = IsAntialiasing ? SmoothingMode.HighQuality : SmoothingMode.Default;

                var points = pointList.Select(n => n.point);

                // Get the line path.
                using GraphicsPath line = LineSeries.GetLinePath(points, LineType.Step, 0, area.AxisX.HalfTickWidth);

                // If the area is oscillator, here is how we draw the limit shades
                if (area is OscillatorArea oa)
                    oa.DrawLimitShade(g, line);

                // Draw the line itself.
                //LineSeries.DrawLine(g, Theme, line, points, Width, LineType.Step);
                g.DrawPath(Theme.ForePen, line);
            }

            // Reset antialiasing.
            g.SmoothingMode = SmoothingMode.Default;
        }

        public (List<(int index, Point point)>, int, int, int) GetPixel(IIndexArea area, AlignType side)
        {
            List<(int index, Point point)> points = new List<(int index, Point point)>();
            int max_y = area.Bottom;
            int min_y = area.Top;
            int pt = 0;

            for (int i = area.StartPt; i < area.StopPt; i++)
            {
                if (i >= Table.Count)
                    break;
                else if (i >= 0)
                {
                    /*
                    double data = Table[i][BarChart.Strategy].PnL;
                    if (!double.IsNaN(data))
                    {
                        int x = area.IndexToPixel(pt);
                        int data_pix = area.AxisY(side).ValueToPixel(data);

                        points.Add((i, new Point(x, data_pix)));
                        if (data_pix < max_y) max_y = data_pix;
                        if (data_pix > min_y) min_y = data_pix;
                    }*/
                }
                pt++;
            }

            return (points, pt, min_y, max_y);
        }
    }
}
