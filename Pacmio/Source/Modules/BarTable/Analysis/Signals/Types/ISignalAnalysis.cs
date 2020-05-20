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
    public interface ISignalAnalysis
    {
        string Name { get; }

        ColorTheme BullishTheme { get; }

        ColorTheme BearishTheme { get; }

        ObjectColumn Signal_Column { get; }
    }
}
