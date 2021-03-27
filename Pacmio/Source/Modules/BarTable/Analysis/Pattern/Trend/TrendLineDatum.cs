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
    public class TrendLineDatum : PatternDatum, IEnumerable<TrendLine>
    {
        /*
        public void AddLine(TrendLine tl) 
        {
            double strength = tl.Strength;

            if (!m_TrendLines.Contains(tl)) 
                m_TrendLines.Add(tl);
            else if(m_TrendLines.Where(n => n == tl).FirstOrDefault() is TrendLine tl0)
            {
                tl0.Strength += tl.Strength;
                strength = tl0.Strength;
            }

            StrengthRange.Insert(strength);
        }*/

        public void AddLine(IEnumerable<PivotPt> points, int x3)
        {
            points.SelectPair().Select(n => new TrendLine(n)).RunEach(n => {
                m_TrendLines.Add(n);
                TotalStrengthRange.Insert(n.Strength);
                Levels.Add(new Level(n.Level(x3), n.Strength));
            });
        }

        private List<TrendLine> m_TrendLines { get; set; } = new();

        public override IEnumerable<IPatternObject> PatternObjects => m_TrendLines.Select(n => n as IPatternObject);

        public IEnumerator<TrendLine> GetEnumerator() => m_TrendLines.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
