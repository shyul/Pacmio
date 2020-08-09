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
    public class TrailingPivotPoint : BarAnalysis
    {
        public TrailingPivotPoint(BarAnalysis ba, int test_interval = 250, int maximumPeakProminence = 100, int minimumPeakProminence = 5)
        {
            TestInterval = test_interval;

            if (ba is ISingleData isd)
            {
                PivotPointAnalysis = new PivotPoint(isd, maximumPeakProminence, minimumPeakProminence);
                PivotPointAnalysis.AddChild(this);
            }
            else if (ba is IDualData idd)
            {
                PivotPointAnalysis = new PivotPoint(idd, maximumPeakProminence, minimumPeakProminence);
                PivotPointAnalysis.AddChild(this);
            }
            else
                throw new ArgumentException("BarAnalysis has to be ISingleData or IDualData");

            string label = "(" + ba.Name + "," + TestInterval + "," + MinimumPeakProminence + ")";
            Name = GetType().Name + label;

            Result_Column = new TrailingPivotPointColumn(Name) { Label = label };
        }

        public TrailingPivotPoint(int test_interval = 250)
        {
            PivotPointAnalysis = BarTable.PivotPointAnalysis;
            TestInterval = test_interval;
            TrendStrengthAnalysis = new TrendStrength();

            string label = "(" + Bar.Column_Close.Name + "," + TestInterval + "," + MinimumPeakProminence + ")";
            Name = GetType().Name + label;

            Result_Column = new TrailingPivotPointColumn(Name) { Label = label };
        }

        #region Parameters

        public override int GetHashCode() => GetType().GetHashCode() ^ Name.GetHashCode();

        public virtual int TestInterval { get; }

        public virtual int MinimumPeakProminence => 5;// PivotPointAnalysis.MinimumPeakProminenceForAnalysis;

        #endregion Parameters

        #region Calculation

        public PivotPoint PivotPointAnalysis { get; }

        public TrendStrength TrendStrengthAnalysis { get; }

        public TrailingPivotPointColumn Result_Column { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            int min_peak_start = bap.StopPt - PivotPointAnalysis.MaximumPeakProminence * 2 - 1;
            if (bap.StartPt > min_peak_start)
                bap.StartPt = min_peak_start;
            else if (bap.StartPt < 0)
                bap.StartPt = 0;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                TrailingPivotPointDatum gpd = new TrailingPivotPointDatum();
                bt[i][Result_Column] = gpd;

                for (int j = MinimumPeakProminence; j < TestInterval; j++)
                {
                    int i_test = i - j;
                    if (i_test < 0) break;

                    Bar b = bt[i_test];

                    double prominence = b[PivotPointAnalysis.Column_Result];
                    double trendStrength = b[TrendStrengthAnalysis.Column_TrendStrength];

                    // For simulation accuracy, the prominence can't be greater than the back testing offset.
                    if (prominence > j) prominence = j;

                    if (prominence > MinimumPeakProminence)// || trendStrength > MinimumTrendStrength)
                    {
                        double high = b[PivotPointAnalysis.Column_High];
                        gpd.PositiveList[i_test] = (b.Time, high, prominence, trendStrength);
                        gpd.LevelRange.Insert(high);
                    }
                    else if (prominence < -MinimumPeakProminence)// || trendStrength < -MinimumTrendStrength)
                    {
                        double low = b[PivotPointAnalysis.Column_Low];
                        gpd.NegativeList[i_test] = (b.Time, low, prominence, trendStrength);
                        gpd.LevelRange.Insert(low);
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
