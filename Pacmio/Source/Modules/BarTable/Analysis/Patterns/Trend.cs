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

namespace Pacmio.Analysis
{
    public class Trend : BarAnalysis, IChartPattern
    {
        public Trend(BarAnalysis ba, NumericColumn column_MaximumRange, int test_interval = 250, int maximumPeakProminence = 100, int minimumPeakProminence = 5)
        {
            string label = "(" + ba.Name + "," + test_interval + "," + maximumPeakProminence + "," + minimumPeakProminence + ")";
            Name = GetType().Name + label;

            //Column_MaximumRange = column_MaximumRange;

            TrailingPivotPointAnalysis = new TrailingPivots(ba, test_interval, maximumPeakProminence, minimumPeakProminence);
            TrailingPivotPointAnalysis.AddChild(this);

            Column_Result = new PatternColumn(this);
            AreaName = (ba is IChartSeries ics) ? ics.AreaName : null;

            this.AddChild(CalculatePivotRange);
        }

        public Trend(int test_interval)
        {
            string label = "(" + test_interval + ")";
            Name = GetType().Name + label;

            ATR = new ATR(14) { ChartEnabled = false };
            ATR.AddChild(this);

            TrailingPivotPointAnalysis = new TrailingPivots(test_interval);
            TrailingPivotPointAnalysis.AddChild(this);

            Column_Result = new PatternColumn(this);
            AreaName = MainBarChartArea.DefaultName;

            this.AddChild(CalculatePivotRange);
        }


        #region Calculation

        public ATR ATR { get; }

        public PivotRanges CalculatePivotRange { get; } = new PivotRanges(); // => BarTable.CalculatePivotRange;

        public TrailingPivots TrailingPivotPointAnalysis { get; }

        public PatternColumn Column_Result { get; }

        public override void Update(BarAnalysisPointer bap) // Cancellation Token should be used
        {
            if (!bap.IsUpToDate && bap.Count > 0)
            {
                bap.StopPt = bap.Count - 1;

                if (bap.StartPt < 0)
                    bap.StartPt = 0;

                Calculate(bap);
                bap.StartPt = bap.StopPt;
                bap.StopPt++;
            }
        }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (bt[i] is Bar b && b[Column_Result] is null && b[TrailingPivotPointAnalysis.Result_Column] is TrailingPivotsDatum gpd)
                {
                    double range_delta = (gpd.LevelRange.Max - gpd.LevelRange.Min) / 2;
                    //double tolerance = range_delta / 25;

                    double tolerance = b[ATR.Column_Result] / 4;
                    double center = b.Close;

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

                            PivotLine line = new PivotLine(this, pt1.Value, pt2.Value, i + 1, tolerance);

                            if (pd.LevelRange.Contains(line.Level))
                            {
                                // consider the distant to date as a factor for fading
                                line.Weight = 1 * line.DeltaX * (p1 + p2 + t1 + t2);
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

        public bool ChartEnabled { get; set; } = true;

        public string AreaName { get; }

        public void DrawBackground(Graphics g, BarChart bc)
        {
            if (ChartEnabled && bc.LastBar_1 is Bar b && AreaName is string areaName && bc[areaName] is Area a && b[Column_Result] is PatternDatum pd)
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
            if (ChartEnabled && AreaName is string areaName && bc[areaName] is Area a && bc.LastBar is Bar b && b[Column_Result] is PatternDatum pd)
            {
                g.SetClip(a.Bounds);
                g.SmoothingMode = SmoothingMode.HighQuality;




            }
            g.SmoothingMode = SmoothingMode.Default;
            g.ResetClip();
        }

        #endregion Chart Graphics
    }
}