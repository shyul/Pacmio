﻿/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;
using System.Windows.Forms;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public class DualColumnPivotAnalysis : PivotAnalysis
    {
        public DualColumnPivotAnalysis(IDualData idd, int maximumPeakProminence = 50, int minimumPeakProminence = 5)
        {
            MaximumPeakProminence = maximumPeakProminence;
            MinimumPeakProminence = minimumPeakProminence;
            DualDataAnalysis = idd;

            string label = "(" + DualDataAnalysis.Name + "," + MaximumPeakProminence + "," + MinimumPeakProminence + ")";
            Name = GroupName = GetType().Name + label;
            Description = Name + " " + label;

            Column_Result = new(Name, label);
            Column_PeakTags = new(Name + "_PIVOTPOINTTAG", "PIVOTPOINT", typeof(TagInfo));

            UpperColor = idd.UpperColor;// Color.Green;
            LowerColor = idd.LowerColor; // Color.Red;
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Column_High.GetHashCode() ^ Column_Low.GetHashCode() ^ MaximumPeakProminence ^ MinimumPeakProminence;

        public IDualData DualDataAnalysis { get; }

        public NumericColumn Column_High => DualDataAnalysis.Column_High;

        public NumericColumn Column_Low => DualDataAnalysis.Column_Low;

        public NumericColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (bt[i] is Bar b)
                {
                    double high = b[Column_High];
                    double low = b[Column_Low];

                    // Get Peak and Troughs
                    int peak_result = 0;
                    int j = 1;
                    bool test_high = true, test_low = true;

                    while (j < MaximumPeakProminence)
                    {
                        if ((!test_high) && (!test_low)) break;

                        int right_index = i + j;
                        if (right_index >= bap.StopPt) break;

                        int left_index = i - j;
                        if (left_index < 0) break;

                        if (test_high)
                        {
                            double left_high = bt[left_index][Column_High];
                            double right_high = bt[right_index][Column_High];

                            if (high >= left_high && high >= right_high)
                            {
                                peak_result = j;
                                if (high == left_high) bt[left_index][Column_Result] = 0;
                            }
                            else
                                test_high = false;
                        }

                        if (test_low)
                        {
                            double left_low = bt[left_index][Column_Low];
                            double right_low = bt[right_index][Column_Low];

                            if (low <= left_low && low <= right_low)
                            {
                                peak_result = -j;
                                if (low == left_low) bt[left_index][Column_Result] = 0;
                            }
                            else
                                test_low = false;
                        }
                        j++;
                    }

                    b[Column_Result] = peak_result;
                }
            }

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (bt[i] is Bar b)
                {
                    double high = b[Column_High];
                    double low = b[Column_Low];
                    double peak_result = b[Column_Result];

                    if (peak_result > MinimumPeakProminence)
                    {
                        b[Column_PeakTags] = new TagInfo(i, high.ToString("G5"), DockStyle.Top, UpperTagTheme);
                    }
                    else if (peak_result < -MinimumPeakProminence)
                    {
                        b[Column_PeakTags] = new TagInfo(i, low.ToString("G5"), DockStyle.Bottom, LowerTagTheme);
                    }
                }
            }
        }

        public override void ConfigChart(BarChart bc) 
        {
            if (DualDataAnalysis is IChartSeries ics && ics.MainSeries is ITagSeries ts)
            {
                ts.TagColumns.Add(Column_PeakTags);
            }
        }
    }
}
