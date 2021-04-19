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
            Time stop = start.AddSeconds(Frequency.Span.TotalSeconds.ToInt32());
            TimeInForce = new TimePeriod(start, stop);

            SignalColumns = new SignalColumn[] { };
            SignalSeries = new(this);
            BarAnalysisSet = new(this);
        }


        public override Filter Filter { get; } // = new MomentumDailyFilter();

        public override IEnumerable<SignalColumn> SignalColumns { get; }

        public SignalColumn VolumeSignal { get; }

        public double MinimumVolume { get; } = 1e5;

        public SignalColumn RelativeVolumeSignal { get; }

        public TimeFrameRelativeVolume RelativeVolume { get; }

        public double MinimumRelativeVolume { get; } = 2;

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            BarTableSet bts = bt.BarTableSet;

            Bar OpenBar = null;

            if (bap.IsLive)
            {
                // Position Source is Lookup Table Strategy to Position (under account...)
                // Check if the quantity is acknowledged or not??
            }
            else
            {
                // Position Source is from position simulation calculator
            }


            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                StrategyDatum sd = b[this];

                // Calculate/Get the position location.

                if (TimeInForce.Contains(b.Time) && sd.Quantity == 0) // Entry, quantity == 0 and no pending order by the strategy
                {
                    Bar time_frame_b = bts[b.Time, Filter];

                    if (TimeInForce.Start == b.Time)
                    {
                        OpenBar = b;
                    }
                    else if (OpenBar is Bar ob && ob.Volume > MinimumVolume && ob[RelativeVolume] > MinimumRelativeVolume && b.Index == ob.Index + 1)
                    {
                        double bull_buy_stop = ob.High;
                        double bear_short_limit = ob.Low;

                        if (b.Close > bull_buy_stop)
                        {
                            sd.Decision = new EntryDecision()
                            {
                                Type = EntryType.BuyStop,
                                ExecutionPrice = ob.High,
                                StopLossPrice = ob.Low,
                                ProfitTakePrice = ob.High + 2 * (ob.High - ob.Low)
                            };

                        }
                        else if (b.Close < bear_short_limit)
                        {
                            sd.Decision = new EntryDecision()
                            {
                                Type = EntryType.SellLimit,
                                ExecutionPrice = ob.Low,
                                StopLossPrice = ob.High,
                                ProfitTakePrice = ob.Low - 2 * (ob.High - ob.Low)
                            };
                        }

                    }
                }
                else // and Exit
                {

                }

                if (!bap.IsLive) // Calculate the position when the table is not Alive. 
                {
                    switch (sd.Decision)
                    {




                        default: break;
                    }

                }
            }
        }
    }
}
