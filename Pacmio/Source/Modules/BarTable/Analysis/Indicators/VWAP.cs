/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Ref 1: https://school.stockcharts.com/doku.php?id=technical_indicators:true_strength_index
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
    public sealed class VWAP : BarAnalysis, ISingleData, IChartSeries
    {
        public VWAP(Frequency frequency)
        {
            Frequency = frequency;
            Column_Typical = BarTable.TrueRangeAnalysis.Column_Typical;

            string label = "(" + Frequency.ToString() + ")";
            Name = GetType().Name + label;
            GroupName = GetType().Name;
            Description = "Volume Weighted Average Price " + label;

            Column_Volume = new NumericColumn(Name + "_CUMUL_V");
            Column_Volume_Price = new NumericColumn(Name + "_CUMUL_VP");
            Column_Result = new NumericColumn(Name) { Label = label };
            LineSeries = new LineSeries(Column_Result)
            {
                Name = Name,
                LegendName = GroupName,
                Label = label,
                IsAntialiasing = true,
                DrawLimitShade = false
            };
        }

        #region Parameters

        public override int GetHashCode() => GetType().GetHashCode() ^ Frequency.GetHashCode();

        /// <summary>
        /// The time period for each average frame
        /// </summary>
        public Frequency Frequency { get; }

        #endregion Parameters

        #region Calculation

        public NumericColumn Column_Typical { get; }

        public NumericColumn Column_Volume { get; }

        public NumericColumn Column_Volume_Price { get; }

        public NumericColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            if (bt.Frequency.Span < Frequency.Span)
            {
                double cumulative_vol = 0;
                double cumulative_price_vol = 0;
                DateTime base_time = DateTime.MinValue;

                if (bap.StartPt > 0)
                {
                    Bar b_1 = bt[bap.StartPt - 1];
                    cumulative_vol = b_1[Column_Volume];
                    cumulative_price_vol = b_1[Column_Volume_Price];
                    base_time = Frequency.Align(b_1.Time);
                }

                for (int i = bap.StartPt; i < bap.StopPt; i++)
                {
                    Bar b = bt[i];
                    DateTime time = Frequency.Align(b.Time);

                    if (time != base_time)
                    {
                        cumulative_vol = 0;
                        cumulative_price_vol = 0;
                    }

                    double volumn = b.Volume;
                    b[Column_Volume] = cumulative_vol += volumn;
                    b[Column_Volume_Price] = cumulative_price_vol += volumn * b[Column_Typical];
                    b[Column_Result] = cumulative_price_vol / cumulative_vol;
                    base_time = time;
                }
            }
        }

        #endregion Calculation

        #region Series

        public Color Color { get => LineSeries.Color; set => LineSeries.Color = LineSeries.EdgeColor = value; }

        public float LineWidth { get => LineSeries.Width; set => LineSeries.Width = value; }

        public LineType LineType { get => LineSeries.LineType; set => LineSeries.LineType = value; }

        public Series MainSeries => LineSeries;

        public LineSeries LineSeries { get; }

        public bool ChartEnabled { get => Enabled && LineSeries.Enabled; set => LineSeries.Enabled = value; }

        public int SeriesOrder { get => LineSeries.Order; set => LineSeries.Order = value; }

        public bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; } = MainBarChartArea.DefaultName;

        public float AreaRatio { get; set; } = 12;

        public void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                BarChartArea a = bc.AddArea(new BarChartArea(bc, AreaName, AreaRatio)
                {
                    HasXAxisBar = HasXAxisBar,
                });
                a.AddSeries(LineSeries);
            }
        }

        #endregion Series
    }
}
