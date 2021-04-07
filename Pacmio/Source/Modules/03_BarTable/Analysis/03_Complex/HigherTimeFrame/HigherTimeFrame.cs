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
    public class HigherTimeFrame : BarAnalysis, ISingleDatum
    {
        public HigherTimeFrame(BarFreq freq, DataType type)
        {
            Name = GetType().Name + "_" + freq + "_" + type;
            Column_Result = new(Name, typeof(HigherTimeFrameDatum));
        }

        public BarFreq BarFreq { get; }

        public DataType DataType { get; }

        public DatumColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            BarTable bt_ht = bt.BarTableSet[BarFreq, DataType];

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                Bar b_ht = bt_ht[b.Time];

                b[Column_Result] = new HigherTimeFrameDatum(b_ht);
            }
        }
    }
}
