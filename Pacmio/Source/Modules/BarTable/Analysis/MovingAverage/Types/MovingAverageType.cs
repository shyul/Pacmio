/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public enum MovingAverageType : int
    {
        Simple = 0,
        Smoothed = 1,
        Exponential = 2,
        Weighted = 3,
        Hull = 4,
    }
}
