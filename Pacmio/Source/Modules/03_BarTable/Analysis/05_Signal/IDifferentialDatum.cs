/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio
{
    public interface IDifferentialDatum
    {
        double Ratio { get; }

        double Difference { get; }

        double DifferenceRatio { get; }
    }
}
