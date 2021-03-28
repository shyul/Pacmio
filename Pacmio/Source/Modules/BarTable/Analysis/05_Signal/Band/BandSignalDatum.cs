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

namespace Pacmio.Analysis
{
    public class BandSignalDatum : SignalDatum
    {
        public BandSignalDatum(Bar b, SignalColumn column, double position) : base(b, column) 
        {
            Position = position;
        }

        public BandSignalType Type { get; set; }

        public double Position { get; } = 50;

        public override string Description => Type.ToString() + " " + Position.ToString("0.##") + "%";
    }
}
