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
        public static int ActiveSubscriptionCount => ActiveMarketDataTickerIds.Values.Where(n => n.IsActive).Count() + ActiveMarketDepth.Count + ActiveRealTimeBars.Count + WatchListManager.GetInteractiveBrokerWatchList().Count();

        public static bool SubscriptionOverflow => ActiveSubscriptionCount > Parameters.MaximumSubscription - 5;

        public static void ClearAllSubscription()
        {
            ActiveMarketDepth.Clear();
            MarketDataTickers.Clear();
            ActiveMarketDataTickerIds.Clear();
        }

        /// <summary>
        /// Maximum Request: 3
        /// </summary>
        public static ConcurrentDictionary<int, Contract> ActiveMarketDepth { get; } = new ConcurrentDictionary<int, Contract>();

        private static ConcurrentDictionary<MarketData, MarketDataRequestStatus> MarketDataTickers { get; } = new ConcurrentDictionary<MarketData, MarketDataRequestStatus>();

        private static ConcurrentDictionary<int, MarketDataRequestStatus> ActiveMarketDataTickerIds { get; } = new ConcurrentDictionary<int, MarketDataRequestStatus>();

        private static MarketDataRequestStatus GetMarketDataRequestStatus(MarketData md)
        {
            lock (MarketDataTickers)
            {
                if (!MarketDataTickers.ContainsKey(md)) MarketDataTickers.TryAdd(md, new MarketDataRequestStatus(md));
                return MarketDataTickers[md];
            }
        }

        public static bool IsActive(MarketData md) => GetMarketDataRequestStatus(md).IsActive;

        private static MarketData GetMarketData(int tickerId)
        {
            lock (ActiveMarketDataTickerIds)
                if (ActiveMarketDataTickerIds.ContainsKey(tickerId))
                    return ActiveMarketDataTickerIds[tickerId].MarketData;
                else
                    return null;
        }

        private static MarketDataRequestStatus GetMarketDataRequestStatus(int tickerId)
        {
            lock (ActiveMarketDataTickerIds)
                if (ActiveMarketDataTickerIds.ContainsKey(tickerId))
                    return ActiveMarketDataTickerIds[tickerId];
                else
                    return null;
        }


        public static IEnumerable<MarketData> ActiveMarketData => MarketDataTickers.Select(n => n.Value).Where(n => n.IsActive).Select(n => n.MarketData);

        private static (int tickerId, string requestType, string exchangeCode, MarketDataRequestStatus mds) RegisterMarketDataRequest(MarketData md)
        {
            lock (ActiveMarketDataTickerIds)
            {
                MarketDataRequestStatus mds = GetMarketDataRequestStatus(md);
                if (Connected && !SubscriptionOverflow && !mds.IsActive && md.Contract.Exchange.Param() is string exchangeCode)
                {
                    mds.IsActive = true;
                    (int tickerId, string requestType) = RegisterRequest(RequestType.RequestMarketData);
                    mds.TickerId = tickerId;
                    ActiveMarketDataTickerIds[tickerId] = mds;
                    return (tickerId, requestType, exchangeCode, mds);
                }
            }

            return (int.MinValue, null, null, null);
        }

        private static MarketDataRequestStatus UnregisterMarketDataRequest(int tickerId, bool cancel = false)
        {
            lock (ActiveMarketDataTickerIds)
            {
                RemoveRequest(tickerId, cancel);
                if (ActiveMarketDataTickerIds.TryRemove(tickerId, out MarketDataRequestStatus mds))
                {
                    mds.IsActive = false;
                    mds.TickerId = int.MinValue;
                    mds.MarketData.Status = MarketTickStatus.DelayedFrozen;

                    return mds;
                }
                else
                {
                    lock (MarketDataTickers)
                    {
                        MarketDataTickers.Select(n => n.Value).Where(n => n.TickerId == tickerId).ToList().ForEach(mds => {
                            mds.IsActive = false;
                            mds.TickerId = int.MinValue;
                            mds.MarketData.Status = MarketTickStatus.DelayedFrozen;
                        });
                    }

                    return null;
                }
            }
        }

        private static void UnregisterMarketDataRequest(MarketData md, bool cancel = false)
        {
            MarketDataRequestStatus mds = GetMarketDataRequestStatus(md);

            if (mds.TickerId > 0)
            {
                RemoveRequest(mds.TickerId, cancel);
                ActiveMarketDataTickerIds.TryRemove(mds.TickerId, out _);
            }

            mds.IsActive = false;
            mds.TickerId = int.MinValue;
            mds.MarketData.Status = MarketTickStatus.DelayedFrozen;
        }
    }


}
