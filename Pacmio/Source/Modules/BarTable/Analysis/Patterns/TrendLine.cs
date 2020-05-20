/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public sealed class TrendLine : BarAnalysis, IPatternAnalysis
    {
        public double Weight => throw new NotImplementedException();

        public double Tolerance => throw new NotImplementedException();

        public ObjectColumn Pattern_Column => throw new NotImplementedException();

        protected override void Calculate(BarAnalysisPointer bap)
        {

        }
    }
}
