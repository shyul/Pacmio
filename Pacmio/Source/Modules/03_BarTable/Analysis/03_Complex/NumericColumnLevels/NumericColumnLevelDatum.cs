/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// BarTable Data Types
/// 
/// ***************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public class NumericColumnLevelDatum : ILevelDatum
    {
        public List<Level> Levels { get; } = new();
    }
}
