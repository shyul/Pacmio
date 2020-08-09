/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public sealed class MainArea : Area
    {
        public const string DefaultName = "Main";

        public MainArea(BarChart chart, int height, float leftAxisRatio = 0.3f) : base(chart, DefaultName, height)
        {
            AxisLeft.HeightRatio = leftAxisRatio;
            BarChart = chart;
            Importance = Importance.Huge;

            // Configure Price series by assigning the chart type
            // ===================================================
            AddSeries(PriceSeries = new OhlcSeries(Bar.Column_Open, Bar.Column_High, Bar.Column_Low, Bar.Column_Close,
                BarTable.GainAnalysis.Column_Percent)
            {
                Order = int.MaxValue,
                Importance = Importance.Huge,
                LegendName = "PriceSeries"
            });

            //PriceSeries.TagColumns.Add(Bar.Column_PeakTags);
            PriceSeries.TagColumns.Add(BarTable.PeakAnalysis.Column_PeakTags);

            // Configure volume series
            // ===================================================
            AddSeries(VolumeSeries = new AdColumnSeries(Bar.Column_Volume, BarTable.GainAnalysis.Column_Percent, 50)
            {
                Order = int.MinValue,
                Side = AlignType.Left,
                Name = Bar.Column_Volume.Name,
                LegendName = "VOLUME",
                LegendLabelFormat = "0.##"
            });
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
                    //int this_color = (int)BarTable[i].ActionType;


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
