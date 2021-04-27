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
            ISingleData crossData_1, // new EMA(9);
            ISingleData crossData_2, // new EMA(20);
            double gap = 4,
            double aboveVolume = 5e5,
            double belowVolume = double.MaxValue,
            double abovePrice = 1,
            double belowPrice = 300,
            BarFreq barFreq = BarFreq.Minute,
            BarFreq fiveMinFreq = BarFreq.Minutes_5)
            : base(barFreq, PriceType.Trades)
        {


            MinimumMinuteVolume = 1e5;
            MinimumMinuteRelativeVolume = 2;
            RewardRiskRatio = 2;

            #region Define Filter

            DailyPriceFilterSignal =
                new SingleDataSignal(BarFreq.Daily, Bar.Column_Typical, new Range<double>(abovePrice, belowPrice))
                { TypeToTrailPoints = new() { { SingleDataSignalType.Within, new double[] { 1 } } } };

            DailyVolumeFilterSignal =
                new SingleDataSignal(BarFreq.Daily, Bar.Column_Volume, new Range<double>(aboveVolume, belowVolume))
                { TypeToTrailPoints = new() { { SingleDataSignalType.Within, new double[] { 1 } } } };

            DailyGapPercentFilterSignal = new SingleDataSignal(BarFreq.Daily, Bar.Column_GapPercent, new Range<double>(-gap, gap))
            {
                TypeToTrailPoints = new()
                {
                    { SingleDataSignalType.Above, new double[] { 1 } },
                    { SingleDataSignalType.Below, new double[] { 1 } },
                }
            };

            Filter = new(new SignalAnalysis[] {
                DailyPriceFilterSignal,
                DailyVolumeFilterSignal,
                DailyGapPercentFilterSignal },
                3,
                3,
                Bar.Column_GainPercent);

            #endregion Define Filter

            #region Define Signals

            FiveMinutesCrossData_1 = crossData_1; 
            FiveMinutesCrossSignal_1 = new DualDataSignal(fiveMinFreq, FiveMinutesCrossData_1)
            {
                TypeToTrailPoints = new()
                {
                    { DualDataSignalType.CrossUp, new double[] { 10 } },
                    { DualDataSignalType.CrossDown, new double[] { -10 } },
                }
            };

            FiveMinutesCrossData_2 = crossData_2; 
            FiveMinutesCrossSignal_2 = new DualDataSignal(fiveMinFreq, FiveMinutesCrossData_2)
            {
                TypeToTrailPoints = new()
                {
                    { DualDataSignalType.CrossUp, new double[] { 10 } },
                    { DualDataSignalType.CrossDown, new double[] { -10 } },
                }
            };

            SignalAnalysisSet = new SignalAnalysisSet(new SignalAnalysis[]
            {


                FiveMinutesCrossSignal_1,
                FiveMinutesCrossSignal_2,
            });

            #endregion Define Signals



            Column_Result = new(Name, typeof(StrategyDatum));
            Time start = new Time(9, 30);
            Time stop = new Time(10, 00);
            TimeInForce = new TimePeriod(start, stop);
            PositionHoldingPeriod = new TimePeriod(start, new Time(12, 00));
        }

        public override Filter Filter { get; }
        public override SignalAnalysisSet SignalAnalysisSet { get; }


        public SingleDataSignal DailyPriceFilterSignal { get; }
        public SingleDataSignal DailyVolumeFilterSignal { get; }
        public SingleDataSignal DailyGapPercentFilterSignal { get; }


        public DualDataSignal FiveMinutesCrossSignal_1 { get; }
        public ISingleData FiveMinutesCrossData_1 { get; }
        public DualDataSignal FiveMinutesCrossSignal_2 { get; }
        public ISingleData FiveMinutesCrossData_2 { get; }



        public TimeFrameRelativeVolume RelativeVolume { get; }
        public TimeFramePricePosition PricePosition { get; }



        public double MinimumMinuteVolume { get; }
        public double MinimumMinuteRelativeVolume { get; }
        public double RewardRiskRatio { get; }



        public const string BullishOpenBarMessage = "BullishOpenBar";
        public const string BearishOpenBarMessage = "BearishOpenBar";

        public const string RangeBarBullishMessage = "RangeBarBullish";
        public const string RangeBarBearishMessage = "RangeBarBearish";

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
                        if (b.Volume > MinimumMinuteVolume && b[RelativeVolume] > MinimumMinuteRelativeVolume)
                        {
                            var (bullish, bearish, _) = Filter.Calculate(b);
                            if (bullish && b[PricePosition] > 0.75)
                            {
                                sd.Message = BullishOpenBarMessage;
                            }
                            else if (bearish && b[PricePosition] < 0.25)
                            {
                                sd.Message = BearishOpenBarMessage;
                            }
                        }
                    }

                    if (sd.Datum_1.Message == BullishOpenBarMessage)
                    {
                        Bar ob = sd.Datum_1.Bar;

                        // TODO: here
                        // Verify higher time frame analysis and indicators 

                        if (string.IsNullOrEmpty(sd.Message))
                        {
                            if (b.Contains(ob.High)) // Bullish / long side no gap entry
                            {
                                sd.StopLossPrice = ob.Low;
                                sd.RiskPart = ob.High - ob.Low;
                                sd.ProfitTakePrice = ob.High + RewardRiskRatio * sd.RiskPart;
                                sd.Message = RangeBarBullishMessage;
                                sd.EntryBarIndex = 0;
                                sd.SendOrder(ob.High, 1, OrderType.Stop);
                            }
                            else if (b.Contains(ob.Low)) // Bearish / short side no gap entry
                            {
                                sd.StopLossPrice = ob.High;
                                sd.RiskPart = ob.High - ob.Low;
                                sd.ProfitTakePrice = ob.Low - RewardRiskRatio * sd.RiskPart;
                                sd.Message = RangeBarBearishMessage;
                                sd.EntryBarIndex = 0;
                                sd.SendOrder(ob.Low, -1, OrderType.Limit);
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
