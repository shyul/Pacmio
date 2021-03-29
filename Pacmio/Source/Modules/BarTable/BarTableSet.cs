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
        public BarTableSet(Contract c, bool ajustDividend)
        {
            Contract = c;
            AdjustDividend = ajustDividend;
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

        public void SetPeriod(Period pd, CancellationTokenSource cts = null) => SetPeriod(new MultiPeriod(pd), cts);

        private Dictionary<(BarFreq freq, DataType type), BarTable> BarTableLUT { get; }
            = new Dictionary<(BarFreq freq, DataType type), BarTable>();

        public IEnumerator<(BarFreq freq, DataType type, BarTable bt)> GetEnumerator()
            => BarTableLUT.Select(n => (n.Key.freq, n.Key.type, n.Value)).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public BarTable this[BarFreq freq, DataType type = DataType.Trades] => GetOrCreateBarTable(freq, type);

        public BarTable GetOrCreateBarTable(BarFreq freq, DataType type, CancellationTokenSource cts = null)
        {
            var key = (freq, type);

            lock (BarTableLUT)
            {
                if (!BarTableLUT.ContainsKey(key))
                {
                    BarTableLUT[key] = LoadBarTable(freq, type, m_MultiPeriod, cts);
                }

                return BarTableLUT[key];
            }
        }

        private BarTable LoadBarTable(BarFreq barFreq, DataType dataType, MultiPeriod mp = null, CancellationTokenSource cts = null)
        {
            BarDataFile bdf_daily_base = Contract.GetBarDataFile(BarFreq.Daily);
            bdf_daily_base.Fetch(Period.Full, cts);

            if (barFreq == BarFreq.Daily && dataType == DataType.Trades)
            {
                BarTable bt = new(this, BarFreq.Daily, DataType.Trades);
                bt.LoadBars(bdf_daily_base, AdjustDividend);
                return bt;
            }
            else if (barFreq > BarFreq.Daily)
            {
                BarDataFile bdf_daily = dataType == DataType.Trades ? bdf_daily_base : Contract.GetBarDataFile(BarFreq.Daily, dataType);

                if (dataType != DataType.Trades)
                    bdf_daily.Fetch(Period.Full, cts);

                BarTable bt_daily = new(this, BarFreq.Daily, dataType);
                var sorted_daily_list = bdf_daily.LoadBars(bt_daily, AdjustDividend);

                BarTable bt = new(this, barFreq, dataType);
                bt.LoadFromSmallerBar(sorted_daily_list);
                return bt;
            }
            else
            {
                BarTable bt = new(this, barFreq, dataType);
                bt.LoadBars(mp, AdjustDividend);
                return bt;
            }
        }
    }
}
