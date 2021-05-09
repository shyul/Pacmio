/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;
using Pacmio.Analysis;

namespace Pacmio
{
    public class ScalpingStrategy : Strategy
    {
        public ScalpingStrategy(
            MovingAverageAnalysis fast_ma,
            MovingAverageAnalysis middle_ma,
            MovingAverageAnalysis slow_ma,
            int BBInterval,
            double BBSpread,
            double minRiskRewardRatio,
            TimeSpan holdingMaxSpan,
            TimePeriod holdingPeriod,
            TimePeriod tif,
            BarFreq fiveMinFreq,
            BarFreq barFreq = BarFreq.Minute)
            : base(minRiskRewardRatio, holdingMaxSpan, holdingPeriod, tif, barFreq, PriceType.Trades)
        {
            FastMovingAverage = fast_ma;
            MiddleMovingAverage = middle_ma;
            SlowMovingAverage = slow_ma;

            Dictionary<DualDataSignalType, double[]> crossPoints = new()
            {
                { DualDataSignalType.CrossUp, new double[] { 10 } },
                { DualDataSignalType.CrossDown, new double[] { -10 } },
            };

            MinutesCrossSignal_1 = new DualDataSignal(TimeInForce, barFreq, FastMovingAverage, MiddleMovingAverage)
            {
                TypeToTrailPoints = crossPoints
            };


            BollingerBand = new Bollinger(BBInterval, BBSpread);


        }

        public DualDataSignal MinutesCrossSignal_1 { get; }

        public MovingAverageAnalysis FastMovingAverage { get; }
        public MovingAverageAnalysis MiddleMovingAverage { get; }
        public MovingAverageAnalysis SlowMovingAverage { get; }


        public BandSignal BollingerBandSignal { get; }

        public Bollinger BollingerBand { get; }


        public OscillatorSignal StrengthSignal { get; }

        public OscillatorAnalysis StrengthIndicator { get; }




        protected override void Calculate(BarAnalysisPointer bap)
        {

        }
    }
}

