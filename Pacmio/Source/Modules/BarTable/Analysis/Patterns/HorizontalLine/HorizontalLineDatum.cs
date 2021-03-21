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
        public void AddLine(IEnumerable<PivotPt> points)
        {
            points.Select(n => new HorizontalLine(n)).RunEach(n => { m_HorizontalLines.Add(n); StrengthRange.Insert(n.Strength); });
        }

        private List<HorizontalLine> m_HorizontalLines { get; } = new();

        public override IEnumerable<IPatternObject> PatternObjects => m_HorizontalLines.Select(n => n as IPatternObject);

        public IEnumerator<HorizontalLine> GetEnumerator() => m_HorizontalLines.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => m_HorizontalLines.GetEnumerator();
    }
}
