/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
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
            PriceSeries.TagColumns.Add(BarTable.PivotPointAnalysis.Column_PeakTags);

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




                if (BarChart.LastBar is Bar b)
                {
                    double last_close = b.Close;
                    var patterns = b.Patterns.Where(n => n.Key.Source.ChartEnabled && n.Key.Source is IChartPattern ig && ig.AreaName == DefaultName).ToArray();
                    //var all_pivots = patterns.SelectMany(n => n.Value.Pivots);

                    Range<double> weights = new Range<double>(double.MaxValue, double.MinValue);

                    foreach (var p0 in patterns)
                    {
                        PatternColumn pc = p0.Key;
                        PatternDatum pd = p0.Value;

                        foreach (IPivot ip in pd.Pivots)
                        {
                            weights.Insert(ip.Weight);
                        }
                    }

                    double maxWeight = weights.Max;

                    foreach (var p0 in patterns)
                    {
                        PatternColumn pc = p0.Key;
                        PatternDatum pd = p0.Value;

                        foreach (IPivot ip in pd.Pivots)
                        {
                            if (ip is PivotLine line)
                            {
                                int x1 = line.X1 - StartPt;
                                if (x1 >= 0)
                                {
                                    int x3 = StopPt - StartPt - 1;
                                    double y1 = line.Y1;
                                    double y3 = y1 + (line.TrendRate * (x3 - x1)); //line.Level;// 

                                    Point pt1 = new Point(IndexToPixel(x1), AxisY(AlignType.Right).ValueToPixel(y1));
                                    Point pt3 = new Point(IndexToPixel(x3), AxisY(AlignType.Right).ValueToPixel(y3));

                                    int intensity = (255 * ip.Weight / maxWeight).ToInt32();

                                    if (intensity > 255) intensity = 255;
                                    else if (intensity < 1) intensity = 1;

                                    if (y3 > y1)
                                        g.DrawLine(new Pen(Color.FromArgb(intensity, 26, 120, 32)), pt1, pt3);
                                    else if (y3 < y1)
                                        g.DrawLine(new Pen(Color.FromArgb(intensity, 120, 26, 32)), pt1, pt3);
                                    else
                                        g.DrawLine(new Pen(Color.FromArgb(intensity, 32, 32, 32)), pt1, pt3);
                                }
                            }
                            else if (ip is PivotLevel level)
                            {
                                int x1 = level.X1 - StartPt;
                                if (x1 >= 0)
                                {
                                    int x3 = StopPt - StartPt - 1;
                                    double y1 = level.Y1;
                                    int py1 = AxisY(AlignType.Right).ValueToPixel(y1);

                                    Point pt1 = new Point(IndexToPixel(x1), py1);
                                    Point pt3 = new Point(IndexToPixel(x3), py1);

                                    int intensity = (255 * ip.Weight / maxWeight).ToInt32();

                                    if (intensity > 255) intensity = 255;
                                    else if (intensity < 1) intensity = 1;

                                    g.DrawLine(new Pen(Color.FromArgb(intensity, 80, 80, 32)), pt1, pt3);
                                }
                            }
                        }
                    }
                }

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
