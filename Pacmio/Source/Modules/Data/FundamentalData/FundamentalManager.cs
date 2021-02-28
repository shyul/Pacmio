﻿/// ***************************************************************************
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
    public static class FundamentalManager
    {
        private static Dictionary<Contract, FundamentalDataList> ContractFundamentalLUT { get; } = new Dictionary<Contract, FundamentalDataList>();

        public static FundamentalDataList GetOrCreateFundamentalData(this Contract c)
        {
            lock (ContractFundamentalLUT)
            {
                if (!ContractFundamentalLUT.ContainsKey(c))
                {
                    ContractFundamentalLUT[c] = FundamentalDataList.LoadFile(c);
                }

                return ContractFundamentalLUT[c];
            }
        }


    }
}
