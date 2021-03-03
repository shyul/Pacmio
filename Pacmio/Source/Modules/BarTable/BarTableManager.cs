/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Xu;

namespace Pacmio
{
    public static class BarTableManager
    {
        private static Dictionary<(Contract c, BarFreq freq), BarTable> ContractDailyBarTableLUT { get; } = new Dictionary<(Contract c, BarFreq freq), BarTable>();

        public static BarTable GetOrCreateDailyBarTable(this Contract c, BarFreq freq)
        {
            var key = (c, freq);

            lock (ContractDailyBarTableLUT)
            {
                if (!ContractDailyBarTableLUT.ContainsKey(key))
                {
                    //ContractBarTableLUT[c] = MarketData.LoadFile(c.Key);
                }

                return ContractDailyBarTableLUT[key];
            }
        }

        public static BarTable CreateBarTable(this Contract c, BarFreq barFreq, BarType barType, Period period)
        {
            return null;
        }


    }
}
