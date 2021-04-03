/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using Xu;

namespace Pacmio.Analysis
{
    public class PivotLevelAnalysis : BarAnalysis, ILevelAnalysis
    {
        public PivotLevelAnalysis()
        {
            Name = GetType().Name;
            Column_Result = new(Name, typeof(PivotLevelDatum));
            AreaName = MainBarChartArea.DefaultName;
        }

        public DatumColumn Column_Result { get; }

        public string AreaName { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                double high = b.High;
                double low = b.Low;
                double p = b.Typical;

                var result = new PivotLevelDatum()
                {
                    P = p,
                    S1 = (2 * p) - high,
                    S2 = p - (high - low),
                    R1 = (2 * p) - low,
                    R2 = p + (high - low),
                };

                result.Levels.AddRange(new Level[] {
                    new Level(result.P),
                    new Level(result.S1),
                    new Level(result.S2),
                    new Level(result.R1),
                    new Level(result.R2),
                });

                b[Column_Result] = result;
            }
        }
    }
}
