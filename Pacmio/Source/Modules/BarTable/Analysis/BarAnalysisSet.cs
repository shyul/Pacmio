/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// BarTable Data Types
/// 
/// ***************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio
{
    public class BarAnalysisSet : IEnumerable<BarAnalysis>
    {
        public BarAnalysisSet() { }

        public BarAnalysisSet(IEnumerable<BarAnalysis> list) => List = list;

        public BarAnalysisSet(BarAnalysisSet bas) => List = bas.List;

        public IEnumerable<BarAnalysis> List
        {
            get
            {
                return m_List;
            }
            set
            {
                lock (m_List)
                {
                    Clear();
                    AddRange(value);
                }
            }
        }

        private readonly List<BarAnalysis> m_List = new List<BarAnalysis>();

        public int Count => m_List.Count;

        public void Clear() => m_List.Clear();

        public IEnumerator<BarAnalysis> GetEnumerator() => m_List.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => m_List.GetEnumerator();

        public bool Contains(BarAnalysis ba) => m_List.Contains(ba);

        public void Add(BarAnalysis ba) => m_List.CheckAdd(ba);

        public void AddRange(IEnumerable<BarAnalysis> list)
        {
            // The purpose of ordering by the children and parents first is
            // We want to add the ones with less dependancy first so its setting
            // can be applied to the Set, instead of been ignored.
            list.OrderBy(n => n.Children.Count).ThenBy(n => n.Parents.Count).ThenBy(n => n.Order).ToList().ForEach(n =>
            {
                SetBarAnalysisParents(n);
                m_List.CheckAdd(n);
                SetBarAnalysisChildren(n);
            });

            PrintList();
        }

        private void SetBarAnalysisParents(BarAnalysis ba)
        {
            ba.Parents.Where(n => n is BarAnalysis).Select(n => n as BarAnalysis).ToList().ForEach(n =>
            {
                SetBarAnalysisParents(n);
                m_List.CheckAdd(n);
            });
        }

        private void SetBarAnalysisChildren(BarAnalysis ba)
        {
            ba.Children.Where(n => n is BarAnalysis).Select(n => n as BarAnalysis).ToList().ForEach(n =>
            {
                m_List.CheckAdd(n);
                SetBarAnalysisChildren(n);
            });
        }

        public void PrintList()
        {
            this.ToList().ForEach(n =>
            {
                Console.WriteLine("BarAnalysisSet | Added BA: " + n.Name);
            });
        }

        /// <summary>
        /// List all BarAnalysis which also present as ChartSeries
        /// </summary>
        public IEnumerable<IChartSeries> ChartSeries => m_List.Where(n => n is IChartSeries ics).Select(n => (IChartSeries)n).OrderBy(n => n.SeriesOrder);

        /// <summary>
        /// List all indicator types, and aggreagte all signal columns here...
        /// </summary>
        public virtual IEnumerable<SignalColumn> SignalColumns => m_List.Where(n => n is Indicator id && id.SignalColumns is SignalColumn[]).SelectMany(n => ((Indicator)n).SignalColumns).Where(n => n.Enabled).OrderBy(n => n.Order);

        public virtual IEnumerable<SignalColumn> SignalColumn(SignalColumnType type) => SignalColumns.Where(n => n.Type == type);
    }
}
