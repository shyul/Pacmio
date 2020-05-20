/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public sealed class SupportResistance : BarAnalysis, IPatternAnalysis
    {
        public double Weight => throw new NotImplementedException();

        /// <summary>
        /// In Percent here of the frame range!!
        /// </summary>
        public double Tolerance { get; set; } = 0.01;

        public int FrameSize { get; set; } = 300;
        
        public int RankLimit { get; set; } = 5;

        public ObjectColumn Pattern_Column { get; }

        /// <summary>
        /// For patterns, do not always calculate the last pt...
        /// Need to figure that out..
        /// </summary>
        /// <param name="bap"></param>
        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            if (bap.StartPt < FrameSize)
            {
                return;
            }

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                SupportResistancePatternDatum dsd = new SupportResistancePatternDatum();
                b[Pattern_Column] = dsd;

                var barList = bt[i - FrameSize, FrameSize];

                double max = barList.Select(n => n.High).Max();
                double min = barList.Select(n => n.Low).Min();

                double delta = max - min;

                if (delta > 0)
                {
                    dsd.Rank.Clear();
                    double slotSize = delta * Tolerance;
                    double baseValue = min;
                    while (baseValue < max)
                    {
                        dsd.Rank.Add(new Range<double>(baseValue, baseValue + delta), 0);
                        baseValue += delta;
                    }

                    foreach (Bar b0 in barList)
                    {
                        double high = b0.High;
                        double low = b0.Low;
                        double open = b0.Open;
                        double close = b0.Close;

                        int volume = b0.Volume.ToInt32();
                        int peak = b0.Peak.ToInt32();

                        dsd.Rank.Where(n => n.Key.Contains(high)).Select(n => n.Key).ToList().ForEach(n => {
                            dsd.Rank[n] += (peak > 0) ? peak * volume : volume;
                        });

                        dsd.Rank.Where(n => n.Key.Contains(low)).Select(n => n.Key).ToList().ForEach(n => {
                            dsd.Rank[n] += (peak < 0) ? -peak * volume : volume;
                        });

                        dsd.Rank.Where(n => n.Key.Contains(open)).Select(n => n.Key).ToList().ForEach(n => {
                            dsd.Rank[n] += volume;
                        });

                        dsd.Rank.Where(n => n.Key.Contains(close)).Select(n => n.Key).ToList().ForEach(n => {
                            dsd.Rank[n] += volume;
                        });
                    }

                    var topRank = dsd.Rank.OrderByDescending(n => n.Value);


                }

            }
        }
    }
}
