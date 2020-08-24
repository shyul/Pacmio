/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using Xu;

namespace Pacmio.Analysis
{
    public class AverageGain : ATR
    {
        public AverageGain(int interval) : base(interval)
        {
            GainAnalysis = BarTable.GainAnalysis;
        }

        public GainAnalysis GainAnalysis { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            NumericColumn column = GainAnalysis.Column_Gain;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                double g = bt[i][column];
                g = (g > 0) ? g : 0;

                if (i > 0)
                    bt[i][Column_Result] = (bt[i - 1][Column_Result] * (Interval - 1) + g) / Interval;
                else
                    bt[0][Column_Result] = g;
            }
        }
    }
}
