﻿/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xu;

namespace Pacmio.Analysis
{
    public abstract class PatternAnalysis : BarAnalysis
    {
        public virtual PatternColumn Column_Result { get; protected set; }

        public abstract int MaximumInterval { get; }

        public string AreaName { get; set; } = "None";

        public bool ChartEnabled { get; set; } = true;

        public int DrawOrder { get; set; } = 0;
    }
}
