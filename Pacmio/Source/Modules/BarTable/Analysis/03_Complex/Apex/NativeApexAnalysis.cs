/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// 1. Pivots
/// 
/// ***************************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public sealed class NativeApexAnalysis : ApexAnalysis
    {
        public NativeApexAnalysis(int maximumPeakProminence = 50, int minimumPeakProminence = 5)
        {
            MaximumPeakProminence = maximumPeakProminence;
            MinimumPeakProminence = minimumPeakProminence;

            string label = "(" + MaximumPeakProminence + "," + MinimumPeakProminence + ")";
            Name = GroupName = GetType().Name + label;
            Description = Name + " " + label;

            Column_PeakTags = new(Name + "_PIVOTPOINTTAG", "PIVOTPOINT", typeof(TagInfo));

            UpperColor = Color.Green;
            LowerColor = Color.Red;
        }

        public override int GetHashCode() => GetType().GetHashCode();

        public override NumericColumn Column_High => Bar.Column_High;

        public override NumericColumn Column_Low => Bar.Column_Low;

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];

                double high = b.High;
                double low = b.Low;

                // Get Peak and Troughs
                int pivot_result = 0, strength_result = 0;
                int j = 1;
                bool test_pivot_high = true, test_pivot_low = true;
                bool test_strength_high = true, test_strength_low = true;
                double last_left_strength_high = high, last_left_strength_low = low;
                double last_right_strength_high = high, last_right_strength_low = low;

                while (j < MaximumPeakProminence && (test_pivot_high || test_pivot_low))
                {
                    int right_index = i + j;
                    if (right_index >= bap.StopPt) break;

                    int left_index = i - j;
                    if (left_index < 0) break;

                    if (test_pivot_high)
                    {
                        double left_high = bt[left_index].High;
                        double right_high = bt[right_index].High;

                        if (high >= left_high && high >= right_high)
                        {
                            pivot_result = j;
                            if (high == left_high)
                                bt[left_index].Pivot = 0;
                        }
                        else
                            test_pivot_high = false;

                        if (test_strength_high)
                        {
                            if (last_left_strength_high > left_high && last_right_strength_high > right_high)
                            {
                                strength_result = j;
                            }
                            else
                                test_strength_high = false;

                            last_left_strength_high = left_high;
                            last_right_strength_high = right_high;
                        }
                    }

                    if (test_pivot_low)
                    {
                        double left_low = bt[left_index].Low;
                        double right_low = bt[right_index].Low;

                        if (low <= left_low && low <= right_low)
                        {
                            pivot_result = -j;
                            if (low == left_low)
                                bt[left_index].Pivot = 0;
                        }
                        else
                            test_pivot_low = false;

                        if (test_strength_low)
                        {
                            if (last_left_strength_low < left_low && last_right_strength_low < right_low)
                            {
                                strength_result = -j;
                            }
                            else
                                test_strength_low = false;

                            last_left_strength_low = left_low;
                            last_right_strength_low = right_low;
                        }
                    }

                    j++;
                }

                b.Pivot = pivot_result;
                b.PivotStrength = strength_result;
            }

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (bt[i] is Bar b)
                {
                    double high = b.High;
                    double low = b.Low;
                    double pivot_result = b.Pivot;

                    if (pivot_result > MinimumPeakProminence)
                    {
                        b[Column_PeakTags] = new TagInfo(i, high.ToString("G5"), DockStyle.Top, UpperTagTheme);
                    }
                    else if (pivot_result < -MinimumPeakProminence)
                    {
                        b[Column_PeakTags] = new TagInfo(i, low.ToString("G5"), DockStyle.Bottom, LowerTagTheme);
                    }
                }
            }
        }

        public override void ConfigChart(BarChart bc)
        {
            bc.MainArea.PriceSeries.TagColumns.Add(Column_PeakTags);
        }
    }
}
