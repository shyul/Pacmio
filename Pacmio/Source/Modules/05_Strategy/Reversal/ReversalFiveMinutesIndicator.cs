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
    public class ReversalFiveMinutesIndicator : SignalIndicator
    {
        public ReversalFiveMinutesIndicator() : base(BarFreq.Daily, PriceType.Trades)
        {

        }

        public override IEnumerable<SignalColumn> SignalColumns { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {


        }
    }
}
