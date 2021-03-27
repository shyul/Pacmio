/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// BarTable Data Types
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;
using Xu.Chart;

namespace Pacmio.Analysis
{
    public class Level : ILevel
    {
        public Level(double val, double strength = 1)
        {
            Value = val;
            Strength = strength;
        }

        public double Value { get; }

        public double Strength { get; }
    }
}
