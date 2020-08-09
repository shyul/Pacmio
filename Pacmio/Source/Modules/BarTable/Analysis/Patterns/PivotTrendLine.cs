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
    public class PivotTrendLine : BarAnalysis
    {
        public PivotTrendLine(BarAnalysis ba, int interval, int minimumPeakProminence, int minimumTrendStrength = 0)
        {
            Interval = interval;
            MinimumPeakProminence = minimumPeakProminence;
            MinimumTrendStrength = minimumTrendStrength;
            SkipLastCount = 5;

            string label = "(" + ba.Name + "," + Interval + "," + MinimumPeakProminence + "," + MinimumTrendStrength + ")";
            Name = GetType().Name + label;

            if (ba is ISingleData isd)
            {
                PeakAnalysis = new PivotPoint(isd, interval);
                PeakAnalysis.AddChild(this);
            }
            else if (ba is IDualData idd)
            {
                PeakAnalysis = new PivotPoint(idd, interval);
                PeakAnalysis.AddChild(this);
            }
            else
                throw new ArgumentException("BarAnalysis has to be ISingleData");

            Result_Column = new GainPointColumn(Name) { Label = label };
        }

        public PivotTrendLine(int interval, int minimumPeakProminence, int minimumTrendStrength = 0)
        {
            Interval = interval;
            MinimumPeakProminence = minimumPeakProminence;
            MinimumTrendStrength = minimumTrendStrength;
            SkipLastCount = 5;

            string label = "(" + Bar.Column_Close.Name + "," + Interval + "," + MinimumPeakProminence + "," + MinimumTrendStrength + ")";
            Name = GetType().Name + label;

            PeakAnalysis = null;

            Result_Column = new GainPointColumn(Name) { Label = label };
        }

        #region Parameters

        public override int GetHashCode() => GetType().GetHashCode() ^ Name.GetHashCode();

        public virtual int Interval { get; }

        public virtual int SkipLastCount { get; }

        public virtual int MinimumPeakProminence { get; }

        public virtual int MinimumTrendStrength { get; }

        #endregion Parameters

        #region Calculation

        public PivotPoint PeakAnalysis { get; }

        public GainPointColumn Result_Column { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            if (bap.StartPt < 0) bap.StartPt = 0;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                GainPointDatum gpd = new GainPointDatum();
                b[Result_Column] = gpd;

                for (int j = SkipLastCount; j < Interval; j++)
                {
                    int i_test = i - j;
                    if (i_test < 0) break;

                    Bar b_test = bt[i_test];

                    if (PeakAnalysis is null)
                    {
                        double prominence = b_test[BarTable.PivotPointAnalysis.Column_Result];
                        double trendStrength = b_test[BarTable.TrendStrengthAnalysis.Column_TrendStrength];

                        // For simulation accuracy, the prominence can't be greater than the back testing offset.
                        if (prominence > j) prominence = j;

                        if (prominence > MinimumPeakProminence)// && trendStrength > MinimumTrendStrength)
                            gpd.PositiveList[i_test] = (b_test.Time, b_test[Bar.Column_High], prominence, trendStrength);
                        else if (prominence < -MinimumPeakProminence)// && trendStrength < -MinimumTrendStrength)
                            gpd.NegativeList[i_test] = (b_test.Time, b_test[Bar.Column_Low], prominence, trendStrength);
                    }
                    else
                    {

                        double prominence = b_test[PeakAnalysis.Column_Result];
                        double value = prominence > 0 ? b_test[PeakAnalysis.Column_High] : b_test[PeakAnalysis.Column_Low];

                        // For simulation accuracy, the prominence can't be greater than the back testing offset.
                        if (prominence > j) prominence = j;

                        double trendStrength = 0;// b_test[GainAnalysis.Column_TrendStrength];

                        if (prominence > MinimumPeakProminence)// && trendStrength > MinimumTrendStrength)
                            gpd.PositiveList[i_test] = (b_test.Time, value, prominence, trendStrength);
                        else if (prominence < -MinimumPeakProminence)// && trendStrength < -MinimumTrendStrength)
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
