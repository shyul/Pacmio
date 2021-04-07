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
using System.Threading.Tasks;
using Xu;
using Pacmio.Analysis;

namespace Pacmio
{
    public sealed class BarTableSet :
        IDataConsumer,
        IDataProvider,
        IDisposable,
        IEnumerable<(BarFreq freq, DataType type, BarTable bt)>
    {
        public BarTableSet(BarTable bt, bool adjustDividend)
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
            lock (DataLockObject)
            {
                IsLive = false;
                var bts = BarTableLUT.Values.ToList();
                BarTableLUT.Clear();
                bts.ForEach(n => n.Dispose());
                GC.Collect();
            }
        }

        public Contract Contract { get; }

        public MarketData MarketData => Contract.MarketData;

        public bool AdjustDividend { get; } = false;

        #region Data Update

        public bool IsLive
        {
            get => m_IsLive;
            private set
            {
                m_IsLive = value;

                if (m_IsLive)
                {
                    Contract.MarketData.AddDataConsumer(this);
                }
                else
                {
                    Contract.MarketData.RemoveDataConsumer(this);
                }
            }
        }

        public bool m_IsLive = false;

        public DateTime UpdateTime { get; private set; }

        public void DataIsUpdated(IDataProvider provider)
        {
            if (provider is MarketData md)
            {

            }

            UpdateTime = DateTime.Now;
        }

        public void InboundLiveTick(DateTime time, double price, double size)
        {
            if (IsLive)
            {
                //Parallel.ForEach(BarTableLUT.Select(n => n.Value), bt =>
                this.Select(n => n.bt).RunEach(bt =>
                {
                    lock (bt.DataLockObject)
                    {
                        if (bt.Status > TableStatus.Loading)
                        {
                            if (bt.BarFreq < BarFreq.Daily) // || bt.LastTime == time.Date)
                            {
                                bt.AddPriceTick(time, price, size);
                            }
                            else if (bt.BarFreq >= BarFreq.Daily) // && bt.LastTime < time.Date)
                            {
                                DateTime date = time.Date;

                                Console.WriteLine(">>> [[[[ Received for " + bt.ToString() + " | LastTime = " + bt.LastTimeBound.Date + " | time = " + date);

                                if (bt.LastTimeBound.Date < date)
                                {
                                    double open = double.IsNaN(MarketData.Open) || MarketData.Open <= 0 ? price : MarketData.Open;
                                    double high = double.IsNaN(MarketData.High) || MarketData.High <= 0 ? price : MarketData.High;
                                    double low = double.IsNaN(MarketData.Low) || MarketData.Low <= 0 ? price : MarketData.Low;
                                    double volume = double.IsNaN(MarketData.Volume) ? price : 0;
                                    bt.MergeFromSmallerBar(time, new Bar(bt, date, open, high, low, price, volume)); ;
                                }
                                else if (bt.LastTimeBound.Date == date)
                                {
                                    bt.AddPriceTick(time, price, size); //, DataSourceType.Manual);
                                }
                            }
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Trading Data
        /// </summary>
        private List<IDataConsumer> DataConsumers { get; set; } = new List<IDataConsumer>();

        public bool AddDataConsumer(IDataConsumer idk)
        {
            lock (DataLockObject)
            {
                if (!DataConsumers.Contains(idk))
                {
                    DataConsumers.Add(idk);
                    return true;
                }
                return false;
            }
        }

        public bool RemoveDataConsumer(IDataConsumer idk)
        {
            lock (DataLockObject)
            {
                if (DataConsumers.Contains(idk))
                {
                    DataConsumers.RemoveAll(n => n == idk);
                    return true;
                }
                return false;
            }
        }

        #endregion Data Update

        public object DataLockObject { get; private set; } = new();

        public MultiPeriod MultiPeriod
        {
            get => m_MultiPeriod;
            set => SetPeriod(m_MultiPeriod);
        }

        private MultiPeriod m_MultiPeriod = null;

        public void SetPeriod(MultiPeriod mp, CancellationTokenSource cts = null)
        {
            lock (DataLockObject)
            {
                bool isLive = false;
                foreach (var pd in mp)
                {
                    if (pd.IsCurrent || pd.Stop.Date > DateTime.Now.Date)
                    {
                        isLive = true;
                        break;
                    }
                }
                IsLive = isLive;

                foreach (BarTable bt in BarTableLUT.Values.Where(bt => bt.BarFreq < BarFreq.Daily))
                {
                    bt.LoadBars(mp, cts);
                }

                m_MultiPeriod = mp;
            }
        }

        public void SetPeriod(Period pd, CancellationTokenSource cts = null)
            => SetPeriod(new MultiPeriod(pd), cts);

        private Dictionary<(BarFreq freq, DataType type), BarTable> BarTableLUT { get; }
            = new Dictionary<(BarFreq freq, DataType type), BarTable>();


        public IEnumerator<(BarFreq freq, DataType type, BarTable bt)> GetEnumerator()
            => BarTableLUT.
            OrderByDescending(n => n.Key.freq).
            ThenByDescending(n => n.Key.type).
            Select(n => (n.Key.freq, n.Key.type, n.Value)).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public BarTable this[BarFreq freq, DataType type = DataType.Trades]
            => GetOrCreateBarTable(freq, type);

        public BarTable GetOrCreateBarTable(BarFreq freq, DataType type, CancellationTokenSource cts = null)
        {
            var key = (freq, type);

            lock (DataLockObject)
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
            lock (DataLockObject)
            {
                foreach (var item in ins)
                {
                    BarTable bt = this[item.freq, item.type];
                    bt.CalculateRefresh(item.bas);
                }
            }
        }
    }
}
