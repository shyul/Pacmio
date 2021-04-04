/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio
{
    public class TrendLineDatum : PatternDatum, IEnumerable<TrendLine>
    {
        public void AddLine(IEnumerable<ApexPt> points, int x3)
        {
            points.SelectPair().Select(n => new TrendLine(n)).RunEach(n => {
                m_TrendLines.Add(n);
                TotalStrengthRange.Insert(n.Strength);
                Levels.Add(new Level(n.Level(x3), n.Strength));
            });
        }

        private List<TrendLine> m_TrendLines { get; set; } = new();

        public int Count => m_TrendLines.Count;

        public IEnumerator<TrendLine> GetEnumerator() => m_TrendLines.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
