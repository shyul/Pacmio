﻿/// ***************************************************************************
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

        public readonly Dictionary<BarTable, BarAnalysisSet> BarTables = new Dictionary<BarTable, BarAnalysisSet>();

        private object DataLockObject { get; } = new object();

        public void SaveBarTables() => BarTables.Keys.AsParallel().ForAll(n => { n.Save(); n.Dispose(); });

        public void Clear()
        {
            SaveBarTables();
            BarTables.Clear();
        }

        public void Dispose() => Clear();

        public BarTable Get(Contract c, BarFreq barFreq, BarType barType)
        {
            var existing_tables = BarTables.Where(n => n.Key == (c, barFreq, barType));
            if (existing_tables.Count() == 1)
            {
                return existing_tables.First().Key;
            }

            return null;
        }

        private BarTable AddContract(Contract c, BarAnalysisSet bas, BarFreq barFreq, BarType barType)
        {
            lock (DataLockObject)
            {
                var existing_tables = BarTables.Where(n => n.Key == (c, barFreq, barType));
                if (existing_tables.Count() == 1)
                {
                    var to_keep_table = existing_tables.First();
                    BarTables[to_keep_table.Key] = bas;
                    return to_keep_table.Key;
                }
                else if (existing_tables.Count() > 1)
                {
                    var to_keep_table = existing_tables.First();
                    existing_tables.ToList().ForEach(n => BarTables.Remove(n.Key));
                    BarTables.Add(to_keep_table.Key, bas);
                    return to_keep_table.Key;
                }
                else
                {
                    BarTable bt = new BarTable(c, barFreq, barType);
                    BarTables[bt] = bas;
                    return bt;
                }
            }
        }

        public BarTable AddContract(Contract c, BarAnalysisSet bas, BarFreq barFreq, BarType barType, Period period, CancellationTokenSource cts)
        {
            lock (DataLockObject)
            {
                BarTable bt = AddContract(c, bas, barFreq, barType);
                bt.Fetch(period, cts);
                return bt;
            }
        }

        public void AddChart(Contract c, BarAnalysisSet bas, BarFreq barFreq, BarType barType, Period period, CancellationTokenSource cts)
        {
            BarTable bt = AddContract(c, bas, barFreq, barType, period, cts);
            AddChart(bt, bas);
        }



        private List<BarTable> AddContract(IEnumerable<(Contract, BarAnalysisSet)> contracts, BarFreq barFreq, BarType barType)
        {
            lock (DataLockObject)
            {
                var to_delete_tables = BarTables.Where(n => n.Key.BarFreq == barFreq && n.Key.Type == barType && !contracts.Select(n => n.Item1).Contains(n.Key.Contract)).ToList();

                to_delete_tables.ForEach(n =>
                {
                    BarTable bt = n.Key;
                    BarTables.Remove(bt);
                    bt.Save();
                    bt.Dispose();
                });
                List<BarTable> add_tables = new List<BarTable>();
                foreach (var (c, bas) in contracts)
                {
                    add_tables.Add(AddContract(c, bas, barFreq, barType));
                }
                return add_tables;
            }
        }

        public List<BarTable> AddContract(IEnumerable<(Contract, BarAnalysisSet)> contracts, BarFreq barFreq, BarType barType, Period period, CancellationTokenSource cts, IProgress<float> progress)
        {
            List<BarTable> add_tables = AddContract(contracts, barFreq, barType);
            UpdatePeriod(barFreq, barType, period, cts, progress);
            return add_tables;
        }

        public void AddChart(IEnumerable<(Contract, BarAnalysisSet)> contracts, BarFreq barFreq, BarType barType, Period period, CancellationTokenSource cts, IProgress<float> progress)
        {
            List<BarTable> barTables = AddContract(contracts, barFreq, barType, period, cts, progress);
            int pt = 0;
            foreach (BarTable bt in barTables) 
            {
                AddChart(bt, BarTables[bt]);
                pt++;
            }
        }

        public void UpdatePeriod(BarFreq barFreq, BarType barType, Period period, CancellationTokenSource cts, IProgress<float> progress)
        {
            lock (DataLockObject)
            {
                PeriodSettings[(barFreq, barType)] = period;
                var tables = BarTables.Where(n => n.Key.BarFreq == barFreq && n.Key.Type == barType);//.OrderByDescending(n => n.Key.BarFreq);
                /*
                ParallelOptions po = new ParallelOptions()
                {
                    CancellationToken = cts.Token,
                    MaxDegreeOfParallelism = 8
                };*/

                int i = 0, count = tables.Count();
                /*
                foreach(var n in tables) 
                    if (cts is CancellationTokenSource cs && !cts.IsCancellationRequested)
                    {
                        n.Key.Fetch(period, cts);
                        n.Key.CalculateOnly(n.Value);
                        i++;
                        progress?.Report(100.0f * i / count);
                    }*/

                progress?.Report(0);
                Parallel.ForEach(tables, n => {
                    if (cts is CancellationTokenSource cs && !cts.IsCancellationRequested)
                    {
                        n.Key.Fetch(period, cts);
                        n.Key.CalculateOnly(n.Value);
                        i++;
                        progress?.Report(100.0f * i / count);
                    }
                });
            }
        }

        private void AddChart(BarTable bt, BarAnalysisSet bas)
        {
            if (BarChart.List.Where(n => n.BarTable == bt).Count() == 0)
            {
                BarChart bc = new BarChart("BarChart", OhlcType.Candlestick);
                bc.ConfigChart(bt, bas);
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
    }
}
