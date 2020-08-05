/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public sealed class Gain : BarAnalysis, IChartSeries
    {
        public Gain(NumericColumn column, int maximumPeakProminence)
        {
            MaximumPeakProminence = maximumPeakProminence;

            string label = Column is null ? "(error)" : (Column == Bar.Column_Close ? "(" + MaximumPeakProminence.ToString() + ")" : "(" + Column.Name + "," + MaximumPeakProminence.ToString() + ")");
            Name = AreaName = GroupName = GetType().Name + label;

            Column = column;
            Column_Gain = new NumericColumn(Name + "_Gain");
            Column_Percent = new NumericColumn(Name + "_Percent");
            Column_TrueRange = new NumericColumn(Name + "_TrueRange");
            Column_Typical = new NumericColumn(Name + "_Typical");
            Column_Peak = new NumericColumn(Name + "_Peak");
            Column_TrendStrength = new NumericColumn(Name + "_TrendStrength");

            Description = (Column == Bar.Column_Close) ? GetType().Name : GetType().Name + " (" + Column.Name + ")";

            ColumnSeries_Gain = new ColumnSeries(Column_Gain, Color.FromArgb(88, 168, 208), Color.FromArgb(32, 104, 136), 50)
            {
                Name = Name + "_GAIN",
                LegendName = GroupName + "_GAIN",
                Label = "GAIN",
                Importance = Importance.Minor,
                Order = 200
            };

            ColumnSeries_Trend = new ColumnSeries(Column_TrendStrength, Color.FromArgb(88, 168, 208), Color.FromArgb(32, 104, 136), 50)
            {
                Name = Name + "_Trend",
                LegendName = GroupName + "_Trend",
                Label = "TREND",
                Importance = Importance.Minor,
                Order = 200
            };
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Column.GetHashCode() ^ MaximumPeakProminence;

        #region Parameters

        public int MaximumPeakProminence { get; }

        public NumericColumn Column { get; } // The source Column

        #endregion Parameters

        #region Calculation

        public NumericColumn Column_Gain { get; }

        public NumericColumn Column_Percent { get; }

        public NumericColumn Column_TrueRange { get; }

        public NumericColumn Column_Typical { get; }

        public NumericColumn Column_Peak { get; }

        public NumericColumn Column_TrendStrength { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            double data_1, trend_1;

            int min_peak_start = bap.StopPt - MaximumPeakProminence * 2 - 1;
            if (bap.StartPt > min_peak_start) bap.StartPt = min_peak_start;

            // Define the bondary condition
            if (bap.StartPt < 1)
            {
                if (bap.StartPt < 0) bap.StartPt = 0;
                data_1 = bt[0][Column];
                trend_1 = 0;
            }
            else
            {
                data_1 = bt[bap.StartPt - 1][Column];
                trend_1 = bt[bap.StartPt - 1][Column_TrendStrength];
            }

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];

                // Get Gain
                double data = b[Column];
                double gain = b[Column_Gain] = data - data_1;

                b[Column_Percent] = (data_1 == 0) ? 0 : (100 * gain / data_1);

                // Get Trend
                double trend = 0;
                if (gain > 0)
                {
                    trend = (trend_1 > 0) ? trend_1 + 1 : 1;
                }
                else if (gain < 0)
                {
                    trend = (trend_1 < 0) ? trend_1 - 1 : -1;
                }

                // Get Peak and Troughs
                int peak_result = 0;
                int j = 1;
                bool test_high = true, test_low = true;

                while (j < MaximumPeakProminence)
                {
                    if ((!test_high) && (!test_low)) break;

                    int right_index = i + j;
                    if (right_index >= bap.StopPt) right_index = bap.StopPt - 1;

                    int left_index = i - j;
                    if (left_index < 0) left_index = 0;

                    double left_data = bt[left_index][Column];
                    double right_data = bt[right_index][Column];

                    if (test_high)
                    {
                        if (data >= left_data && data >= right_data)
                        {
                            peak_result = j;
                            if (data == left_data) bt[left_index][Column_Peak] = 0;
                        }
                        else
                            test_high = false;
                    }

                    if (test_low)
                    {
                        if (data <= left_data && data <= right_data)
                        {
                            peak_result = -j;
                            if (data == left_data) bt[left_index][Column_Peak] = 0;
                        }
                        else
                            test_low = false;
                    }
                    j++;
                }

                b[Column_Peak] = peak_result;
                b[Column_TrendStrength] = trend_1 = trend;
                data_1 = data;
            }
        }

        #endregion Calculation

        #region Series

        public Color Color { get => ColumnSeries_Gain.Color; set => ColumnSeries_Gain.Color = ColumnSeries_Gain.ShadeColor = value; }

        public ColumnSeries ColumnSeries_Gain { get; }

        public ColumnSeries ColumnSeries_Trend { get; }

        public bool ChartEnabled { get => Enabled && ColumnSeries_Gain.Enabled; set => ColumnSeries_Gain.Enabled = ColumnSeries_Trend.Enabled = value; }

        public int SeriesOrder { get => ColumnSeries_Gain.Order; set => ColumnSeries_Gain.Order = ColumnSeries_Trend.Order = value; }

        public bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; }

        public float AreaRatio { get; set; } = 12;

        public void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                Area a_gain = bc.AddArea(new Area(bc, AreaName + "_gain", AreaRatio)
                {
                    HasXAxisBar = HasXAxisBar,
                });
                a_gain.AddSeries(ColumnSeries_Gain);

                Area a_trend = bc.AddArea(new Area(bc, AreaName + "_trend", AreaRatio)
                {
                    HasXAxisBar = HasXAxisBar,
                });
                a_trend.AddSeries(ColumnSeries_Trend);
            }
        }

        #endregion Series
    }
}
