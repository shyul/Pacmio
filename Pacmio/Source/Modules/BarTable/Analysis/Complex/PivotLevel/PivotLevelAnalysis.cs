/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The intermediate calculation for most pattern analysis
/// 
/// ***************************************************************************

using System;
using Xu;

namespace Pacmio.Analysis
{
    public class PivotLevelAnalysis : BarAnalysis
    {
        public PivotLevelAnalysis()
        {
            Name = GetType().Name;
            Column_Result = new(Name, typeof(PivotLevelDatum));
        }

        public DatumColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (bt[i] is Bar b)
                {
                    double high = b.High;
                    double low = b.Low;
                    double close = b.Close;
                    double p = (high + low + close) / 3;

                    b[Column_Result] = new PivotLevelDatum()
                    {
                        P = p,
                        S1 = (2 * p) - high,
                        S2 = p - (high - low),
                        R1 = (2 * p) - low,
                        R2 = p + (high - low),
                    };
                }
            }
        }
    }
}
