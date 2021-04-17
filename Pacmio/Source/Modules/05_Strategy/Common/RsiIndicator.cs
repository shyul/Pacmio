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
    public class RsiIndicator : Indicator
    {
        public RsiIndicator(BarFreq barFreq, PriceType type) : base(barFreq, type) { }

        // 2 minute RSI of Below 25 / Above 75
        public RSI RSI { get; } = new RSI(14);

        public SingleDataSignal OverBoughtSignal { get; }

        public SingleDataSignal OverSoldSignal { get; }

        public override IEnumerable<SignalColumn> SignalColumns { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {

        }
    }
}
