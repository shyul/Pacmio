/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// 1. Pivots
/// 2. Trend Strength
/// 
/// ***************************************************************************

using System;
using System.Linq;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public sealed class NativePivotAnalysis : PeakAnalysis
    {
        public NativePivotAnalysis() 
        {
            // Add Parents
        }

        public override int GetHashCode() => GetType().GetHashCode();

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
            }
        }
    }
}
