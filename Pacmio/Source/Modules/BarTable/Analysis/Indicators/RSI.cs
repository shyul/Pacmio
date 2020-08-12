/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
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

namespace Pacmio
{
    public sealed class RSI : ATR, IOscillator
    {
        public RSI(int interval, double lowerLimit = 30, double upperLimit = 70)
        {
            Interval = interval;
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;

            string label = "(" + Interval.ToString() + ")";
            Name = GetType().Name + label;
            AreaName = GroupName = GetType().Name;
            Description = "Relative Strength Index " + label;

            AverageGain = new AverageGain(Interval) { ChartEnabled = false };
            AverageGain.AddChild(this);

            AverageLoss = new AverageLoss(Interval) { ChartEnabled = false };
            AverageLoss.AddChild(this);

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

        #region Calculation

        public double Reference { get; set; } = 50;

        public double UpperLimit { get; set; } = 70;

        public double LowerLimit { get; set; } = 30;

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

        #region Series

        public Color UpperColor { get; set; } = Color.ForestGreen;

        public Color LowerColor { get; set; } = Color.Crimson;

        public override void ConfigChart(BarChart bc)
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
