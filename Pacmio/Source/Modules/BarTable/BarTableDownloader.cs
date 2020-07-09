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
using Xu;

namespace Pacmio
{
    public static class BarTableDownloader
    {
        public static void Download(IEnumerable<Contract> contracts, IEnumerable<(BarFreq freq, BarType type, Period period)> settings, CancellationTokenSource cts, IProgress<float> progress)
        {
            List<(BarFreq freq, BarType type, Period period)> settings_list = new List<(BarFreq freq, BarType type, Period period)>() { (BarFreq.Daily, BarType.Trades, new Period(new DateTime(1000, 1, 1), DateTime.Now)) };

            var priority_settings = settings.Where(n => n.type == BarType.Trades && (n.freq == BarFreq.Hourly || n.freq == BarFreq.Minute)).OrderByDescending(n => n.freq);
            settings_list.AddRange(priority_settings);

            var remaining_settings = settings.Where(n => !priority_settings.Contains(n) && !(n.freq == BarFreq.Daily && n.type == BarType.Trades)).OrderByDescending(n => n.freq);
            settings_list.AddRange(remaining_settings);

            ParallelOptions po = new ParallelOptions()
            {
                //CancellationToken = cts.Token,
                MaxDegreeOfParallelism = Math.Ceiling(Root.DegreeOfParallelism / 3D).ToInt32(1)
            };

            int i = 0, count = contracts.Count() * settings_list.Count();
            Parallel.ForEach(contracts, po, c => {
                if (cts.Continue())
                {
                    foreach (var (freq, type, period) in settings_list)
                    {
                        if (cts.Continue())
                        {
                            BarTable bt = new BarTable(c, freq, type);
                            bt.Fetch(period, cts);
                            i++;
                            if (cts.Continue()) progress?.Report(100.0f * i / count);
                        }
                        else
                            return;
                    }
                }
                else
                    return;
            });
        }
    }
}
