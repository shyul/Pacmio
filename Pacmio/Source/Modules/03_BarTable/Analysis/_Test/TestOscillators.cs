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
    public static class TestOscillators
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

                var rsi = new RSI(14)
                {
                    UpperColor = Color.Blue,
                    LowerColor = Color.DarkOrange,
                    AreaRatio = 8,
                    HasXAxisBar = true,
                    Order = int.MaxValue - 1,
                    AreaOrder = int.MaxValue - 10
                };

                var aroon = new Aroon() { HasXAxisBar = true, Order = int.MaxValue };
                //aroon.LineSeries.Enabled = false;

                List<BarAnalysis> sample_list = new()
                {
                    //new Bollinger(20, 2.0),
                    //new ATR(14),
                    new ULTO(),
                    //aroon,
                    //new HMA(10) { Color = Color.Plum },
                    //new HMA(20) { Color = Color.Green },
                    //new SMA(20) { Color = Color.Orange },

                    //new MACD(12, 26, 9),
                    //new STO(8, 3, 3) { Order = int.MaxValue - 1 },
                    //new VO() { HasXAxisBar = true, Order = int.MaxValue },
                    //new Chanderlier(22, 3) { UpperColor = Color.Blue, LowerColor = Color.Plum },
                    
                    //new ADX(14),
                    new CCI(20, 0.015),
                    new CHOP(),
                    new MFI(14),
                    rsi,
                    //new TSI(25,13,7),
                    //new VWAP(BarFreq.Annually) { Color = Color.Plum, LineWidth = 2  },
                    new WaveTrend(10, 21, 4, 0.015) { AreaRatio = 15, HasXAxisBar = true, Order = int.MaxValue },
                    volumeEma,
                };

                BarAnalysisSet bas = new(sample_list);

                return bas;
            }
        }
    }
}
