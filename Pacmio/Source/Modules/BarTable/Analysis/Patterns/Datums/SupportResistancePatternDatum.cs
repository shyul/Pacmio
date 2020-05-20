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
    public class SupportResistancePatternDatum : IPatternDatum
    {
        public Dictionary<Range<double>, int> Rank { get; } = new Dictionary<Range<double>, int>();

        public List<TrendLineInfo> TrendLines { get; } = new List<TrendLineInfo>();
    }
}
