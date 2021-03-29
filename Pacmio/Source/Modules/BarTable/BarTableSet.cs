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
    public sealed class BarTableSet : IDisposable, IEnumerable<(BarFreq freq, DataType type, BarTable bt)>
    {
        public BarTableSet(Contract c)
        {
            Contract = c;
        }

        ~BarTableSet() => Dispose();

        public void Dispose()
        {
            lock (BarTableLUT)
            {
                foreach (BarTable bt in BarTableLUT.Values.Where(n => n.Type != DataType.Trades || n.BarFreq < BarFreq.Daily))
                {
                    bt.Dispose();
                }
                BarTableLUT.Clear();
            }
        }

        public Contract Contract { get; }

        // public Period Period { get; }

        public void AddPeriod(Period pd, BarFreq freq, DataType type)
        {
            // Does not apply to Daily and above tables...

        }

        public Period Period { get; }

        private Dictionary<(BarFreq freq, DataType type), BarTable> BarTableLUT { get; } 
            = new Dictionary<(BarFreq freq, DataType type), BarTable>();

        public IEnumerator<(BarFreq freq, DataType type, BarTable bt)> GetEnumerator()
            => BarTableLUT.Select(n => (n.Key.freq, n.Key.type, n.Value)).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public BarTable GetOrCreateBarTable(BarFreq freq, DataType type = DataType.Trades, bool adjustDividend = false, CancellationTokenSource cts = null)
        {
            Period pd = new(Period);
            var key = (freq, type);
            lock (BarTableLUT)
            {
                if (!BarTableLUT.ContainsKey(key))
                {
                    if (type == DataType.Trades && freq >= BarFreq.Daily)
                        BarTableLUT[key] = Contract.GetOrCreateDailyBarTable(freq);
                    else
                    {
                        DateTime newStart = pd.Start.AddSeconds(-1000 * freq.GetAttribute<BarFreqInfo>().Frequency.Span.TotalSeconds);
                        pd.Insert(newStart); // Always have enough bars to buffer for good TA averages.

                        // TODO: non-Trades and above daily BarTable shall be downloaded at full range...
                        BarTableLUT[key] = Contract.LoadBarTable(pd, freq, type, adjustDividend, cts);
                    }
                }

                return BarTableLUT[key];
            }
        }

        public Bar Find(DateTime time, BarFreq freq, DataType type = DataType.Trades)
        {
            BarTable bt = GetOrCreateBarTable(freq, type);
            return bt[time];
        }
    }

    public sealed class BarTableGroup 
    {
        private Dictionary<Contract, BarTableSet> ContractBarTableSetLUT { get; } = new Dictionary<Contract, BarTableSet>();

        public BarTable GetOrCreateBarTable(Contract c, BarFreq freq, DataType type = DataType.Trades, bool adjustDividend = false, CancellationTokenSource cts = null)
        {
            if (!ContractBarTableSetLUT.ContainsKey(c))
                ContractBarTableSetLUT[c] = new BarTableSet(c);

            BarTableSet bts = ContractBarTableSetLUT[c];

            return bts.GetOrCreateBarTable(freq, type, adjustDividend, cts);
        }

    }
}
