/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// Opening Range Breakout Trading Strategy
/// https://www.warriortrading.com/opening-range-breakout/
/// 
/// /// 1. Strong in the pre-market session, (Possible surging at the market open 9:30 AM)
/// 2. Trade as soon as 9:30, the beginning of the day, (some avoid trade at the first 5 minutes)
/// 3. Flat top breakout or bull flag in pre-market, tradeable when pre-market has 500k volume
/// 
/// Filter Daily ?? what is the most obvious setup? ->> biggest percentage gapper with the most volume
/// 1) Stocks under $20. Definitly above $1
/// 2) Biggest percentage gappers, at least 4% gap, sorted by percentage
/// 3) Pre-Market Volume
/// 4) News confirmation, Earnings Report
/// 
/// ---> 1 min Bars, Then 5 min Bars
/// 1) At least 50K, maybe 100k of the pre-market Volume in the pre-market session
/// 2) Relative Volume of 2
/// 3) price consolidate to (High of the intraday time range percent)
/// 
/// 
/// Entry
/// one minute ORB  first, then five minutes ORB
/// Start with half size position
/// 
/// Condition A: In between suppprt and resistance
/// 1. High of the first candle of the opening as BUY STOP (a few cents above, add percentage variable for OrderRule???) entry, and Low as STOP LOSS
/// 
/// Large Offer in the L2 just in case, at the end of the offer depletion
/// 
/// 
/// Note: Can also trade the first pull back, or even the second pullback
/// Note: Why 1 minute? We need so much volume that 5 minute would be too wide to contain the volume
/// Note: L2 resistance!!
/// 
/// Exit:
/// Whenever the price is up cover the risk portion (1:1), sell half of the position and set the stop to breakeven
/// If the daily chart is "good", and "strong catalyst", the position can hold longer
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;

namespace Pacmio.Analysis
{
    public class GapGoOrbStrategy : Strategy
    {
        public GapGoOrbStrategy(
            double minimumMinuteVolume = 1e5,
            double minimumMinuteRelativeVolume = 1.1,
            double gapPercent = 4,
            double minPrice = 1,
            double maxPrice = 300,
            double minVolume = 5e5,
            double maxVolume = double.MaxValue,
            double minRiskRewardRatio = 2,
            BarFreq fiveMinFreq = BarFreq.Minutes_5,
            BarFreq barFreq = BarFreq.Minute)
            : this(
            new EMA(9) { Color = Color.DeepSkyBlue },
            new EMA(20) { Color = Color.Pink },
            minimumMinuteVolume,
            minimumMinuteRelativeVolume,
            gapPercent,
            minPrice,
            maxPrice,
            minVolume,
            maxVolume,
            minRiskRewardRatio,
            new TimeSpan(1000, 1, 1, 1, 1),
            new TimePeriod(new Time(9, 30), new Time(12, 00)),
            new TimePeriod(new Time(9, 30), new Time(10, 00)),
            fiveMinFreq,
            barFreq)
        { }

        public GapGoOrbStrategy(
            ISingleData crossData_1,
            ISingleData crossData_2,
            double minimumMinuteVolume,
            double minimumMinuteRelativeVolume,
            double gapPercent,
            double minPrice,
            double maxPrice,
            double minVolume,
            double maxVolume,
            double minRiskRewardRatio,
            TimeSpan holdingMaxSpan,
            TimePeriod holdingPeriod,
            TimePeriod tif,
            BarFreq fiveMinFreq,
            BarFreq barFreq = BarFreq.Minute)
            : base(minRiskRewardRatio, holdingMaxSpan, holdingPeriod, tif, barFreq, PriceType.Trades)
        {
            MinimumMinuteVolume = minimumMinuteVolume;
            MinimumMinuteRelativeVolume = minimumMinuteRelativeVolume;

            #region Define Filter

            Filter = GapFilter = new GapFilter(gapPercent, minPrice, maxPrice, minVolume, maxVolume, BarFreq.Daily, PriceType.Trades);

            #endregion Define Filter

            #region Define Signals

            Dictionary<DualDataSignalType, double[]> dualDataPoints = new()
            {
                { DualDataSignalType.CrossUp, new double[] { 10 } },
                { DualDataSignalType.CrossDown, new double[] { -10 } },
            };

            FiveMinutesCrossData_1 = crossData_1;
            FiveMinutesCrossSignal_1 = new DualDataSignal(TimeInForce, fiveMinFreq, FiveMinutesCrossData_1)
            {
                TypeToTrailPoints = dualDataPoints
            };

            FiveMinutesCrossData_2 = crossData_2;
            FiveMinutesCrossSignal_2 = new DualDataSignal(TimeInForce, fiveMinFreq, FiveMinutesCrossData_2)
            {
                TypeToTrailPoints = dualDataPoints
            };

            #endregion Define Signals

            Label = "(" + gapPercent + "," + minimumMinuteVolume + "," + minimumMinuteRelativeVolume + "," + fiveMinFreq + "," + crossData_1.Name + "," + crossData_2.Name + "," + minRiskRewardRatio + "," + GapFilter.Name + "," + barFreq + ")";
            Name = GetType().Name + Label;
            AreaName = GroupName = GetType().Name;
            Description = "Gap and Go ORB Strategy " + Label;

            Column_Result = new(this, typeof(StrategyDatum));

            RelativeVolume = new TimeFrameRelativeVolume(tif);
            PricePosition = new TimeFramePricePosition();

            RelativeVolume.AddChild(this);
            PricePosition.AddChild(this);

            AnalysisSet =
                new BarAnalysisSet(new SignalAnalysis[] {
                    FiveMinutesCrossSignal_1,
                    FiveMinutesCrossSignal_2,
                    this
                });
        }

        #region Entry Signals

        public GapFilter GapFilter { get; }

        public TimeFrameRelativeVolume RelativeVolume { get; }
        public TimeFramePricePosition PricePosition { get; }

        public double MinimumMinuteVolume { get; }
        public double MinimumMinuteRelativeVolume { get; }

        public const string BullishOpenBarMessage = "BullishOpenBar";
        public const string BearishOpenBarMessage = "BearishOpenBar";

        public const string RangeBarBullishMessage = "RangeBarBullish";
        public const string RangeBarBearishMessage = "RangeBarBearish";

        #endregion Entry Signals

        #region Exit Signals

        public DualDataSignal FiveMinutesCrossSignal_1 { get; }
        public ISingleData FiveMinutesCrossData_1 { get; }
        public DualDataSignal FiveMinutesCrossSignal_2 { get; }
        public ISingleData FiveMinutesCrossData_2 { get; }

        #endregion Exit Signals

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            BarTableSet bts = bt.BarTableSet;
            MultiPeriod testPeriods = bts.MultiPeriod;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                StrategyDatum sd = b[this];

                #region Identify the indicators

                if (testPeriods.Contains(b.Time))
                {
                    if (b.Time == TimeInForce.Start)
                    {
                        double flt = b[Filter];
                        if (flt != 0 && b.Volume > MinimumMinuteVolume && b[RelativeVolume] > MinimumMinuteRelativeVolume)
                        {
                            Console.WriteLine("flt = " + flt + " | b[PricePosition] = " + b[PricePosition] + " | b.Volume = " + b.Volume + " | b[RelativeVolume] = " + b[RelativeVolume]);

                            if (flt > 0 && b[PricePosition] > 0.6)
                            {
                                sd.Message = BullishOpenBarMessage;
                                //sd.SetPoints(new double[] { 1 });
                                Console.WriteLine(bt.Contract + " | " + b.Time + " | Bullish ORB");
                            }
                            else if (flt < 0 && b[PricePosition] < 0.4)
                            {
                                sd.Message = BearishOpenBarMessage;
                                //sd.SetPoints(new double[] { -1 });
                                Console.WriteLine(bt.Contract + " | " + b.Time + " | Bearish ORB");
                            }
                        }
                    }

                    if (sd.Datum_1 is StrategyDatum sd_1)
                    {
                        Bar ob = sd_1.Bar;
                        if (string.IsNullOrEmpty(sd.Message))
                        {
                            // TODO: here
                            // Verify higher time frame analysis and indicators 

                            if (sd_1.Message == BullishOpenBarMessage)
                            {
                                if (b.Contains(ob.High)) // Bullish / long side no gap entry
                                {
                                    sd.StopLossPrice = ob.Low;
                                    sd.RiskPart = ob.High - ob.Low;
                                    sd.ProfitTakePrice = ob.High + MinimumRiskRewardRatio * sd.RiskPart;
                                    sd.Message = RangeBarBullishMessage;
                                    sd.EntryBarIndex = 0;
                                    sd.SendOrder(ob.High, 1, OrderType.Stop);
                                    sd.SetPoints(new double[] { 3 });
                                }
                                else if (b.Contains(ob.Low)) // Bearish / short side no gap entry
                                {
                                    sd.StopLossPrice = ob.High;
                                    sd.RiskPart = ob.High - ob.Low;
                                    sd.ProfitTakePrice = ob.Low - MinimumRiskRewardRatio * sd.RiskPart;
                                    sd.Message = RangeBarBearishMessage;
                                    sd.EntryBarIndex = 0;
                                    sd.SendOrder(ob.Low, -1, OrderType.Limit);
                                    sd.SetPoints(new double[] { -3 });
                                }
                            }
                            else if (sd_1.Message == BearishOpenBarMessage)
                            {
                                if (b.Contains(ob.High))
                                {
                                    sd.SetPoints(new double[] { 3 });
                                }
                                else if (b.Contains(ob.Low))
                                {
                                    sd.SetPoints(new double[] { -3 });
                                }
                            }
                        }
                    }
                }

                #endregion Identify the indicators

                #region Check profit and stop

                // Dedicated function to handle stop out here!

                ///sd.StopLossOrTakeProfit(0.5);

                // Profit taking or Stop Signal Trigger!
                if (sd.Quantity > 0)
                {
                    // Find exit signals
                    if (bts[b.Time, FiveMinutesCrossSignal_1] is DualDataSignalDatum sd_1 && sd_1.List.Contains(DualDataSignalType.CrossDown))
                    {
                       // sd.SendOrder(b.Low, -1, OrderType.MidPrice);
                    }
                    else if (bts[b.Time, FiveMinutesCrossSignal_2] is DualDataSignalDatum sd_2 && sd_2.List.Contains(DualDataSignalType.CrossDown))
                    {
                       // sd.SendOrder(b.Low, -1, OrderType.MidPrice);
                    }
                    else if (bts[b.Time, BarFreq.Minutes_5] is Bar min5_b && (min5_b.Bar_1 is Bar min5_b_1) && (min5_b.Low < min5_b_1.Low))
                    {
                       // sd.SendOrder(b.Low, -1, OrderType.MidPrice);
                    }
                }
                else if (sd.Quantity < 0)
                {
                    // Find exit signals
                    if (bts[b.Time, FiveMinutesCrossSignal_1] is DualDataSignalDatum sd_1 && sd_1.List.Contains(DualDataSignalType.CrossUp))
                    {
                       // sd.SendOrder(b.High, 1, OrderType.MidPrice);
                    }
                    else if (bts[b.Time, FiveMinutesCrossSignal_2] is DualDataSignalDatum sd_2 && sd_2.List.Contains(DualDataSignalType.CrossUp))
                    {
                       // sd.SendOrder(b.High, 1, OrderType.MidPrice);
                    }
                    else if (bts[b.Time, BarFreq.Minutes_5] is Bar min5_b && (min5_b.Bar_1 is Bar min5_b_1) && (min5_b.High > min5_b_1.High))
                    {
                       // sd.SendOrder(b.High, 1, OrderType.MidPrice);
                    }
                }

                #endregion Check profit and stop
            }
        }
    }
}

/*
flt = 50 | b[PricePosition] = 0.338523489932886 | b.Volume = 867000 | b[RelativeVolume] = 2.97018979355943
flt = 4.28070175438596 | b[PricePosition] = 0.527068965517241 | b.Volume = 1184500 | b[RelativeVolume] = 2.07935930230895
flt = 25.9179265658747 | b[PricePosition] = 0.0813008130081278 | b.Volume = 5192200 | b[RelativeVolume] = 2.59448583363598

 
 * 
flt = 0 | b[PricePosition] = 0.363333333333333 | b.Volume = 17000 | b[RelativeVolume] = 2.43978349120433
flt = 50 | b[PricePosition] = 0.338523489932886 | b.Volume = 867000 | b[RelativeVolume] = 2.97065286455576
flt = -32.1428571428571 | b[PricePosition] = 0.0174262734584451 | b.Volume = 672400 | b[RelativeVolume] = 1.37175033677593
flt = 0 | b[PricePosition] = 0.677966101694915 | b.Volume = 155800 | b[RelativeVolume] = 0.26585200160423
flt = 0 | b[PricePosition] = 0 | b.Volume = 157900 | b[RelativeVolume] = 0.432411677440811
flt = 0 | b[PricePosition] = 0.709219858156029 | b.Volume = 421500 | b[RelativeVolume] = 0.897196185965426
flt = 0 | b[PricePosition] = 0.560624999999997 | b.Volume = 279100 | b[RelativeVolume] = 0.57408785412396
flt = 0 | b[PricePosition] = 0.184210526315789 | b.Volume = 766400 | b[RelativeVolume] = 1.47100837834769
flt = 0 | b[PricePosition] = 0.674157303370786 | b.Volume = 361000 | b[RelativeVolume] = 1.17204167202466
flt = 0 | b[PricePosition] = 0.2598 | b.Volume = 167100 | b[RelativeVolume] = 0.624498487307333
flt = 0 | b[PricePosition] = 0.754803921568628 | b.Volume = 171300 | b[RelativeVolume] = 1.01744814969792
flt = 0 | b[PricePosition] = 0.909166666666668 | b.Volume = 63300 | b[RelativeVolume] = 0.344541264011892
flt = 6.54205607476634 | b[PricePosition] = 0.726027397260275 | b.Volume = 197700 | b[RelativeVolume] = 0.728953749202267
flt = 0 | b[PricePosition] = 0.863181818181821 | b.Volume = 112800 | b[RelativeVolume] = 0.477343103644483
flt = 0 | b[PricePosition] = 0.25662162162162 | b.Volume = 244100 | b[RelativeVolume] = 1.36924455990646
flt = 0 | b[PricePosition] = 0.612676056338028 | b.Volume = 4800 | b[RelativeVolume] = 1.44493490195471
flt = 11.4624505928854 | b[PricePosition] = 0.635220125786164 | b.Volume = 6700 | b[RelativeVolume] = 1.58911195509684
flt = 0 | b[PricePosition] = 0.178938053097345 | b.Volume = 543800 | b[RelativeVolume] = 0.976196633296514
flt = 0 | b[PricePosition] = 0.773584905660379 | b.Volume = 476300 | b[RelativeVolume] = 1.33895091037413
flt = 0 | b[PricePosition] = 0.400000000000001 | b.Volume = 158000 | b[RelativeVolume] = 0.478609810673021
flt = 0 | b[PricePosition] = 0.586034482758622 | b.Volume = 162000 | b[RelativeVolume] = 0.464158597108738
flt = 6.70241286863271 | b[PricePosition] = 0.821666036001945 | b.Volume = 48600 | b[RelativeVolume] = 1.61431075534915
flt = 0 | b[PricePosition] = 0.661290322580646 | b.Volume = 10000 | b[RelativeVolume] = 1.34770414935195
flt = 0 | b[PricePosition] = 0.312774869109947 | b.Volume = 1389300 | b[RelativeVolume] = 1.84278014060005
flt = 0 | b[PricePosition] = 0.219081632653061 | b.Volume = 439500 | b[RelativeVolume] = 0.416468946175368
flt = 0 | b[PricePosition] = 0.707446808510638 | b.Volume = 373300 | b[RelativeVolume] = 0.670691744878929
flt = 0 | b[PricePosition] = 0.189161554192229 | b.Volume = 1876000 | b[RelativeVolume] = 2.12046380476107
flt = 24.8711340206186 | b[PricePosition] = 0.682445759368836 | b.Volume = 75300 | b[RelativeVolume] = 1.38266801327763
flt = 9.64207450693937 | b[PricePosition] = 0.454735135135135 | b.Volume = 1810600 | b[RelativeVolume] = 1.37921172140104
flt = 0 | b[PricePosition] = 0.373184357541899 | b.Volume = 141000 | b[RelativeVolume] = 0.697500416327948
flt = 0 | b[PricePosition] = 0.847908745247147 | b.Volume = 435200 | b[RelativeVolume] = 0.335013562270828
flt = 0 | b[PricePosition] = 0.406790832564358 | b.Volume = 933900 | b[RelativeVolume] = 0.841669621560834
flt = -11.2704918032787 | b[PricePosition] = 0.797235023041475 | b.Volume = 30800 | b[RelativeVolume] = 0.94965629213435
flt = 0 | b[PricePosition] = 0.305810397553516 | b.Volume = 766000 | b[RelativeVolume] = 0.835635984520676
flt = -4.93827160493828 | b[PricePosition] = 0.961832061068702 | b.Volume = 0 | b[RelativeVolume] = 0.285330747466388
flt = 0 | b[PricePosition] = 1 | b.Volume = 726000 | b[RelativeVolume] = 0.795716993474523
flt = 0 | b[PricePosition] = 0.655256723716382 | b.Volume = 219000 | b[RelativeVolume] = 1.58712785881621
flt = 8.46994535519126 | b[PricePosition] = 0.843076923076922 | b.Volume = 1033600 | b[RelativeVolume] = 1.22458316463869
flt = 0 | b[PricePosition] = 0.0218508997429312 | b.Volume = 1013900 | b[RelativeVolume] = 1.19730175977013
flt = 0 | b[PricePosition] = 0.0561797752808986 | b.Volume = 1421400 | b[RelativeVolume] = 0.807284342887022
flt = 0 | b[PricePosition] = 0.913462414578588 | b.Volume = 594500 | b[RelativeVolume] = 0.672785005838557
flt = 0 | b[PricePosition] = 0.678722627737226 | b.Volume = 426000 | b[RelativeVolume] = 0.647357050369625
flt = 0 | b[PricePosition] = 0.289855072463767 | b.Volume = 98300 | b[RelativeVolume] = 0.928869265091888
flt = 0 | b[PricePosition] = 0.625033783783785 | b.Volume = 849700 | b[RelativeVolume] = 1.23196836973812
flt = 0 | b[PricePosition] = 0.577181208053694 | b.Volume = 297700 | b[RelativeVolume] = 0.621304875759382
flt = 0 | b[PricePosition] = 0 | b.Volume = 806400 | b[RelativeVolume] = 0.661599807195545
flt = 0 | b[PricePosition] = 1 | b.Volume = 1659800 | b[RelativeVolume] = 1.50406862376604
flt = 0 | b[PricePosition] = 0.273743016759777 | b.Volume = 699900 | b[RelativeVolume] = 0.586037134561341
flt = 0 | b[PricePosition] = 0.161016949152542 | b.Volume = 464900 | b[RelativeVolume] = 0.716984406114945
flt = 0 | b[PricePosition] = 0.225563909774435 | b.Volume = 13000 | b[RelativeVolume] = 0.58172045745945
flt = 0 | b[PricePosition] = 0.195652173913042 | b.Volume = 400300 | b[RelativeVolume] = 1.01304539941346
flt = 0 | b[PricePosition] = 0.0931395348837206 | b.Volume = 235100 | b[RelativeVolume] = 0.500037955879186
flt = 0 | b[PricePosition] = 0.516842105263161 | b.Volume = 149100 | b[RelativeVolume] = 0.527772303288534
flt = 4.28070175438596 | b[PricePosition] = 0.527068965517241 | b.Volume = 1184500 | b[RelativeVolume] = 2.0793593023091
flt = -12.7946127946128 | b[PricePosition] = 0.0975244299674266 | b.Volume = 16600 | b[RelativeVolume] = 0.708475809982871
flt = 0 | b[PricePosition] = 0.0930232558139536 | b.Volume = 316800 | b[RelativeVolume] = 0.316081585974624
flt = 0 | b[PricePosition] = 0.337748344370862 | b.Volume = 546000 | b[RelativeVolume] = 0.945805406841266
flt = -14.4316730523627 | b[PricePosition] = 0.183132530120482 | b.Volume = 141700 | b[RelativeVolume] = 1.88326017542683
flt = 0 | b[PricePosition] = 0.482758620689656 | b.Volume = 498300 | b[RelativeVolume] = 0.435068009214798
flt = 0 | b[PricePosition] = 0.0677966101694929 | b.Volume = 15900 | b[RelativeVolume] = 0.467171938068537
flt = 0 | b[PricePosition] = 0.311499999999998 | b.Volume = 454500 | b[RelativeVolume] = 0.592349082134739
flt = 0 | b[PricePosition] = 0 | b.Volume = 687700 | b[RelativeVolume] = 0.812812324049853
flt = 25.9179265658747 | b[PricePosition] = 0.0813008130081278 | b.Volume = 5192200 | b[RelativeVolume] = 2.59448583363598
flt = 0 | b[PricePosition] = 0 | b.Volume = 2100400 | b[RelativeVolume] = 0.593316217117696
flt = 0 | b[PricePosition] = 0.260563380281694 | b.Volume = 3900 | b[RelativeVolume] = 0.146419083131891
flt = 0 | b[PricePosition] = 0.82051282051282 | b.Volume = 21600 | b[RelativeVolume] = 0.759985516244062
flt = 0 | b[PricePosition] = 0.209743589743587 | b.Volume = 655600 | b[RelativeVolume] = 0.465178124544975
flt = 0 | b[PricePosition] = 0.784457831325301 | b.Volume = 743300 | b[RelativeVolume] = 0.872097194212652
flt = 0 | b[PricePosition] = 0.0526315789473762 | b.Volume = 229700 | b[RelativeVolume] = 0.240060123019714
flt = 0 | b[PricePosition] = 0 | b.Volume = 621400 | b[RelativeVolume] = 0.682795147805114
flt = 0 | b[PricePosition] = 0.556818181818175 | b.Volume = 636400 | b[RelativeVolume] = 0.671421185408098
flt = 0 | b[PricePosition] = 0.765833333333334 | b.Volume = 486800 | b[RelativeVolume] = 0.648774989117412
flt = 0 | b[PricePosition] = 0.0675280898876399 | b.Volume = 403900 | b[RelativeVolume] = 0.740255710506631
flt = 0 | b[PricePosition] = 1 | b.Volume = 432300 | b[RelativeVolume] = 0.653982025551379
flt = 0 | b[PricePosition] = 0.723274336283187 | b.Volume = 294800 | b[RelativeVolume] = 0.851017190790155
flt = 0 | b[PricePosition] = 0.460317460317465 | b.Volume = 259100 | b[RelativeVolume] = 0.89072082901813
flt = 0 | b[PricePosition] = 0.662222222222224 | b.Volume = 189600 | b[RelativeVolume] = 0.648157080940856
flt = 0 | b[PricePosition] = 0.0138888888888836 | b.Volume = 256900 | b[RelativeVolume] = 0.53841765271577
flt = 0 | b[PricePosition] = 0.210526315789471 | b.Volume = 261200 | b[RelativeVolume] = 0.68342511906012
flt = 0 | b[PricePosition] = 0.447999999999997 | b.Volume = 186700 | b[RelativeVolume] = 0.650103231809241
flt = 0 | b[PricePosition] = 0.180327868852457 | b.Volume = 201300 | b[RelativeVolume] = 1.08592417524625
flt = 0 | b[PricePosition] = 0.161944444444446 | b.Volume = 419800 | b[RelativeVolume] = 1.11352267529641
flt = 6.65024630541873 | b[PricePosition] = 0.926829268292684 | b.Volume = 635400 | b[RelativeVolume] = 1.34097201776693
flt = 0 | b[PricePosition] = 0.313432835820896 | b.Volume = 534100 | b[RelativeVolume] = 1.47071870687825
 
 */