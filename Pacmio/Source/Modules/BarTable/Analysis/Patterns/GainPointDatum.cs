﻿/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using Xu;

namespace Pacmio
{
    public class GainPointColumn : Column
    {
        public GainPointColumn(string name) => Name = Label = name;

        public GainPointColumn(string name, string label)
        {
            Name = name;
            Label = label;
        }
    }

    public class GainPointDatum
    {
        public Dictionary<int, (DateTime time, double value, double prominence, double trendStrength)> PositiveList { get; } = new Dictionary<int, (DateTime time, double value, double prominence, double trendStrength)>();
        public Dictionary<int, (DateTime time, double value, double prominence, double trendStrength)> NegativeList { get; } = new Dictionary<int, (DateTime time, double value, double prominence, double trendStrength)>();
    }
}
