/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
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
    public abstract class ConvergeDivergeAnalysis : SingleDataAnalysis
    {
        public MovingAverageAnalysis Fast_MA { get; }

        public MovingAverageAnalysis Slow_MA { get; }

        public MovingAverageAnalysis MACD_SL { get; }

        #region Series

        public double Reference { get; set; } = 0;

        public double UpperLimit { get; set; } = double.NaN;

        public double LowerLimit { get; set; } = double.NaN;

        public Color UpperColor { get; set; } = Color.Green;

        public Color LowerColor { get; set; } = Color.OrangeRed;

        public ColumnSeries ColumnSeries { get; }

        public LineSeries LineSeries_SL { get; }

        public override bool ChartEnabled { get => Enabled && LineSeries.Enabled; set => ColumnSeries.Enabled = LineSeries.Enabled = value; }

        public override void ConfigChart(BarChart bc)
        {
            if (ChartEnabled)
            {
                BarChartOscillatorArea a = bc[AreaName] is BarChartOscillatorArea oa ? oa :
                    bc.AddArea(new BarChartOscillatorArea(bc, AreaName, AreaRatio)
                    {
                        Order = AreaOrder,
                        Reference = Reference,
                        UpperLimit = UpperLimit,
                        LowerLimit = LowerLimit,
                        UpperColor = UpperColor,
                        LowerColor = LowerColor,
                        //FixedTickStep_Right = 10,
                    });
                a.AddSeries(ColumnSeries);
                a.AddSeries(LineSeries);
                a.AddSeries(LineSeries_SL);
            }
        }

        #endregion Series
    }
}
