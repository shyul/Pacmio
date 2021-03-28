/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio.Analysis
{
    public class BandSignal : SignalAnalysis
    {
        public BandSignal(NumericColumn source, IDualData band) 
        {
        
        }

        public override DatumColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {

        }
    }
}
