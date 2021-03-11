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
                foreach (BarTable bt in BarTableLUT.Values.Where(n => n.Type != BarType.Trades || n.BarFreq < BarFreq.Daily))
                {
                    bt.Dispose();
                }
                BarTableLUT.Clear();
            }
        }

        public Contract Contract { get; }

        public Period Period
        {
            get => m_Period; 
            
            set
            {
                if(m_Period != value) 
                {
                    m_Period = value;
                


                }
            }
        }

        private Period m_Period = Period.Empty;

        public DateTime Stop => Period.Stop;

        public DateTime Start => Period.Start;

        public bool IsCurrent => Period.IsCurrent;

        private Dictionary<(BarFreq BarFreq, BarType BarType), BarTable> BarTableLUT { get; } = new Dictionary<(BarFreq BarFreq, BarType BarType), BarTable>();

        public BarTable GetOrCreateBarTable(BarFreq freq, BarType type, bool adjustDividend = false, CancellationTokenSource cts = null)
        {
            var key = (freq, type);

            lock (BarTableLUT)
            {
                if (!BarTableLUT.ContainsKey(key))
                {
                    if (type == BarType.Trades && freq >= BarFreq.Daily)
                        BarTableLUT[key] = Contract.GetOrCreateDailyBarTable(freq);
                    else
                        BarTableLUT[key] = Contract.LoadBarTable(Period, freq, type, adjustDividend, cts);
                }

                return BarTableLUT[key];
            }
        }

        public void Calculate(Strategy s, Period pd, CancellationTokenSource cts = null)
        {
            Period = pd;
            foreach (var ba in s.BarAnalysisLUT)
            {
                if (!cts.IsContinue()) return;
                BarTable bt = GetOrCreateBarTable(ba.Key.BarFreq, ba.Key.BarType, false, cts);
                bt.CalculateRefresh(ba.Value);
            }
        }

        public void Calculate(Strategy s, CancellationTokenSource cts = null)
        {
            foreach (var ba in s.BarAnalysisLUT)
            {
                if (!cts.IsContinue()) return;
                BarTable bt = GetOrCreateBarTable(ba.Key.BarFreq, ba.Key.BarType, false, cts);
                bt.CalculateRefresh(ba.Value);
            }
        }
    }
}
