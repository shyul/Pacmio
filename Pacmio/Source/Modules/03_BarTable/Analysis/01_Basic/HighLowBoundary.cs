/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Linq;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public sealed class HighLowBoundary : BarAnalysis, IDualData
    {
        public HighLowBoundary(NumericColumn column_High, NumericColumn column_Low, int length = 200)
        {
            MaximumTrailingLength = length;

            Source_Column_High = column_High;
            Source_Column_Low = column_Low;

            Name = GetType().Name + "_" + column_High.Name + "_" + column_Low.Name + "_" + MaximumTrailingLength;

            Column_High = new NumericColumn(Name + "_High", "High");
            Column_Low = new NumericColumn(Name + "_Low", "Low");
        }

        public HighLowBoundary(IDualData analysis, int length = 200)
        {
            MaximumTrailingLength = length;

            Source_Column_High = analysis.Column_High;
            Source_Column_Low = analysis.Column_Low;

            Name = GetType().Name + "_" + analysis.Name + "_" + MaximumTrailingLength;

            Column_High = new NumericColumn(Name + "_High", "High");
            Column_Low = new NumericColumn(Name + "_Low", "Low");

            analysis.AddChild(this);
        }

        public int MaximumTrailingLength { get; }

        //public IDualData DualData { get; }

        public NumericColumn Source_Column_High { get; }

        public NumericColumn Source_Column_Low { get; }

        public NumericColumn Column_High { get; }

        public NumericColumn Column_Low { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                var bars = bt[i, MaximumTrailingLength];
                double high = bars.Select(n => n[Source_Column_High]).Max();
                double low = bars.Select(n => n[Source_Column_Low]).Min();
                b[Column_High] = high;
                b[Column_Low] = low;
            }
        }

        public Color UpperColor => Color.Green;

        public Color LowerColor => Color.Red;
    }
}
