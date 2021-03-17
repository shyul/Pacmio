/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public interface IBarChartArea : IIndexArea
    {
        BarChart BarChart { get; }

        BarTable BarTable { get; }
    }
}
