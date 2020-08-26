/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// The intermediate calculation for most pattern analysis
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using Xu;

namespace Pacmio.Analysis
{
    public class PivotLevels : BarAnalysis
    {
        public PivotLevels(BarTableSet barTableSet, BarFreq barFreq = BarFreq.Daily)
        {
            BarTableSet = barTableSet;
            BarFreq = barFreq;
            Frequency = BarFreq.GetAttribute<BarFreqInfo>().Result.Frequency;


        }

        public BarTableSet BarTableSet { get; }

        public BarFreq BarFreq { get; }

        public Frequency Frequency { get; }

        public PatternColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            Contract c = bt.Contract;

            BarTable higer_time_frame_table = BarTableSet.Get(c, BarFreq, bt.Type);



            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (bt[i] is Bar b)
                {
                    DateTime time = Frequency.Align(b.Time);



                    int index = higer_time_frame_table.IndexOf(ref time);

                    if (index > 1)
                    {
                        index--;

                        Bar xb = higer_time_frame_table[index];

                        double high = xb.High;
                        double low = xb.Low;
                        double close = xb.Close;

                        double p = (high + low + close) / 3;
                        double s1 = (2 * p) - high;
                        double s2 = p - (high - low);
                        double r1 = (2 * p) - low;
                        double r2 = p + (high - low);

                        //PatternDatum pd = new PatternDatum(center - range_delta, center + range_delta);

                    }
                }

            }
        }
    }
}
