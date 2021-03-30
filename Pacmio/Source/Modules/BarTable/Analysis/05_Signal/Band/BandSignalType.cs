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
    public enum BandSignalType : int
    {
        LowerPenetrate = -3,

        LowerBound = -2,

        LowerHalf = -1,

        None = 0,

        UpperHalf = 1,

        UpperBound = 2,

        UpperPenetrate = 3,
    }
}
