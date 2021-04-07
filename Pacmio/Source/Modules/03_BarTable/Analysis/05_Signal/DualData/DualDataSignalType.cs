/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio
{
    public enum DualDataSignalType : int
    {
        None = 0,

        Above,

        Below,

        Expansion,

        Contraction,

        CrossUp,

        CrossDown,

        TrendUp,

        TrendDown,

        BounceUp = 100,

        BounceDown = -100,
    }
}
