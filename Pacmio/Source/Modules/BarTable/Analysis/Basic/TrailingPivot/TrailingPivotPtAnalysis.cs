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
    public class TrailingPivotPtAnalysis : BarAnalysis
    {
        public TrailingPivotPtAnalysis(NativePivotAnalysis ans)
        {
            NativePivotAnalysis = ans;

            MaximumInterval = ans.MaximumPeakProminence;
            string label = "(" + Bar.Column_Close.Name + "," + MaximumInterval + "," + MinimumPeakProminence + ")";
            Name = GetType().Name + label;

            NativePivotAnalysis.AddChild(this);
        }

        public TrailingPivotPtAnalysis(int test_length = 250)
        {
            NativePivotAnalysis = new NativePivotAnalysis(test_length);

            MaximumInterval = test_length;
            string label = "(" + Bar.Column_Close.Name + "," + MaximumInterval + "," + MinimumPeakProminence + ")";
            Name = GetType().Name + label;

            NativePivotAnalysis.AddChild(this);
        }

        #region Parameters

        public override int GetHashCode() => GetType().GetHashCode() ^ Name.GetHashCode();

        public NativePivotAnalysis NativePivotAnalysis { get; }

        public virtual int MaximumInterval { get; }

        public virtual int MinimumPeakProminence => NativePivotAnalysis.MinimumPeakProminence;

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
                    lock (b0.PivotPtLockObject)
                    {
                        b0.ResetPivotPtList();

                        for (int j = MinimumPeakProminence; j < MaximumInterval; j++)
                        {
                            int index = i - j;
                            if (index < 0) break;

                            if (bt[index] is Bar b)
                            {
                                double pivot = b.Pivot;
                                //double trendStrength = b.TrendStrength;

                                double strength = pivot + (b.TrendStrength + b.PivotStrength) / 2;

                                if (pivot > j) pivot = j;

                                if (pivot > MinimumPeakProminence)
                                {
                                    double high = b.High;
                                    b0.PositivePivotPtList[index] = new(index, b.Time, high, strength); // pivot, trendStrength);
                                    b0.PivotRange.Insert(high);
                                }
                                else if (pivot < -MinimumPeakProminence)
                                {
                                    double low = b.Low;
                                    b0.NegativePivotPtList[index] = new(index, b.Time, low, strength); // , pivot, trendStrength);
                                    b0.PivotRange.Insert(low);
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
