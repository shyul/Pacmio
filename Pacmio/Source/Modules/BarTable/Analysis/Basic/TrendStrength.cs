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
            Column_GapPercent = new NumericColumn("GapPercent" + label) { Label = label };

            ColumnSeries_TrendStrength = new AdColumnSeries(Column_TrendStrength, Column_TrendStrength, 50, 0, 0)
            {
                Name = Name,
                LegendName = "Trend " + label,
                Label = "",
                Importance = Importance.Major,
                Side = AlignType.Right,
                IsAntialiasing = false
            };

            ColumnSeries_GapPercent = new AdColumnSeries(Column_GapPercent, Column_GapPercent, 50, 0, 0)
            {
                Name = Name,
                LegendName = "Gap " + label + " %",
                Label = "",
                Importance = Importance.Major,
                Side = AlignType.Right,
                IsAntialiasing = false
            };

            UpperColor = Color.DodgerBlue;
            LowerColor = Color.MediumVioletRed;
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Column_High.GetHashCode() ^ Column_Low.GetHashCode() ^ Column_Close.GetHashCode();

        #region Calculation

        public NumericColumn Column_High { get; }

        public NumericColumn Column_Low { get; }

        public NumericColumn Column_Close { get; }

        public NumericColumn Column_TrendStrength { get; }

        public NumericColumn Column_GapPercent { get; }

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
                high_1 = b_1[Column_High];
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

                if (i > 0)
                {
                    if (low > high_1)
                    {
                        b[Column_GapPercent] = 100 * (low - high_1) / high_1;
                    }
                    else if (high < low_1)
                    {
                        b[Column_GapPercent] = 100 * (high - low_1) / low_1;
                    }
                    else
                        b[Column_GapPercent] = 0;
                }
                else
                    b[Column_GapPercent] = 0;

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

        public Color Color { get => UpperColor; set => UpperColor = value; }

        public Color UpperColor
        {
            get => ColumnSeries_TrendStrength.Color;

            set
            {
                Color c = value;
                ColumnSeries_TrendStrength.Color = ColumnSeries_GapPercent.Color = c.GetBrightness() < 0.6 ? c.Brightness(0.85f) : c.Brightness(-0.85f);
                ColumnSeries_TrendStrength.EdgeColor = ColumnSeries_TrendStrength.TextTheme.ForeColor = c;
                ColumnSeries_GapPercent.EdgeColor = ColumnSeries_GapPercent.TextTheme.ForeColor = c;
            }
        }

        public Color LowerColor
        {
            get => ColumnSeries_TrendStrength.LowerColor;

            set
            {
                Color c = value;
                ColumnSeries_TrendStrength.LowerColor = ColumnSeries_GapPercent.LowerColor = c.GetBrightness() < 0.6 ? c.Brightness(0.85f) : c.Brightness(-0.85f);
                ColumnSeries_TrendStrength.LowerEdgeColor = ColumnSeries_TrendStrength.LowerTextTheme.ForeColor = c;
                ColumnSeries_GapPercent.LowerEdgeColor = ColumnSeries_GapPercent.LowerTextTheme.ForeColor = c;
            }
        }

        public Series MainSeries => ColumnSeries_TrendStrength;

        public AdColumnSeries ColumnSeries_TrendStrength { get; }

        public AdColumnSeries ColumnSeries_GapPercent { get; }

        public bool ChartEnabled
        {
            get => Enabled && ColumnSeries_TrendStrength.Enabled;
            set => ColumnSeries_TrendStrength.Enabled = ColumnSeries_GapPercent.Enabled = value;
        }

        public int SeriesOrder
        {
            get => ColumnSeries_TrendStrength.Order;

            set
            {
                ColumnSeries_TrendStrength.Order = value;
                ColumnSeries_GapPercent.Order = value + 20;
            }
        }

        public bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; }

        public float AreaRatio { get; set; } = 8;

        public void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                OscillatorArea area_trend = bc["TrendStrength_" + AreaName] is OscillatorArea oa ? oa :
                    bc.AddArea(new OscillatorArea(bc, "TrendStrength_" + AreaName, AreaRatio)
                    {
                        Reference = 0,
                        HasXAxisBar = false,
                    });

                area_trend.AddSeries(ColumnSeries_TrendStrength);

                OscillatorArea area_gap = bc["GapPercent_" + AreaName] is OscillatorArea oa_gap ? oa_gap :
                    bc.AddArea(new OscillatorArea(bc, "GapPercent_" + AreaName, AreaRatio)
                    {
                        Reference = 0,
                        HasXAxisBar = HasXAxisBar,
                    });

                area_gap.AddSeries(ColumnSeries_GapPercent);
            }
        }

        #endregion Series
    }
}
