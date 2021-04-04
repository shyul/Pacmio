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

namespace Pacmio.Analysis
{
    public class TrailingTrendStrengthAnalysis : BarAnalysis, IDualData
    {
        public TrailingTrendStrengthAnalysis(TrendLineAnalysis tla) : this(tla, tla.MaximumInterval) { }

        public TrailingTrendStrengthAnalysis(TrendLineAnalysis tla, int length)
        {
            TrendLineAnalysis = tla;
            MaximumTrailingLength = Math.Min(length, tla.MaximumInterval);

            HighLowBoundary = new HighLowBoundary(tla.TrailingPivotPointAnalysis.ApexAnalysis.Column_High, tla.TrailingPivotPointAnalysis.ApexAnalysis.Column_Low, MaximumTrailingLength);
            Name = GetType().Name + "_" + tla.Name + "_" + MaximumTrailingLength;

            Column_High = new NumericColumn(Name + "_Bullish", "Bullish");
            Column_Low = new NumericColumn(Name + "_Bearish", "Bearish");

            TrendLineAnalysis.AddChild(HighLowBoundary);
            HighLowBoundary.AddChild(this);
        }

        public int MaximumTrailingLength { get; }

        public TrendLineAnalysis TrendLineAnalysis { get; }

        public HighLowBoundary HighLowBoundary { get; }

        public NumericColumn Column_High { get; }

        public NumericColumn Column_Low { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];

                if (b[TrendLineAnalysis] is TrendLineDatum tpd)
                {
                    double range = (b[HighLowBoundary.Column_High] - b[HighLowBoundary.Column_Low]) / 4; // TODO: Range shall be adjustable too.
                    double middlePoint = (b[TrendLineAnalysis.TrailingPivotPointAnalysis.ApexAnalysis.Column_High] + b[TrendLineAnalysis.TrailingPivotPointAnalysis.ApexAnalysis.Column_Low]) / 2;
                    
                    //Range<double> targetRange = new(middlePoint - range, middlePoint + range);
                    Range<double> targetRange = new(b[HighLowBoundary.Column_Low], b[HighLowBoundary.Column_High]);

                    double bullish = 0, bearish = 0;
                    tpd.Where(n => n.X1 > (i - MaximumTrailingLength) && targetRange.Contains(n.Level(i))).RunEach(n =>
                    {
                        if (n.TrendRate > 0)
                            bullish += Math.Abs(n.Strength);
                        else if (n.TrendRate < 0)
                            bearish -= Math.Abs(n.Strength); // TODO: Separate these into two bullish and bearish, so we can have indecision indication, not accumulation
                    });

                    b[Column_High] = bullish;
                    b[Column_Low] = bearish;
                }
            }
        }

        public Color UpperColor => Color.Green;

        public Color LowerColor => Color.Red;
    }
}
