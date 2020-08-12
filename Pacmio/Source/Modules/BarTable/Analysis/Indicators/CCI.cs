/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Ref 1: https://school.stockcharts.com/doku.php?id=technical_indicators:commodity_channel_index_cci
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
    public sealed class CCI : ATR, IOscillator
    {
        public CCI(int interval, double constant = 0.015)
        {
            Interval = interval;
            Constant = constant;

            string label = "(" + Interval.ToString() + ")";
            Name = GetType().Name + label;
            AreaName = GroupName = GetType().Name;
            Description = "Commodity Channel Index " + label;

            SMA_TP = new SMA(BarTable.TrueRangeAnalysis.Column_Typical, interval);
            MDEV_TP = new MDEV(BarTable.TrueRangeAnalysis.Column_Typical, SMA_TP);

            SMA_TP.AddChild(MDEV_TP);
            MDEV_TP.AddChild(this);

            Column_Result = new NumericColumn(Name) { Label = label };
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

        public double Constant { get; }

        #region Calculation

        public double Reference { get; set; } = 0;

        public double UpperLimit { get; set; } = 100;

        public double LowerLimit { get; set; } = -100;

        public SMA SMA_TP { get; }

        public MDEV MDEV_TP { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                b[Column_Result] = (b.Typical - b[SMA_TP.Column_Result]) / (Constant * b[MDEV_TP.Column_Result]);
            }
        }

        #endregion Calculation

        #region Series

        public Color UpperColor { get; set; } = Color.Green;

        public Color LowerColor { get; set; } = Color.OrangeRed;

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
                        FixedTickStep_Right = 50,
                    });

                a.AddSeries(LineSeries);
            }
        }

        #endregion Series
    }
}