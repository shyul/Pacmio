/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using Xu;

namespace Pacmio
{
    public class TrendAnalysis : BarAnalysis, IPattern
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

        public virtual int MinimumRank { get; }

        public double Tolerance { get; }

        public GainPointAnalysis GainPointAnalysis { get; }

        public PatternColumn Result_Column { get; }

        public string AreaName { get; }

       

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

                /*
                var pos_points = gpd.PositiveList.Select(n => (n.Key, n.Value.value));
                var neg_points = gpd.NegativeList.Select(n => (n.Key, n.Value.value));

                var all_points = pos_points.Concat(neg_points);

                List<(int x1, double y1, int x2, double y2, double rate)> lines = new List<(int x1, double y1, int x2, double y2, double rate)>();
                for (int j = 0; j < all_points.Count(); j++)
                {
                    var (x1, y1) = all_points.ElementAt(j);
                    for (int k = j + 1; k < all_points.Count(); k++)
                    {
                        var (x2, y2) = all_points.ElementAt(k);
                        double rate = (y2 - y1) / (x2 - x1);
                        lines.Add((x1, y1, x2, y2, rate));
                    }
                }*/

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


                        TrendLine tl = new TrendLine(pt1.Value.time, pt1.Value.level, rate, Tolerance, distance, IsLogarithmic);
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
    }
}
