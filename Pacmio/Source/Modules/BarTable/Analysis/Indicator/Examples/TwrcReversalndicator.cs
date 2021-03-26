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
    public class TwrcReversalndicator : Indicator
    {
        public TwrcReversalndicator()
        {
            string label = "(" + "," + ")";
            GroupName = Name = GetType().Name + label;

            RSI = new RSI(5);
            BollingerBand = new Bollinger(20, 2);
            CandleStickDojiMarubozuAnalysis = new CandleStickDojiMarubozuAnalysis();
            NarrowRange = new NarrowRange();

            RSI.AddChild(this);
            BollingerBand.AddChild(this);
            CandleStickDojiMarubozuAnalysis.AddChild(this);
            NarrowRange.AddChild(this);

            var csd = new DebugColumnSeries(NarrowRange.Column_Result);

            NarrowRange.AddChild(csd);

            csd.AddChild(this);

            RSISignalColumn = new SignalColumn(Name + "_" + RSI.Name + "_Singal")
            {
                BullishColor = RSI.UpperColor,
                BearishColor = RSI.LowerColor
            };

            BollingerBandSignalColumn = new SignalColumn(Name + "_" + BollingerBand.Name + "_Singal")
            {
                BullishColor = Color.YellowGreen,
                BearishColor = Color.Pink,
            };

            CandleStickSignalColumn = new SignalColumn(Name + "_" + CandleStickDojiMarubozuAnalysis.Name + "_Singal")
            {
                BullishColor = Color.BlueViolet,
                BearishColor = Color.DarkOrange
            };

            SignalColumns = new SignalColumn[] { RSISignalColumn, BollingerBandSignalColumn, CandleStickSignalColumn };

            SignalSeries = new(this);
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ RSI.GetHashCode() ^ BollingerBand.GetHashCode();

        public TimePeriod TimeInForce { get; } = new TimePeriod(new Time(9, 25), new Time(16));

        public IOscillator RSI { get; }

        public SignalColumn RSISignalColumn { get; protected set; }

        public Dictionary<Range<double>, double[]> RSILevelScores = new() {
            { new Range<double>(-1, 5), new double[] { -7, -5 } },
            { new Range<double>(5, 10), new double[] { -3 } },
            { new Range<double>(10, 20), new double[] { -1 } },
            { new Range<double>(20, 80), new double[] { 0 } },
            { new Range<double>(80, 90), new double[] { 1 } },
            { new Range<double>(90, 95), new double[] { 3 } },
            { new Range<double>(95, 101), new double[] { 7, 5 } }
        };

        public NarrowRange NarrowRange { get; }

        public IDualData BollingerBand { get; }

        public SignalColumn BollingerBandSignalColumn { get; protected set; }

        public double[] BollingerBandScores { get; } = { 5, 2, -2, -5 };

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
                    double[] score_rsi = RSILevelScores.Where(n => n.Key.Contains(rsi)).Select(n => n.Value).FirstOrDefault();

                    double bbh = b[BollingerBand.Column_High];
                    double bbl = b[BollingerBand.Column_Low];

                    double[] score_bb = new double[] { };
                    double trend = b.TrendStrength;

                    double[] score_candle = new double[] { };

                    if (rsi > 80 && trend > 3)
                    {
                        if (low > bbh)
                            score_bb = new double[] { 5 * trend, 3 * trend };
                        else if (high > bbh)
                            score_bb = new double[] { 2 * trend };

                        if (b.CandleStickList.Contains(CandleStickType.Doji))
                        {
                            score_candle = new double[] { 5 * trend };
                        }
                        else if (nr > 4)
                        {
                            score_candle = new double[] { 5 * trend };
                        }
                    }
                    else if (rsi < 20 && trend < -3)
                    {
                        if (high < bbl)
                            score_bb = new double[] { 5 * trend, 3 * trend };
                        else if (low < bbl)
                            score_bb = new double[] { 2 * trend };

                        if (b.CandleStickList.Contains(CandleStickType.Doji))
                        {
                            score_candle = new double[] { 5 * trend };
                        }
                        else if (nr > 4)
                        {
                            score_candle = new double[] { 5 * trend };
                        }
                    }

                    b.SetSignal(RSISignalColumn, score_rsi, "rsi = " + rsi.ToString("0.##"));
                    b.SetSignal(BollingerBandSignalColumn, score_bb, "BB Penetration");
                    b.SetSignal(CandleStickSignalColumn, score_candle, "Candle Match");
                }
            }
        }
    }
}
