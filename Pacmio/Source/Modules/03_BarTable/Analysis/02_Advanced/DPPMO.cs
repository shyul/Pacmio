/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Ref 1: https://school.stockcharts.com/doku.php?id=technical_indicators:dppmo
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
    public sealed class DPPMO : OscillatorAnalysis
    {
        public DPPMO(int interval)
        {
            Interval = interval;
            SmoothMultiplier = 2 / Interval;

            Label = "(" + Interval.ToString() + ")";
            Name = GetType().Name + Label;
            AreaName = GroupName = GetType().Name;
            Description = "Price Momentum Oscillator " + Label;

            Column_Signal = new NumericColumn(Name + "_Signal", Label);
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

            Reference = 0;
            UpperLimit = 3;
            LowerLimit = -3;

            Color = Color.FromArgb(255, 96, 96, 96);
            UpperColor = Color.ForestGreen;
            LowerColor = Color.Crimson;
        }

        public override string Label { get; }

        public int Interval { get; }

        public double SmoothMultiplier { get; }

        public NumericColumn Column_Signal { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            double last_cema35 = bap.StartPt > 0 ? bt[bap.StartPt - 1][Column_Result] / 10 : bt[0].GainPercent;
            double last_signal = bap.StartPt > 0 ? bt[bap.StartPt - 1][Column_Signal] : bt[0].GainPercent;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                last_cema35 = (b.GainPercent * SmoothMultiplier) + (last_cema35 * (1 - SmoothMultiplier));
                double result = b[Column_Result] = last_cema35 * 10;
                last_signal = b[Column_Signal] = ((result - last_signal) * (2 / 20)) + last_signal;
            }
        }
    }
}
