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
    public static class TestGapGo
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

                SMA slow_MA = new SMMA(5) { Color = Color.Orange, LineWidth = 2 };
                SMA fast_MA = new EMA(5) { Color = Color.DodgerBlue, LineWidth = 1 };
                var ma_cross = new MovingAverageCrossIndicator(fast_MA, slow_MA);

                DebugSeries csd_range = new DebugColumnSeries(Bar.Column_Range);
                DebugSeries csd_nr = new DebugColumnSeries(Bar.Column_NarrowRange);
                DebugSeries csd_gp = new DebugColumnSeriesOsc(Bar.Column_GapPercent);
                DebugSeries csd_typ = new DebugLineSeries(Bar.Column_Typical);
                DebugSeries csd_pivot = new DebugColumnSeriesOsc(Bar.Column_Pivot);
                DebugSeries csd_trend = new DebugColumnSeriesOsc(Bar.Column_TrendStrength);

                PositionOfTimeframe potf = new PositionOfTimeframe(BarFreq.Annually);
                DebugSeries csd_potf = new DebugLineSeries(potf);

                List<BarAnalysis> sample_list = new List<BarAnalysis>
                {
                    new PivotLevelAnalysis(),
                    //new RelativeAnalysis(),
                    //new NarrowRange(),

                    new CandleStickDojiMarubozuAnalysis(),
                    new CandleStickShadowStarAnalysis(),

                    //new GainAnalysis(),
                    //new TrueRange(),
                    //new TrendStrength(),
                    //new PivotPointAnalysis(),
                    //new ATR(14),

                    //new ADX(14),
                    //new ULTO(),

                    //aroon,
                    //ma_cross,
                    csd_potf,
                    //new CHOP(),
                    
                    //new RSI(14),
                    //new CCI(20, 0.015),
                    //new MFI(14),
                    //new WaveTrend(10, 21, 4, 0.015) { AreaRatio = 15, HasXAxisBar = true, Order = int.MaxValue },
                    //new MACD(12, 26, 9),


                    //new STO(8, 3, 3) { Order = int.MaxValue - 1 },
                    //new VO() { HasXAxisBar = true, Order = int.MaxValue },


                    //rsi,
                    //new PivotPointAnalysis(rsi, 100),

                    //new TSI(25,13,7),
                    //new Chanderlier(22, 3) { UpperColor = Color.Blue, LowerColor = Color.Plum },
                    //csd_range,
                    //csd_nr,
                    //csd_gp,
                    csd_typ,
                    csd_pivot,
                    csd_trend,

                    volumeEma,
                    //new VWAP(new Frequency(TimeUnit.Days)) { Color = Color.Plum, LineWidth = 2  },

                    new TrendLineAnalysis(260),
                    new GetReversalIndexFromRangeBoundAnalysis(),
                };

                BarAnalysisSet bas = new BarAnalysisSet(sample_list);

                return bas;
            }
        }
    }
}
