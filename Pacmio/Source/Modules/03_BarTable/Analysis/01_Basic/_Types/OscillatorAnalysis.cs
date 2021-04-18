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
    public abstract class OscillatorAnalysis : SingleDataAnalysis, IOscillator
    {
        public double Reference { get; set; } = 50;

        public double UpperLimit { get; set; } = 80;

        public double LowerLimit { get; set; } = 20;

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
