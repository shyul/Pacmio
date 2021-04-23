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
        public GapGoOrbStrategy(BarFreq barFreq) : base(barFreq, PriceType.Trades)
        {





            Column_Result = new(Name, typeof(StrategyDatum));
            Time start = new Time(9, 30);
            Time stop = new Time(10, 00); // start.AddSeconds(Frequency.Span.TotalSeconds.ToInt32());
            TimeInForce = new TimePeriod(start, stop);

            SignalColumns = new SignalColumn[] { };
            SignalSeries = new(this);
            BarAnalysisSet = new(this);
        }


        public override Filter Filter { get; } // = new MomentumDailyFilter();

        public override IEnumerable<SignalColumn> SignalColumns { get; }


        public GapGoIntermediateIndicator IntermediateIndicator { get; }

        public SignalColumn VolumeSignal { get; }

        public double MinimumVolume { get; } = 1e5;

        public SignalColumn RelativeVolumeSignal { get; }

        public TimeFrameRelativeVolume RelativeVolume { get; }

        public double MinimumRelativeVolume { get; } = 2;

        public double RewardRiskRatio { get; } = 2;

        public const string OpenBarMessage = "OpenBar";
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

                if (testPeriods.Contains(b.Time))
                {
                    if (b.Time == TimeInForce.Start)
                    {
                        if (b.Volume > MinimumVolume && b[RelativeVolume] > MinimumRelativeVolume)
                        {
                            sd.Message = OpenBarMessage;
                        }
                    }

                    if (sd.Datum_1.Message == OpenBarMessage)
                    {
                        Bar ob = sd.Datum_1.Bar;


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

                // Dedicated function to handle stop out here!
                sd.GuardStopLoss();

                






                // Profit taking here.
                if (sd.Quantity > 0) // || sd.Datum_1.Message == RangeBarBullishMessage)
                {
                    // Find exit signals
                    if (bts[b.Time, IntermediateIndicator] is Bar time_frame_b)
                    {
                        double ema_1 = time_frame_b[IntermediateIndicator.MovingAverage_1];
                        double ema_2 = time_frame_b[IntermediateIndicator.MovingAverage_2];



                        // New (5 Minutes) Low
                        if ((time_frame_b.Bar_1 is Bar b_1) && (time_frame_b.Low < b_1.Low))
                        {
                            sd.SendOrder(time_frame_b.Low, -1, OrderType.MidPrice);

                        }
                    }







                    if (b.Contains(sd.ProfitTakePrice))
                    {
                        sd.SendOrder(sd.ProfitTakePrice, -0.5, OrderType.Limit);
                        sd.ProfitTakePrice += sd.RiskPart;
                    }
                    else if (b.Low > sd.ProfitTakePrice)
                    {
                        sd.SendOrder(b.Low, -0.5, OrderType.Limit);
                        sd.ProfitTakePrice = b.Low + sd.RiskPart;
                    }

                    sd.StopLossPrice = Math.Max(sd.AveragePrice, sd.StopLossPrice + sd.RiskPart);
                }
                else if (sd.Quantity < 0) // || sd.Datum_1.Message == RangeBarBearishMessage)
                {
                    // Find exit signals
                    if (bts[b.Time, IntermediateIndicator] is Bar time_frame_b)
                    {
                        double ema_1 = time_frame_b[IntermediateIndicator.MovingAverage_1];
                        double ema_2 = time_frame_b[IntermediateIndicator.MovingAverage_2];

                        // if (cross above ema_1 or cross above ema_2) then exit

                        // New (5 Minutes) High
                        if ((time_frame_b.Bar_1 is Bar b_1) && (time_frame_b.High < b_1.High))
                        {
                            sd.SendOrder(time_frame_b.High, 1, OrderType.MidPrice);
                        }
                    }

                    if (b.Contains(sd.ProfitTakePrice))
                    {
                        sd.SendOrder(sd.ProfitTakePrice, 0.5, OrderType.Stop);
                        sd.ProfitTakePrice -= sd.RiskPart;
                    }
                    else if (b.High < sd.ProfitTakePrice)
                    {
                        sd.SendOrder(b.High, 0.5, OrderType.Limit);
                        sd.ProfitTakePrice = b.High - sd.RiskPart;
                    }

                    sd.StopLossPrice = Math.Min(sd.AveragePrice, sd.StopLossPrice - sd.RiskPart);
                }

                // Time stop
            }
        }
    }
}
