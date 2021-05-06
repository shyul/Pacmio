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
            double minimumMinuteRelativeVolume = 2,
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
            TimePeriod.Full, //new TimePeriod(new Time(9, 30), new Time(12, 00)),
            TimePeriod.Full, //new TimePeriod(new Time(9, 30), new Time(10, 00)),
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
                    //Console.Write(".");

                    if (b.Time == TimeInForce.Start)
                    {
                        if (b.Volume > MinimumMinuteVolume && b[RelativeVolume] > MinimumMinuteRelativeVolume)
                        {
                            double flt = b[Filter];
                            if (flt > 0 && b[PricePosition] > 0.75)
                            {
                                sd.Message = BullishOpenBarMessage;
                                //sd.SetPoints(new double[] { 1 });
                                Console.WriteLine(bt.Contract + " | " + b.Time + " | Bullish ORB");
                            }
                            else if (flt < 0 && b[PricePosition] < 0.25)
                            {
                                sd.Message = BearishOpenBarMessage;
                                //sd.SetPoints(new double[] { -1 });
                                Console.WriteLine(bt.Contract + " | " + b.Time + " | Bearish ORB");
                            }
                            else
                                Console.Write("@");
                        }
                    }

                    if (sd.Datum_1 is StrategyDatum sd_1 )
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
                sd.StopLossOrTakeProfit(0.5);

                // Profit taking or Stop Signal Trigger!
                if (sd.Quantity > 0)
                {
                    // Find exit signals
                    if (bts[b.Time, FiveMinutesCrossSignal_1] is DualDataSignalDatum sd_1 && sd_1.List.Contains(DualDataSignalType.CrossDown))
                    {
                        sd.SendOrder(b.Low, -1, OrderType.MidPrice);
                    }
                    else if (bts[b.Time, FiveMinutesCrossSignal_2] is DualDataSignalDatum sd_2 && sd_2.List.Contains(DualDataSignalType.CrossDown))
                    {
                        sd.SendOrder(b.Low, -1, OrderType.MidPrice);
                    }
                    else if (bts[b.Time, BarFreq.Minutes_5] is Bar min5_b && (min5_b.Bar_1 is Bar min5_b_1) && (min5_b.Low < min5_b_1.Low))
                    {
                        sd.SendOrder(b.Low, -1, OrderType.MidPrice);
                    }
                }
                else if (sd.Quantity < 0)
                {
                    // Find exit signals
                    if (bts[b.Time, FiveMinutesCrossSignal_1] is DualDataSignalDatum sd_1 && sd_1.List.Contains(DualDataSignalType.CrossUp))
                    {
                        sd.SendOrder(b.High, 1, OrderType.MidPrice);
                    }
                    else if (bts[b.Time, FiveMinutesCrossSignal_2] is DualDataSignalDatum sd_2 && sd_2.List.Contains(DualDataSignalType.CrossUp))
                    {
                        sd.SendOrder(b.High, 1, OrderType.MidPrice);
                    }
                    else if (bts[b.Time, BarFreq.Minutes_5] is Bar min5_b && (min5_b.Bar_1 is Bar min5_b_1) && (min5_b.High > min5_b_1.High))
                    {
                        sd.SendOrder(b.High, 1, OrderType.MidPrice);
                    }
                }

                #endregion Check profit and stop
            }
        }
    }
}
