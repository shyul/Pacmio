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
        /*
        public static void AddColumnSeries(this BarChart bc, NumericColumn data)
        {
            AdColumnSeries ColumnSeries = new(data, data, 50, 0, 0)
            {
                Name = data.Name,
                LegendName = "Legend_" + data.Name,
                Label = "",
                Importance = Importance.Major,
                Side = AlignType.Right,
                IsAntialiasing = false,
                Enabled = true,
            };

            string AreaName = data.Name + "_Area";
            float AreaRatio = 10;

            lock (bc.GraphicsLockObject)
            {
                BarChartArea area =
                    bc[AreaName] is BarChartArea oa ?
                    oa :
                    bc.AddArea(new BarChartArea(bc, AreaName, AreaRatio)
                    {
                        //Reference = 0,
                        HasXAxisBar = false,
                    });

                area.AddSeries(ColumnSeries);
            }
        }

        public static void AddLineSeries(this BarChart bc, NumericColumn data)
        {


        }
        */
    }
}
