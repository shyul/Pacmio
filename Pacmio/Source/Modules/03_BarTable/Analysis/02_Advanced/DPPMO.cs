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
    public sealed class DPPMO : IntervalAnalysis, IOscillator
    {
        public DPPMO(int interval) : base(interval)
        {
            SmoothMultiplier = 2 / Interval;
        }

        public double SmoothMultiplier { get; }

        public double Reference { get; set; } = 0;

        public double UpperLimit { get; set; } = 3;

        public double LowerLimit { get; set; } = -3;

        public Color UpperColor { get; set; } = Color.ForestGreen;

        public Color LowerColor { get; set; } = Color.Crimson;

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
