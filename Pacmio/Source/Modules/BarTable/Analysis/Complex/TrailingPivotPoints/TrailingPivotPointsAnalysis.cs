/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The intermediate calculation for most pattern analysis
/// 
/// ***************************************************************************

using System;
using Xu;

namespace Pacmio.Analysis
{
    public class TrailingPivotPointsAnalysis : BarAnalysis
    {
        /*
        public TrailingPivotPointsAnalysis(BarAnalysis ba, int test_interval = 250, int maximumPeakProminence = 100, int minimumPeakProminence = 5)
        {
            TestLength = test_interval;

            if (ba is ISingleData isd)
            {
                PivotPointAnalysis = new(isd, maximumPeakProminence, minimumPeakProminence);
                PivotPointAnalysis.AddChild(this);
            }
            else if (ba is IDualData idd)
            {
                PivotPointAnalysis = new(idd, maximumPeakProminence, minimumPeakProminence);
                PivotPointAnalysis.AddChild(this);
            }
            else
                throw new("BarAnalysis has to be ISingleData or IDualData");

            string label = "(" + ba.Name + "," + TestLength + "," + MinimumPeakProminenceForAnalysis + ")";
            Name = GetType().Name + label;

            Result_Column = new(Name, label, typeof(TrailingPivotPointsDatum));
        }*/

        public TrailingPivotPointsAnalysis(int test_length = 250)
        {
            PivotPointAnalysis = BarTable.PivotPointAnalysis;
            TestLength = test_length;
            TrendStrengthAnalysis = new();

            string label = "(" + Bar.Column_Close.Name + "," + TestLength + "," + MinimumPeakProminenceForAnalysis + ")";
            Name = GetType().Name + label;

            Result_Column = new(Name, label, typeof(TrailingPivotPointsDatum));
        }

        #region Parameters

        public override int GetHashCode() => GetType().GetHashCode() ^ Name.GetHashCode();

        public virtual int TestLength { get; }

        public virtual int MinimumPeakProminenceForAnalysis => 5;

        #endregion Parameters

        #region Calculation

        public PivotPointAnalysis PivotPointAnalysis { get; }

        public TrendStrength TrendStrengthAnalysis { get; }

        public DatumColumn Result_Column { get; }

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
                if (bt[i] is Bar b0)
                {
                    TrailingPivotPointsDatum gpd = new();

                    for (int j = MinimumPeakProminenceForAnalysis; j < TestLength; j++)
                    {
                        int i_test = i - j;
                        if (i_test < 0) break;

                        if (bt[i_test] is Bar b)
                        {
                            double prominence = b[PivotPointAnalysis.Column_Result];
                            double trendStrength = b[TrendStrengthAnalysis.Column_TrendStrength];

                            // For simulation accuracy, the prominence can't be greater than the back testing offset.
                            if (prominence > j) prominence = j;

                            if (prominence > MinimumPeakProminenceForAnalysis)// || trendStrength > MinimumTrendStrength)
                            {
                                double high = b[PivotPointAnalysis.Column_High];
                                gpd.PositiveList[i_test] = new(i_test, b.Time, high, prominence, trendStrength);
                                gpd.LevelRange.Insert(high);
                            }
                            else if (prominence < -MinimumPeakProminenceForAnalysis)// || trendStrength < -MinimumTrendStrength)
                            {
                                double low = b[PivotPointAnalysis.Column_Low];
                                gpd.NegativeList[i_test] = new(i_test, b.Time, low, prominence, trendStrength);
                                gpd.LevelRange.Insert(low);
                            }
                        }
                    }

                    b0[Result_Column] = gpd;
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
