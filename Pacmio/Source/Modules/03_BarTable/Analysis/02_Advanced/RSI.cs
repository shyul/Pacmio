/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// RSI
/// 1. Over bought and over sold levels
/// 2. Trend Condition
/// 3. Showing Divergence
/// 
/// Ref 1:
/// Ref 2: https://school.stockcharts.com/doku.php?id=trading_strategies:rsi2
/// Ref 2: https://school.stockcharts.com/doku.php?id=technical_indicators:introduction_to_technical_indicators_and_oscillators
/// Ref 3: https://www.udemy.com/course/advance-trading-strategies/learn/lecture/9783962#overview
/// Ref 4: https://youtu.be/fJ1_un39T1o  
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
    public sealed class RSI : OscillatorAnalysis
    {
        public RSI(int interval, double lowerLimit = 20, double upperLimit = 80)
        {
            Interval = interval;
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;

            Label = "(" + Interval.ToString() + ")";
            Name = GetType().Name + Label;
            AreaName = GroupName = GetType().Name;
            Description = "Relative Strength Index " + Label;

            AverageGain = new AverageGain(Interval) { ChartEnabled = false };
            AverageGain.AddChild(this);

            AverageLoss = new AverageLoss(Interval) { ChartEnabled = false };
            AverageLoss.AddChild(this);

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
            UpperLimit = 80;
            LowerLimit = 20;

            Color = Color.FromArgb(255, 96, 96, 96);
            UpperColor = Color.ForestGreen;
            LowerColor = Color.Crimson;
        }

        public override string Label { get; }

        #region Calculation

        public int Interval { get; }

        public AverageGain AverageGain { get; }

        public AverageLoss AverageLoss { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                double avgLoss = b[AverageLoss.Column_Result];
                b[Column_Result] = (avgLoss != 0) ? 100 - 100 / (1 + b[AverageGain.Column_Result] / avgLoss) : 100;
            }
        }

        #endregion Calculation
    }
}
