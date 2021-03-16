/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// For example, calculate relative volume
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public sealed class RelativeAnalysis : BarAnalysis, ISingleData, IChartSeries
    {
        public RelativeAnalysis() : this(Bar.Column_Volume) { }

        public RelativeAnalysis(NumericColumn column)
        {
            Column = column;

            string label = (Column is null) ? "(error)" : ((Column == Bar.Column_Volume) ? "(Volume)" : "(" + Column.Name + ")");
            Name = GetType().Name + label;
            GroupName = (Column == Bar.Column_Close) ? GetType().Name : GetType().Name + " (" + Column.Name + ")";
            Description = "Simple Moving Average " + label;

            Column_Result = new NumericColumn(Name) { Label = label };

            ColumnSeries = new(Column_Result, Column_Result, 50, 0, 0)
            {
                Name = Name,
                LegendName = "Weight",
                Label = "",
                Importance = Importance.Major,
                Side = AlignType.Right,
                IsAntialiasing = false
            };

            ChartEnabled = true;
        }

        public NumericColumn Column { get; private set; }

        public NumericColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            int startPt = bap.StartPt;
            if (startPt == 0) startPt = 1;

            for (int i = startPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                double v = b[Column];
                double v_1 = bt[i - 1][Column];

                if (v_1 != 0)
                    b[Column_Result] = v / v_1;
                else
                    continue;
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

        public bool ChartEnabled
        {
            get => Enabled && ColumnSeries.Enabled;
            set => ColumnSeries.Enabled = value;
        }

        public int SeriesOrder
        {
            get => ColumnSeries.Order;
            set => ColumnSeries.Order = value;
        }

        public bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; }

        public float AreaRatio { get; set; } = 8;

        public void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                BarChartOscillatorArea area_weight = bc["Weight_" + AreaName] is BarChartOscillatorArea oa ? oa :
                    bc.AddArea(new BarChartOscillatorArea(bc, "Weight_" + AreaName, AreaRatio)
                    {
                        Reference = 0,
                        HasXAxisBar = false,
                    });

                area_weight.AddSeries(ColumnSeries);
            }
        }
    }
}
