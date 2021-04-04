/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The intermediate calculation for most pattern analysis
/// 
/// ***************************************************************************

using System;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public class TrailingApexPtAnalysis : ApexAnalysis, ISingleDatum
    {
        public TrailingApexPtAnalysis(int maximumInterval = 250)
        {
            ApexAnalysis = new NativeApexAnalysis(maximumInterval);

            MaximumPeakProminence = maximumInterval;
            MinimumPeakProminence = ApexAnalysis.MinimumPeakProminence;

            string label = "(" + Bar.Column_Close.Name + "," + MaximumPeakProminence + "," + MinimumPeakProminence + ")";
            Name = GetType().Name + label;

            ApexAnalysis.AddChild(this);
            Column_Result = new(Name, typeof(TrailingApexPtDatum));
        }

        public TrailingApexPtAnalysis(ApexAnalysis pa)
        {
            ApexAnalysis = pa;

            MaximumPeakProminence = pa.MaximumPeakProminence;
            MinimumPeakProminence = ApexAnalysis.MinimumPeakProminence;

            string label = "(" + Bar.Column_Close.Name + "," + MaximumPeakProminence + "," + MinimumPeakProminence + ")";
            Name = GetType().Name + label;

            ApexAnalysis.AddChild(this);
            Column_Result = new(Name, typeof(TrailingApexPtDatum));
        }

        #region Parameters

        public override int GetHashCode() => GetType().GetHashCode() ^ Name.GetHashCode();

        public override NumericColumn Column_High => ApexAnalysis.Column_High;

        public override NumericColumn Column_Low => ApexAnalysis.Column_Low;

        public ApexAnalysis ApexAnalysis { get; }

        public DatumColumn Column_Result { get; }

        #endregion Parameters

        #region Calculation

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (bt[i] is Bar b0)
                {
                    TrailingApexPtDatum tpd = new(this);

                    for (int j = MinimumPeakProminence; j < MaximumPeakProminence; j++)
                    {
                        int index = i - j;
                        if (index < 0) break;

                        if (bt[index] is Bar b)
                        {
                            double pivot = b.Pivot;
                            if (pivot > j - 1) pivot = j - 1;

                            double strength = pivot + (b.TrendStrength + b.PivotStrength) / 2;

                            if (pivot > MinimumPeakProminence)
                            {
                                double high = b.High;
                                tpd.PositiveApexPtList[index] = new(index, b.Time, high, strength); // pivot, trendStrength);
                                tpd.PivotRange.Insert(high);
                            }
                            else if (pivot < -MinimumPeakProminence)
                            {
                                double low = b.Low;
                                tpd.NegativeApexPtList[index] = new(index, b.Time, low, strength); // , pivot, trendStrength);
                                tpd.PivotRange.Insert(low);
                            }
                        }
                    }

                    double range_delta = (tpd.PivotRange.Max - tpd.PivotRange.Min) / 2;
                    double center = (b0[Column_High] + b0[Column_Low]) / 2;
                    tpd.TotalLevelRange = new(center - range_delta, center + range_delta);

                    b0[Column_Result] = tpd;
                }
            }

            #endregion Calculation
        }
    }
}
