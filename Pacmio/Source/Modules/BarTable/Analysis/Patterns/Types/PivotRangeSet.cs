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
    public class PivotRangeSet : IEquatable<PivotRangeSet>, IEquatable<IChartPattern>, IEquatable<IArea>, IEquatable<string>
    {
        public PivotRangeSet(IChartPattern pattern) => AreaName = pattern.AreaName;

        public string AreaName { get; }

        public List<PivotRange> RangeList { get; } = new List<PivotRange>();

        public void Reset() => RangeList.Clear();

        public void Insert(IPivot p, double tolerance)
        {
            double level = p.Level;
            var list = RangeList.Where(n => n.Offset(level) <= tolerance).OrderBy(n => n.Offset(level));
            if (list.Count() > 0)
            {
                list.First().Insert(p);
            }
            else
            {
                PivotRange pr = new PivotRange();
                RangeList.Add(pr);
                pr.Insert(p);
            }
        }

        #region Equality

        public override int GetHashCode() => GetType().GetHashCode() ^ AreaName.GetHashCode();

        public bool Equals(PivotRangeSet other) => AreaName == other.AreaName;
        public static bool operator !=(PivotRangeSet s1, PivotRangeSet s2) => !s1.Equals(s2);
        public static bool operator ==(PivotRangeSet s1, PivotRangeSet s2) => s1.Equals(s2);

        public bool Equals(IChartPattern other) => AreaName == other.AreaName;
        public static bool operator !=(PivotRangeSet s1, IChartPattern s2) => !s1.Equals(s2);
        public static bool operator ==(PivotRangeSet s1, IChartPattern s2) => s1.Equals(s2);

        public bool Equals(IArea other) => AreaName == other.Name;
        public static bool operator !=(PivotRangeSet s1, IArea s2) => !s1.Equals(s2);
        public static bool operator ==(PivotRangeSet s1, IArea s2) => s1.Equals(s2);

        public bool Equals(string other) => AreaName == other;
        public static bool operator !=(PivotRangeSet s1, string s2) => !s1.Equals(s2);
        public static bool operator ==(PivotRangeSet s1, string s2) => s1.Equals(s2);

        public override bool Equals(object other)
        {
            if (other is PivotRangeSet prs) return Equals(prs);
            else if (other is IChartPattern icp) return Equals(icp);
            else if (other is IArea a) return Equals(a);
            else if (other is string s) return Equals(s);
            else return false;
        }

        public static bool operator !=(PivotRangeSet s1, object s2) => !s1.Equals(s2);
        public static bool operator ==(PivotRangeSet s1, object s2) => s1.Equals(s2);

        #endregion Equality
    }
}
