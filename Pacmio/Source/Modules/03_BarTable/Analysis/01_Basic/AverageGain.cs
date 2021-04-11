/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using Xu;

namespace Pacmio.Analysis
{
    public class AverageGain : IntervalAnalysis
    {
        public AverageGain(int interval) : base(interval)
        {
            Description = "Average Gain " + Label;
        }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                double gain = bt[i].Gain;
                gain = (gain > 0) ? gain : 0;

                if (i > 0)
                    bt[i][Column_Result] = (bt[i - 1][Column_Result] * (Interval - 1) + gain) / Interval;
                else
                    bt[0][Column_Result] = gain;
            }
        }
    }
}
