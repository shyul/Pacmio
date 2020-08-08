/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Drawing;
using System.Linq;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public class TrendStrength : BarAnalysis, IChartSeries
    {
        public TrendStrength()
        {
            Column_High = Bar.Column_High;
            Column_Low = Bar.Column_Low;
            Column_Close = Bar.Column_Close;

            string label = "(" + Column_High.Name + "," + Column_Low.Name + "," + Column_Close.Name + ")";
            GroupName = Name = GetType().Name + label;
            Description = Name + " " + label;
            AreaName = label;

            Column_TrendStrength = new NumericColumn(Name) { Label = label };

            ColumnSeries_TrendStrength = new ColumnSeries(Column_TrendStrength, Color.FromArgb(88, 168, 208), Color.FromArgb(32, 104, 136), 50)
            {
                Name = Name,
                LegendName = GroupName + "_" + GetType().Name,
                Label = GetType().Name,
                Importance = Importance.Minor,
                Order = 200
            };
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Column_High.GetHashCode() ^ Column_Low.GetHashCode() ^ Column_Close.GetHashCode();

        #region Calculation

        public NumericColumn Column_High { get; }

        public NumericColumn Column_Low { get; }

        public NumericColumn Column_Close { get; }

        public NumericColumn Column_TrendStrength { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            double high_1, low_1, close_1, trend_1;

            if (bap.StartPt < 1)
            {
                Bar b = bt[0];
                if (bap.StartPt < 0) bap.StartPt = 0;
                high_1 = b[Column_High];
                low_1 = b[Column_Low];
                close_1 = b[Column_Close];
                trend_1 = b[Column_TrendStrength] = 0;
            }
            else
            {
                Bar b_1 = bt[bap.StartPt - 1];
                high_1 = b_1.High;
                low_1 = b_1[Column_Low];
                close_1 = b_1[Column_Close];
                trend_1 = b_1.TrendStrength;
            }

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                double high = b[Column_High];
                double low = b[Column_Low];
                double close = b[Column_Close];

                double trend = 0;
                if (close > close_1 || (high > high_1 && low > low_1))
                {
                    trend = (trend_1 > 0) ? trend_1 + 1 : 1;
                }
                else if (close < close_1 || (high < high_1 && low < low_1))
                {
                    trend = (trend_1 < 0) ? trend_1 - 1 : -1;
                }

                b[Column_TrendStrength] = trend_1 = trend;
                high_1 = high;
                low_1 = low;
                close_1 = close;
            }
        }

        #endregion Calculation

        #region Series

        public Color Color
        {
            get => ColumnSeries_TrendStrength.Color;
            set
            {
                ColumnSeries_TrendStrength.Color = value;
                ColumnSeries_TrendStrength.ShadeColor = value;
            }
        }

        public ColumnSeries ColumnSeries_TrendStrength { get; }

        public bool ChartEnabled { get => Enabled && ColumnSeries_TrendStrength.Enabled; set => ColumnSeries_TrendStrength.Enabled = value; }

        public int SeriesOrder
        {
            get => ColumnSeries_TrendStrength.Order; set
            {
                ColumnSeries_TrendStrength.Order = value;
            }
        }

        public bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; }

        public float AreaRatio { get; set; } = 8;

        public void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                Area truerange_area = bc.AddArea(new Area(bc, "TrendStrength" + AreaName, AreaRatio)
                {
                    HasXAxisBar = false,
                });
                truerange_area.AddSeries(ColumnSeries_TrendStrength);
            }
        }

        #endregion Series
    }
}
