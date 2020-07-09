/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// BarTable Data Types
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Text;
using System.IO;
using Xu;
using Xu.Chart;
using System.Windows.Forms;

namespace Pacmio
{
    public class BarTableSet : IDisposable
    {
        public readonly Dictionary<(BarFreq barFreq, BarType barType), Period> PeriodSettings = new Dictionary<(BarFreq barFreq, BarType barType), Period>();

        public readonly HashSet<BarTable> BarTables = new HashSet<BarTable>();

        private object DataLockObject { get; } = new object();

        public void SaveBarTables() => BarTables.AsParallel().ForAll(n => { n.Save(); n.Dispose(); });

        public void Clear()
        {
            SaveBarTables();
            BarTables.Clear();
        }

        public void Dispose() => Clear();

        public BarTable Get(Contract c, BarFreq barFreq, BarType barType)
        {
            var existing_tables = BarTables.Where(n => n == (c, barFreq, barType));
            if (existing_tables.Count() == 1)
                return existing_tables.First();
            else
                return null;
        }

        private BarTable AddContract(Contract c, BarFreq barFreq, BarType barType)
        {
            lock (DataLockObject)
            {
                var existing_tables = BarTables.Where(n => n == (c, barFreq, barType));
                if (existing_tables.Count() == 1)
                {
                    var to_keep_table = existing_tables.First();
                    return to_keep_table;
                }
                else if (existing_tables.Count() > 1)
                {
                    var to_keep_table = existing_tables.First();
                    existing_tables.ToList().ForEach(n => BarTables.Remove(n));
                    BarTables.Add(to_keep_table);
                    return to_keep_table;
                }
                else
                {
                    BarTable bt = new BarTable(c, barFreq, barType);
                    BarTables.Add(bt);
                    return bt;
                }
            }
        }

        public BarTable AddContract(Contract c, BarFreq barFreq, BarType barType, ref Period period, CancellationTokenSource cts)
        {
            lock (DataLockObject)
            {
                BarTable bt = AddContract(c, barFreq, barType);
                bt.Fetch(period, cts);
                period = bt.Period;
                return bt;
            }
        }

        public void AddChart(Contract c, TradeRule tr, BarFreq barFreq, BarType barType, ref Period period, CancellationTokenSource cts)
        {
            BarTable bt = AddContract(c, barFreq, barType);
            AddChart(bt, tr);
            if (cts.Continue())
            {
                if (bt.Status == TableStatus.Default || bt.Count == 0 || period != bt.Period)
                {
                    bt.Fetch(period, cts);
                    period = bt.Period;
                }
                bt.CalculateOnly(tr);
            }
        }

        private List<BarTable> AddContract(IEnumerable<Contract> contracts, BarFreq barFreq, BarType barType)
        {
            lock (DataLockObject)
            {
                var to_delete_tables = BarTables.Where(n => n.BarFreq == barFreq && n.Type == barType && !contracts.Contains(n.Contract)).ToList();

                to_delete_tables.ForEach(n =>
                {
                    BarTable bt = n;
                    BarTables.Remove(bt);
                    bt.Save();
                    bt.Dispose();
                });
                List<BarTable> add_tables = new List<BarTable>();
                foreach (var c in contracts)
                {
                    add_tables.Add(AddContract(c, barFreq, barType));
                }
                return add_tables;
            }
        }

        public List<BarTable> AddContract(IEnumerable<Contract> contracts, BarFreq barFreq, BarType barType, Period period, CancellationTokenSource cts, IProgress<float> progress)
        {
            if (cts.Continue())
            {
                List<BarTable> add_tables = AddContract(contracts, barFreq, barType);
                UpdatePeriod(barFreq, barType, period, cts, progress);
                return add_tables;
            }
            return new List<BarTable>();
        }

        public void AddChart(IEnumerable<(Contract, TradeRule)> contracts, BarFreq barFreq, BarType barType, Period period, CancellationTokenSource cts, IProgress<float> progress)
        {
            progress?.Report(0);
            PeriodSettings[(barFreq, barType)] = period;

            ParallelOptions po = new ParallelOptions()
            {
                //CancellationToken = cts.Token,
                MaxDegreeOfParallelism = Math.Ceiling(Root.DegreeOfParallelism / 3D).ToInt32(1)
            };

            int i = 0, count = contracts.Count();
            Parallel.ForEach(contracts, po, n => {
                if (cts.Continue())
                {
                    var (c, tr) = n;
                    AddChart(c, tr, barFreq, barType, ref period, cts);
                    i++;
                    if (cts.Continue()) progress?.Report(100.0f * i / count);
                }
            });
        }

        public void Calculate(TradeRule tr, CancellationTokenSource cts, IProgress<float> progress)
        {
            progress?.Report(0);
            ParallelOptions po = new ParallelOptions()
            {
                //CancellationToken = cts.Token,
                MaxDegreeOfParallelism = Root.DegreeOfParallelism
            };
            int i = 0, count = BarTables.Count();
            Parallel.ForEach(BarTables, po, bt => {
                if (cts.Continue())
                {
                    bt.CalculateOnly(tr);
                    i++;
                    if (cts.Continue()) progress?.Report(100.0f * i / count);
                }
            });
        }

        private void AddChart(BarTable bt, TradeRule tr)
        {
            if (BarChart.List.Where(n => n.BarTable == bt).Count() == 0)
            {
                BarChart bc = new BarChart("BarChart", OhlcType.Candlestick);
                bc.ConfigChart(bt, tr);
                if (Root.Form.InvokeRequired)
                {
                    Root.Form.Invoke((MethodInvoker)delegate
                    {
                        Root.Form.AddForm(DockStyle.Fill, 0, bc);
                    });
                }
                else
                {
                    Root.Form.AddForm(DockStyle.Fill, 0, bc);
                }
            }
        }

        /// <summary>
        /// TODO: The Cancellation is not efficient
        /// </summary>
        /// <param name="barFreq"></param>
        /// <param name="barType"></param>
        /// <param name="period"></param>
        /// <param name="cts"></param>
        /// <param name="progress"></param>
        public void UpdatePeriod(BarFreq barFreq, BarType barType, Period period, CancellationTokenSource cts, IProgress<float> progress)
        {
            progress?.Report(0);
            if (cts.Continue())
                lock (DataLockObject)
                {
                    PeriodSettings[(barFreq, barType)] = period;
                    var tables = BarTables.Where(n => n.BarFreq == barFreq && n.Type == barType);//.OrderByDescending(n => n.Key.BarFreq);

                    ParallelOptions po = new ParallelOptions()
                    {
                        //CancellationToken = cts.Token,
                        MaxDegreeOfParallelism = Root.DegreeOfParallelism
                    };

                    int i = 0, count = tables.Count();
                    Parallel.ForEach(tables, po, n => {
                        if (cts.Continue())
                        {
                            n.Fetch(period, cts);
                            i++;
                            if (cts.Continue()) progress?.Report(100.0f * i / count);
                        }
                    });
                }
        }
    }
}
