/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public sealed class NativeBarAnalysis : BarAnalysis, ISingleData, IDualData
    {
        public NumericColumn Column_Result => Bar.Column_Close;

        public NumericColumn Column_High => Bar.Column_High;

        public NumericColumn Column_Low => Bar.Column_Low;

        public Color UpperColor => Color.Green;

        public Color LowerColor => Color.Red;

        protected override void Calculate(BarAnalysisPointer bap) { }
    }
}
