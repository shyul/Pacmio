/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;
using System.Windows.Forms;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public sealed class PivotPoint : BarAnalysis, ISingleData, IChartSeries
    {
        public PivotPoint(NumericColumn column, int maximumPeakProminence)
        {
            MaximumPeakProminence = maximumPeakProminence;
            Column_High = Column_Low = column;

            string label = "(" + Column_High.Name + "," + maximumPeakProminence + ")";
            Name = AreaName = GroupName = GetType().Name + label;
            Description = Name + " " + label;

            Column_Result = new NumericColumn(Name) { Label = label };
            Column_PeakTags = new TagColumn(Name + "_PIVOTPOINTTAG", "PIVOTPOINT");

            ColumnSeries = new AdColumnSeries(Column_Result, Column_Result, 50, 0, 0)
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

        public PivotPoint(ISingleData isd, int maximumPeakProminence)
        {
            MaximumPeakProminence = maximumPeakProminence;
            Column_High = Column_Low = isd.Column_Result;

            string label = "(" + Column_High.Name + "," + maximumPeakProminence + ")";
            Name = AreaName = GroupName = GetType().Name + label;
            Description = Name + " " + label;

            Column_Result = new NumericColumn(Name) { Label = label };
            Column_PeakTags = new TagColumn(Name + "_PIVOTPOINTTAG", "PIVOTPOINT");

            ColumnSeries = new AdColumnSeries(Column_Result, Column_Result, 50, 0, 0)
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

        public PivotPoint(NumericColumn column_high, NumericColumn column_low, int maximumPeakProminence)
        {
            MaximumPeakProminence = maximumPeakProminence;
            Column_High = column_high;
            Column_Low = column_low;

            string label = "(" + Column_High.Name + "," + Column_Low.Name + "," + maximumPeakProminence + ")";
            Name = AreaName = GroupName = GetType().Name + label;
            Description = Name + " " + label;

            Column_Result = new NumericColumn(Name) { Label = label };
            Column_PeakTags = new TagColumn(Name + "_PIVOTPOINTTAG", "PIVOTPOINT");

            ColumnSeries = new AdColumnSeries(Column_Result, Column_Result, 50, 0, 0)
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

        public PivotPoint(IDualData idd, int maximumPeakProminence)
        {
            MaximumPeakProminence = maximumPeakProminence;
            Column_High = idd.Column_High;
            Column_Low = idd.Column_Low;

            string label = "(" + Column_High.Name + "," + Column_Low.Name + "," + maximumPeakProminence + ")";
            Name = AreaName = GroupName = GetType().Name + label;
            Description = Name + " " + label;

            Column_Result = new NumericColumn(Name) { Label = label };
            Column_PeakTags = new TagColumn(Name + "_PIVOTPOINTTAG", "PIVOTPOINT");

            ColumnSeries = new AdColumnSeries(Column_Result, Column_Result, 50, 0, 0)
            {
                Name = Name,
                LegendName = "Pivot Point " + label,
                Label = "",
                Importance = Importance.Major,
                Side = AlignType.Right,
                IsAntialiasing = false
            };

            if (idd is IChartSeries ics && ics.MainSeries is ITagSeries ts)
            {
                ts.TagColumns.Add(Column_PeakTags);
            }

            idd.AddChild(this);

            UpperColor = idd.UpperColor;
            LowerColor = idd.LowerColor;
        }

        public PivotPoint(int maximumPeakProminence = 100)
        {
            MaximumPeakProminence = maximumPeakProminence;
            Column_High = Bar.Column_High;
            Column_Low = Bar.Column_Low;

            string label = "(" + Column_High.Name + "," + Column_Low.Name + "," + maximumPeakProminence + ")";
            Name = AreaName = GroupName = GetType().Name + label;
            Description = Name + " " + label;

            Column_Result = new NumericColumn(Name) { Label = label };
            Column_PeakTags = new TagColumn(Name + "_PIVOTPOINTTAG", "PIVOTPOINT");

            ColumnSeries = new AdColumnSeries(Column_Result, Column_Result, 50, 0, 0)
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

        public override int GetHashCode() => GetType().GetHashCode() ^ Column_High.GetHashCode() ^ Column_Low.GetHashCode() ^ MaximumPeakProminence;

        public int MaximumPeakProminence { get; }

        public int MinimumPeakProminenceForTagDisplay { get; set; } = 5;

        public NumericColumn Column_High { get; }

        public NumericColumn Column_Low { get; }

        public NumericColumn Column_Result { get; }

        public TagColumn Column_PeakTags { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            int min_peak_start = bap.StopPt - MaximumPeakProminence * 2 - 1;
            if (bap.StartPt > min_peak_start) bap.StartPt = min_peak_start;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
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

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                double high = b[Column_High];
                double low = b[Column_Low];
                double peak_result = b[Column_Result];

                if (peak_result > MinimumPeakProminenceForTagDisplay)
                {
                    b[Column_PeakTags] = new TagInfo(i, high.ToString("G5"), DockStyle.Top, ColumnSeries.TextTheme);
                }
                else if (peak_result < -MinimumPeakProminenceForTagDisplay)
                {
                    b[Column_PeakTags] = new TagInfo(i, low.ToString("G5"), DockStyle.Bottom, ColumnSeries.LowerTextTheme);
                }
            }
        }

        public Color Color { get => UpperColor; set => UpperColor = value; }

        public Color UpperColor
        {
            get => ColumnSeries.Color;

            set
            {
                Color c = value;
                ColumnSeries.Color = c.GetBrightness() < 0.6 ? c.Brightness(0.85f) : c.Brightness(-0.85f);
                ColumnSeries.EdgeColor = ColumnSeries.TextTheme.ForeColor = c;
            }
        }

        public Color LowerColor
        {
            get => ColumnSeries.LowerColor;

            set
            {
                Color c = value;
                ColumnSeries.LowerColor = c.GetBrightness() < 0.6 ? c.Brightness(0.85f) : c.Brightness(-0.85f);
                ColumnSeries.LowerEdgeColor = ColumnSeries.LowerTextTheme.ForeColor = c;
            }
        }

        public Series MainSeries => ColumnSeries;

        public AdColumnSeries ColumnSeries { get; }

        public bool ChartEnabled { get => Enabled && ColumnSeries.Enabled; set => ColumnSeries.Enabled = value; }

        public int SeriesOrder { get => ColumnSeries.Order; set => ColumnSeries.Order = value; }

        public bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; }

        public float AreaRatio { get; set; } = 8;

        public void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                Area a_gain = bc.AddArea(new Area(bc, AreaName, AreaRatio)
                {
                    HasXAxisBar = HasXAxisBar,
                });
                a_gain.AddSeries(ColumnSeries);
            }
        }
    }

}
