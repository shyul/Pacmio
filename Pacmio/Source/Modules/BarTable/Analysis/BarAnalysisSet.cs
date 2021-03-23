/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// BarTable Data Types
/// 
/// ***************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xu;
using Pacmio.Analysis;

namespace Pacmio
{
    public class BarAnalysisSet : IEnumerable<BarAnalysis>
    {
        public BarAnalysisSet(Indicator ind) { List = new List<BarAnalysis>() { ind }; }

        public BarAnalysisSet(IEnumerable<BarAnalysis> list) => List = list;

        public BarAnalysisSet(BarAnalysisSet bas) => List = bas.List;

        public IEnumerable<BarAnalysis> List
        {
            get
            {
                return m_List;
            }
            private set
            {
                lock (m_List)
                {
                    Clear();
                    AddRange(value);
                }
            }
        }

        private List<BarAnalysis> m_List { get; } = new List<BarAnalysis>();

        public int Count => m_List.Count;

        public void Clear() => m_List.Clear();

        public IEnumerator<BarAnalysis> GetEnumerator() => m_List.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => m_List.GetEnumerator();

        public bool Contains(BarAnalysis ba) => m_List.Contains(ba);

        //public bool Contains(Column ba) => m_List.Contains(ba);

        private void AddRange(IEnumerable<BarAnalysis> list)
        {
            // The purpose of ordering by the children and parents first is
            // We want to add the ones with less dependancy first so its setting
            // can be applied to the Set, instead of been ignored.
            //list.OrderBy(n => n.Children.Count).ThenBy(n => n.Parents.Count).ThenBy(n => n.Order).ToList().ForEach(n =>
            list.OrderBy(n => n.Order).ToList().ForEach(n =>
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
            this.RunEach(n =>
            {
                Console.WriteLine("BarAnalysisSet | BA: " + n.Name);
            });
        }

        public IEnumerable<PatternAnalysis> PatternAnalyses => m_List.SelectType<PatternAnalysis, BarAnalysis>();

        public List<PatternColumn> PatternColumns => PatternAnalyses.Select(n => n.Column_Result).ToList();

        public IEnumerable<IChartBackground> ChartBackgrounds => m_List.SelectType<IChartBackground, BarAnalysis>().OrderBy(n => n.DrawOrder);

        public IEnumerable<IChartSeries> ChartSeries => m_List.SelectType<IChartSeries, BarAnalysis>().OrderBy(n => n.DrawOrder);

        public IEnumerable<IChartOverlay> ChartOverlays => m_List.SelectType<IChartOverlay, BarAnalysis>().OrderBy(n => n.DrawOrder);

        public IEnumerable<ITagAnalysis> TagSeries => m_List.SelectType<ITagAnalysis, BarAnalysis>();

        /// <summary>
        /// List all indicator types, and aggreagte all signal columns here...
        /// </summary>
        public virtual IEnumerable<SignalColumn> SignalColumns => m_List.Where(n => n is Indicator id && id.SignalColumns is SignalColumn[]).SelectMany(n => ((Indicator)n).SignalColumns).Where(n => n.Enabled).OrderBy(n => n.Order);
    }
}
