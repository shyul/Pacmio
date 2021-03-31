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
    public static class TestReversal
    {
        public static BarAnalysisSet BarAnalysisSet
        {
            get
            {
                var volumeEma = new EMA(Bar.Column_Volume, 20) { Color = Color.DeepSkyBlue, LineWidth = 2 };
                volumeEma.LineSeries.Side = AlignType.Left;
                volumeEma.LineSeries.Label = volumeEma.GetType().Name + "(" + volumeEma.Interval.ToString() + ")";
                volumeEma.LineSeries.LegendName = "VOLUME";
                volumeEma.LineSeries.LegendLabelFormat = "0.##";

                var rvol = new Relative(Bar.Column_Volume, volumeEma);

                DebugSeries csd_rvol = new DebugColumnSeries(rvol.Column_Result);

                var rsi = new RSI(5)
                {
                    UpperColor = Color.Blue,
                    LowerColor = Color.DarkOrange,
                    AreaRatio = 8,
                    HasXAxisBar = true,
                    Order = int.MaxValue - 1,
                    AreaOrder = int.MaxValue - 10
                };

                List<BarAnalysis> sample_list = new()
                {
                    rvol,
                    csd_rvol,

                    new CandleStickDojiMarubozuSignal(),
                    new CandleStickShadowStarSignal(),

                    new Bollinger(20, 2),
                    rsi,

                    new VWAP(BarFreq.Daily) { Color = Color.Plum, LineWidth = 2  },
                };

                BarAnalysisSet bas = new(sample_list);

                return bas;
            }
        }
    }
}
