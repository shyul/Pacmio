﻿/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    /// <summary>
    /// TODO: make this a specical case.......
    /// TODO: Do we really need this ???
    /// </summary>
    public sealed class HighTimeFrameSeries : BarAnalysis
    {
        public HighTimeFrameSeries(BarTableSet bts, BarFreq sourceBarFreq, NumericColumn source_column) 
        {
            BarTableSet = bts;
            SourceBarFreq = sourceBarFreq;
            SourceFrequency = SourceBarFreq.GetAttribute<BarFreqInfo>().Result.Frequency;
            Source_Column = source_column;


        }

        public BarFreq SourceBarFreq { get; private set; }

        public Frequency SourceFrequency { get; }

        public NumericColumn Source_Column { get; }

        public NumericColumn Result_Column { get; }

        public BarTableSet BarTableSet { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;
            if (BarTableSet.Get(bt.Contract, SourceBarFreq, bt.Type) is BarTable bt_high)
            {
                for (int i = bap.StartPt; i < bap.StopPt; i++)
                {
                    Bar b = bt[i];
                    DateTime time = SourceFrequency.Align(b.Time);

                    Bar b_high = bt_high[time];

                    b[Result_Column] = b_high[Source_Column];

                }
            }
        }
    }
}
