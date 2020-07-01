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
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Pacmio
{
    public class Portfolio : IDisposable
    {
        public readonly Dictionary<(BarFreq barFreq, BarType barType), Period> PeriodSettings = new Dictionary<(BarFreq barFreq, BarType barType), Period>();

        public readonly Dictionary<(Contract c, BarFreq barFreq, BarType barType), BarAnalysisList> AnalysisSettings = new Dictionary<(Contract c, BarFreq barFreq, BarType barType), BarAnalysisList>();

        public readonly Dictionary<(Contract c, BarFreq barFreq, BarType barType), BarTable> BarTables = new Dictionary<(Contract c, BarFreq barFreq, BarType barType), BarTable>();

        private object DataLockObject { get; } = new object();

        public void SaveBarTables() => BarTables.Values.AsParallel().ForAll(n => { n.Save(); n.Dispose(); });

        public void AddBarTable(BarTable bt, CancellationTokenSource cts)
        {
            lock (DataLockObject)
            {
                if (!PeriodSettings.ContainsKey((bt.BarFreq, bt.Type)))
                    PeriodSettings[(bt.BarFreq, bt.Type)] = bt.Period;
                else if (bt.Period != PeriodSettings[(bt.BarFreq, bt.Type)])
                    bt.Fetch(PeriodSettings[(bt.BarFreq, bt.Type)], cts);

                BarTables.Add(bt.Info, bt);
            }
        }

        public void AddBarTable(BarTable bt, BarAnalysisList bas, CancellationTokenSource cts)
        {
            lock (DataLockObject)
            {
                AddBarTable(bt, cts);
                AnalysisSettings[bt.Info] = bas;
                bt.CalculateOnly(bas);
            }
        }

        public void UpdateContract(BarFreq barFreq, BarType barType, IEnumerable<(Contract, BarAnalysisList)> contracts)
        {
            lock (DataLockObject)
            {
                var to_delete_analyses = AnalysisSettings.Where(n => n.Key.barFreq == barFreq && n.Key.barType == barType && !contracts.Select(n => n.Item1).Contains(n.Key.c)).ToList();
                to_delete_analyses.ForEach(n => AnalysisSettings.Remove(n.Key));

                var to_delete_tables = BarTables.Where(n => n.Key.barFreq == barFreq && n.Key.barType == barType && !contracts.Select(n => n.Item1).Contains(n.Key.c)).ToList();

                to_delete_tables.ForEach(n => {
                    BarTable bt = BarTables[n.Key];
                    BarTables.Remove(n.Key);
                    bt.Save();
                    bt.Dispose();
                });

                foreach (var (c, bas) in contracts)
                {
                    var key = (c, barFreq, barType);
                    AnalysisSettings[key] = bas;
                    if (!BarTables.ContainsKey(key))
                        BarTables[key] = new BarTable(c, barFreq, barType);
                }
            }
        }

        public void UpdatePeriod(BarFreq barFreq, BarType barType, Period period, CancellationTokenSource cts, IProgress<float> progress)
        {
            lock (DataLockObject)
            {
                PeriodSettings[(barFreq, barType)] = period;
                var tables = BarTables.Where(n => n.Key.barFreq == barFreq && n.Key.barType == barType).Select(n => n.Value);

                ParallelOptions po = new ParallelOptions()
                {
                    CancellationToken = cts.Token,
                    MaxDegreeOfParallelism = 8
                };

                int i = 0, count = tables.Count();
                Parallel.ForEach(tables, po, n =>
                {
                    n.Fetch(period, cts);
                    if (AnalysisSettings.ContainsKey((n.Contract, barFreq, barType)))
                        n.CalculateOnly(AnalysisSettings[(n.Contract, barFreq, barType)]);

                    progress?.Report(100.0f * i / count);
                    i++;
                });
            }
        }

        #region Charts

        public void GetChart()
        {


        }

        #endregion Charts

        public void Clear()
        {
            BarTables.Values.AsParallel().ForAll(n => { n.Save(); n.Dispose(); });
            BarTables.Clear();
        }

        public void Dispose()
        {
            Clear();
        }


    }
}
