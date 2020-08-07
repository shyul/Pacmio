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
    public sealed class Peak : BarAnalysis, ISingleData, IChartSeries, IChartGraphics
    {
        public Peak(NumericColumn column, int maximumPeakProminence)
        {
            MaximumPeakProminence = maximumPeakProminence;
            Column_High = Column_Low = column;


        }

        public Peak(ISingleData isd, int maximumPeakProminence)
        {
            MaximumPeakProminence = maximumPeakProminence;

            if (isd is IChartSeries ics)
            {
                GraphicsAreaName = ics.AreaName;
            }

        }



        public Peak(NumericColumn column_high, NumericColumn column_low, int maximumPeakProminence)
        {
            MaximumPeakProminence = maximumPeakProminence;
            Column_High = column_high;
            Column_Low = column_low;


        }

        public Peak(IDualData idd, int maximumPeakProminence)
        {
            MaximumPeakProminence = maximumPeakProminence;

            if (idd is IChartSeries ics)
            {
                GraphicsAreaName = ics.AreaName;
            }

        }

        public int MaximumPeakProminence { get; }

        public NumericColumn Column_High { get; }

        public NumericColumn Column_Low { get; }

        public NumericColumn Result_Column { get; }

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
                            if (high == left_high) bt[left_index][Result_Column] = 0;
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
                            if (low == left_low) bt[left_index][Result_Column] = 0;
                        }
                        else
                            test_low = false;
                    }
                    j++;
                }

                b[Result_Column] = peak_result;
            }
        }

        public Color Color
        {
            get => ColumnSeries_Peak.Color;
            set
            { 
                ColumnSeries_Peak.Color = value; 
                ColumnSeries_Peak.ShadeColor = value; 
            }
        }

        public ColumnSeries ColumnSeries_Peak { get; }

        public bool ChartEnabled { get => Enabled && ColumnSeries_Peak.Enabled; set => ColumnSeries_Peak.Enabled = value; }

        public int SeriesOrder { get => ColumnSeries_Peak.Order; set => ColumnSeries_Peak.Order = value; }

        public bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; }

        public float AreaRatio { get; set; } = 8;

        public void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                Area a_gain = bc.AddArea(new Area(bc, AreaName + "_peak", AreaRatio)
                {
                    HasXAxisBar = HasXAxisBar,
                });
                a_gain.AddSeries(ColumnSeries_Peak);
            }
        }

        public string GraphicsAreaName { get; }

        public void DrawOverlay(Graphics g, BarChart bc)
        {

        }

        public void DrawBackground(Graphics g, BarChart bc)
        {

        }
    }

}
