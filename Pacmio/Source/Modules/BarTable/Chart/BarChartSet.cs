/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Xu;
using Xu.Chart;
using System.Drawing;

namespace Pacmio
{
    public class BarChartSet
    {
        /*
        private static PacmioForm Form => Root.Form;

        public readonly List<BarChart> List = new List<BarChart>();

        public void RemoveChart(BarChart bc) 
        {
            List.CheckRemove(bc);
            bc.Close();
        }

        public void UpdateAllCharts(Period pd, CancellationTokenSource cts, IProgress<float> progress)
        {
            List.ForEach(n => {
                if (!cts.IsCancellationRequested)
                {
                    BarTable bt = n.BarTable;
                    bt.Fetch(pd, cts);
                }
            });
        }

        public void ResetAllChartsPointer()
        {
            List.ForEach(bc => {
                bc.StopPt = bc.BarTable.LastIndex;

                if (bc.IsActive)
                {
                    bc.UpdateUI();
                }
            });
        }
        */
        public static List<BarAnalysis> SampleChartConfig()
        {
            var volumeEma = new EMA(Bar.Column_Volume, 20) { Color = Color.DeepSkyBlue, LineWidth = 2 };
            volumeEma.LineSeries.Side = AlignType.Left;

            //var ema5_smma5_cross = new DualData(new EMA(5) { Color = Color.Teal }, new EMA(13) { Color = Color.Peru });
            var mfi = new MFI(14) { Order = 99 };
            var rsi = new RSI(14);
            //var divergence = new Divergence(rsi);
            //var indicator_reference_cross = new ConstantData(rsi, new Range<double>(49, 51));

            //var boll = new Bollinger(20, 2.0);
            List<BarAnalysis> barAnalyses = new List<BarAnalysis>
            {
                volumeEma,
                //ema5,
                //smma5,
                //ema5_smma5_cross,
               
                //new BoxRange(14),
                //new Bollinger(100, 2.0),
                new Bollinger(20, 2.0),
                new Chanderlier(22, 3) { Color = Color.Blue, Low_Color = Color.Plum },
                //rsi,
   
                //new SMA(5) { Color = Color.Blue },
                //new SMA(20) { Color = Color.Green },
                //new SMA(50) { Color = Color.Tomato },
                new EMA(200) { Color = Color.Salmon.Opaque(50), LineWidth = 10 },
                new EMA(50) { Color = Color.Green, LineWidth = 2 },
                new EMA(25) { Color = Color.DarkGreen, LineWidth = 1 },
                //new HMA(16) { Color = Color.LimeGreen },
                //new WMA(16) { Color = Color.LimeGreen },
                //new EMA(5) { Color = Color.SteelBlue },
                //new SMMA(5) { Color = Color.CadetBlue },
                //new RSI(14),
                //new MFI(14),
                new MACD(12, 26, 9),
                //new STO(14, 3, 3),
                new TSI(25,13,7),
                //new ADX(60),
                
                new WaveTrend(10, 21, 4, 0.015),
                new PSAR(0.02, 0.2),
                new VWAP(new Frequency(TimeUnit.Days)) { Color = Color.HotPink },
                //new Pivot(BarFreq.Daily),
                //ema5_smma5_cross,
                //divergence
                mfi,
                rsi,
                //new CCI(20, 0.015),
                //new ADX(14) { Order = 100, HasXAxisBar = true },


                //indicator_reference_cross,
                //new CandleStick(),
            };
            return barAnalyses;
        }
    }
}
