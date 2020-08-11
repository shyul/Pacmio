/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public class TrendAnalysis : BarAnalysis, IPattern, IChartGraphics
    {
        public TrendAnalysis(BarAnalysis ba, int test_interval = 250, int maximumPeakProminence = 100, int minimumPeakProminence = 5)
        {
            string label = "(" + ba.Name + "," + test_interval + "," + maximumPeakProminence + "," + minimumPeakProminence + ")";
            Name = GetType().Name + label;

            TrailingPivotPointAnalysis = new TrailingPivotPoint(ba, test_interval, maximumPeakProminence, minimumPeakProminence);
            TrailingPivotPointAnalysis.AddChild(this);

            Result_Column = new PatternColumn(this);
            GraphicsAreaName = (ba is IChartSeries ics) ? ics.AreaName : null;
        }

        public TrendAnalysis(int test_interval)
        {

            string label = "(" + test_interval + ")";
            Name = GetType().Name + label;

            TrailingPivotPointAnalysis = new TrailingPivotPoint(test_interval);
            TrailingPivotPointAnalysis.AddChild(this);

            Result_Column = new PatternColumn(this);
            GraphicsAreaName = MainArea.DefaultName;
        }

        public virtual int TestInterval => TrailingPivotPointAnalysis.TestInterval;

        public virtual int MaximumResultCount { get; }

        public double Tolerance => 0.01;

        #region Calculation

        public TrailingPivotPoint TrailingPivotPointAnalysis { get; }

        public PatternColumn Result_Column { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            if (bap.StartPt < 0) bap.StartPt = 0;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                TrailingPivotPointDatum gpd = b[TrailingPivotPointAnalysis.Result_Column];

                PatternDatum pd = b[Result_Column] = new PatternDatum();

                var all_points = gpd.PositiveList.Concat(gpd.NegativeList).OrderBy(n => n.Key).ToArray();
                Range<double> weight_range = new Range<double>(0);

                for (int j = 0; j < all_points.Length; j++)
                {
                    var pt1 = all_points[j];
                    double p1 = Math.Abs(pt1.Value.Prominece);
                    double t1 = Math.Abs(pt1.Value.TrendStrength);

                    if (pt1.Value.Prominece > 8)
                    {
                        double w = 2 * ((p1 * p1) + t1);

                        // consider the distant to date as a factor for fading

                        pd.Pivots.Add(new PivotLevel(this, pt1.Value, w));
                    }

                    for (int k = j + 1; k < all_points.Length; k++)
                    {
                        var pt2 = all_points[k];
                        double p2 = Math.Abs(pt2.Value.Prominece);
                        double t2 = Math.Abs(pt2.Value.TrendStrength);


                        // Also get pt3 here...
                        // consider the distant to date as a factor for fading

                        int distance = Math.Abs(pt2.Key - pt1.Key);
                        //double w = 1 * (distance > 100 ? 100 : distance) * (p1 + p2 + t1 + t2);
                        double w = 1 * distance * (p1 + p2 + t1 + t2);
                        pd.Pivots.Add(new PivotLine(this, pt1.Value, pt2.Value, w));
                    }
                }


                // Merge to the Area's PivotWeightList

            }
        }

        #endregion Calculation

        #region Chart Graphics

        public bool ChartEnabled { get; set; } = true;






        public string GraphicsAreaName { get; }



        public void DrawBackground(Graphics g, BarChart bc)
        {
            if (ChartEnabled && GraphicsAreaName is string areaName && bc[areaName] is Area a)
            {
                g.SetClip(a.Bounds);
                g.SmoothingMode = SmoothingMode.HighQuality;
                /*
                Bar b = bc.LastBar;

                if (b[Result_Column] is PatternDatum pd && pd.PivotLines is List<PivotLine> lines)
                    foreach (PivotLine tl in lines)
                    {
                        int x1 = tl.StartIndex;
                        int x1_idx = x1 - bc.StartPt;

                        if (x1_idx >= 0)
                        {
                            //int x1 = tl.StartIndex + tl.Distance;
                            int x2 = bc.StopPt - 1;

                            double y1 = tl.StartLevel;
                            //double y1 = tl.StartLevel + (tl.TrendRate * tl.Distance);
                            double y2 = y1 + (tl.TrendRate * Math.Abs(x2 - x1));

                            Point pt1 = new Point(a.IndexToPixel(x1_idx), a.AxisY(AlignType.Right).ValueToPixel(y1));
                            Point pt2 = new Point(a.IndexToPixel(x2 - bc.StartPt), a.AxisY(AlignType.Right).ValueToPixel(y2));

                            int intensity = (255 * tl.Weight / pd.MaxTrendLineWeight).ToInt32();

                            if (intensity > 255) intensity = 255;
                            else if (intensity < 0) intensity = 0;

                            if (y2 > y1)
                                g.DrawLine(new Pen(Color.FromArgb(intensity, 26, 120, 32)), pt1, pt2);
                            else if (y2 < y1)
                                g.DrawLine(new Pen(Color.FromArgb(intensity, 120, 26, 32)), pt1, pt2);
                            else
                                g.DrawLine(new Pen(Color.FromArgb(intensity, 32, 32, 32)), pt1, pt2);
                        }
                    }
                */

            }
            g.SmoothingMode = SmoothingMode.Default;
            g.ResetClip();
        }

        public void DrawOverlay(Graphics g, BarChart bc)
        {
            if (ChartEnabled && GraphicsAreaName is string areaName && bc[areaName] is Area a)
            {
                g.SetClip(a.Bounds);
                g.SmoothingMode = SmoothingMode.HighQuality;
                /*
                Bar b = bc.LastBar;

                if (b[Result_Column] is PatternDatum pd)
                    foreach (var (weight, level) in pd.Levels)
                    {
                        int intensity = (255 * weight / pd.MaxTrendLineWeight).ToInt32();

                        if (intensity > 255) intensity = 255;
                        else if (intensity < 0) intensity = 0;

                        //int x1 = bc.StopPt - bc.StartPt - 10;
                        int x2 = bc.StopPt - bc.StartPt - 1;

                        Point pt1 = new Point(a.IndexToPixel(x2), a.AxisY(AlignType.Right).ValueToPixel(level));
                        Point pt2 = new Point(a.Right, a.AxisY(AlignType.Right).ValueToPixel(level));

                        g.DrawLine(new Pen(Color.FromArgb(intensity, 32, 32, 32)), pt1, pt2);
                    }*/
            }
            g.SmoothingMode = SmoothingMode.Default;
            g.ResetClip();
        }

        #endregion Chart Graphics
    }
}
