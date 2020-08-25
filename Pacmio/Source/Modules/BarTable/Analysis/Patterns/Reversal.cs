﻿/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public class Reversal : BarAnalysis, IChartSeries
    {
        public Reversal()
        {
            GroupName = Name = GetType().Name;
            Description = Name;
            AreaName = Name;

            Column_Weight = new NumericColumn(Name + "_Weight") { Label = "Weight" };
            Column_TotalWeight = new NumericColumn(Name + "_TotalWeight") { Label = "TotalWeight" };

            ColumnSeries_Weight = new AdColumnSeries(Column_Weight, Column_Weight, 50, 0, 0)
            {
                Name = Name,
                LegendName = "Weight",
                Label = "",
                Importance = Importance.Major,
                Side = AlignType.Right,
                IsAntialiasing = false
            };

            ColumnSeries_TotalWeight = new AdColumnSeries(Column_TotalWeight, Column_TotalWeight, 50, 0, 0)
            {
                Name = Name,
                LegendName = "TotalWeight",
                Label = "",
                Importance = Importance.Major,
                Side = AlignType.Right,
                IsAntialiasing = false
            };

            UpperColor = Color.DodgerBlue;
            LowerColor = Color.MediumVioletRed;

            CalculatePivotRange.AddChild(this);
        }

        public CalculatePivotRange CalculatePivotRange { get; } = new CalculatePivotRange();

        public ISingleData SourceAnalysis { get; protected set; }

        /// <summary>
        /// Absolute Weight
        /// </summary>
        public NumericColumn Column_Weight { get; protected set; }

        /// <summary>
        /// Total movement weight
        /// </summary>
        public NumericColumn Column_TotalWeight { get; protected set; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            if (bap.StartPt < 0)
                bap.StartPt = 0;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (bt[i] is Bar b)
                {
                    if (SourceAnalysis is null && b.GetPivotRangeDatum() is PivotRangeDatum prd)
                    {
                        double open = b.Open;
                        double close = b.Close;
                        double high = b.High;
                        double low = b.Low;

                        // Range intersection
                        // Cross up is plus, cross down is negative
                        double w_gap = bt[i - 1] is Bar b_1 ? prd.Weight(b_1.Close, open) : 0; // gap up or down

                        if (close >= open) // Rising candle
                        {
                            double ol = prd.Weight(open, low);
                            double lh = prd.Weight(low, high);
                            double hc = prd.Weight(high, close);
                            b[Column_Weight] = w_gap + ol + lh + hc;
                            b[Column_TotalWeight] = Math.Abs(w_gap) + Math.Abs(ol) + Math.Abs(lh) + Math.Abs(hc);
                        }
                        else
                        {
                            double oh = prd.Weight(open, high);
                            double hl = prd.Weight(high, low);
                            double lc = prd.Weight(low, close);
                            b[Column_Weight] = w_gap + oh + hl + lc;
                            b[Column_TotalWeight] = Math.Abs(w_gap) + Math.Abs(oh) + Math.Abs(hl) + Math.Abs(lc);
                        }

                        // Abs the cross up and cross down is get its total travelling length

                        // Less to zero sum but large distance means reversal

                        // Large sum and large abs sums means penitration
                    }
                }

            }
        }

        #region Series

        public Color Color { get => UpperColor; set => UpperColor = value; }

        public Color UpperColor
        {
            get => ColumnSeries_Weight.Color;

            set
            {
                Color c = value;
                ColumnSeries_Weight.Color = ColumnSeries_TotalWeight.Color = c.GetBrightness() < 0.6 ? c.Brightness(0.85f) : c.Brightness(-0.85f);
                ColumnSeries_Weight.EdgeColor = ColumnSeries_Weight.TextTheme.ForeColor = c;
                ColumnSeries_TotalWeight.EdgeColor = ColumnSeries_TotalWeight.TextTheme.ForeColor = c;
            }
        }

        public Color LowerColor
        {
            get => ColumnSeries_Weight.LowerColor;

            set
            {
                Color c = value;
                ColumnSeries_Weight.LowerColor = ColumnSeries_TotalWeight.LowerColor = c.GetBrightness() < 0.6 ? c.Brightness(0.85f) : c.Brightness(-0.85f);
                ColumnSeries_Weight.LowerEdgeColor = ColumnSeries_Weight.LowerTextTheme.ForeColor = c;
                ColumnSeries_TotalWeight.LowerEdgeColor = ColumnSeries_TotalWeight.LowerTextTheme.ForeColor = c;
            }
        }

        public Series MainSeries => ColumnSeries_Weight;

        public AdColumnSeries ColumnSeries_Weight { get; }

        public AdColumnSeries ColumnSeries_TotalWeight { get; }

        public bool ChartEnabled
        {
            get => Enabled && ColumnSeries_Weight.Enabled;
            set => ColumnSeries_Weight.Enabled = ColumnSeries_TotalWeight.Enabled = value;
        }

        public int SeriesOrder
        {
            get => ColumnSeries_Weight.Order;

            set
            {
                ColumnSeries_Weight.Order = value;
                ColumnSeries_TotalWeight.Order = value + 20;
            }
        }

        public bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; }

        public float AreaRatio { get; set; } = 8;

        public void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                BarChartOscillatorArea area_trend = bc["Weight_" + AreaName] is BarChartOscillatorArea oa ? oa :
                    bc.AddArea(new BarChartOscillatorArea(bc, "TotalWeight_" + AreaName, AreaRatio)
                    {
                        Reference = 0,
                        HasXAxisBar = false,
                    });

                area_trend.AddSeries(ColumnSeries_Weight);

                BarChartOscillatorArea area_gap = bc["GapPercent_" + AreaName] is BarChartOscillatorArea oa_gap ? oa_gap :
                    bc.AddArea(new BarChartOscillatorArea(bc, "GapPercent_" + AreaName, AreaRatio)
                    {
                        Reference = 0,
                        HasXAxisBar = HasXAxisBar,
                    });

                area_gap.AddSeries(ColumnSeries_TotalWeight);
            }
        }

        #endregion Series
    }
}