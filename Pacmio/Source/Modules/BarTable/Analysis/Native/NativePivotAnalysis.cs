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
    public sealed class NativePivotAnalysis : PeakAnalysis
    {
        public NativePivotAnalysis(int maximumPeakProminence = 50, int minimumPeakProminence = 5)
        {
            MaximumPeakProminence = maximumPeakProminence;
            MinimumPeakProminence = minimumPeakProminence;

            string label = "(" + MaximumPeakProminence + "," + MinimumPeakProminence + ")";
            Name = AreaName = GroupName = GetType().Name + label;
            Description = Name + " " + label;

            Column_PeakTags = new(Name + "_PIVOTPOINTTAG", "PIVOTPOINT", typeof(TagInfo));

            ColumnSeries = new(Bar.Column_Pivot, 50, 0, 0)
            {
                Name = Name,
                LegendName = "Pivot Point " + label,
                Label = "",
                Importance = Importance.Major,
                Side = AlignType.Right,
                IsAntialiasing = false
            };

            UpperColor = Color.Green;
            LowerColor = Color.Red;
        }

        public override int GetHashCode() => GetType().GetHashCode();

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];

                double high = b.High;
                double low = b.Low;

                // Get Peak and Troughs
                int peak_result = 0;
                int j = 1;
                bool test_high = true, test_low = true;

                while (j < MaximumPeakProminence && (test_high || test_low))
                {
                    int right_index = i + j;
                    if (right_index >= bap.StopPt) break;

                    int left_index = i - j;
                    if (left_index < 0) break;
                    
                    if (test_high)
                    {
                        double left_high = bt[left_index].High;
                        double right_high = bt[right_index].High;

                        if (high >= left_high && high >= right_high)
                        {
                            peak_result = j;
                            if (high == left_high) bt[left_index].Pivot = 0;
                        }
                        else
                            test_high = false;
                    }

                    if (test_low)
                    {
                        double left_low = bt[left_index].Low;
                        double right_low = bt[right_index].Low;

                        if (low <= left_low && low <= right_low)
                        {
                            peak_result = -j;
                            if (low == left_low) bt[left_index].Pivot = 0;
                        }
                        else
                            test_low = false;
                    }
                    j++;
                }

                b.Pivot = peak_result;
                /*
                if (b.Pivot < 0)
                {
                    Console.WriteLine(b.Time + " | " + peak_result);
                }*/
            }

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (bt[i] is Bar b)
                {
                    double high = b.High;
                    double low = b.Low;
                    double peak_result = b.Pivot;

                    if (peak_result > MinimumPeakProminence)
                    {
                        b[Column_PeakTags] = new TagInfo(i, high.ToString("G5"), DockStyle.Top, ColumnSeries.TextTheme);
                    }
                    else if (peak_result < -MinimumPeakProminence)
                    {
                        b[Column_PeakTags] = new TagInfo(i, low.ToString("G5"), DockStyle.Bottom, ColumnSeries.LowerTextTheme);
                    }
                }
            }
        }
    }
}
