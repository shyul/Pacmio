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

namespace Pacmio
{
    public class TrendAnalysis : BarAnalysis, IPattern
    {
        public TrendAnalysis(BarAnalysis ba, int interval, int minimumPeakProminence, int minimumTrendStrength = 1, double tolerance = 0.03, bool isLogarithmic = false)
        {
            Tolerance = tolerance;
            IsLogarithmic = isLogarithmic;
            GainPointAnalysis = new GainPointAnalysis(ba, interval, minimumPeakProminence, minimumTrendStrength);
        }

        public TrendAnalysis(int interval, int minimumPeakProminence, int minimumTrendStrength = 1, double tolerance = 0.03, bool isLogarithmic = false)
        {
            Tolerance = tolerance;
            IsLogarithmic = isLogarithmic;
            GainPointAnalysis = new GainPointAnalysis(interval, minimumPeakProminence, minimumTrendStrength);
        }

        public bool IsLogarithmic { get; }

        public virtual int Interval => GainPointAnalysis.Interval;

        public virtual int MinimumRank { get; }

        public double Tolerance { get; }

        public GainPointAnalysis GainPointAnalysis { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            if (bap.StartPt < 0) bap.StartPt = 0;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                GainPointDatum gpd = b[GainPointAnalysis.Result_Column];

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
                }
            }
        }
    }
}
