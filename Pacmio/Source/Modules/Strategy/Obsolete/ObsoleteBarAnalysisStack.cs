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

namespace Pacmio
{
    public sealed class ObsoleteBarAnalysisStack
    {
        private readonly List<BarAnalysis> List = new List<BarAnalysis>();



        //public IEnumerable<ConditionAnalysis> ConditionAnalyses => List.Where(ba => ba is ConditionAnalysis).Select(ca => (ConditionAnalysis)ca);

        /// <summary>
        /// Do not use this for massive calculation task!
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public BarAnalysis this[string name]
        {
            get
            {
                var list = List.Where(n => n.Name == name);

                if (list.Count() > 0)
                    return list.First();
                else
                    return null;
            }
        }

        public T CheckAddAnalysis<T>(T ba) where T : BarAnalysis
        {
            if (!List.Contains(ba))
            {
                List.Add(ba);
                return ba;
            }
            else
            {
                ba.Cancel();
                T ba2 = (T)(List.Where(n => n == ba).First());
                return ba2;
            }
        }

        /// <summary>
        /// Remove a column
        /// Check if the column has dependencies
        /// </summary>
        /// <param name="ba"></param>
        /// <returns></returns>
        public void RemoveAnalysis(BarAnalysis ba)
        {
            List.CheckRemove(ba);
            //ba.Enabled = false;
            ba.Cancel();
        }

        /// <summary>
        /// Clean up unused analysis and data points
        /// </summary>
        //public void CleanOrphanedColumns() => Rows.ForEach(n => n.CleanUp());

        //private IEnumerable<BarAnalysis> CalculationItems => List.Where(n => n.Enabled).OrderBy(n => n.Order);
        //public void Calculate(BarTable bt) => bt.Calculate(CalculationItems);
        //public void Calculate(IEnumerable<BarTable> btList) => Parallel.ForEach(btList, bt => Calculate(bt));
    }
}
