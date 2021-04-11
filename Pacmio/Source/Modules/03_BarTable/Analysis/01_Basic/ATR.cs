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
    public sealed class ATR : IntervalAnalysis
    {
        public ATR(int interval = 14) : base(interval)
        {
            Description = "Average True Range " + Label;
        }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                double tr = bt[i].TrueRange;

                if (i < Interval)
                {
                    double tr_ma = 0;
                    for (int j = 0; j < Interval; j++)
                    {
                        int k = i - j;
                        if (k < 0) k = 0;
                        tr_ma += bt[k].TrueRange;
                    }
                    bt[i][Column_Result] = tr_ma / Interval;
                }
                else
                {
                    double prior_tr_ma = bt[i - 1][Column_Result];
                    bt[i][Column_Result] = (tr + (prior_tr_ma * (Interval - 1))) / Interval;
                }
            }
        }
    }
}
