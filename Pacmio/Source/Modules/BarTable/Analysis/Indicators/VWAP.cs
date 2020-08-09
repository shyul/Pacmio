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

namespace Pacmio
{
    public sealed class VWAP : BarAnalysis, ISingleData, IChartSeries
    {
        public VWAP(Frequency frequency)
        {
            Frequency = frequency;

            string label = "(" + Frequency.ToString() + ")";
            Name = GetType().Name + label;
            GroupName = GetType().Name;
            Description = "Volume Weighted Average Price " + label;

            Volume_Column = new NumericColumn(Name + "_CUMUL_V");
            Volume_Price_Column = new NumericColumn(Name + "_CUMUL_VP");
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

        public NumericColumn Volume_Column { get; }

        public NumericColumn Volume_Price_Column { get; }

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
                    cumulative_vol = b_1[Volume_Column];
                    cumulative_price_vol = b_1[Volume_Price_Column];
                    //base_time = b_1.Time;
                    base_time = Frequency.Align(b_1.Time);
                }


                for (int i = bap.StartPt; i < bap.StopPt; i++)
                {
                    Bar b = bt[i];
                    //DateTime time = b.Time;
                    DateTime time = Frequency.Align(b.Time);

                    //if (time.Day != base_time.Day)
                    if (time != base_time)
                    {
                        cumulative_vol = 0;
                        cumulative_price_vol = 0;
                    }

                    double volumn = b.Volume;
                    b[Volume_Column] = cumulative_vol += volumn;
                    b[Volume_Price_Column] = cumulative_price_vol += volumn * b.Typical;
                    b[Column_Result] = cumulative_price_vol / cumulative_vol;
                    base_time = time;
                }
            }
        }

        #endregion Calculation

        #region Series

        public Color Color { get => LineSeries.Color; set => LineSeries.Color = LineSeries.ShadeColor = value; }

        public float LineWidth { get => LineSeries.Width; set => LineSeries.Width = value; }

        public LineType LineType { get => LineSeries.LineType; set => LineSeries.LineType = value; }

        public Series MainSeries => LineSeries;

        public LineSeries LineSeries { get; }

        public bool ChartEnabled { get => Enabled && LineSeries.Enabled; set => LineSeries.Enabled = value; }

        public int SeriesOrder { get => LineSeries.Order; set => LineSeries.Order = value; }

        public bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; } = MainArea.DefaultName;

        public float AreaRatio { get; set; } = 12;

        public void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                Area a = bc.AddArea(new Area(bc, AreaName, AreaRatio)
                {
                    HasXAxisBar = HasXAxisBar,
                });
                a.AddSeries(LineSeries);
            }
        }

        #endregion Series
    }
}
