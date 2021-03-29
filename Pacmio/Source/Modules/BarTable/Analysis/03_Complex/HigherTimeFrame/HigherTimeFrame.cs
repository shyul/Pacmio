/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio.Analysis
{
    public class HigherTimeFrame : BarAnalysis, ISingleComplex
    {
        public HigherTimeFrame(BarTableGroup group, BarFreq freq, DataType type)
        {
            Name = GetType().Name + "_" + freq + "_" + type + "_" + group.GetHashCode();
            Column_Result = new(Name, typeof(HigherTimeFrameDatum));
        }

        public BarFreq BarFreq { get; }

        public DataType DataType { get; }

        public BarTableGroup BarTableGroup { get; }

        public DatumColumn Column_Result { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            BarTable bt_ht = BarTableGroup.GetOrCreateBarTable(bt.Contract, BarFreq, DataType);


            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                Bar b = bt[i];
                Bar b_ht = bt_ht[b.Time];

                b[Column_Result] = new HigherTimeFrameDatum(b_ht);
            }
        }
    }
}
