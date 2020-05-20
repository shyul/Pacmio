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
    public class PivotPatternDatum : IPatternDatum
    {
        public Period Length { get; set; }

        public double Weight { get; set; }

        public double Tolerance { get; set; }

        public double R2 { get; set; }

        public double R1 { get; set; }

        public double P { get; set; }

        public double S1 { get; set; }

        public double S2 { get; set; }

        public List<TrendLineInfo> TrendLines { get; } = new List<TrendLineInfo>();
    }
}
