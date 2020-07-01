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
        public string Name { get; set; } = "Default BarAnalysisGroup";

        public int Order { get; set; } = 0;

        public IEnumerable<BarAnalysis> List
        {
            get
            {
                lock (m_List) return m_List.ToArray();
            }
            set
            {
                lock (m_List)
                {
                    m_List.Clear();
                    value.ToList().ForEach(n => {
                        SetBarAnalysisParents(n);
                        m_List.CheckAdd(n);
                        Console.WriteLine(Name + " | Added BA: " + n.Name);
                        SetBarAnalysisChildren(n);
                    });
                }
            }
        }


        private readonly List<BarAnalysis> m_List = new List<BarAnalysis>();

        private void SetBarAnalysisParents(BarAnalysis ba)
        {
            ba.Parents.Where(n => n is BarAnalysis).Select(n => (BarAnalysis)n).ToList().ForEach(n =>
            {
                SetBarAnalysisParents(n);
                m_List.CheckAdd(n);
                Console.WriteLine(Name + " | Added BA: " + n.Name);
            });
        }

        private void SetBarAnalysisChildren(BarAnalysis ba)
        {
            ba.Children.Where(n => n is BarAnalysis).Select(n => (BarAnalysis)n).ToList().ForEach(n =>
            {
                m_List.CheckAdd(n);
                Console.WriteLine(Name + " | Added BA: " + n.Name);
                SetBarAnalysisChildren(n);
            });
        }


    }
}
