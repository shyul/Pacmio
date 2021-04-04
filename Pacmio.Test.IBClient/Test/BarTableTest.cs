using Pacmio;
using System.Collections.Generic;
using System.Drawing;
using Xu;
using Pacmio.Analysis;

namespace TestClient
{
    public static class BarTableTest
    {
        //public static BarTableSet BarTableSet { get; } = new BarTableSet();

        public static BarAnalysisSet TestBarAnalysisSet
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
                //var ma_cross = new CrossIndicator(fast_MA, slow_MA);

                DebugColumnSeries csd_range = new(Bar.Column_Range);
                DebugColumnSeries csd_nr = new(Bar.Column_NarrowRange);
                DebugColumnSeriesOsc csd_gp = new(Bar.Column_GapPercent);
                DebugColumnSeries csd_typ = new(Bar.Column_Typical);
                DebugColumnSeriesOsc csd_pivot = new(Bar.Column_Pivot);
                DebugColumnSeriesOsc csd_trend = new(Bar.Column_TrendStrength);

                List<BarAnalysis> sample_list = new()
                {
                    new PivotLevelAnalysis(),
                    //new RelativeAnalysis(),
                    //new NarrowRange(),

                    new CandleStickDojiMarubozuSignal(),
                    new CandleStickShadowStarSignal(),

                    //new GainAnalysis(),
                    //new TrueRange(),
                    //new TrendStrength(),
                    //new PivotPointAnalysis(),
                    //new ATR(14),

                    //new ADX(14),
                    //new ULTO(),

                    //aroon,
                    //ma_cross,

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
                };

                BarAnalysisSet bas = new(sample_list);

                return bas;
            }
        }

        public static BarAnalysisSet TestBarAnalysisSet2
        {
            get
            {
                var volumeEma = new EMA(Bar.Column_Volume, 20) { Color = Color.DeepSkyBlue, LineWidth = 2 };
                volumeEma.LineSeries.Side = AlignType.Left;
                volumeEma.LineSeries.Label = volumeEma.GetType().Name + "(" + volumeEma.Interval.ToString() + ")";
                volumeEma.LineSeries.LegendName = "VOLUME";
                volumeEma.LineSeries.LegendLabelFormat = "0.##";

                //var ema5_smma5_cross = new DualData(new EMA(5) { Color = Color.Teal }, new EMA(13) { Color = Color.Peru });
                //var mfi = new MFI(14) { Order = 99 };
                var rsi = new RSI(14) { AreaRatio = 8, HasXAxisBar = true, Order = int.MaxValue - 1, AreaOrder = int.MaxValue - 10 };
                //var divergence = new Divergence(rsi);
                //var indicator_reference_cross = new ConstantData(rsi, new Range<double>(49, 51));

                //var boll = new Bollinger(20, 2.0);

                //SMA slow_MA = new EMA(50) { Color = Color.YellowGreen, LineWidth = 2 };
                //SMA fast_MA = new EMA(25) { Color = Color.DodgerBlue, LineWidth = 1 };
                SMA slow_MA = new SMMA(5) { Color = Color.Orange, LineWidth = 2 };
                SMA fast_MA = new EMA(5) { Color = Color.DodgerBlue, LineWidth = 1 };
                //var ma_cross = new MovingAverageCrossIndicator(fast_MA, slow_MA) { Order = int.MinValue + 10 };
                //var ma_cross = new MovingAverageCrossIndicator(MovingAverageType.Exponential, 25, MovingAverageType.Exponential, 50);

                List<BarAnalysis> sample_list = new()
                {                        
                    //mfi,
                    
                    volumeEma,
                    //ema5,
                    //smma5,
                    //ema5_smma5_cross,
               
                    //new BoxRange(14),
                    ///new SMA(20) { Color = Color.Teal.Opaque(50), LineWidth = 10 },
                    ///new Bollinger(20, 2.0),
                    //new Chanderlier(22, 3) { Color = Color.Blue, Low_Color = Color.Plum },
                    //rsi,
   
                    //new SMA(5) { Color = Color.Blue },
                    //new SMA(20) { Color = Color.Green },
                    //new SMA(50) { Color = Color.Tomato },
              
                    //fast_MA,
                    //slow_MA,

                    //new HMA(16) { Color = Color.LimeGreen },
                    //new WMA(16) { Color = Color.LimeGreen },
                    //new EMA(5) { Color = Color.SteelBlue },
                    //new SMMA(5) { Color = Color.CadetBlue },
                    //new RSI(14),
                    //new MFI(14),
                    //new MACD(12, 26, 9),
                    //new STO(14, 3, 3),
                    //new TSI(25,13,7),

                    ///new PSAR(0.02, 0.2),
                    new VWAP(BarFreq.Daily) { Color = Color.Plum, LineWidth = 2  },
                    //new Pivot(BarFreq.Daily),
                    //ema5_smma5_cross,
                    //divergence

                    ///new WaveTrend(10, 21, 4, 0.015) { AreaRatio = 15, HasXAxisBar = true, Order = int.MaxValue },
                    new ADX(14) { AreaRatio = 10,  Order = int.MaxValue - 10 },
                    //new CCI(20, 0.015),
                    //new ADX(14) { Order = 100, HasXAxisBar = true },
                 
                    //new SingleColumnAnalysis(rsi),
                    rsi,
                    //ma_cross,
                    //new CandleStick(),

                    //new GainPointAnalysis(200, 3, 1),
                    new TrendLineAnalysis(260),
                };

                BarAnalysisSet bas = new(sample_list);

                return bas;
            }
        }
    }
}
