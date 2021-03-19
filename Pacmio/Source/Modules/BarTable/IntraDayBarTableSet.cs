/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xu;

namespace Pacmio
{
    public sealed class IntraDayBarTableSet : IDisposable
    {
        public IntraDayBarTableSet(Contract c)
        {
            Contract = c;
        }

        ~IntraDayBarTableSet() => Dispose();

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

        private Dictionary<(BarFreq BarFreq, DataType BarType), BarTable> BarTableLUT { get; } 
            = new Dictionary<(BarFreq BarFreq, DataType BarType), BarTable>();

        public BarTable GetOrCreateBarTable(Period pd, BarFreq freq, DataType type, bool adjustDividend, CancellationTokenSource cts = null)
        {
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
                        BarTableLUT[key] = Contract.LoadBarTable(pd, freq, type, adjustDividend, cts);
                    }
                }

                return BarTableLUT[key];
            }
        }
    }
}
