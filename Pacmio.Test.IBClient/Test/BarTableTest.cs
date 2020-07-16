using Pacmio;
using System.Collections.Generic;
using System.Drawing;
using Xu;

namespace TestClient
{
    public static class BarTableTest
    {
        public static BarTableSet BarTableSet { get; } = new BarTableSet();

        public static BarAnalysisSet TestBarAnalysisSet
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
                var rsi = new RSI(14) { AreaRatio = 8 };
                //var divergence = new Divergence(rsi);
                //var indicator_reference_cross = new ConstantData(rsi, new Range<double>(49, 51));

                //var boll = new Bollinger(20, 2.0);

                //SMA slow_MA = new EMA(50) { Color = Color.YellowGreen, LineWidth = 2 };
                //SMA fast_MA = new EMA(25) { Color = Color.DodgerBlue, LineWidth = 1 };
                SMA slow_MA = new SMMA(5) { Color = Color.Orange, LineWidth = 2 };
                SMA fast_MA = new EMA(5) { Color = Color.DodgerBlue, LineWidth = 1 };
                var ma_cross = new MovingAverageCrossIndicator(fast_MA, slow_MA) { Order = int.MinValue + 10 };

                //var ma_cross = new MovingAverageCrossIndicator(MovingAverageType.Exponential, 25, MovingAverageType.Exponential, 50);

                List<BarAnalysis> sample_list = new List<BarAnalysis>
                {                        
                    //mfi,
                    rsi,
                    volumeEma,
                    //ema5,
                    //smma5,
                    //ema5_smma5_cross,
               
                    //new BoxRange(14),
                    new SMA(20) { Color = Color.Teal.Opaque(50), LineWidth = 10 },
                    new Bollinger(20, 2.0),
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
                    
                
                       
                    new PSAR(0.02, 0.2),
                    new VWAP(new Frequency(TimeUnit.Days)) { Color = Color.Plum, LineWidth = 2  },
                    //new Pivot(BarFreq.Daily),
                    //ema5_smma5_cross,
                    //divergence

                    new WaveTrend(10, 21, 4, 0.015) { AreaRatio = 15,HasXAxisBar = true, Order = int.MaxValue },
                    new ADX(14) { AreaRatio = 10,  Order = int.MaxValue - 10 },
                    //new CCI(20, 0.015),
                    //new ADX(14) { Order = 100, HasXAxisBar = true },
                   
                    new ConstantIndicator(rsi),
                    ma_cross,
                    //new CandleStick(),
                };

                BarAnalysisSet bas = new BarAnalysisSet(sample_list);

                return bas;
            }
        }


        public static BarAnalysisSet EmptyBarAnalysisSet
        {
            get
            {
                BarAnalysisSet bas = new BarAnalysisSet()
                {
                    List = new List<BarAnalysis>()
                };

                return bas;
            }
        }
    }
}
