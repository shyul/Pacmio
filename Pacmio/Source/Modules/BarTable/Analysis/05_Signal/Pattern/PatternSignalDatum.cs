/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio
{
    public class PatternSignalDatum : SignalDatum
    {
        public PatternSignalDatum(Bar b, SignalColumn column) : base(b, column) { }

        public double Reversal { get; }

        public double Momentum { get; }

        // Trending up or trending down? TrendLineDatum shall provide the information!!
    }
}
