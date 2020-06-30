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
    public interface IOscillator : ISingleData
    {
        double Reference { get; }

        double LowerLimit { get; }

        double UpperLimit { get; }

        Color LowerColor { get; }

        Color UpperColor { get; }

        string AreaName { get; }

        int AreaOrder { get; }
    }
}
