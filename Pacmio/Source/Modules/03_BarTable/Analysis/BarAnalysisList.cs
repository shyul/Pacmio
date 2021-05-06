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

namespace Pacmio
{
    public class BarAnalysisList : IEnumerable<BarAnalysis>, ISignalSource
    {
        public BarAnalysisList(BarAnalysisSet bas, IEnumerable<BarAnalysis> list) 
        {
            BarAnalysisSet = bas;
            List = list;
        }

        public BarAnalysisList(IEnumerable<BarAnalysis> list) => List = list;

        public BarAnalysisSet BarAnalysisSet { get; } = null;

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
            ba.Parents.Where<BarAnalysis>().RunEach(n =>
            {
                SetBarAnalysisParents(n);
                m_List.CheckAdd(n);
            });
        }

        private void SetBarAnalysisChildren(BarAnalysis ba)
        {
            ba.Children.Where<BarAnalysis>().RunEach(n =>
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

        public IEnumerable<PatternAnalysis> PatternAnalyses => m_List.Where<PatternAnalysis>();

        public IEnumerable<SignalAnalysis> SignalList => BarAnalysisSet is BarAnalysisSet bas ? bas.SignalList : m_List.Where<SignalAnalysis>();

        public IEnumerable<Strategy> StrategyList => m_List.Where<Strategy>();

        public IEnumerable<IChartBackground> ChartBackgrounds => m_List.Where<IChartBackground>().OrderBy(n => n.DrawOrder);

        public IEnumerable<IChartSeries> ChartSeries => m_List.Where<IChartSeries>().OrderBy(n => n.DrawOrder);

        public IEnumerable<IChartOverlay> ChartOverlays => m_List.Where<IChartOverlay>().OrderBy(n => n.DrawOrder);

        public IEnumerable<ITagAnalysis> TagSeries => m_List.Where<ITagAnalysis>();
    }
}
