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
        public TrailingPivotPointsAnalysis(int test_length = 250)
        {
            //PivotPointAnalysis = BarTable.PivotAnalysis;
            TestLength = test_length;
            //TrendStrengthAnalysis = new();

            //BarTable.GainAnalysis

            string label = "(" + Bar.Column_Close.Name + "," + TestLength + "," + MinimumPeakProminenceForAnalysis + ")";
            Name = GetType().Name + label;
        }

        #region Parameters

        public override int GetHashCode() => GetType().GetHashCode() ^ Name.GetHashCode();

        public virtual int TestLength { get; }

        public virtual int MinimumPeakProminenceForAnalysis => 5;

        #endregion Parameters

        #region Calculation

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
                    lock (b0.PatternPointLockObject) 
                    {
                        b0.ResetPatternPointList();

                        for (int j = MinimumPeakProminenceForAnalysis; j < TestLength; j++)
                        {
                            int i_test = i - j;
                            if (i_test < 0) break;

                            if (bt[i_test] is Bar b)
                            {
                                double prominence = b.Pivot; //[PivotPointAnalysis.Column_Result];
                                double trendStrength = b.TrendStrength;//[TrendStrengthAnalysis.Column_TrendStrength];

                                // For simulation accuracy, the prominence can't be greater than the back testing offset.
                                if (prominence > j) prominence = j;

                                if (prominence > MinimumPeakProminenceForAnalysis)// || trendStrength > MinimumTrendStrength)
                                {
                                    double high = b.High;
                                    b0.PositivePatternPointList[i_test] = new(i_test, b.Time, high, prominence, trendStrength);
                                    b0.PatternPointsRange.Insert(high);
                                }
                                else if (prominence < -MinimumPeakProminenceForAnalysis)// || trendStrength < -MinimumTrendStrength)
                                {
                                    double low = b.Low;
                                    b0.NegativePatternPointList[i_test] = new(i_test, b.Time, low, prominence, trendStrength);
                                    b0.PatternPointsRange.Insert(low);
                                }
                            }
                        }

                    }
                }
            }

            #endregion Calculation
        }
    }
}
