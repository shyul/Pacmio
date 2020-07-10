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
using System.Windows.Forms;

namespace Pacmio
{
    public class BarAnalysisSet
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
                    m_List.Clear();
                    value.ToList().ForEach(n => {
                        SetBarAnalysisParents(n);
                        m_List.CheckAdd(n);
                        SetBarAnalysisChildren(n);
                    });
                }

                PrintList();
            }
        }

        private readonly List<BarAnalysis> m_List = new List<BarAnalysis>();

        public void Clear() => m_List.Clear();

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
            List.ToList().ForEach(n => {
                Console.WriteLine("BarAnalysisSet | Added BA: " + n.Name);
            });
        }

        public List<SignalColumn> SignalColumns { get; } = new List<SignalColumn>();
    }
}
