/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public class TrendAnalysis : BarAnalysis, IPattern, IChartOverlay
    {
        public TrendAnalysis(BarAnalysis ba, int interval, int minimumPeakProminence, int minimumTrendStrength = 1, double tolerance = 0.03, bool isLogarithmic = false)
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

        public TrendAnalysis(int interval, int minimumPeakProminence, int minimumTrendStrength = 1, double tolerance = 0.03, bool isLogarithmic = false)
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

                PatternDatum pd = new PatternDatum(bt.BarFreq, AreaName);
                b[Result_Column] = pd;

                var all_points = gpd.PositiveList.Concat(gpd.NegativeList);
                for (int j = 0; j < all_points.Count(); j++)
                {
                    var pt1 = all_points.ElementAt(j);
                    for (int k = j + 1; k < all_points.Count(); k++)
                    {
                        var pt2 = all_points.ElementAt(k);

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
                /*
                foreach(var (x1, y1, x2, y2, rate) in lines) 
                {
                    Console.WriteLine(rate);
                }*/
            }
        }

        public void Draw(Graphics g, BarChart bc, BarTable bt)
        {
            if (AreaName is string areaName && bc[areaName] is Area a)
            {
                Bar b = bc.LastBar;

                PatternDatum pd = b[Result_Column];

                foreach (TrendLine tl in pd.TrendLines)
                {
                    int x1 = tl.StartIndex;
                    int x2 = x1 + tl.Distance;

                    double y1 = tl.StartLevel;
                    double y2 = y1 + (tl.TrendRate * tl.Distance);

                    Point pt1 = new Point(a.IndexToPixel(x1 - bc.StartPt), a.AxisY(AlignType.Right).ValueToPixel(y1));
                    Point pt2 = new Point(a.IndexToPixel(x2 - bc.StartPt), a.AxisY(AlignType.Right).ValueToPixel(y2));
                    g.DrawLine(new Pen(Color.Black), pt1, pt2);
                }

                //g.DrawLine(new Pen(Color.Black), new Point(a.Left, a.Top), new Point(a.Right, a.Bottom));
                //Console.WriteLine(b.Index);

            }
        }
    }
}
