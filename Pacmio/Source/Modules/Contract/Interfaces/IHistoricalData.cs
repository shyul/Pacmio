/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    public interface IHistoricalData
    {
        DateTime BarTableEarliestTime { get; set; }
    }
}
