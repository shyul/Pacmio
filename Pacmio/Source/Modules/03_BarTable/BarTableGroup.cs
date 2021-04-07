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
        public bool AdjustDividend { get; } = false;

        public int Count => ContractBarTableSetLUT.Count();

        public object DataLockObject { get; private set; } = new();

        private Dictionary<Contract, BarTableSet> ContractBarTableSetLUT { get; } = new Dictionary<Contract, BarTableSet>();

        public BarTableSet this[Contract c]
        {
            get
            {
                lock (DataLockObject) 
                {
                    if (!ContractBarTableSetLUT.ContainsKey(c))
                        ContractBarTableSetLUT[c] = new BarTableSet(c, AdjustDividend);

                    return ContractBarTableSetLUT[c];
                }
            }
        }

        public BarTable this[Contract c, BarFreq freq, MarketDataType type = MarketDataType.Trades]
        {
            get
            {
                lock (DataLockObject)
                {
                    if (!ContractBarTableSetLUT.ContainsKey(c))
                        ContractBarTableSetLUT[c] = new BarTableSet(c, AdjustDividend);

                    BarTableSet bts = ContractBarTableSetLUT[c];
                    return bts.GetOrCreateBarTable(freq, type);
                }
            }
        }
    }
}
