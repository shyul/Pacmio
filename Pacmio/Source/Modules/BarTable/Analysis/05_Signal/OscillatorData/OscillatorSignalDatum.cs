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
    public class OscillatorSignalDatum : SignalDatum
    {
        public OscillatorSignalDatum(Bar b, SignalColumn column) : base(b, column) { }

        public OscillatorSignalType Type { get; set; } = OscillatorSignalType.None;

        public override string Description => Type.ToString();
    }
}
