/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// https://school.stockcharts.com/doku.php?id=trading_strategies:narrow_range_day_nr7
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
    public sealed class NarrowRange : BarAnalysis, ISingleData, IChartSeries
    {
        public NarrowRange(int max_interval = 9)
        {
            MaximumInterval = max_interval;

            string label = "(" + MaximumInterval.ToString() + ")";
            Name = GetType().Name + label;
            GroupName = GetType().Name;
            Description = "Narrow Range " + label;

            Column_Result = new NumericColumn(Name, label);

            ColumnSeries = new(Column_Result, Column_Result, 50, 0, 0)
            {
                Name = Name,
                LegendName = "NarrowRange",
                Label = "",
                Importance = Importance.Major,
                Side = AlignType.Right,
                IsAntialiasing = false
            };

            ChartEnabled = true;
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ MaximumInterval;

        public int MaximumInterval { get; }

        public NumericColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                double range = b.High - b.Low;

                int j = 1;
                for (; j <= MaximumInterval; j++)
                {
                    int k = i - j;

                    if (k < 0)
                    {
                        k--;
                        break;
                    }
                    else
                    {
                        Bar b_1 = bt[k];
                        double range_1 = b_1.High - b_1.Low;

                        if (range >= range_1)
                            break;
                    }
                }

                b[Column_Result] = j;
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
                BarChartOscillatorArea area_weight = bc["NarrowRange_" + AreaName] is BarChartOscillatorArea oa ? oa :
                    bc.AddArea(new BarChartOscillatorArea(bc, "NarrowRange_" + AreaName, AreaRatio)
                    {
                        Reference = 0,
                        HasXAxisBar = false,
                    });

                area_weight.AddSeries(ColumnSeries);
            }
        }
    }
}
