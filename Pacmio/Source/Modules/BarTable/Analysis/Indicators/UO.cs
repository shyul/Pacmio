/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Ultimate Oscillator
/// 
/// Ref 1: https://school.stockcharts.com/doku.php?id=technical_indicators:ultimate_oscillator
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
    public class UO : BarAnalysis, IOscillator
    {
        public UO(int interval = 14)
        {
            Interval = interval;

            string label = "(" + Interval.ToString() + ")";
            Name = GetType().Name + label;
            AreaName = GroupName = Name;
            Description = "Choppiness Index " + label;

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

        public int Interval { get; }



        #endregion Parameters

        #region Calculation

        public PriceChannel PriceChannel { get; }

        public NumericColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (bt[i] is Bar b)
                {



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

        public int SeriesOrder { get => LineSeries.Order; set => LineSeries.Order = value; }

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