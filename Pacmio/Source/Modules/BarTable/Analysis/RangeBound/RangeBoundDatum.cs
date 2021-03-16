/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
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
    public class RangeBoundDatum
    {
        public List<RangeBound> BoxList { get; } = new List<RangeBound>();

        public void Reset() => BoxList.Clear();

        public void Insert(IPatternObject p)
        {
            double level = p.Level;
            var list = BoxList.Where(n => n.Offset(level) <= p.Tolerance).ToArray();
            if (list.Count() > 0)
            {
                list.OrderBy(n => n.Offset(level)).First().Insert(p);
            }
            else
            {
                RangeBound pr = new RangeBound();
                BoxList.Add(pr);
                pr.Insert(p);
            }
        }

        public double Weight(double min, double max)
        {
            Range<double> r = new Range<double>(min);
            r.Insert(max);

            double w = BoxList.Where(n => n.Box.Intersects(r)).Select(n => n.Weight).Sum();

            if (min < max) return w;
            else if (min > max) return -w;
            else return 0;
        }
    }
}
