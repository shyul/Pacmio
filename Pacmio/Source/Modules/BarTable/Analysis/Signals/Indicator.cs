/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// https://support.stockcharts.com/doku.php?id=scans:library:sample_scans
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xu;

namespace Pacmio
{
    /// <summary>
    /// Indication: Move into Either Enter or Exit
    /// Passive: Only yield signal score
    /// </summary>
    public abstract class Indicator : BarAnalysis
    {
        public abstract IEnumerable<SignalColumn> SignalColumns { get; }
    }
}
