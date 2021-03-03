/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xu;

namespace Pacmio
{
    public class MultiTimeFrameSet : IDisposable
    {
        public MultiTimeFrameSet(Contract c, Period pd)
        {
            Contract = c;
            Period = pd;
        }

        public Contract Contract { get; }

        public Period Period { get; }

        private Dictionary<(BarFreq freq, BarType type), BarTable> TableLUT { get; } = new Dictionary<(BarFreq freq, BarType type), BarTable>();

        public void Dispose()
        {
            lock (TableLUT)
            {
                foreach (BarTable bt in TableLUT.Values.Where(n => n.BarFreq < BarFreq.Daily))
                {
                    bt.Dispose();
                }
                TableLUT.Clear();
            }
        }

        public BarTable GetOrCreateBarTable(BarFreq freq, BarType type)
        {
            var key = (freq, type);

            if (type == BarType.Trades && freq >= BarFreq.Daily)
            {
                return Contract.GetOrCreateDailyBarTable(freq);
            }
            else
            {
                lock (TableLUT)
                {
                    if (!TableLUT.ContainsKey(key))
                    {
                        //ContractBarTableLUT[c] = MarketData.LoadFile(c.Key);
                    }

                    return TableLUT[key];
                }
            }
        }
    }
}
