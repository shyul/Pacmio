﻿/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public abstract class PatternAnalysis : BarAnalysis, IChartAnalysis, ILevelAnalysis
    {
        public override void Update(BarAnalysisPointer bap)
        {
            if (!bap.IsUpToDate)
            {
                bap.StopPt = bap.Count;// - 1;

                if (bap.StartPt < 0)
                    bap.StartPt = 0;
                else if (bap.StartPt > bap.StopPt)
                    bap.StartPt = bap.StopPt;// - 1;

                Calculate(bap);
                bap.StartPt = bap.Count;
            }
        }

        public virtual DatumColumn Column_Result { get; protected set; }

        public abstract int MaximumInterval { get; }

        public string AreaName { get; set; } = "None";

        public bool ChartEnabled { get; set; } = true;

        public int DrawOrder { get; set; } = 0;
    }
}
