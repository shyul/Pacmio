/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio
{
    public class HigherTimeBar : BarAnalysis, ISingleDatum
    {
        public HigherTimeBar(BarFreq freq, PriceType type = PriceType.Trades)
        {
            BarFreq = freq;
            PriceType = type;
            Label = "(" + freq + "," + type + ")";
            Name = GetType().Name + Label;
            Column_Result = new(Name, typeof(HigherTimeBarDatum));
        }

        public BarFreq BarFreq { get; }

        public PriceType PriceType { get; }

        public DatumColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            BarTable bt_ht = bt.BarTableSet[BarFreq, PriceType];

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                Bar b_ht = bt_ht[b.Time];

                b[Column_Result] = new HigherTimeBarDatum(b_ht);
            }
        }
    }
}
