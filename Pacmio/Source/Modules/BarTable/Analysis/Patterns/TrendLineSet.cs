/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// ***************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xu;

namespace Pacmio
{
    public class TrendLineSet : IEnumerable<TrendLineDatum>
    {
        public List<TrendLineDatum> m_List { get; } = new List<TrendLineDatum>();

        public IEnumerator<TrendLineDatum> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        // Utilities for Trend Line crossing, approaching, *** fake breakout, estimate the price target (distance) signals...
        /// Up Trend is the indication, a pattern is the varlidation
    }
}
