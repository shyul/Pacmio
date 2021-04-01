/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Ref 1: https://www.tradingview.com/scripts/choppinessindex/?solution=43000501980
/// Ref 2: https://www.incrediblecharts.com/indicators/choppiness-index.php
/// Ref 3: https://tradingsim.com/blog/choppiness-index-indicator/
/// 
/// #1 – Buy or Sell the Breakout after extreme Choppiness Index Readings
/// #2 – Ride the Trend using the Choppiness Index Indicator
/// #3 - Trade within Choppy Markets
/// #4 – Walk away from stocks that do not trade nicely with the Choppiness Index Indicator
/// 
/// Noteably: Trend usually starts when it cross down 50.
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
    public class CHOP : BarAnalysis, IOscillator
    {
        public CHOP(int interval = 14)
        {
            PriceChannel = new PriceChannel(interval) { ChartEnabled = false };
            Multiplier = 100 / Math.Log10(interval);

            string label = "(" + Interval.ToString()  + ")";
            Name = GetType().Name + label;
            AreaName = GroupName = GetType().Name;
            Description = "Choppiness Index " + label;

            PriceChannel.AddChild(this);

            Column_Result = new NumericColumn(Name);
            LineSeries = new LineSeries(Column_Result)
            {
                Name = Name,
                Label = label,
                LegendName = GroupName,
                Importance = Importance.Major,
                IsAntialiasing = true,
                DrawLimitShade = true,
            };

            Color = Color.FromArgb(255, 96, 96, 96);
        }

        #region Parameters

        public double Reference { get; set; } = 50;

        public double UpperLimit { get; set; } = 60;

        public double LowerLimit { get; set; } = 40;

        public int Interval => PriceChannel.Interval;

        public double Multiplier { get; }

        #endregion Parameters

        #region Calculation

        //public NumericColumn TR => BarTable.TrueRangeAnalysis.Column_TrueRange;

        public PriceChannel PriceChannel { get; }

        public NumericColumn Column_MaxHigh => PriceChannel.Column_High;

        public NumericColumn Column_MinLow => PriceChannel.Column_Low;

        public NumericColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                double tr_sum = 0;
                for (int j = 0; j < Interval; j++)
                {
                    int k = i - j;
                    if (k < 0) k = 0;
                    if (bt[k] is Bar b_tr)
                        tr_sum += b_tr.TrueRange;
                }

                if (bt[i] is Bar b)
                {
                    double diff = b[Column_MaxHigh] - b[Column_MinLow];
                    b[Column_Result] = (diff > 0) ? Multiplier * Math.Log10(tr_sum / diff) : 0;
                }
            }
        }

        #endregion Calculation

        #region Series

        public Color Color { get => LineSeries.Color; set => LineSeries.Color = value; }

        public float LineWidth { get => LineSeries.Width; set => LineSeries.Width = value; }

        public LineType LineType { get => LineSeries.LineType; set => LineSeries.LineType = value; }

        public Series MainSeries => LineSeries;

        public LineSeries LineSeries { get; protected set; }

        public Color UpperColor { get; set; } = Color.ForestGreen;

        public Color LowerColor { get; set; } = Color.Crimson;

        public virtual bool ChartEnabled { get => Enabled && LineSeries.Enabled; set => LineSeries.Enabled = value; }

        public int DrawOrder { get => LineSeries.Order; set => LineSeries.Order = value; }

        public virtual bool HasXAxisBar { get; set; } = false;

        public string AreaName { get; protected set; }

        public float AreaRatio { get; set; } = 8;

        public int AreaOrder { get; set; } = 0;

        public void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                BarChartOscillatorArea a = bc[AreaName] is BarChartOscillatorArea oa ? oa :
                    bc.AddArea(new BarChartOscillatorArea(bc, AreaName, AreaRatio)
                    {
                        Reference = Reference,
                        UpperLimit = UpperLimit,
                        LowerLimit = LowerLimit,
                        UpperColor = UpperColor,
                        LowerColor = LowerColor,
                        FixedTickStep_Right = 10,
                    });

                a.AddSeries(LineSeries);
            }
        }

        #endregion Series
    }
}