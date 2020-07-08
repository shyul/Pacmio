using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Xu;
using Xu.Chart;
using Pacmio;
using Pacmio.IB;
using System.Threading;
using IbXmlScannerParameter;

namespace TestClient
{
    public static class BarTableTest
    {
        public static CancellationTokenSource DownloadCancellationTokenSource { get; set; }

        public static IProgress<float> DownloadProgress { get; set; }

        public static IProgress<float> DetailedProgress { get; set; }

        public static Period Period { get; set; } = new Period();

        public static List<string> SymbolList { get; private set; } = new List<string>();

        public static List<BarFreq> BarFreqs { get; private set; } = new List<BarFreq>();

        public static void Worker()
        {
            //var list = ContractList.Values.Where(n => n.GetAttribute<ExchangeInfo>().Result?.Region.Name == "US" && SymbolList.Contains(n.Name));
            var list = ContractList.GetList(SymbolList.Where(n => n.Length > 0), "US");
            //var list = ContractList.GetOrFetch(SymbolList, "US", DownloadCancellationTokenSource, DetailedProgress);

            Console.WriteLine("list.Count() = " + list.Count());
            int total_downloads = BarFreqs.Count * list.Count();
            int pt = 0;
            foreach (BarFreq freq in BarFreqs)
            {
                foreach (Contract c in list)
                {/*
                    BarTable bt = new BarTable(c, freq, BarType.Trades);
                    bt.Reset(Period, DownloadCancellationTokenSource, DetailedProgress);

                    while (!bt.ReadyForTickCalculation)
                    {
                        Thread.Sleep(100);
                        if (DownloadCancellationTokenSource.IsCancellationRequested) break;
                    }

                    bt.LoadJsonFileToFileData();
                    bt.TransferActualValuesFromBarsToFileData();
                    bt.SaveFileDataToJsonFile();*/

                    pt++;
                    DownloadProgress?.Report(100.0f * pt / total_downloads);

                    if (DownloadCancellationTokenSource.IsCancellationRequested) break;
                }
                BusinessInfoList.Save();
                if (DownloadCancellationTokenSource.IsCancellationRequested) break;
            }
        }



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

                BarAnalysisSet bas = new BarAnalysisSet("Default BarAnalysisSet")
                {
                    List = new List<BarAnalysis>
                    {                        
                        //mfi,
                        rsi,
                        volumeEma,
                        //ema5,
                        //smma5,
                        //ema5_smma5_cross,
               
                        //new BoxRange(14),
                        //new Bollinger(100, 2.0),
                        new Bollinger(20, 2.0),
                        //new Chanderlier(22, 3) { Color = Color.Blue, Low_Color = Color.Plum },
                        //rsi,
   
                        //new SMA(5) { Color = Color.Blue },
                        //new SMA(20) { Color = Color.Green },
                        //new SMA(50) { Color = Color.Tomato },
                        new EMA(200) { Color = Color.Salmon.Opaque(50), LineWidth = 10 },
                        new EMA(50) { Color = Color.Tomato, LineWidth = 2 },
                        new EMA(25) { Color = Color.DarkGreen, LineWidth = 1 },
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
                        new VWAP(new Frequency(TimeUnit.Days)) { Color = Color.HotPink },
                        //new Pivot(BarFreq.Daily),
                        //ema5_smma5_cross,
                        //divergence

                        new WaveTrend(10, 21, 4, 0.015){ AreaRatio = 15},
                        new ADX(14) { AreaRatio = 10 },
                        //new CCI(20, 0.015),
                        //new ADX(14) { Order = 100, HasXAxisBar = true },


                        //indicator_reference_cross,
                        //new CandleStick(),
                    }
                };

                return bas;
            }
        }


        public static BarAnalysisSet EmptyBarAnalysisSet
        {
            get
            {
                BarAnalysisSet bas = new BarAnalysisSet("Default BarAnalysisSet")
                {
                    List = new List<BarAnalysis>()
                };

                return bas;
            }
        }
    }
}
