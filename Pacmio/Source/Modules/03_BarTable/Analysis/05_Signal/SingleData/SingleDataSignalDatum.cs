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
using Xu;

namespace Pacmio
{
    public class SingleDataSignalDatum : SignalDatum, IDifferentialDatum
    {
        public SingleDataSignalDatum(Bar b, SignalColumn column) : base(b, column) { }

        public List<SingleDataSignalType> List { get; } = new();

        public double Ratio { get; set; }

        public double Difference { get; set; }

        public double DifferenceRatio { get; set; }

        public override string Description => List.ToString(',');
    }

    public interface IDifferentialDatum
    {
        double Ratio { get; }

        double Difference { get; }

        double DifferenceRatio { get; }
    }
}
