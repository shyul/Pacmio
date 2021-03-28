/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio.Analysis
{
    public enum BandSignalType : int
    {
        UpperPenetrate = 3,

        UpperBound = 2,

        UpperHalf = 1,

        None = 0,

        LowerHalf = -1,

        LowerBound = -2,

        LowerPenetrate = -3,
    }
}
