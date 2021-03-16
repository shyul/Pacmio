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

namespace Pacmio
{
    public enum DualColumnType : int
    {
        Above,

        Below,

        Expansion,

        Contraction,

        CrossUp,

        CrossDown,

        TrendUp,

        TrendDown,
    }
}
