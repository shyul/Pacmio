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

namespace Pacmio
{
    public class Level
    {
        public Level(double val, double strength = 1)
        {
            LevelValue = val;
            Strength = strength;
        }

        public double LevelValue { get; }

        public double Strength { get; }
    }
}
