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
    public class TrendAnalysis : BarAnalysis, IPattern, IChartCustomGraphics
    {
        public TrendAnalysis(BarAnalysis ba, int interval, int minimumPeakProminence, int minimumTrendStrength = 0, double tolerance = 0.03, bool isLogarithmic = false)
        {
            Tolerance = tolerance;
            IsLogarithmic = isLogarithmic;
            string label = "(" + ba.Name + "," + interval + "," + minimumPeakProminence + "," + minimumTrendStrength + ")";
            Name = GetType().Name + label;

            GainPointAnalysis = new GainPointAnalysis(ba, interval, minimumPeakProminence, minimumTrendStrength);
            GainPointAnalysis.AddChild(this);

            Result_Column = new PatternColumn(Name) { Label = label };

            AreaName = (ba is IChartSeries ics) ? ics.AreaName : null;
        }

        public TrendAnalysis(int interval, int minimumPeakProminence, int minimumTrendStrength = 0, double tolerance = 0.03, bool isLogarithmic = false)
        {
            Tolerance = tolerance;
            IsLogarithmic = isLogarithmic;
            string label = "(" + Bar.Column_Close.Name + "," + interval + "," + minimumPeakProminence + "," + minimumTrendStrength + ")";
            Name = GetType().Name + label;

            GainPointAnalysis = new GainPointAnalysis(interval, minimumPeakProminence, minimumTrendStrength);
            GainPointAnalysis.AddChild(this);

            Result_Column = new PatternColumn(Name) { Label = label };
            AreaName = MainArea.DefaultName;
        }

        public bool IsLogarithmic { get; }

        public virtual int Interval => GainPointAnalysis.Interval;

        public virtual int MaximumResultCount { get; }

        public double Tolerance { get; }

        public GainPointAnalysis GainPointAnalysis { get; }

        public PatternColumn Result_Column { get; }

        public string AreaName { get; }

        public Color Color { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            if (bap.StartPt < 0) bap.StartPt = 0;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                GainPointDatum gpd = b[GainPointAnalysis.Result_Column];

                PatternDatum pd = b[Result_Column] = new PatternDatum(bt.BarFreq, AreaName);

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
                for (int j = 0; j < all_points.Length; j++)
                {
                    var pt1 = all_points[j];
                    for (int k = j + 1; k < all_points.Length; k++)
                    {
                        var pt2 = all_points[k];

                        int x1 = pt1.Key;
                        int x2 = pt2.Key;

                        double y1 = pt1.Value.level;
                        double y2 = pt2.Value.level;

                        double prominence = pt1.Value.prominence + pt2.Value.prominence;

                        int distance = x2 - x1;
                        double rate = (y2 - y1) / distance;

                        TrendLine tl = new TrendLine(x1, y1, distance, rate, Tolerance, Math.Abs(distance * prominence), IsLogarithmic);
                        pd.TrendLines.Add(tl);
                    }
                }

                Range<double> range = new Range<double>(0);
                foreach (TrendLine tl in pd.TrendLines)
                {
                    range.Insert(tl.Weight);
                }

                pd.MaxTrendLineWeight = range.Max;

                /*
                foreach(var (x1, y1, x2, y2, rate) in lines) 
                {
                    Console.WriteLine(rate);
                }*/
            }
        }

        public void DrawBackground(Graphics g, BarChart bc, BarTable bt)
        {
            if (AreaName is string areaName && bc[areaName] is Area a)
            {
                g.SetClip(a.Bounds);
                g.SmoothingMode = SmoothingMode.HighQuality;

                Bar b = bc.LastBar;

                if (b[Result_Column] is PatternDatum pd && pd.TrendLines is List<TrendLine> lines)
                    foreach (TrendLine tl in lines)
                    {
                        int x1 = tl.StartIndex;
                        //int x1 = tl.StartIndex + tl.Distance;
                        int x2 = bc.StopPt;

                        double y1 = tl.StartLevel;
                        //double y1 = tl.StartLevel + (tl.TrendRate * tl.Distance);
                        double y2 = y1 + (tl.TrendRate * Math.Abs(x2 - x1));

                        Point pt1 = new Point(a.IndexToPixel(x1 - bc.StartPt), a.AxisY(AlignType.Right).ValueToPixel(y1));
                        Point pt2 = new Point(a.IndexToPixel(x2 - bc.StartPt), a.AxisY(AlignType.Right).ValueToPixel(y2));

                        int intensity = (255 * tl.Weight / pd.MaxTrendLineWeight).ToInt32();

                        if (intensity > 255) intensity = 255;
                        else if (intensity < 0) intensity = 0;

                        g.DrawLine(new Pen(Color.FromArgb(intensity, 32, 32, 32)), pt1, pt2);
                    }

                //g.DrawLine(new Pen(Color.Black), new Point(a.Left, a.Top), new Point(a.Right, a.Bottom));
                //Console.WriteLine(b.Index);

            }
            g.SmoothingMode = SmoothingMode.Default;
            g.ResetClip();
        }

        public void DrawOverlay(Graphics g, BarChart bc, BarTable bt)
        {
            if (AreaName is string areaName && bc[areaName] is Area a)
            {
                g.SetClip(a.Bounds);
                g.SmoothingMode = SmoothingMode.HighQuality;


            }
            g.SmoothingMode = SmoothingMode.Default;
            g.ResetClip();
        }
    }
}
