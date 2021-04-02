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
        public double Reference { get; set; } = 0;

        public double UpperLimit { get; set; } = 3;

        public double LowerLimit { get; set; } = -3;

        public Color UpperColor { get; set; } = Color.ForestGreen;

        public Color LowerColor { get; set; } = Color.Crimson;

        protected override void Calculate(BarAnalysisPointer bap)
        {

        }
    }
}
