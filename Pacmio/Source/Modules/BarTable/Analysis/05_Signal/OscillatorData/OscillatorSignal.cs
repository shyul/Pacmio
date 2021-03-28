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
    public class OscillatorSignal : SignalAnalysis
    {
        public OscillatorSignal(IOscillator iosc)
        {
            OscillatorAnalysis = iosc;

            string label = "(" + iosc.Name + ")";
            GroupName = Name = GetType().Name + label;
            Column_Result = new(Name, typeof(OscillatorSignalDatum));

            iosc.AddChild(this);
        }

        public IOscillator OscillatorAnalysis { get; }

        public double UpperLimit => OscillatorAnalysis.UpperLimit;

        public double LowerLimit => OscillatorAnalysis.LowerLimit;

        public override DatumColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];

                OscillatorSignalDatum d = new();
                b[Column_Result] = d;
            }
        }
    }
}
