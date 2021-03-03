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
    public static class FundamentalManager
    {
        private static Dictionary<(string name, Exchange exchange, string typeName), FundamentalData> ContractFundamentalLUT { get; }
            = new Dictionary<(string name, Exchange exchange, string typeName), FundamentalData>();

        public static FundamentalData GetOrCreateFundamentalData(this Contract c) => GetOrCreateFundamentalData(c.Key);

        public static FundamentalData GetOrCreateFundamentalData(this (string name, Exchange exchange, string typeName) key)
        {
            lock (ContractFundamentalLUT)
            {
                if (!ContractFundamentalLUT.ContainsKey(key))
                {
                    ContractFundamentalLUT[key] = FundamentalData.LoadFile(key);
                }

                return ContractFundamentalLUT[key];
            }
        }

        public static void Save()
        {
            lock (ContractFundamentalLUT)
            {
                Parallel.ForEach(ContractFundamentalLUT.Values.Where(n => n.IsModified), bi => bi.SaveFile());
            }
        }
    }
}
