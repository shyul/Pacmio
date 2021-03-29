/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xu;

namespace Pacmio
{
    public sealed class BarTableGroup
    {
        private Dictionary<Contract, BarTableSet> ContractBarTableSetLUT { get; } = new Dictionary<Contract, BarTableSet>();

        public BarTableSet this[Contract c] 
        {
            get
            {
                if (!ContractBarTableSetLUT.ContainsKey(c))
                    ContractBarTableSetLUT[c] = new BarTableSet(c, false);

                return ContractBarTableSetLUT[c];
            }
        }



        public BarTable GetOrCreateBarTable(Contract c, BarFreq freq, DataType type = DataType.Trades, CancellationTokenSource cts = null)
        {
            if (!ContractBarTableSetLUT.ContainsKey(c))
                ContractBarTableSetLUT[c] = new BarTableSet(c, AdjustDividend);

            BarTableSet bts = ContractBarTableSetLUT[c];

            return bts.GetOrCreateBarTable(freq, type, cts);
        }

        public bool AdjustDividend { get; } = false;

    }
}
