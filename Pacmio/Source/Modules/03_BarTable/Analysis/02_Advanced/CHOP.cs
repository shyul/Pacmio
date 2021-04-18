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
    public class CHOP : OscillatorAnalysis
    {
        public CHOP(int interval = 14)
        {
            PriceChannel = new PriceChannel(interval) { ChartEnabled = false };
            Multiplier = 100 / Math.Log10(interval);

            Label = "(" + Interval.ToString() + ")";
            Name = GetType().Name + Label;
            AreaName = GroupName = GetType().Name;
            Description = "Choppiness Index " + Label;

            PriceChannel.AddChild(this);

            Column_Result = new NumericColumn(Name, Label);
            LineSeries = new LineSeries(Column_Result)
            {
                Name = Name,
                Label = Label,
                LegendName = GroupName,
                Importance = Importance.Major,
                IsAntialiasing = true,
                DrawLimitShade = true,
            };

            Reference = 50;
            UpperLimit = 60;
            LowerLimit = 40;

            Color = Color.FromArgb(255, 96, 96, 96);
        }

        #region Parameters

        public override string Label { get; }

        public int Interval => PriceChannel.Interval;

        public double Multiplier { get; }

        #endregion Parameters

        #region Calculation

        public PriceChannel PriceChannel { get; }

        public NumericColumn Column_MaxHigh => PriceChannel.Column_High;

        public NumericColumn Column_MinLow => PriceChannel.Column_Low;

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
    }
}