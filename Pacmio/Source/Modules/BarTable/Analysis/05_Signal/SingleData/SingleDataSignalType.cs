/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio.Analysis
{
    public enum SingleDataSignalType : int
    {
        None = 0,

        Within = 1,

        EnterFromBelow = 1,

        EnterFromAbove = -1,

        Above = 3,

        Below = -3,

        ExitAbove = 5,

        ExitBelow = -5,

        CrossUp = 6,

        CrossDown = -6
    }
}
