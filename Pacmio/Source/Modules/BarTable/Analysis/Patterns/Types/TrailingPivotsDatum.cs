/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using Xu;

namespace Pacmio
{
    public class TrailingPivotsDatum
    {
        public Range<double> LevelRange { get; set; } = new Range<double>(double.MaxValue, double.MinValue);

        public Dictionary<int, Pivot> PositiveList { get; } = new Dictionary<int, Pivot>();

        public Dictionary<int, Pivot> NegativeList { get; } = new Dictionary<int, Pivot>();
    }
}
