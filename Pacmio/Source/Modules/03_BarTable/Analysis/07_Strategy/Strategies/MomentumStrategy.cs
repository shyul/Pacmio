/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// Filter:
/// Price $1 to $10
/// 1M in volume, Relative Volume of 2
/// Low Float, less than 2e7 shares
/// 5 min Volume Surge 2000% (20x)
/// 
/// Entry
/// 1. Ideally Next to 9 EMA, or 20 EMA
/// 2. Float under 50 million shares, and prefer under 10 Million shares OR trade with VWAP
/// 3. Bid and ask with in "5 cents" tight spread
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;

namespace Pacmio.Analysis
{
    public class MomentumStrategy : Strategy
    {
        public MomentumStrategy(
            
            double minRiskRewardRatio,
            TimeSpan holdingMaxSpan,
            TimePeriod holdingPeriod,
            TimePeriod tif,
            BarFreq fiveMinFreq,
            BarFreq barFreq = BarFreq.Minute) 
            : base(minRiskRewardRatio, holdingMaxSpan, holdingPeriod, tif, barFreq, PriceType.Trades)
        {
            Filter = new PriceVolumeFilter(1, 10, 1e6, double.MaxValue, BarFreq.Daily, PriceType.Trades);

            EMA9 = new EMA(9);
            FiveMinutesCrossSignal_1 = new DualDataSignal(TimeInForce, fiveMinFreq, EMA9)
            {
                TypeToTrailPoints = new()
                {
                    { DualDataSignalType.CrossUp, new double[] { 10 } },
                    { DualDataSignalType.CrossDown, new double[] { -10 } },
                }
            };

            EMA20 = new EMA(20);
            FiveMinutesCrossSignal_2 = new DualDataSignal(TimeInForce, fiveMinFreq, EMA20)
            {
                TypeToTrailPoints = new()
                {
                    { DualDataSignalType.CrossUp, new double[] { 10 } },
                    { DualDataSignalType.CrossDown, new double[] { -10 } },
                }
            };

            Label = "(" + fiveMinFreq + "," + EMA9.Name + "," + EMA20.Name + "," + minRiskRewardRatio + "," + Filter.Name + "," + barFreq + ")";
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

        /// <summary>
        /// Ideally the entry point shall close to the EMA9 or EMA20 support level.
        /// </summary>
        public MovingAverageAnalysis EMA9 { get; } = new EMA(9);

        public MovingAverageAnalysis EMA20 { get; } = new EMA(20);

        public DualDataSignal FiveMinutesCrossSignal_1 { get; }

        public DualDataSignal FiveMinutesCrossSignal_2 { get; }



        protected override void Calculate(BarAnalysisPointer bap)
        {
           
        }
    }
}
