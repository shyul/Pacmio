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
using Pacmio.Analysis;

namespace Pacmio
{
    public sealed class BarTableSet :
        IDataProvider,
        IDataConsumer,
        IDisposable,
        IEnumerable<(BarFreq freq, DataType type, BarTable bt)>
    {
        public BarTableSet(BarTable bt, bool adjustDividend = false)
        {
            Contract = bt.Contract;
            AdjustDividend = adjustDividend;
            BarTableLUT[(bt.BarFreq, bt.Type)] = bt;
        }

        public BarTableSet(Contract c, bool adjustDividend)
        {
            Contract = c;
            AdjustDividend = adjustDividend;
        }

        ~BarTableSet() => Dispose();

        public void Dispose()
        {
            lock (BarTableLUT)
            {
                var bts = BarTableLUT.Values.ToList();
                BarTableLUT.Clear();
                bts.ForEach(n => n.Dispose());
            }
        }

        public Contract Contract { get; }

        public bool AdjustDividend { get; } = false;

        public MultiPeriod MultiPeriod
        {
            get => m_MultiPeriod;
            set => SetPeriod(m_MultiPeriod);
        }

        private MultiPeriod m_MultiPeriod = null;

        public void SetPeriod(MultiPeriod mp, CancellationTokenSource cts = null)
        {
            lock (BarTableLUT)
            {
                foreach (BarTable bt in BarTableLUT.Values)
                {
                    bt.LoadBars(mp, AdjustDividend, cts);
                }

                m_MultiPeriod = mp;
            }
        }

        public void SetPeriod(Period pd, CancellationTokenSource cts = null)
            => SetPeriod(new MultiPeriod(pd), cts);

        private Dictionary<(BarFreq freq, DataType type), BarTable> BarTableLUT { get; }
            = new Dictionary<(BarFreq freq, DataType type), BarTable>();


        public IEnumerator<(BarFreq freq, DataType type, BarTable bt)> GetEnumerator()
            => BarTableLUT.Select(n => (n.Key.freq, n.Key.type, n.Value)).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public BarTable this[BarFreq freq, DataType type = DataType.Trades]
            => GetOrCreateBarTable(freq, type);

        public BarTable GetOrCreateBarTable(BarFreq freq, DataType type, CancellationTokenSource cts = null)
        {
            var key = (freq, type);

            lock (BarTableLUT)
            {
                if (!BarTableLUT.ContainsKey(key))
                {
                    BarTableLUT[key] = this.LoadBarTable(freq, type, m_MultiPeriod, cts);
                }

                return BarTableLUT[key];
            }
        }

        public void CalculateRefresh(IndicatorSet ins)
        {
            foreach (var item in ins)
            {
                BarTable bt = this[item.freq, item.type];
                bt.CalculateRefresh(item.bas);
            }
        }





        public DateTime UpdateTime => throw new NotImplementedException();

        public void DataIsUpdated(IDataProvider provider)
        {
            throw new NotImplementedException();
        }

        public bool AddDataConsumer(IDataConsumer idk)
        {
            throw new NotImplementedException();
        }

        public bool RemoveDataConsumer(IDataConsumer idk)
        {
            throw new NotImplementedException();
        }
    }
}
