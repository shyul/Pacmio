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

        public static BarChart GetChart(this BarTable bt, BarAnalysisList bat)
        {
            BarChart bc = new("BarChart", OhlcType.Candlestick);
            bc.Config(bt, bat);
            Root.Form.AddForm(DockStyle.Fill, 0, bc);
            return bc;
        }

        public static void GetChart(this BarTableSet bts, BarAnalysisSet bas)
        {
            bts.CalculateRefresh(bas);

            foreach ((BarFreq freq, PriceType type, BarAnalysisList bat) in bas)
            {
                BarChart bc = new("BarChart", OhlcType.Candlestick);
                BarTable bt = bts[freq, type];
                bc.Config(bt, bat);
                Root.Form.AddForm(DockStyle.Fill, 0, bc);
            }
        }

        
        private static List<BarChart> List { get; } = new();

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
