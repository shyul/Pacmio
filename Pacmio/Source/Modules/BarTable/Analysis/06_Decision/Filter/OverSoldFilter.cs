/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xu;

namespace Pacmio.Analysis
{
    public class OverSoldFilter : IndicatorFilter
    {
        public override double HighScoreLimit => 100;

        public override double LowScoreLimit => -100;

        public override IEnumerable<SignalColumn> SignalColumns { get; } = new List<SignalColumn>();

        //public Indicator 

        public IOscillator OverSoldAnalysis { get; set; }

        protected override void Calculate(BarAnalysisPointer bap)
        {

        }
    }
}
