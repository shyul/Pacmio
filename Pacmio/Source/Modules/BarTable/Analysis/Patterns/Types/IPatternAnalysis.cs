/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public interface IPatternAnalysis
    {
        string Name { get; }

        double Weight { get; }

        double Tolerance { get; }

        ObjectColumn Pattern_Column { get; }
    }
}
