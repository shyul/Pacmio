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
    public class TrendAnalysis : BarAnalysis, IChartPattern
    {
        public TrendAnalysis(BarAnalysis ba, NumericColumn column_MaximumRange, int test_interval = 250, int maximumPeakProminence = 100, int minimumPeakProminence = 5)
        {
            string label = "(" + ba.Name + "," + test_interval + "," + maximumPeakProminence + "," + minimumPeakProminence + ")";
            Name = GetType().Name + label;

            //Column_MaximumRange = column_MaximumRange;

            TrailingPivotPointAnalysis = new TrailingPivotPoint(ba, test_interval, maximumPeakProminence, minimumPeakProminence);
            TrailingPivotPointAnalysis.AddChild(this);

            Result_Column = new PatternColumn(this);
            AreaName = (ba is IChartSeries ics) ? ics.AreaName : null;

            //PivotRange_Column = new PivotRangeColumn(AreaName);
        }

        public TrendAnalysis(int test_interval)
        {
            string label = "(" + test_interval + ")";
            Name = GetType().Name + label;

            //Column_MaximumRange = BarTable.TrueRangeAnalysis.Column_TrueRange;
            ATR = new ATR(14);
            ATR.AddChild(this);

            TrailingPivotPointAnalysis = new TrailingPivotPoint(test_interval);
            TrailingPivotPointAnalysis.AddChild(this);

            Result_Column = new PatternColumn(this);
            AreaName = MainArea.DefaultName;

            //PivotRange_Column = new PivotRangeColumn(AreaName);
        }

        public virtual int TestInterval => TrailingPivotPointAnalysis.TestInterval;

        public virtual int MaximumResultCount { get; }

        public double Tolerance => 0.01;

        #region Calculation

        public ATR ATR { get; }

        public TrailingPivotPoint TrailingPivotPointAnalysis { get; }

        public PatternColumn Result_Column { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            /*
            int min_peak_start = bap.StopPt - TrailingPivotPointAnalysis.PivotPointAnalysis.MaximumPeakProminence * 2 - 1;
            if (bap.StartPt > min_peak_start)
                bap.StartPt = min_peak_start;
            else 
            */
            if (bap.StartPt < 0)
                bap.StartPt = 0;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                if (b[Result_Column] is null)
                {
                    double tolerance = b[ATR.Column_Result] / 4;
                    TrailingPivotPointDatum gpd = b[TrailingPivotPointAnalysis.Result_Column];

                    double center = b.Close;
                    double range_delta = (gpd.LevelRange.Max - gpd.LevelRange.Min) / 2;

                    PatternDatum pd = new PatternDatum(center - range_delta, center + range_delta);

                    var all_points = gpd.PositiveList.Concat(gpd.NegativeList).OrderBy(n => n.Key).ToArray();

                    for (int j = 0; j < all_points.Length; j++)
                    {
                        var pt1 = all_points[j];
                        double p1 = Math.Abs(pt1.Value.Prominece);
                        double t1 = Math.Abs(pt1.Value.TrendStrength);

                        if (pt1.Value.Prominece > 8 && pd.LevelRange.Contains(pt1.Value.Level))
                        {
                            // consider the distant to date as a factor for fading
                            double w = 2 * ((p1 * p1) + t1);
                            PivotLevel level = new PivotLevel(this, pt1.Value, tolerance)
                            {
                                Weight = w
                            };
                            pd.Add(level);
                        }

                        for (int k = j + 1; k < all_points.Length; k++)
                        {
                            var pt2 = all_points[k];
                            double p2 = Math.Abs(pt2.Value.Prominece);
                            double t2 = Math.Abs(pt2.Value.TrendStrength);

                            PivotLine line = new PivotLine(this, pt1.Value, pt2.Value, i, tolerance);

                            if (pd.LevelRange.Contains(line.Level))
                            {
                                // consider the distant to date as a factor for fading
                                line.Weight = 1 * line.DeltaX * (p1 + p2 + t1 + t2);
                                pd.Add(line);
                            }
                        }
                    }

                    b[Result_Column] = pd;
                }
            }
        }

        #endregion Calculation

        #region Chart Graphics

        public bool ChartEnabled { get; set; } = true;

        public string AreaName { get; }

        public void DrawBackground(Graphics g, BarChart bc)
        {
            if (ChartEnabled && AreaName is string areaName && bc[areaName] is Area a && bc.LastBar is Bar b && b[Result_Column] is PatternDatum pd)
            {
                int StartPt = a.StartPt;
                int StopPt = a.StopPt;

                g.SetClip(a.Bounds);
                g.SmoothingMode = SmoothingMode.HighQuality;

                double maxWeight = pd.WeightRange.Max;
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

                            Point pt1 = new Point(a.IndexToPixel(x1), a.AxisY(AlignType.Right).ValueToPixel(y1));
                            Point pt3 = new Point(a.IndexToPixel(x3), a.AxisY(AlignType.Right).ValueToPixel(y3));

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
                            int py1 = a.AxisY(AlignType.Right).ValueToPixel(y1);

                            Point pt1 = new Point(a.IndexToPixel(x1), py1);
                            Point pt3 = new Point(a.IndexToPixel(x3), py1);

                            int intensity = (255 * ip.Weight / maxWeight).ToInt32();

                            if (intensity > 255) intensity = 255;
                            else if (intensity < 1) intensity = 1;

                            g.DrawLine(new Pen(Color.FromArgb(intensity, 80, 80, 32)), pt1, pt3);
                        }
                    }
                }
            }
            g.SmoothingMode = SmoothingMode.Default;
            g.ResetClip();
        }

        public void DrawOverlay(Graphics g, BarChart bc)
        {
            if (ChartEnabled && AreaName is string areaName && bc[areaName] is Area a)
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
