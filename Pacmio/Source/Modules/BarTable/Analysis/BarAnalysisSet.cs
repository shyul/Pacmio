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
                    Add(value);
                }
            }
        }

        private readonly List<BarAnalysis> m_List = new List<BarAnalysis>();

        public IEnumerator<BarAnalysis> GetEnumerator() => m_List.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => m_List.GetEnumerator();

        public void Clear() => m_List.Clear();

        public void Add(BarAnalysis ba) => m_List.CheckAdd(ba);

        public void Add(IEnumerable<BarAnalysis> list) 
        {
            list.ToList().ForEach(n =>
            {
                SetBarAnalysisParents(n);
                m_List.CheckAdd(n);
                SetBarAnalysisChildren(n);
            });

            PrintList();
        }

        private void SetBarAnalysisParents(BarAnalysis ba)
        {
            ba.Parents.Where(n => n is BarAnalysis).Select(n => (BarAnalysis)n).ToList().ForEach(n =>
            {
                SetBarAnalysisParents(n);
                m_List.CheckAdd(n);
            });
        }

        private void SetBarAnalysisChildren(BarAnalysis ba)
        {
            ba.Children.Where(n => n is BarAnalysis).Select(n => (BarAnalysis)n).ToList().ForEach(n =>
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

        public List<SignalColumn> SignalColumns { get; } = new List<SignalColumn>();
    }
}
