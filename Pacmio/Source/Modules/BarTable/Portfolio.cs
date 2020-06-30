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
        public readonly ConcurrentDictionary<(Contract c, BarFreq barFreq, BarType barType), (Period period, BarAnalysisList barAnalyses)> Settings = new ConcurrentDictionary<(Contract c, BarFreq barFreq, BarType barType), (Period period, BarAnalysisList barAnalyses)>();

        public readonly ConcurrentDictionary<(Contract c, BarFreq barFreq, BarType barType), BarTable> BarTables = new ConcurrentDictionary<(Contract c, BarFreq barFreq, BarType barType), BarTable>();

        public void SaveBarTables() => BarTables.Values.AsParallel().ForAll(n => { n.Save(); n.Dispose(); });

        public void Fetch(CancellationTokenSource cts, IProgress<float> progress)
        {
            Task.Run(() => {
                lock (Settings)
                {
                    SaveBarTables();
                    BarTables.Clear();

                    int i = 0, count = Settings.Count;
                    Parallel.ForEach(Settings.Keys, n => {
                        var (c, barFreq, barType) = n;
                        BarTable bt = new BarTable(c, barFreq, barType) { Portfolio = this };
                        BarTables.TryAdd((c, barFreq, barType), bt);

                        BarTables[(c, barFreq, barType)].Fetch(Settings[(c, barFreq, barType)].period, cts);

                        progress?.Report(100.0f * i / count);
                        i++;
                    });
                    /*
                    foreach (var (c, barFreq, barType) in Settings.Keys)
                    {
                        BarTable bt = new BarTable(c, barFreq, barType) { Portfolio = this };
                        BarTables.TryAdd((c, barFreq, barType), bt);

                        BarTables[(c, barFreq, barType)].Fetch(Settings[(c, barFreq, barType)].period, cts);

                        progress?.Report(100.0f * i / count);
                        i++;
                    }*/
                }
            }, cts.Token);
        }

        public void Calculate(CancellationTokenSource cts, IProgress<float> progress)
        {
            Task.Run(() => {
                lock (Settings)
                {
                    int i = 0, count = Settings.Count;
                    Parallel.ForEach(Settings.Keys, n => {
                        var (c, barFreq, barType) = n;
                        if (BarTables.ContainsKey((c, barFreq, barType)))
                        {
                            BarTables[(c, barFreq, barType)].CalculateOnly(Settings[(c, barFreq, barType)].barAnalyses.List);
                        }
                        progress?.Report(100.0f * i / count);
                        i++;
                    });
                }
            }, cts.Token);
        }


        #region Charts




        private static PacmioForm Form => Root.Form;

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
