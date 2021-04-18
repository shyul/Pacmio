/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public abstract class MovingAverageAnalysis : CustomIntervalAnalysis
    {
        protected MovingAverageAnalysis(NumericColumn column, int interval) : base(column, interval) { }

        protected MovingAverageAnalysis() { }
    }
}
