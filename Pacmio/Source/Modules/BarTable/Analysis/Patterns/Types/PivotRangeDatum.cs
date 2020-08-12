/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// BarTable Data Types
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public class PivotRangeDatum
    {
        public List<PivotRange> RangeList { get; } = new List<PivotRange>();

        public void Reset() => RangeList.Clear();

        public void Insert(IPivot p)
        {
            double level = p.Level;
            var list = RangeList.Where(n => n.Offset(level) <= p.Tolerance).ToArray();
            if (list.Count() > 0)
            {
                list.OrderBy(n => n.Offset(level)).First().Insert(p);
            }
            else
            {
                PivotRange pr = new PivotRange();
                RangeList.Add(pr);
                pr.Insert(p);
            }
        }
    }
}
