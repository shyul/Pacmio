/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public class DivergenceDatum : SignalDatum
    {
        public DivergenceDatum(Bar b, SignalColumn column) : base(b, column) { }

        public DivergenceSignalType Type { get; set; }
    }
}
