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
    public class BandSignalDatum : SignalDatum, IDifferentialDatum
    {
        public BandSignalDatum(Bar b, SignalColumn column, double ratio) : base(b, column)
        {
            Ratio = ratio;
        }

        public BandSignalType Type { get; set; }

        public double Ratio { get; } = 50;

        public double Difference { get; set; }

        public double DifferenceRatio { get; set; }

        public override string Description => Type.ToString() + " " + Ratio.ToString("0.##") + "%";
    }
}
