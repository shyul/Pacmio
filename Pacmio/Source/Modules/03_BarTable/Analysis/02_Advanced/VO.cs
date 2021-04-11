/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Volume Oscillator
/// 
/// Ref 1: https://www.fidelity.com/learning-center/trading-investing/technical-analysis/technical-indicator-guide/volume-oscillator
/// Ref 2: 
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
    public class VO : BarAnalysis, IOscillator
    {
        public VO(int interval_fast = 14, int interval_slow = 28)
        {
            Fast_MA = new EMA(Bar.Column_Volume, interval_fast) { ChartEnabled = false };
            Slow_MA = new EMA(Bar.Column_Volume, interval_slow) { ChartEnabled = false };

            string label = "(" + Interval_Fast.ToString() + "," + Interval_Slow.ToString() + ")";
            Name = GetType().Name + label;
            AreaName = GroupName = GetType().Name;
            Description = "Volume Oscillator " + label;

            Fast_MA.AddChild(this);
            Slow_MA.AddChild(this);

            Column_Result = new NumericColumn(Name);
            LineSeries = new LineSeries(Column_Result)
            {
                Name = this.Name,
                Label = label,
                LegendName = GroupName,
                Importance = Importance.Major,
                IsAntialiasing = true,
                DrawLimitShade = true,
            };

            Color = Color.FromArgb(255, 96, 96, 96);
        }

        #region Parameters

        public double Reference { get; set; } = 0;

        public double UpperLimit { get; set; } = 15;

        public double LowerLimit { get; set; } = -15;

        public int Interval_Fast => Fast_MA.Interval;

        public int Interval_Slow => Slow_MA.Interval;

        #endregion Parameters

        #region Calculation

        public MovingAverage Fast_MA { get; }

        public MovingAverage Slow_MA { get; }

        public NumericColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (bt[i] is Bar b)
                {
                    double fast_ma = b[Fast_MA.Column_Result];
                    double slow_ma = b[Slow_MA.Column_Result];
                    b[Column_Result] = 100 * (fast_ma - slow_ma) / slow_ma;
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