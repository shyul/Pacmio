/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************


namespace Pacmio
{
    public class AverageLoss : AverageGain
    {
        public AverageLoss(int interval) : base(interval) { }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                double g = bt[i].Gain;
                g = (g < 0) ? -g : 0;

                if (i > 0)
                    bt[i][Result_Column] = (bt[i - 1][Result_Column] * (Interval - 1) + g) / Interval;
                else
                    bt[0][Result_Column] = g;
            }
        }
    }
}
