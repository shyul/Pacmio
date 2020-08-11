﻿/// ***************************************************************************
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
        }

        public virtual int TestInterval => TrailingPivotPointAnalysis.TestInterval;

        public virtual int MaximumResultCount { get; }

        public double Tolerance => 0.01;

        #region Calculation

        public ATR ATR { get; }

        public TrailingPivotPoint TrailingPivotPointAnalysis { get; }

        public PatternColumn Result_Column { get; }

        public PivotRangeSet PivotRangeSet { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            if (bap.StartPt < 0) bap.StartPt = 0;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                double tolerance = b[ATR.Column_Result];
                TrailingPivotPointDatum gpd = b[TrailingPivotPointAnalysis.Result_Column];

                PatternDatum pd = b[Result_Column] = new PatternDatum();
                PivotRangeSet prs = b[this];

                var all_points = gpd.PositiveList.Concat(gpd.NegativeList).OrderBy(n => n.Key).ToArray();

                for (int j = 0; j < all_points.Length; j++)
                {
                    var pt1 = all_points[j];
                    double p1 = Math.Abs(pt1.Value.Prominece);
                    double t1 = Math.Abs(pt1.Value.TrendStrength);

                    if (pt1.Value.Prominece > 8)
                    {
                        // consider the distant to date as a factor for fading
                        double w = 2 * ((p1 * p1) + t1);
                        PivotLevel level = new PivotLevel(this, pt1.Value)
                        {
                            Weight = w
                        };
                        pd.Pivots.Add(level);
                        prs.Insert(level, tolerance);
                    }

                    for (int k = j + 1; k < all_points.Length; k++)
                    {
                        var pt2 = all_points[k];
                        double p2 = Math.Abs(pt2.Value.Prominece);
                        double t2 = Math.Abs(pt2.Value.TrendStrength);
                        PivotLine line = new PivotLine(this, pt1.Value, pt2.Value, i);
                        // consider the distant to date as a factor for fading
                        line.Weight = 1 * line.DeltaX * (p1 + p2 + t1 + t2);
                        pd.Pivots.Add(line);
                        prs.Insert(line, tolerance);
                    }
                }


                // Merge to the Area's PivotWeightList

            }
        }

        #endregion Calculation

        #region Chart Graphics

        public bool ChartEnabled { get; set; } = true;






        public string AreaName { get; }



        public void DrawBackground(Graphics g, BarChart bc)
        {
            if (ChartEnabled && AreaName is string areaName && bc[areaName] is Area a)
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
