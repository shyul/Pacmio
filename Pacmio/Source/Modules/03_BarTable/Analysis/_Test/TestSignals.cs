/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public static class TestSignals
    {
        public static BarAnalysisList BarAnalysisSet
        {
            get
            {
                var volumeEma = new EMA(Bar.Column_Volume, 20) { Color = Color.DeepSkyBlue, LineWidth = 2 };
                volumeEma.LineSeries.Side = AlignType.Left;
                volumeEma.LineSeries.Label = volumeEma.GetType().Name + "(" + volumeEma.Interval.ToString() + ")";
                volumeEma.LineSeries.LegendName = "VOLUME";
                volumeEma.LineSeries.LegendLabelFormat = "0.##";

                MovingAverageAnalysis slow_MA = new SMMA(5) { Color = Color.Orange, LineWidth = 2 };
                MovingAverageAnalysis fast_MA = new EMA(5) { Color = Color.DodgerBlue, LineWidth = 1 };




                var twrc = new SwingTradeDailyFilter();

                var chop = new CHOP(20);

                List<BarAnalysis> sample_list = new()
                {

                    chop
                };

                BarAnalysisList bat = new(sample_list);

                return bat;


            }
        }



    }
}
