/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// The intermediate calculation for most pattern analysis
/// 
/// ***************************************************************************

using System;
using Xu;

namespace Pacmio
{
    public class GainPointAnalysis : BarAnalysis
    {
        public GainPointAnalysis(BarAnalysis ba, int interval, int minimumPeakProminence, int minimumTrendStrength = 1)
        {
            Interval = interval;
            MinimumPeakProminence = minimumPeakProminence;
            MinimumTrendStrength = minimumTrendStrength;

            string label = "(" + ba.Name + "," + Interval + "," + MinimumPeakProminence + "," + MinimumTrendStrength + ")";
            Name = GetType().Name + label;

            if (ba is ISingleData isd)
            {
                GainAnalysis = new Gain(isd.Result_Column, interval);
                GainAnalysis.AddChild(this);
            }
            else
                throw new ArgumentException("BarAnalysis has to be ISingleData");

            Result_Column = new GainPointColumn(Name) { Label = label };
        }

        public GainPointAnalysis(int interval, int minimumPeakProminence, int minimumTrendStrength = 1)
        {
            Interval = interval;
            MinimumPeakProminence = minimumPeakProminence;
            MinimumTrendStrength = minimumTrendStrength;

            string label = "(" + Bar.Column_Close.Name + "," + Interval + "," + MinimumPeakProminence + "," + MinimumTrendStrength + ")";
            Name = GetType().Name + label;

            GainAnalysis = null;

            Result_Column = new GainPointColumn(Name) { Label = label };
        }

        #region Parameters

        public override int GetHashCode() => GetType().GetHashCode() ^ Name.GetHashCode();

        public virtual int Interval { get; }

        public virtual int MinimumPeakProminence { get; }

        public virtual int MinimumTrendStrength { get; }

        #endregion Parameters

        #region Calculation

        public Gain GainAnalysis { get; }

        public GainPointColumn Result_Column { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            //int min_start = bap.StopPt - Interval - 1;
            //if (bap.StartPt > min_start) bap.StartPt = min_start;
            if (bap.StartPt < 0) bap.StartPt = 0;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                GainPointDatum gpd = new GainPointDatum();
                b[Result_Column] = gpd;

                for (int j = 0; j < Interval; j++)
                {
                    int i_test = i - j;
                    if (i_test < 0) break;

                    Bar b_test = bt[i_test];

                    if (GainAnalysis is null)
                    {
                        double prominence = b_test[Bar.Column_Peak];
                        double trendStrength = b_test[Bar.Column_TrendStrength];

                        if (prominence > MinimumPeakProminence && trendStrength > MinimumTrendStrength)
                            gpd.PositiveList[i_test] = (b_test.Time, b_test[Bar.Column_High], prominence, trendStrength);
                        else if (prominence < -MinimumPeakProminence && trendStrength < -MinimumTrendStrength)
                            gpd.NegativeList[i_test] = (b_test.Time, b_test[Bar.Column_Low], prominence, trendStrength);
                    }
                    else
                    {
                        double value = b_test[GainAnalysis.Column];
                        double prominence = b_test[GainAnalysis.Column_Peak];
                        double trendStrength = b_test[GainAnalysis.Column_TrendStrength];

                        if (prominence > MinimumPeakProminence && trendStrength > MinimumTrendStrength)
                            gpd.PositiveList[i_test] = (b_test.Time, value, prominence, trendStrength);
                        else if (prominence < -MinimumPeakProminence && trendStrength < -MinimumTrendStrength)
                            gpd.NegativeList[i_test] = (b_test.Time, value, prominence, trendStrength);
                    }
                }
            }
            /*
            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                GainPointDatum gpd = b[Result_Column];

                Console.WriteLine("\n\n" + b.Time + "\n");

                foreach (var val in gpd.PositiveList)
                {
                    Console.WriteLine("Positive: " + val.Key + ", " + val.Value.time + ", " + val.Value.value + ", " + val.Value.prominence + ", " + val.Value.trendStrength);
                }

                foreach (var val in gpd.NegativeList)
                {
                    Console.WriteLine("Negative: " + val.Key + ", " + val.Value.time + ", " + val.Value.value + ", " + val.Value.prominence + ", " + val.Value.trendStrength);
                }
            }*/

            #endregion Calculation
        }
    }
}
