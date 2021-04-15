/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Xu;

namespace Pacmio
{
    public interface IGapPercentFilter : IFilter
    {
        double BullishGapPercent { get; }

        double BearishGapPercent { get; }
    }
}
