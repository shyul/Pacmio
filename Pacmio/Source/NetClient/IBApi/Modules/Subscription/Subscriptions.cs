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
        public static int ActiveSubscriptionCount => ActiveMarketDataTicks.Count + ActiveMarketDepth.Count + ActiveRealTimeBars.Count + ActiveScanners.Count;

        public static bool SubscriptionOverflow => ActiveSubscriptionCount > Parameters.MaximumSubscription - 5;

        public static ConcurrentDictionary<int, MarketData> ActiveMarketDataTicks { get; } = new ConcurrentDictionary<int, MarketData>();

        /// <summary>
        /// Maximum Request: 3
        /// </summary>
        public static ConcurrentDictionary<int, Contract> ActiveMarketDepth { get; } = new ConcurrentDictionary<int, Contract>();

        private static ConcurrentDictionary<int, WatchList> ActiveScanners { get; } = new ConcurrentDictionary<int, WatchList>();

    }
}
