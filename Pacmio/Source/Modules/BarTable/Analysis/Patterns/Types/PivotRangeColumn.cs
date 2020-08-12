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
    public class PivotRangeColumn : Column, IEquatable<PivotRangeColumn>, IEquatable<IChartPattern>, IEquatable<IArea>, IEquatable<string>
    {
        public PivotRangeColumn(string name) => Name = Label = name;

        public string AreaName => Name;

        #region Equality
        public override int GetHashCode() => AreaName.GetHashCode();

        public bool Equals(PivotRangeColumn other) => AreaName == other.AreaName;
        public static bool operator !=(PivotRangeColumn s1, PivotRangeColumn s2) => !s1.Equals(s2);
        public static bool operator ==(PivotRangeColumn s1, PivotRangeColumn s2) => s1.Equals(s2);

        public bool Equals(IChartPattern other) => AreaName == other.AreaName;
        public static bool operator !=(PivotRangeColumn s1, IChartPattern s2) => !s1.Equals(s2);
        public static bool operator ==(PivotRangeColumn s1, IChartPattern s2) => s1.Equals(s2);

        public bool Equals(IArea other) => AreaName == other.Name;
        public static bool operator !=(PivotRangeColumn s1, IArea s2) => !s1.Equals(s2);
        public static bool operator ==(PivotRangeColumn s1, IArea s2) => s1.Equals(s2);

        public bool Equals(string other) => AreaName == other;
        public static bool operator !=(PivotRangeColumn s1, string s2) => !s1.Equals(s2);
        public static bool operator ==(PivotRangeColumn s1, string s2) => s1.Equals(s2);

        public override bool Equals(object other)
        {
            if (other is PivotRangeColumn prs) return Equals(prs);
            else if (other is IChartPattern icp) return Equals(icp);
            else if (other is IArea a) return Equals(a);
            else if (other is string s) return Equals(s);
            else return false;
        }

        public static bool operator !=(PivotRangeColumn s1, object s2) => !s1.Equals(s2);
        public static bool operator ==(PivotRangeColumn s1, object s2) => s1.Equals(s2);

        #endregion Equality
    }
}
