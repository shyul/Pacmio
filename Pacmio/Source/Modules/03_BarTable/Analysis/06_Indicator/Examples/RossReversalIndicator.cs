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
    public class RossReversalIndicator : Indicator
    {
        public RossReversalIndicator()
        {
            string label = "(" + "," + ")";
            GroupName = Name = GetType().Name + label;

            RSI = new RSI(5);
            RsiSignal = new OscillatorSignal(RSI);

            BollingerBand = new Bollinger(20, 2);
            BollingerBandSignal = new BandSignal(Bar.Column_Typical, BollingerBand)
            {
                BullishColor = Color.DeepSkyBlue, //.YellowGreen,
                BearishColor = Color.Pink,
            };

            CandleStickDojiMarubozuAnalysis = new CandleStickDojiMarubozuSignal();
            NarrowRange = new NarrowRange();


            var csd = new DebugColumnSeries(NarrowRange.Column_Result);

            CandleStickSignalColumn = new SignalColumn(this, CandleStickDojiMarubozuAnalysis.Name)
            {
                BullishColor = Color.BlueViolet,
                BearishColor = Color.DarkOrange
            };




            TimeInForce = new TimePeriod(new Time(9, 25), new Time(16));

            NarrowRange.AddChild(csd);
            NarrowRange.AddChild(this);
            RsiSignal.AddChild(this);
            BollingerBandSignal.AddChild(this);
            CandleStickDojiMarubozuAnalysis.AddChild(this);
            csd.AddChild(this);

            SignalColumns = new SignalColumn[] { RsiSignal.Column_Result, BollingerBandSignal.Column_Result, CandleStickSignalColumn };
            SignalSeries = new(this);
            BarAnalysisSet = new(this);
        }



        public override int GetHashCode() => GetType().GetHashCode() ^ RSI.GetHashCode() ^ BollingerBand.GetHashCode();

        public override IEnumerable<SignalColumn> SignalColumns { get; }


        // Filter: 500000 Volume

        // Long short --> supported by market strength, supported by sector strength

        // Sector extremes


        #region High / Low of the Day / VWAP / Pivot / Important Level / 10-1-Half Dollar number sup/res

        // Get High or low from Daily BarTale

        // ******* Get percentage 

        public IOscillator RSI { get; }

        public OscillatorSignal RsiSignal { get; }

        #endregion


        #region 3 - 5, 5 - 10 Conseculative Candles

        public CHOP Chop { get; }

        public SignalColumn Chop_Signal { get; }


        // ******* local trend pattern

        // length of the candle

        // Number of the consecutive candles

        // distance against the EMA9 line.

        #endregion


        // Exit: boundce back to 9 EMA or VWAP












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

        #endregion


        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];

                if (TimeInForce.Contains(b.Time))
                {
                    double close = b.Close;
                    double low = b.Low;
                    double high = b.High;
                    double trend = b.TrendStrength;

                    double nr = b[NarrowRange.Column_Result];
                    double rsi = b[RSI.Column_Result];

                    // b[BollingerBandSignal.Column_Result].Multiply(trend);

                    double[] points_candle = new double[] { };

                    if (rsi > 80 && trend > 3)
                    {
                        if (b.CandleStickList.Contains(CandleStickType.Doji))
                        {
                            points_candle = new double[] { 5 * trend };
                        }
                        else if (nr > 4)
                        {
                            points_candle = new double[] { 5 * trend };
                        }
                    }
                    else if (rsi < 20 && trend < -3)
                    {
                        if (b.CandleStickList.Contains(CandleStickType.Doji))
                        {
                            points_candle = new double[] { 5 * trend };
                        }
                        else if (nr > 4)
                        {
                            points_candle = new double[] { 5 * trend };
                        }
                    }

                    SignalDatum sd_candle = new SignalDatum(b, CandleStickSignalColumn, points_candle);
                }
            }
        }
    }
}
