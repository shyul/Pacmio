﻿/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
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

namespace Pacmio.Analysis
{
    public sealed class CCI : OscillatorAnalysis
    {
        public CCI(int interval, double constant = 0.015)
        {
            Interval = interval;
            Constant = constant;
            Column_Typical = Bar.Column_Typical;

            Label = "(" + Interval.ToString() + "," + Constant + ")";
            Name = GetType().Name + Label;
            AreaName = GroupName = GetType().Name;
            Description = "Commodity Channel Index " + Label;

            SMA_TP = new SMA(Bar.Column_Typical, interval);
            MDEV_TP = new MDEV(Bar.Column_Typical, SMA_TP);

            SMA_TP.AddChild(MDEV_TP);
            MDEV_TP.AddChild(this);

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
            UpperLimit = 100;
            LowerLimit = -100;

            Color = Color.FromArgb(255, 96, 96, 96);
            UpperColor = Color.Green;
            LowerColor = Color.OrangeRed;
        }

        public int Interval { get; }

        public double Constant { get; }

        #region Calculation

        public NumericColumn Column_Typical { get; }

        public SMA SMA_TP { get; }

        public MDEV MDEV_TP { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                b[Column_Result] = (b[Column_Typical] - b[SMA_TP.Column_Result]) / (Constant * b[MDEV_TP.Column_Result]);
            }
        }

        #endregion Calculation

        #region Series

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