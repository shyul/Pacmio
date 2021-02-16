/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xu;

namespace Pacmio
{
    public class EntryMethod
    {
        #region Order Settings

        public MultiTimePeriod TradeTimeOfDay { get; set; }

        public Frequency WaitLengthForOutstandingOrder { get; }

        public double MaximumPriceGoingPositionFromDecisionPointPrecent { get; }

        public double MaximumPriceGoinNegativeFromDecisionPointPrecent { get; }

        #endregion Order Settings
    }


}
