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

            Result_Column = new PatternColumn(Name) { Label = label };
            GraphicsAreaName = (ba is IChartSeries ics) ? ics.AreaName : null;
        }

        public TrendAnalysis(int test_interval)
        {

            string label = "(" + test_interval + ")";
            Name = GetType().Name + label;

            TrailingPivotPointAnalysis = new TrailingPivotPoint(test_interval);
            TrailingPivotPointAnalysis.AddChild(this);

            Result_Column = new PatternColumn(Name) { Label = label };
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

                PatternDatum pd = b[Result_Column] = new PatternDatum(bt.BarFreq, GraphicsAreaName);

                /*
                for (int j = 0; j < gpd.PositiveList.Count(); j++)
                {
                    var pt1 = gpd.PositiveList.ElementAt(j);
                    for (int k = j + 1; k < gpd.PositiveList.Count(); k++)
                    {
                        var pt2 = gpd.PositiveList.ElementAt(k);

                        int x1 = pt1.Key;
                        int x2 = pt2.Key;

                        double y1 = pt1.Value.level;
                        double y2 = pt2.Value.level;

                        int distance = x2 - x1;
                        double rate = (y2 - y1) / distance;

                        TrendLine tl = new TrendLine(x1, y1, distance, rate, Tolerance, distance, IsLogarithmic);
                        pd.TrendLines.Add(tl);
                    }
                }

                for (int j = 0; j < gpd.NegativeList.Count(); j++)
                {
                    var pt1 = gpd.NegativeList.ElementAt(j);
                    for (int k = j + 1; k < gpd.NegativeList.Count(); k++)
                    {
                        var pt2 = gpd.NegativeList.ElementAt(k);

                        int x1 = pt1.Key;
                        int x2 = pt2.Key;

                        double y1 = pt1.Value.level;
                        double y2 = pt2.Value.level;

                        int distance = x2 - x1;
                        double rate = (y2 - y1) / distance;

                        TrendLine tl = new TrendLine(x1, y1, distance, rate, Tolerance, distance, IsLogarithmic);
                        pd.TrendLines.Add(tl);
                    }
                }*/

                var all_points = gpd.PositiveList.Concat(gpd.NegativeList).OrderBy(n => n.Key).ToArray();
                Range<double> weight_range = new Range<double>(0);

                for (int j = 0; j < all_points.Length; j++)
                {
                    var pt1 = all_points[j];
                    int x1 = pt1.Key;
                    double y1 = pt1.Value.level;
                    double p1 = Math.Abs(pt1.Value.prominence);

                    if (p1 > 8)
                    {
                        //Console.WriteLine("Adding level line: " + p1);
                        TrendLine tl = new TrendLine(x1, y1, j - x1, 0d, Tolerance, p1 * p1 * 100);
                        pd.TrendLines.Add(tl);
                        pd.Levels.Add((p1 * p1 * 100, y1));
                    }

                    for (int k = j + 1; k < all_points.Length; k++)
                    {
                        var pt2 = all_points[k];
                        int x2 = pt2.Key;
                        double y2 = pt2.Value.level;

                        double prominence = Math.Abs(pt1.Value.prominence) + Math.Abs(pt2.Value.prominence);

                        int distance = x2 - x1;
                        double rate = (y2 - y1) / distance;
                        double weight = Math.Abs(distance * prominence);

                        weight_range.Insert(weight);

                        TrendLine tl = new TrendLine(x1, y1, distance, rate, Tolerance, weight);
                        pd.TrendLines.Add(tl);

                        int x3 = i;
                        double y3 = y1 + (rate * Math.Abs(x3 - x1));
                        pd.Levels.Add((weight, y3));
                    }
                }

                /*
                foreach (TrendLine tl in pd.TrendLines)
                {
                    weight_range.Insert(tl.Weight);
                }
                */

                pd.MaxTrendLineWeight = weight_range.Max;

                /*
                foreach(var (x1, y1, x2, y2, rate) in lines) 
                {
                    Console.WriteLine(rate);
                }*/
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

                Bar b = bc.LastBar;

                if (b[Result_Column] is PatternDatum pd && pd.TrendLines is List<TrendLine> lines)
                    foreach (TrendLine tl in lines)
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

                //g.DrawLine(new Pen(Color.Black), new Point(a.Left, a.Top), new Point(a.Right, a.Bottom));
                //Console.WriteLine(b.Index);

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
                    }
            }
            g.SmoothingMode = SmoothingMode.Default;
            g.ResetClip();
        }

        #endregion Chart Graphics
    }
}
