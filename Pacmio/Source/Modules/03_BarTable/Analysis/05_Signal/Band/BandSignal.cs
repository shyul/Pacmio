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
    public class BandSignal : SignalAnalysis
    {
        public BandSignal(TimePeriod tif, BarFreq barFreq, NumericColumn column, IDualData band, PriceType priceType = PriceType.Trades)
            : base(tif, barFreq, priceType)
        {
            Column = column;
            Band = band;

            string label = "(" + Column.Name + "," + band.Name + ")";
            GroupName = Name = GetType().Name + label;
            Column_Result = new(this, typeof(BandSignalDatum));

            BullishColor = Band.UpperColor;
            BearishColor = Band.LowerColor;

            Band.AddChild(this);
        }

        public NumericColumn Column { get; }

        public IDualData Band { get; }

        public override SignalColumn Column_Result { get; }


        public Dictionary<Range<double>, (BandSignalType Type, double[] Points)> PercentToTrailPoints = new()
        {
            { new Range<double>(double.MinValue, 0), (BandSignalType.LowerPenetrate, new double[] { -7, -5 }) },
            { new Range<double>(0, 10), (BandSignalType.LowerBound, new double[] { -3 }) },
            { new Range<double>(10, 45), (BandSignalType.LowerHalf, new double[] { -1 }) },
            { new Range<double>(45, 55), (BandSignalType.None, new double[] { 0 }) },
            { new Range<double>(55, 90), (BandSignalType.UpperHalf, new double[] { 1 }) },
            { new Range<double>(90, 100), (BandSignalType.UpperBound, new double[] { 3 }) },
            { new Range<double>(100, double.MaxValue), (BandSignalType.UpperPenetrate, new double[] { 7, 5 }) }
        };

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                if (BarFreq >= BarFreq.Daily || TimeInForce.Contains(b.Time))
                {
                    double bbh = b[Band.Column_High];
                    double bbl = b[Band.Column_Low];
                    double reference = b[Column];

                    double span = bbh - bbl;

                    double ratio = 50;

                    if (span != 0)
                        ratio = (reference - bbl) * 100.0 / span;
                    else if (reference > bbh)
                        ratio = 101;
                    else if (reference < bbl)
                        ratio = -1;

                    var datum = PercentToTrailPoints.Where(n => n.Key.Contains(ratio)).Select(n => n.Value).FirstOrDefault();

                    BandSignalDatum d = new(b, Column_Result, ratio)
                    {
                        Type = datum.Type,
                        Difference = span,
                        DifferenceRatio = bbl != 0 ? span / bbl : 0
                    };

                    d.SetPoints(datum.Points);
                }
            }
        }
    }
}
