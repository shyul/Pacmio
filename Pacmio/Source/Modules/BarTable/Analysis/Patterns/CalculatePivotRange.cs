/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public class CalculatePivotRange : BarAnalysis
    {
        public CalculatePivotRange() => Name = GetType().Name;

        protected override void Calculate(BarAnalysisPointer bap)
        {
            BarTable bt = bap.Table;

            int min_peak_start = bap.StopPt - BarTable.PivotPointAnalysis.MaximumPeakProminence * 2 - 1;
            if (bap.StartPt > min_peak_start)
                bap.StartPt = min_peak_start;
            else if (bap.StartPt < 0)
                bap.StartPt = 0;

            for (int i = bap.StartPt; i < bap.StopPt; i++)
            {
                if (bt[i] is Bar b) b.CalculatePivotRange();
            }
        }
    }
}
