/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public class PatternColumn : Column, IEquatable<IChartPattern>, IEquatable<IArea>, IEquatable<string>
    {
        public PatternColumn(IChartPattern source)
        {
            Source = source;
            Name = Label = Source.Name;
        }

        public IChartPattern Source { get; }

        public string AreaName => Source.AreaName;

        //public Range<double> WeightRange { get; } = new Range<double>(double.MaxValue, double.MinValue);

        #region Equality

        public override int GetHashCode() => Name.GetHashCode();

        public bool Equals(IChartPattern other) => AreaName == other.AreaName;
        public static bool operator !=(PatternColumn s1, IChartPattern s2) => !s1.Equals(s2);
        public static bool operator ==(PatternColumn s1, IChartPattern s2) => s1.Equals(s2);

        public bool Equals(IArea other) => AreaName == other.Name;
        public static bool operator !=(PatternColumn s1, IArea s2) => !s1.Equals(s2);
        public static bool operator ==(PatternColumn s1, IArea s2) => s1.Equals(s2);

        public bool Equals(string other) => AreaName == other;
        public static bool operator !=(PatternColumn s1, string s2) => !s1.Equals(s2);
        public static bool operator ==(PatternColumn s1, string s2) => s1.Equals(s2);

        public override bool Equals(object other)
        {
            if (other is IChartPattern icp) return Equals(icp);
            else if (other is IArea a) return Equals(a);
            else if (other is string s) return Equals(s);
            else return false;
        }

        public static bool operator !=(PatternColumn s1, object s2) => !s1.Equals(s2);
        public static bool operator ==(PatternColumn s1, object s2) => s1.Equals(s2);

        #endregion Equality
    }
}
