/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Ref 1: https://school.stockcharts.com/doku.php?id=technical_indicators:dppmo
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public sealed class DPPMO : ATR, IOscillator
    {
        public double Reference => throw new NotImplementedException();

        public double UpperLimit => throw new NotImplementedException();

        public double LowerLimit => throw new NotImplementedException();

        public Color UpperColor => throw new NotImplementedException();

        public Color LowerColor => throw new NotImplementedException();

        protected override void Calculate(BarAnalysisPointer bap)
        {

        }
    }
}
