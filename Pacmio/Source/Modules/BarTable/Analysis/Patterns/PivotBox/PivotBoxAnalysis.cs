/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public class PivotBoxAnalysis : BarAnalysis
    {
        public PivotBoxAnalysis() 
            => Name = GetType().Name;
        
        public override void Update(BarAnalysisPointer bap) // Cancellation Token should be used
        {
            if (!bap.IsUpToDate && bap.Count > 0)
            {
                bap.StopPt = bap.Count - 1;

                if (bap.StartPt < 0)
                    bap.StartPt = 0;

                Calculate(bap);
                bap.StartPt = bap.StopPt;
                bap.StopPt++;
            }
        }

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (bt[i] is Bar b) b.CalculatePivotBoxDatums();
            }
        }
    }
}
