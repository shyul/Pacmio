/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public sealed class MainArea : Area
    {
        public const string DefaultName = "Main";

        public MainArea(BarChart chart, string name, int height, float leftAxisRatio = 0.3f) : base(chart, name, height)
        {
            AxisLeft.HeightRatio = leftAxisRatio;
            BarChart = chart;
            Importance = Importance.Huge;

            // Configure Price series by assigning the chart type
            // ===================================================
            AddSeries(PriceSeries = new OhlcSeries(BarTable.Column_Open, BarTable.Column_High, BarTable.Column_Low, BarTable.Column_Close, 
                BarTable.Column_Percent)
            {
                Order = int.MaxValue,
                Importance = Importance.Huge,
                LegendName = "PriceSeries"
            });

            PriceSeries.TagColumns.Add(BarTable.Column_PeakTags);

            // Configure volume series
            // ===================================================
            AddSeries(VolumeSeries = new AdColumnSeries(BarTable.Column_Volume, BarTable.Column_Percent, 50)
            {
                Order = int.MinValue,
                Side = AlignType.Left,
                Name = BarTable.Column_Volume.Name,
                LegendName = "VOLUME",
                LegendLabelFormat = "0.##"
            });

            //Table.Column_SHAPE.BullishTheme = new ColorTheme(Color.DarkGray, Color.Gray);
            //Table.Column_SHAPE.BearishTheme = PriceSeries.DownTheme;
        }

        public BarChart BarChart { get; private set; }

        public BarTable BarTable => BarChart.BarTable;

        public readonly OhlcSeries PriceSeries;

        public readonly AdColumnSeries VolumeSeries;

        public override void Coordinate()
        {
            base.Coordinate();

            int pt = 0;
            for (int i = StartPt; i < StopPt; i++)
            {
                if (i >= Table.Count) break;
                if (i >= 0)
                {
                    int this_color = (int)BarTable[i].ActionType;


                }
                pt++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        public override void DrawCustomBackground(Graphics g)
        {
            if (Table.Count > 0)
            {
                /*
                int stopPt = StopPt - 1;
                if (stopPt < 0) stopPt = 0;
                else if (stopPt >= Table.Count) stopPt = Table.Count - 1;*/

                // *********************************
                // * 2. Draw Patterns in the chart *
                // *********************************

                g.SetClip(Bounds);

                // ******************************
                // * 3. Draw pivot level
                // ******************************

                //double last_close = Table[stopPt].Close;


                // Draw Polygons and trendline


                g.ResetClip();
            }
        }

        public override void DrawCustomOverlay(Graphics g)
        {
            if (Table.Count > 0)
            {
                g.SetClip(Bounds);
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

        public override void DrawCursor(Graphics g, ITable table)
        {
            if (Chart.SelectedDataPointUnregulated >= 0)
            {
                //int pt = SelectedDataPoint;

            }
            //base.DrawCursor(g);
        }
    }
}
