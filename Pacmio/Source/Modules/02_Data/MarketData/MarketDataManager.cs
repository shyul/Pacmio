/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xu;

namespace Pacmio
{
    public static class MarketDataManager
    {
        private static Dictionary<Contract, MarketData> ContractMarketDataLUT { get; } = new Dictionary<Contract, MarketData>();

        public static MarketData GetOrCreateMarketData(this Contract c)
        {
            lock (ContractMarketDataLUT)
            {
                if (!ContractMarketDataLUT.ContainsKey(c))
                {
                    ContractMarketDataLUT[c] = MarketData.LoadFile(c.Key);
                }

                return ContractMarketDataLUT[c];
            }
        }

        #region IB Subscription

        /// <summary>
        /// https://interactivebrokers.github.io/tws-api/tick_types.html
        /// string genericTickList = "236,375";  // 292 is news and 233 is RTVolume
        /// 
        /// Has to support renewed option...
        /// 
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool StartTicks(this Contract c)
        {
            MarketData md = c.GetOrCreateMarketData();
            md.FilteredTicks = true;
            md.EnableNews = true;
            md.EnableShortableShares = true;
            return IB.Client.SendRequest_MarketData(md);
        }

        public static bool Start(this MarketData md) => IB.Client.SendRequest_MarketData(md);

        public static bool Snapshot(this MarketData md) => IB.Client.SendRequest_MarketData(md, true);

        public static void Stop(this MarketData md) => IB.Client.SendCancel_MarketData(md);

        #endregion IB Subscription

        #region File system

        public static void Save()
        {
            lock (ContractMarketDataLUT)
            {
                Parallel.ForEach(ContractMarketDataLUT.Values.Where(n => n.IsModified), c => { c.SaveFile(); });
            }
        }

        #endregion File system
    }
}
