﻿/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public enum DualDataType : int
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