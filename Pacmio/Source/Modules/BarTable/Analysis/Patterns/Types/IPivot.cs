/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;

namespace Pacmio
{
    public interface IPivot
    {
        IChartPattern Source { get; }

        double Weight { get; }

        double Level { get; }

        double Tolerance { get; }
    }
}
