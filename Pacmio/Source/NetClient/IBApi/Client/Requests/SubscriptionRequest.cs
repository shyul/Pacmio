/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System.Collections.Generic;
using System.Collections.Concurrent;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {


        public static int ActiveSubscriptionCount => ActiveMarketDataTicks.Count + ActiveMarketDepth.Count + ActiveRealTimeBars.Count + ScanRequestList.Count;

        public static bool SubscriptionOverflow => ActiveSubscriptionCount > Parameters.MaximumSubscription - 5;
    }
}
