/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;

namespace Pacmio
{
    public class OscillatorSignal : SignalAnalysis
    {
        public OscillatorSignal(TimePeriod tif, BarFreq barFreq, IOscillator iosc, PriceType priceType = PriceType.Trades)
            : base(tif, barFreq, priceType)
        {
            OscillatorAnalysis = iosc;

            Label = "(" + iosc.Name + "," + barFreq + "," + priceType + ")";
            GroupName = Name = GetType().Name + Label;
            Column_Result = new(this, typeof(OscillatorSignalDatum));

            BullishColor = OscillatorAnalysis.UpperColor;
            BearishColor = OscillatorAnalysis.LowerColor;

            OscillatorAnalysis.AddChild(this);
        }

        public IOscillator OscillatorAnalysis { get; }

        public double UpperLimit => OscillatorAnalysis.UpperLimit;

        public double LowerLimit => OscillatorAnalysis.LowerLimit;

        public Dictionary<Range<double>, double[]> LevelToTrailPoints { get; set; } = new()
        {
            { new Range<double>(double.MinValue, 5), new double[] { -7, -5 } },
            { new Range<double>(5, 10), new double[] { -3 } },
            { new Range<double>(10, 20), new double[] { -1 } },
            { new Range<double>(20, 80), new double[] { 0 } },
            { new Range<double>(80, 90), new double[] { 1 } },
            { new Range<double>(90, 95), new double[] { 3 } },
            { new Range<double>(95, double.MaxValue), new double[] { 7, 5 } }
        };

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                if (BarFreq >= BarFreq.Daily || TimeInForce.Contains(b.Time))
                {
                    OscillatorSignalDatum d = new(b, Column_Result);

                    double rsi = b[OscillatorAnalysis];

                    if (rsi >= UpperLimit)
                        d.Type = OscillatorSignalType.OverBought;
                    else if (rsi <= LowerLimit)
                        d.Type = OscillatorSignalType.OverSold;

                    d.SetPoints(LevelToTrailPoints.Where(n => n.Key.Contains(rsi)).Select(n => n.Value).FirstOrDefault());
                }
            }
        }
    }
}
