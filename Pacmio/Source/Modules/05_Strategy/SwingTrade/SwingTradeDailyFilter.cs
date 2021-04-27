/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
///     Swing Trading: Find, Evaluate, & Execute
///     https://youtu.be/cMmW12Smmt4
/// 
///     3 Criteria for Finding Swing Stocks
///     https://youtu.be/GHG3Kf-FYvw
/// 
///     3 (Powerful) Swing Trading Strategies
///     https://youtu.be/MK2V6GKBmf0
///     
///     *** Stonks | How To Scan For Stocks Using Finviz For Swing Traders
///     https://www.youtube.com/watch?v=MMpqYK9C2iI
///     
///     https://support.stockcharts.com/doku.php?id=scans:library:sample_scans
/// 
///     https://school.stockcharts.com/doku.php?id=technical_indicators:sctr
/// 
/// 1. Volatility: 
///     https://www.investopedia.com/terms/v/volatility.asp#:~:text=Volatility%20is%20a%20statistical%20measure,same%20security%20or%20market%20index.
///     https://www.wallstreetmojo.com/volatility-formula/
///     ATR %
///     Week %
///     
/// 2. A stock is a good deal, RSI is below 30, buying low and selling high. No buying high and expect to sell higher...
///     Cross Below 30, within 5 Bars (4 hours)
/// 
/// 3. Signs of up trend -- using trendline
/// 
/// 
/// 4. Descent about of trading volume
///     100,000
/// 
/// Trailing 180 days.
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;

namespace Pacmio.Analysis
{
    public class SwingTradeDailyFilter : SignalIndicator
    {
        public SwingTradeDailyFilter() : base(BarFreq.Daily, PriceType.Trades)
        {
            RSI rsi = new(14);
            Bollinger bb = new Bollinger(20, 2);

            OscillatorAnalysis = rsi;
            BollingerBand = bb;

            OscillatorSignal = new OscillatorSignal(rsi);
            BollingerBandSignal = new BandSignal(Bar.Column_Typical, bb)
            {
                BullishColor = Color.DeepSkyBlue, //.YellowGreen,
                BearishColor = Color.Pink,
            };

            SingleDataSignal = new SingleDataSignal(Bar.Column_Volume, new Range<double>(1e5));

            string label = "(" + "," + ")";
            GroupName = Name = GetType().Name + label;

            OscillatorSignal.AddChild(this);
            BollingerBandSignal.AddChild(this);
            SingleDataSignal.AddChild(this);





            ChopAnalysis = new CHOP(20);
            ChopAnalysis.AddChild(this);
            StandardDeviationAnalysis = new STDEV(Bar.Column_Typical, 20);
            StandardDeviationAnalysis.AddChild(this);


            BullishPointLimit = 1;
            BearishPointLimit = -1;


            SignalColumns = new SignalColumn[] { OscillatorSignal.Column_Result, BollingerBandSignal.Column_Result, SingleDataSignal.Column_Result };
            BarAnalysisSet = new(this);
            SignalSeries = new(this);
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ OscillatorAnalysis.GetHashCode() ^ BollingerBand.GetHashCode() ^ SingleDataSignal.GetHashCode();

        public override IEnumerable<SignalColumn> SignalColumns { get; } 


        public CHOP ChopAnalysis { get; }

        public STDEV StandardDeviationAnalysis { get; }


        public IOscillator OscillatorAnalysis { get; }

        public OscillatorSignal OscillatorSignal { get; }


        public IDualData BollingerBand { get; }

        public BandSignal BollingerBandSignal { get; }


        public SMA MA_Long { get; }


        public SMA MA_Medium { get; }

        public SingleDataSignal SingleDataSignal { get; }
        /*
        Long-Term Indicators(weighting)
        --------------------------------

          * Percent above/below 200-day EMA(30%)
          * 125-Day Rate-of-Change(30%)

        Medium-Term Indicators(weighting)
        ----------------------------------

          * Percent above/below 50-day EMA(15%)
          * 20-day Rate-of-Change(15%)

        Short-Term Indicators(weighting)
        ---------------------------------

          * 3-day slope of PPO(12,26,9) Histogram/3 (5%)
          * 14-day RSI(5%)
        
         */

        protected override void Calculate(BarAnalysisPointer bap)
        {
            // Filter Volume

            // Filter Chop Index

            // Add SMA?

            BarTable bt = bap.Table;
            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];

                double close = b.Close;
                double low = b.Low;
                double high = b.High;

                double volume = b.Volume;

                double point = 0;

                if (low > 2 && high < 50 && volume * close > 1e7 && (b.Gap > 0.5 || b.Gap < -0.5))
                {
                    point++;
                }
                else
                {
                    SignalColumns.Select(sc => b[sc]).RunEach(sd => { if (sd is not null) sd.ResetPoints(); });
                }




                double chop = b[ChopAnalysis];

                double rsi = b[OscillatorAnalysis];

                OscillatorSignalDatum osd = b[OscillatorSignal] as OscillatorSignalDatum;

            }
        }
    }
}
