/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xu;
using Xu.Chart;
using Pacmio.Analysis;

namespace Pacmio
{
    public class BarChartArea : Area, IBarChartArea
    {
        public BarChartArea(BarChart chart, string name, float height, float leftAxisRatio = 1) : base(chart, name, height)
        {
            AxisLeft.HeightRatio = leftAxisRatio;
            BarChart = chart;
        }

        public BarChart BarChart { get; private set; }

        public BarTable BarTable => BarChart.BarTable;

        public override void DrawCustomBackground(Graphics g) => DrawCustomBackground(g, this);

        public override void DrawCustomOverlay(Graphics g) => DrawCustomOverlay(g, this);

        public static void DrawCustomBackground(Graphics g, IBarChartArea a)
        {
            if (a.BarTable.Count > 0)
            {
                /*
                int stopPt = StopPt - 1;
                if (stopPt < 0) stopPt = 0;
                else if (stopPt >= Table.Count) stopPt = Table.Count - 1;*/

                // *********************************
                // * 2. Draw Patterns in the chart *
                // *********************************

                g.SetClip(a.Bounds);

                // ******************************
                // * 3. Draw pivot level
                // ******************************

                int x1 = (a.Bounds.Right - a.Bounds.Width * 0.1).ToInt32();
                int x2 = a.Bounds.Right;

                int full_width = x2 - x1;

                g.ResetClip();
            }
        }

        public static void DrawCustomOverlay(Graphics g, IBarChartArea a)
        {
            if (a.BarTable.Count > 0)
            {
                g.SetClip(a.Bounds);
                /*
                int pt = 0;
                for (int i = StartPt; i < StopPt; i++)
                {
                    if (i >= Table.Count) break;
                    if (i >= 0)
                    {
                        int this_color = (int)Table[i].ActionType;

                        if (SignalArea.ColorPalette.ContainsKey(this_color))
                        {
                            int x = IndexToPixel(pt) - (AxisX.TickWidth / 4);
                            g.FillRectangle(SignalArea.ColorPalette[this_color], x, Top, (AxisX.TickWidth / 2), Size.Height);
                        }
                    }
                    pt++;
                }*/
                g.ResetClip();
            }
        }


    }
}
