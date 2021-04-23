/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;

namespace Pacmio.Analysis
{
    public class GapGoIntermediateIndicator : Indicator
    {
        public GapGoIntermediateIndicator(BarFreq brafreq) : base(brafreq, PriceType.Trades)
        {

        }

        public MovingAverageAnalysis MovingAverage_1 { get; }

        public MovingAverageAnalysis MovingAverage_2 { get; }

        public override IEnumerable<SignalColumn> SignalColumns { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
            }
        }
    }
}
