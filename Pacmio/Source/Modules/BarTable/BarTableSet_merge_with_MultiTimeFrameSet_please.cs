/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// BarTableSet
/// 1. For Live Trading Only
/// 2. Shall be static
/// 3. Avoid duplicated data;
/// 4. Avoid duplocated calculations
/// 5. Thus, imporved and centrallized data parallelism
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xu;
using Xu.Chart;

namespace Pacmio
{/*
    public class BarTableSet : IDisposable
    {
        #region Download / Fetch Operation
        
        public static void Download(IEnumerable<Contract> contracts, IEnumerable<(BarFreq freq, BarType type, Period period)> settings, CancellationTokenSource cts, IProgress<float> progress)
        {
            List<(BarFreq freq, BarType type, Period period)> settings_list = new List<(BarFreq freq, BarType type, Period period)>() { (BarFreq.Daily, BarType.Trades, new Period(new DateTime(1000, 1, 1), DateTime.Now)) };

            var priority_settings = settings.Where(n => n.type == BarType.Trades && (n.freq == BarFreq.Hourly || n.freq == BarFreq.Minute)).OrderByDescending(n => n.freq);
            settings_list.AddRange(priority_settings);

            var remaining_settings = settings.Where(n => !priority_settings.Contains(n) && !(n.freq == BarFreq.Daily && n.type == BarType.Trades)).OrderByDescending(n => n.freq);
            settings_list.AddRange(remaining_settings);

            ParallelOptions po = new ParallelOptions()
            {
                MaxDegreeOfParallelism = Math.Ceiling(Root.DegreeOfParallelism / 3D).ToInt32(1)
            };

            int i = 0, count = contracts.Count() * settings_list.Count();
            Parallel.ForEach(contracts, po, c =>
            {
                if (cts.IsContinue())
                {
                    foreach (var (freq, type, period) in settings_list)
                    {
                        if (cts.IsContinue())
                        {
                            BarTable bt = new BarTable(c, freq, type);
                            bt.Fetch(period, cts);
                            i++;
                            if (cts.IsContinue()) progress?.Report(100.0f * i / count);
                        }
                        else
                            return;
                    }
                }
                else
                    return;
            });
        

#endregion Download / Fetch Operation

    public Dictionary<(BarFreq barFreq, BarType barType), Period> PeriodSettings { get; } = new Dictionary<(BarFreq barFreq, BarType barType), Period>();

        public HashSet<BarTable> BarTables { get; } = new HashSet<BarTable>();

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
            lock (BarTables)
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
            lock (BarTables)
            {
                BarTable bt = AddContract(c, barFreq, barType);
                bt.Fetch(period, cts);
                period = bt.Period;
                return bt;
            }
        }

        public void AddChart(Contract c, BarAnalysisSet ias, BarFreq barFreq, BarType barType, ref Period period, CancellationTokenSource cts)
        {
            BarTable bt = AddContract(c, barFreq, barType);
            AddChart(bt, ias);
            if (cts.IsContinue())
            {
                if (bt.Status == TableStatus.Default || bt.Count == 0 || period != bt.Period)
                {
                    bt.Fetch(period, cts);
                    period = bt.Period;
                }
                bt.CalculateRefresh(ias);
            }
        }

        private List<BarTable> AddContract(IEnumerable<Contract> contracts, BarFreq barFreq, BarType barType)
        {
            lock (BarTables)
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
            if (cts.IsContinue())
            {
                List<BarTable> add_tables = AddContract(contracts, barFreq, barType);
                UpdatePeriod(barFreq, barType, period, cts, progress);
                return add_tables;
            }
            return new List<BarTable>();
        }

        public void AddChart(IEnumerable<(Contract, BarAnalysisSet)> contracts, BarFreq barFreq, BarType barType, Period period, CancellationTokenSource cts, IProgress<float> progress)
        {
            progress?.Report(0);
            PeriodSettings[(barFreq, barType)] = period;

            ParallelOptions po = new ParallelOptions()
            {
                //CancellationToken = cts.Token,
                MaxDegreeOfParallelism = Math.Ceiling(Root.DegreeOfParallelism / 3D).ToInt32(1)
            };

            int i = 0, count = contracts.Count();
            Parallel.ForEach(contracts, po, n =>
            {
                if (cts.IsContinue())
                {
                    var (c, ias) = n;
                    AddChart(c, ias, barFreq, barType, ref period, cts);
                    i++;
                    if (cts.IsContinue()) progress?.Report(100.0f * i / count);
                }
            });
        }

        public void Calculate(BarAnalysisSet bas, CancellationTokenSource cts, IProgress<float> progress)
        {
            progress?.Report(0);
            ParallelOptions po = new ParallelOptions()
            {
                //CancellationToken = cts.Token,
                MaxDegreeOfParallelism = Root.DegreeOfParallelism
            };
            int i = 0, count = BarTables.Count();
            Parallel.ForEach(BarTables, po, bt =>
            {
                if (cts.IsContinue())
                {
                    bt.CalculateRefresh(bas);
                    i++;
                    if (cts.IsContinue()) progress?.Report(100.0f * i / count);
                }
            });
        }

        private void AddChart(BarTable bt, BarAnalysisSet bas)
        {
            if (BarChart.List.Where(n => n.BarTable == bt).Count() == 0)
            {
                BarChart bc = new BarChart("BarChart", OhlcType.Candlestick);
                bc.Config(bt, bas);
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

        public void UpdatePeriod(BarFreq barFreq, BarType barType, Period period, CancellationTokenSource cts, IProgress<float> progress)
        {
            progress?.Report(0);
            if (cts.IsContinue())
                lock (BarTables)
                {
                    PeriodSettings[(barFreq, barType)] = period;
                    var tables = BarTables.Where(n => n.BarFreq == barFreq && n.Type == barType);

                    ParallelOptions po = new ParallelOptions()
                    {
                        //CancellationToken = cts.Token,
                        MaxDegreeOfParallelism = Root.DegreeOfParallelism
                    };

                    int i = 0, count = tables.Count();
                    Parallel.ForEach(tables, po, n =>
                    {
                        if (cts.IsContinue())
                        {
                            n.Fetch(period, cts);
                            i++;
                            if (cts.IsContinue()) progress?.Report(100.0f * i / count);
                        }
                        else
                            return;
                    });
                }
        }
    }*/
}
