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
    public interface ICandleStick
    {
        double Open { get; }

        double High { get; }

        double Low { get; }

        double Close { get; }

        List<CandleStickType> CandleStickTypes { get; }
    }
}
