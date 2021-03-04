/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System.Collections.Generic;
using System.Collections.Concurrent;
using Xu;
using System.Linq;

namespace Pacmio.IB
{
    public static partial class Client
    {
        public static int ActiveSubscriptionCount => ActiveTickerIdMarketDataLUT.Values.Where(n => n.IsLive).Count() + ActiveMarketDepth.Count + ActiveRealTimeBars.Count + WatchListManager.WatchListByType<InteractiveBrokerWatchList>().Count();

        public static bool SubscriptionOverflow => ActiveSubscriptionCount > Parameters.MaximumSubscription - 5;

        public static void ClearAllSubscription()
        {
            ActiveMarketDepth.Clear();
            ActiveTickerIdMarketDataLUT.Clear();
        }
    }
}
