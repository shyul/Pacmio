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

namespace Pacmio.Analysis
{
    public class HorizontalLineDatum : PatternDatum, IEnumerable<HorizontalLine>
    {
        public void AddLine(IEnumerable<ApexPt> points)
        {
            points.Select(n => new HorizontalLine(n)).RunEach(n => {
                m_HorizontalLines.Add(n);
                TotalStrengthRange.Insert(n.Strength);
                Levels.Add(new Level(n.Y1, n.Strength));
            });
        }

        private List<HorizontalLine> m_HorizontalLines { get; } = new();

        public IEnumerator<HorizontalLine> GetEnumerator() => m_HorizontalLines.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => m_HorizontalLines.GetEnumerator();
    }
}
