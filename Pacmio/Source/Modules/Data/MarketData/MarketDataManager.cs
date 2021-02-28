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
        private static Dictionary<Contract, MarketData> ContractToMarketDataLUT { get; } = new Dictionary<Contract, MarketData>();

        //public static Dictionary<MarketData, MarketDataRequestStatus> MarketDataTickers { get; } = new Dictionary<MarketData, MarketDataRequestStatus>();

        public static MarketData GetOrCreateMarketData(this Contract c)
        {
            lock (ContractToMarketDataLUT)
            {
                if (!ContractToMarketDataLUT.ContainsKey(c)) 
                {
                    // ContractToMarketDataLUT[c] = new MarketData(c);
                }

                return ContractToMarketDataLUT[c];
            }
        }

        #region File system

        #endregion File system
    }
}
