/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;

namespace Pacmio.Analysis
{
    public class RossReversalndicator : Indicator
    {
        public RossReversalndicator()
        {
            string label = "(" + "," + ")";
            GroupName = Name = GetType().Name + label;

            RSI = new RSI(5);
            RsiSignal = new OscillatorSignal(RSI);

            BollingerBand = new Bollinger(20, 2);
            BollingerBandSignal = new BandSignal(Bar.Column_Typical, BollingerBand);

            CandleStickDojiMarubozuAnalysis = new CandleStickDojiMarubozuAnalysis();
            NarrowRange = new NarrowRange();

            /*
            BollingerBandSignalColumn = new SignalColumn(this, BollingerBand.Name)
            {
                BullishColor = Color.YellowGreen,
                BearishColor = Color.Pink,
            };
            */


            NarrowRange.AddChild(this);

            var csd = new DebugColumnSeries(NarrowRange.Column_Result);
            NarrowRange.AddChild(csd);

            CandleStickSignalColumn = new SignalColumn(this, CandleStickDojiMarubozuAnalysis.Name)
            {
                BullishColor = Color.BlueViolet,
                BearishColor = Color.DarkOrange
            };

            SignalColumns = new SignalColumn[] { RsiSignal.Column_Result, BollingerBandSignal.Column_Result, CandleStickSignalColumn };

            SignalSeries = new(this);



            RsiSignal.AddChild(this);
            BollingerBandSignal.AddChild(this);
            CandleStickDojiMarubozuAnalysis.AddChild(this);
            csd.AddChild(this);
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ RSI.GetHashCode() ^ BollingerBand.GetHashCode();

        public TimePeriod TimeInForce { get; } = new TimePeriod(new Time(9, 25), new Time(16));



        public IOscillator RSI { get; }

        public OscillatorSignal RsiSignal { get; }



        public IDualData BollingerBand { get; }

        public BandSignal BollingerBandSignal { get; }

        //public SignalColumn BollingerBandSignalColumn { get; protected set; }

        //public double[] BollingerBandScores { get; } = { 5, 2, -2, -5 };



        public NarrowRange NarrowRange { get; }

        public CandleStickDojiMarubozuAnalysis CandleStickDojiMarubozuAnalysis { get; }

        public SignalColumn CandleStickSignalColumn { get; protected set; }





        public override IEnumerable<SignalColumn> SignalColumns { get; }

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
                    double nr = b[NarrowRange.Column_Result];

                    double rsi = b[RSI.Column_Result];

                    //double bbh = b[BollingerBand.Column_High];
                    //double bbl = b[BollingerBand.Column_Low];

                    //double[] points_bb = new double[] { };


                    double trend = b.TrendStrength;

                    double[] points_candle = new double[] { };

                    if (rsi > 80 && trend > 3)
                    {
                        /*
                        if (low > bbh)
                            points_bb = new double[] { 5 * trend, 3 * trend };
                        else if (high > bbh)
                            points_bb = new double[] { 2 * trend };*/

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
                        /*
                        if (high < bbl)
                            points_bb = new double[] { 5 * trend, 3 * trend };
                        else if (low < bbl)
                            points_bb = new double[] { 2 * trend };*/

                        if (b.CandleStickList.Contains(CandleStickType.Doji))
                        {
                            points_candle = new double[] { 5 * trend };
                        }
                        else if (nr > 4)
                        {
                            points_candle = new double[] { 5 * trend };
                        }
                    }

                    //SignalDatum sd_bb = new SignalDatum(b, BollingerBandSignalColumn, points_bb);
                    SignalDatum sd_candle = new SignalDatum(b, CandleStickSignalColumn, points_candle);
                }
            }
        }
    }
}
