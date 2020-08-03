/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// https://school.stockcharts.com/doku.php?id=technical_indicators:ultimate_oscillator
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
    public sealed class ULT : ATR, IOscillator
    {
        public double Reference => throw new NotImplementedException();

        public double LowerLimit => throw new NotImplementedException();

        public double UpperLimit => throw new NotImplementedException();

        public Color LowerColor => throw new NotImplementedException();

        public Color UpperColor => throw new NotImplementedException();

        protected override void Calculate(BarAnalysisPointer bap)
        {
            throw new NotImplementedException();
        }
    }
}
