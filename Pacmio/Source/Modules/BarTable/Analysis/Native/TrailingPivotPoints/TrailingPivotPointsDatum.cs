/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using Xu;

namespace Pacmio
{
    public class TrailingPivotPointsDatum : IDatum
    {
        public Range<double> PatternLevelLevelRange { get; set; } = new Range<double>(double.MaxValue, double.MinValue);

        public Dictionary<int, PatternPoint> PositivePatternPointList { get; } = new Dictionary<int, PatternPoint>();

        public Dictionary<int, PatternPoint> NegativePatternPointList { get; } = new Dictionary<int, PatternPoint>();
    }
}
