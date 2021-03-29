﻿/// ***************************************************************************
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
    public class DualDataSignalDatum : SignalDatum
    {
        public DualDataSignalDatum(Bar b, SignalColumn column) : base(b, column) { }

        public List<DualDataSignalType> List { get; } = new List<DualDataSignalType>();

        public double Ratio { get; set; }

        public double Difference { get; set; }

        public double DifferenceRatio { get; set; }
    }
}