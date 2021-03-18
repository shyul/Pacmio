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
    public class SingleColumnPivotAnalysis : PivotAnalysis, ISingleData, IChartSeries
    {
        public SingleColumnPivotAnalysis(NumericColumn column, int maximumPeakProminence, int minimumPeakProminenceForAnalysis = 5)
        {
            MaximumPeakProminence = maximumPeakProminence;
            MinimumPeakProminence = minimumPeakProminenceForAnalysis;
            Column = column;

            string label = "(" + Column.Name + "," + maximumPeakProminence + "," + MinimumPeakProminence + ")";
            Name = AreaName = GroupName = GetType().Name + label;
            Description = Name + " " + label;

            Column_Result = new(Name) { Label = label };
            Column_PeakTags = new(Name + "_PIVOTPOINTTAG", "PIVOTPOINT", typeof(TagInfo));

            ColumnSeries = new(Column_Result, Column_Result, 50, 0, 0)
            {
                Name = Name,
                LegendName = GroupName + ": ",
                Label = "Pivot Point ",
                Importance = Importance.Major,
                Side = AlignType.Right,
                IsAntialiasing = false,
                Order = 200
            };

            UpperColor = Color.Green;
            LowerColor = Color.Red;
        }

        public SingleColumnPivotAnalysis(ISingleData isd, int maximumPeakProminence, int minimumPeakProminenceForAnalysis = 5)
        {
            MaximumPeakProminence = maximumPeakProminence;
            MinimumPeakProminence = minimumPeakProminenceForAnalysis;
            Column = isd.Column_Result;

            string label = "(" + Column.Name + "," + maximumPeakProminence + "," + MinimumPeakProminence + ")";
            Name = AreaName = GroupName = GetType().Name + label;
            Description = Name + " " + label;

            Column_Result = new(Name) { Label = label };
            Column_PeakTags = new(Name + "_PIVOTPOINTTAG", "PIVOTPOINT", typeof(TagInfo));

            ColumnSeries = new(Column_Result, Column_Result, 50, 0, 0)
            {
                Name = Name,
                LegendName = "Pivot Point " + label,
                Label = "",
                Importance = Importance.Major,
                Side = AlignType.Right,
                IsAntialiasing = false
            };

            if (isd is IChartSeries ics && ics.MainSeries is ITagSeries ts)
            {
                ts.TagColumns.Add(Column_PeakTags);
            }

            isd.AddChild(this);

            if (isd is IOscillator iosc)
            {
                UpperColor = iosc.UpperColor;
                LowerColor = iosc.LowerColor;
            }
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Column.GetHashCode() ^ MaximumPeakProminence ^ MinimumPeakProminence;

        public NumericColumn Column { get; }

        public NumericColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (bt[i] is Bar b)
                {
                    double val = b[Column];

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
                            double left_high = bt[left_index][Column];
                            double right_high = bt[right_index][Column];

                            if (val >= left_high && val >= right_high)
                            {
                                peak_result = j;
                                if (val == left_high) bt[left_index][Column_Result] = 0;
                            }
                            else
                                test_high = false;
                        }

                        if (test_low)
                        {
                            double left_low = bt[left_index][Column];
                            double right_low = bt[right_index][Column];

                            if (val <= left_low && val <= right_low)
                            {
                                peak_result = -j;
                                if (val == left_low) bt[left_index][Column_Result] = 0;
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
                    double peak_result = b[Column_Result];

                    if (peak_result > MinimumPeakProminence)
                    {
                        b[Column_PeakTags] = new TagInfo(i, b[Column].ToString("G5"), DockStyle.Top, ColumnSeries.TextTheme);
                    }
                    else if (peak_result < -MinimumPeakProminence)
                    {
                        b[Column_PeakTags] = new TagInfo(i, b[Column].ToString("G5"), DockStyle.Bottom, ColumnSeries.LowerTextTheme);
                    }
                }
            }
        }
    }
}
