/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using Xu;

namespace Pacmio.Analysis
{
    public class AverageLoss : AverageGain
    {
        public AverageLoss(int interval) : base(interval) { }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                double loss = bt[i].Gain;
                loss = (loss < 0) ? -loss : 0;

                if (i > 0)
                    bt[i][Column_Result] = (bt[i - 1][Column_Result] * (Interval - 1) + loss) / Interval;
                else
                    bt[0][Column_Result] = loss;
            }
        }
    }
}
