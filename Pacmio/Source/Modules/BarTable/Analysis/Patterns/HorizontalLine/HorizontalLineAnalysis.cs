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
    public class HorizontalLineAnalysis : PatternAnalysis, IChartBackground
    {
        public override int MaximumInterval => throw new NotImplementedException();

        protected override void Calculate(BarAnalysisPointer bap)
        {

        }

        public void DrawBackground(Graphics g, BarChart bc)
        {

        }
    }
}
