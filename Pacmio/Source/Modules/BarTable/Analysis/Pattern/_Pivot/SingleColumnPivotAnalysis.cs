/// ***************************************************************************
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
    public class SingleColumnPivotAnalysis : PivotAnalysis, ISingleData
    {
        public SingleColumnPivotAnalysis(NumericColumn column, int maximumPeakProminence, int minimumPeakProminenceForAnalysis = 5)
        {
            Column = column;
            MaximumPeakProminence = maximumPeakProminence;
            MinimumPeakProminence = minimumPeakProminenceForAnalysis;

            string label = "(" + Column.Name + "," + maximumPeakProminence + "," + MinimumPeakProminence + ")";
            Name = GroupName = GetType().Name + label;
            Description = Name + " " + label;

            Column_Result = new(Name, label);
            Column_Strength_Result = new(Name + "_STRENGTH", label + "_STRENGTH");
            Column_PeakTags = new(Name + "_PIVOTPOINTTAG", "PIVOTPOINT", typeof(TagInfo));

            UpperColor = Color.Green;
            LowerColor = Color.Red;

            AreaName = null;
        }

        public SingleColumnPivotAnalysis(ISingleData isd, int maximumPeakProminence, int minimumPeakProminenceForAnalysis = 5)
        {
            SingleDataAnalysis = isd;
            Column = isd.Column_Result;
            MaximumPeakProminence = maximumPeakProminence;
            MinimumPeakProminence = minimumPeakProminenceForAnalysis;

            string label = "(" + Column.Name + "," + maximumPeakProminence + "," + MinimumPeakProminence + ")";
            Name = GroupName = GetType().Name + label;
            Description = Name + " " + label;

            Column_Result = new(Name, label);
            Column_Strength_Result = new(Name + "_STRENGTH", label + "_STRENGTH");
            Column_PeakTags = new(Name + "_PIVOTPOINTTAG", "PIVOTPOINT", typeof(TagInfo));

            isd.AddChild(this);

            if (isd is IOscillator iosc)
            {
                UpperColor = iosc.UpperColor;
                LowerColor = iosc.LowerColor;
            }

            AreaName = isd is IChartAnalysis ica ? ica.AreaName : null;
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Column.GetHashCode() ^ MaximumPeakProminence ^ MinimumPeakProminence;

        public ISingleData SingleDataAnalysis { get; }

        public NumericColumn Column { get; }

        public override NumericColumn Column_High => Column;

        public override NumericColumn Column_Low => Column;

        public NumericColumn Column_Result { get; }

        public NumericColumn Column_Strength_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (bt[i] is Bar b)
                {
                    double val = b[Column];

                    // Get Peak and Troughs
                    int pivot_result = 0, strength_result = 0;
                    int j = 1;
                    bool test_pivot_high = true, test_pivot_low = true;
                    bool test_strength_high = true, test_strength_low = true;
                    double last_left_strength_high = val, last_left_strength_low = val;
                    double last_right_strength_high = val, last_right_strength_low = val;

                    while (j < MaximumPeakProminence && (test_pivot_high || test_pivot_low))
                    {
                        //if ((!test_pivot_high) && (!test_pivot_low)) break;

                        int right_index = i + j;
                        if (right_index >= bap.StopPt) break;

                        int left_index = i - j;
                        if (left_index < 0) break;

                        if (test_pivot_high)
                        {
                            double left_high = bt[left_index][Column];
                            double right_high = bt[right_index][Column];

                            if (val >= left_high && val >= right_high)
                            {
                                pivot_result = j;
                                if (val == left_high)
                                    bt[left_index][Column_Result] = 0;
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

                            if (test_pivot_low)
                            {
                                double left_low = bt[left_index][Column];
                                double right_low = bt[right_index][Column];

                                if (val <= left_low && val <= right_low)
                                {
                                    pivot_result = -j;
                                    if (val == left_low)
                                        bt[left_index][Column_Result] = 0;
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

                        b[Column_Result] = pivot_result;
                        b[Column_Strength_Result] = strength_result;
                    }
                }
            }

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (bt[i] is Bar b)
                {
                    double val = b[Column];
                    double peak_result = b[Column_Result];

                    if (peak_result > MinimumPeakProminence)
                    {
                        b[Column_PeakTags] = new TagInfo(i, val.ToString("G5"), DockStyle.Top, UpperTagTheme);
                    }
                    else if (peak_result < -MinimumPeakProminence)
                    {
                        b[Column_PeakTags] = new TagInfo(i, val.ToString("G5"), DockStyle.Bottom, LowerTagTheme);
                    }
                }
            }
        }

        public override void ConfigChart(BarChart bc)
        {
            if (SingleDataAnalysis is IChartSeries ics && ics.MainSeries is ITagSeries ts)
            {
                ts.TagColumns.Add(Column_PeakTags);
            }
        }
    }
}
