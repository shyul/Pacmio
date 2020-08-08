﻿/// ***************************************************************************
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
    public class TrueRange : BarAnalysis, IChartSeries
    {
        public TrueRange()
        {
            Column_High = Bar.Column_High;
            Column_Low = Bar.Column_Low;
            Column_Close = Bar.Column_Close;

            string label = "(" + Column_High.Name + "," + Column_Low.Name + "," + Column_Close.Name + ")";
            GroupName = Name = GetType().Name + label;
            Description = Name + " " + label;
            AreaName = label;

            Column_TrueRange = new NumericColumn(Name) { Label = label };
            Column_Typical = new NumericColumn("Typical" + label) { Label = label };

            ColumnSeries_TrueRange = new ColumnSeries(Column_TrueRange, Color.FromArgb(88, 168, 208), Color.FromArgb(32, 104, 136), 50)
            {
                Name = Name,
                LegendName = GroupName + ": ",
                Label = GetType().Name,
                Importance = Importance.Major,
                Side = AlignType.Right,
                IsAntialiasing = false,
                Order = 200
            };

            ColumnSeries_Typical = new ColumnSeries(Column_Typical, Color.FromArgb(88, 168, 208), Color.FromArgb(32, 104, 136), 50)
            {
                Name = "Typical " + label,
                LegendName = "Typical " + label,
                Label = "Typical",
                Importance = Importance.Major,
                Side = AlignType.Right,
                IsAntialiasing = false,
                Order = 200
            };
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ Column_High.GetHashCode() ^ Column_Low.GetHashCode() ^ Column_Close.GetHashCode();

        #region Calculation

        public NumericColumn Column_High { get; }

        public NumericColumn Column_Low { get; }

        public NumericColumn Column_Close { get; }

        public NumericColumn Column_TrueRange { get; }

        public NumericColumn Column_Typical { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            double close_1;

            if (bap.StartPt < 1)
            {
                Bar b = bt[0];
                if (bap.StartPt < 0) bap.StartPt = 0;
                close_1 = b[Column_Close];
            }
            else
            {
                Bar b_1 = bt[bap.StartPt - 1];
                close_1 = b_1[Column_Close];
            }

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                double high = b[Column_High];
                double low = b[Column_Low];
                double close = b[Column_Close];

                double[] list = new double[] { (high - low), Math.Abs(high - close_1), Math.Abs(low - close_1) };

                b[Column_TrueRange] = list.Max();
                b[Column_Typical] = (high + low + close) / 3.0;

                close_1 = close;
            }
        }

        #endregion Calculation

        #region Series

        public Color Color
        {
            get => ColumnSeries_TrueRange.Color;
            set
            {
                ColumnSeries_TrueRange.Color = value;
                ColumnSeries_TrueRange.ShadeColor = value;
                ColumnSeries_Typical.Color = value;
                ColumnSeries_Typical.ShadeColor = value;
            }
        }

        public ColumnSeries ColumnSeries_TrueRange { get; }

        public ColumnSeries ColumnSeries_Typical { get; }

        public bool ChartEnabled { get => Enabled && ColumnSeries_TrueRange.Enabled; set => ColumnSeries_TrueRange.Enabled = ColumnSeries_Typical.Enabled = value; }

        public int SeriesOrder
        {
            get => ColumnSeries_TrueRange.Order; set
            {
                ColumnSeries_TrueRange.Order = value;
                ColumnSeries_Typical.Order = value + 1;
            }
        }

        public bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; }

        public float AreaRatio { get; set; } = 8;

        public void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                Area truerange_area = bc.AddArea(new Area(bc, "TrueRange" + AreaName, AreaRatio)
                {
                    HasXAxisBar = false,
                });
                truerange_area.AddSeries(ColumnSeries_TrueRange);

                Area typical_area = bc.AddArea(new Area(bc, "Typical" + AreaName, AreaRatio)
                {
                    HasXAxisBar = HasXAxisBar,
                });
                typical_area.AddSeries(ColumnSeries_Typical);
            }
        }

        #endregion Series
    }
}
