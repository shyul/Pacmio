/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public static class BarChartManager
    {
        private static List<BarChart> List { get; } = new();

        public static BarChart GetChart(this BarTable bt, BarAnalysisSet bas)
        {
            BarChart bc = new("BarChart", OhlcType.Candlestick);
            bc.Config(bt, bas);
            Root.Form.AddForm(DockStyle.Fill, 0, bc);
            return bc;
        }

        public static BarChart GetChart(this BarTable bt, Strategy inds)
            => GetChart(bt, inds[bt].BarAnalysisSet);

        public static IEnumerable<BarChart> GetCharts(this BarTableSet bts, Strategy inds, Period periodLimit, CancellationTokenSource cts = null)
        {
            bts.Build(inds, periodLimit, cts);
            return bts.Select(n => n.bt.GetChart(inds[n.bt].BarAnalysisSet));
        }

        public static void Add(BarChart bc)
        {
            lock (List)
                List.CheckAdd(bc);
        }

        public static void Remove(BarChart bc)
        {
            lock (List)
                List.CheckRemove(bc);
        }

        public static void RemoveAll()
        {
            lock (List)
                List.ForEach(n => n.Close());
        }

        public static void PointerToEndAll()
        {
            lock (List) List.ForEach(bc => { bc.PointerSnapToEnd(); });
        }
    }
}
