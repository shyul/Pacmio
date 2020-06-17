/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Ref 1: https://school.stockcharts.com/doku.php?id=technical_indicators:commodity_channel_index_cci
/// 
/// ***************************************************************************

using System;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public sealed class CCI : ATR, IOscillator
    {
        public CCI(int interval, double constant)
        {
            Interval = interval;
            Constant = constant;

            string label = "(" + Interval.ToString() + ")";
            Name = GetType().Name + label;
            AreaName = GroupName = GetType().Name;
            Description = "Commodity Channel Index " + label;

            SMA_TP = new SMA(Bar.Column_Typical, interval);
            MDEV_TP = new MDEV(Bar.Column_Typical, SMA_TP);

            SMA_TP.AddChild(MDEV_TP);
            MDEV_TP.AddChild(this);

            Result_Column = new NumericColumn(Name) { Label = label };
            LineSeries = new LineSeries(Result_Column)
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
                b[Result_Column] = (b.Typical - b[SMA_TP.Result_Column]) / (Constant * b[MDEV_TP.Result_Column]);
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
                OscillatorArea a = bc[AreaName] is OscillatorArea oa ? oa :
                    bc.AddArea(new OscillatorArea(bc, AreaName, 10)
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