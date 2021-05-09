/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// Filter
/// $15 ~ $250
/// Volume 500000
/// Relative Volume 1x
/// RSI 25 Max
/// 4x 5 min cons candle
/// 5D average volume 300000
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;

namespace Pacmio.Analysis
{
    public class ReversalStrategy : Strategy
    {
        public ReversalStrategy(
            double aboveVolume,
            double belowVolume,
            double abovePrice,
            double belowPrice,
            double minRiskRewardRatio,
            TimeSpan holdingMaxSpan,
            TimePeriod holdingPeriod,
            TimePeriod tif,
            BarFreq barFreq = BarFreq.Minute)
            : base(minRiskRewardRatio, holdingMaxSpan, holdingPeriod, tif, barFreq, PriceType.Trades)
        {


            DailyPriceFilterSignal =
                new SingleDataSignal(TimePeriod.Full, BarFreq.Daily, Bar.Column_Typical, new Range<double>(abovePrice, belowPrice))
                {
                    TypeToTrailPoints = new()
                    {
                        { SingleDataSignalType.Within, new double[] { 1 } }
                    }
                };

            DailyVolumeFilterSignal =
                new SingleDataSignal(TimePeriod.Full, BarFreq.Daily, Bar.Column_Volume, new Range<double>(aboveVolume, belowVolume))
                {
                    TypeToTrailPoints = new()
                    {
                        { SingleDataSignalType.Within, new double[] { 1 } }
                    }
                };






            AnalysisSet =
                new BarAnalysisSet(new SignalAnalysis[]
                {
                    this
                });
        }


        public Range<double> PriceRange { get; } = new Range<double>(15, 250);

        public Range<double> VolumeRange { get; } = new Range<double>(5e5, double.MaxValue);

        #region Filter Signals

        public SingleDataSignal DailyPriceFilterSignal { get; }
        public SingleDataSignal DailyVolumeFilterSignal { get; }



        #endregion Filter Signals


        // Filter: 500000 Volume

        // Long short --> supported by market strength, supported by sector strength

        // Sector extremes


        #region High / Low of the Day / VWAP / Pivot / Important Level / 10-1-Half Dollar number sup/res

        // Get High or low from Daily BarTale

        // ******* Get percentage 

        //public IOscillator RSI { get; }

        //public OscillatorSignal RsiSignal { get; }

        #endregion


        #region 3 - 5, 5 - 10 Conseculative Candles

        //public CHOP Chop { get; }

        //public SignalColumn Chop_Signal { get; }


        // ******* local trend pattern

        // length of the candle

        // Number of the consecutive candles

        // distance against the EMA9 line.

        #endregion


        // Exit: boundce back to 9 EMA or VWAP


        /// <summary>
        /// Or we can use Relative Volume of 1 ~ 1.5
        /// </summary>
        public Range<double> Volume5DAverageRange { get; } = new Range<double>(3e5, double.MaxValue);

        // 1 ~ 1.5
        public RelativeToAverage RelativeVolume { get; } = new RelativeToAverage(Bar.Column_Volume, 5);

        public NarrowRange NarrowRange { get; }

        public CandleStickDojiMarubozuSignal CandleStickDojiMarubozuAnalysis { get; }

        public SignalColumn CandleStickSignalColumn { get; protected set; }



        #region Last Candle's shape

        // Doji? Hammer?

        public IDualData BollingerBand { get; }

        public BandSignal BollingerBandSignal { get; }



        // fully of outside of the BB -> H is lower than BBL, or L is higher than BBL

        // Near daily support level.... Daily Pivot, SMA,  TrendLines

        #endregion

        #region Execution

        // STOP: high or low of the triggering candle

        // adjust the size with the stop distance.

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            BarTableSet bts = bt.BarTableSet;
            MultiPeriod testPeriods = bts.MultiPeriod;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                StrategyDatum sd = b[this];
            }
        }

        #endregion



    }
}
