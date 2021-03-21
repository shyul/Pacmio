/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public class TrendLineAnalysis : PatternAnalysis, IChartBackground
    {
        public TrendLineAnalysis(int test_interval)
        {
            string label = "(" + test_interval + ")";
            Name = GetType().Name + label;

            ATR = new ATR(14) { ChartEnabled = false };
            ATR.AddChild(this);

            TrailingPivotPointAnalysis = new TrailingPivotPtAnalysis(test_interval);
            TrailingPivotPointAnalysis.AddChild(this);

            Column_Result = new PatternColumn(this, test_interval);
            AreaName = MainBarChartArea.DefaultName;

            this.AddChild(RangeBoundAnalysis);
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Name.GetHashCode();

        #region Calculation

        public ATR ATR { get; }

        public RangeBoundAnalysis RangeBoundAnalysis { get; } = new(); // => BarTable.CalculatePivotRange;

        public TrailingPivotPtAnalysis TrailingPivotPointAnalysis { get; }

        public override int MaximumInterval => TrailingPivotPointAnalysis.MaximumInterval;

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (bt[i] is Bar b && b[Column_Result] is null)
                {
                    double range_delta = (b.PivotRange.Max - b.PivotRange.Min) / 2;
                    double tolerance = b[ATR.Column_Result] / 4;
                    double center = b.Close;

                    PatternDatum pd = new PatternDatum(center - range_delta, center + range_delta);

                    var all_points = b.PivotPts;

                    for (int j = 0; j < all_points.Length; j++)
                    {
                        var pt1 = all_points[j];
                        var pt1_strength = pt1.Value.Strength;

                        if (pt1_strength > 8 && pd.LevelRange.Contains(pt1.Value.Level))
                        {
                            // consider the distant to date as a factor for fading
                            HorizontalLine level = new(this, pt1.Value, tolerance)
                            {
                                Weight = pt1_strength * 2
                            };
                            pd.Add(level);
                        }

                        for (int k = j + 1; k < all_points.Length; k++)
                        {
                            var pt2 = all_points[k];
                            var pt2_strength = pt2.Value.Strength;
                            TrendLine line = new(this, pt1.Value, pt2.Value, i + 1, tolerance);

                            if (pd.LevelRange.Contains(line.Level))
                            {
                                // consider the distant to date as a factor for fading
                                line.Weight = 1 * line.DeltaX * (pt1_strength + pt2_strength);
                                pd.Add(line);
                            }
                        }
                    }

                    b[Column_Result] = pd;

                }
            }
        }

        #endregion Calculation

        #region Chart Graphics

        public void DrawBackground(Graphics g, BarChart bc)
        {
            if (ChartEnabled && bc.LastBar_1 is Bar b && AreaName is string areaName && bc[areaName] is Area a && b[Column_Result] is PatternDatum pd)
            {
                int StartPt = a.StartPt;
                int StopPt = a.StopPt;

                g.SetClip(a.Bounds);
                g.SmoothingMode = SmoothingMode.HighQuality;

                double maxWeight = pd.WeightRange.Max;
                foreach (IPatternObject ip in pd)
                {
                    if (ip is TrendLine line)
                    {
                        int x1 = line.X1 - StartPt;
                        if (x1 >= 0)
                        {
                            int x3 = StopPt - StartPt - 1;
                            double y1 = line.Y1;
                            double y3 = y1 + (line.TrendRate * (x3 - x1));

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
                    else if (ip is HorizontalLine level)
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

        #endregion Chart Graphics
    }
}