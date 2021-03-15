/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
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
    public class PatternColumn : DatumColumn //, IEquatable<PatternColumn>, IEquatable<IChartPattern>, IEquatable<IArea>, IEquatable<string>
    {
        public PatternColumn(IChartPattern source, int maximumTrailingIndex = int.MaxValue)
            : base(source.Name, typeof(PatternDatum))
        {
            Source = source;
            //Name = Label = Source.Name;
            MaximumTrailingIndex = maximumTrailingIndex;
        }

        public IChartPattern Source { get; }

        public string AreaName => Source.AreaName;

        /// <summary>
        /// Future Index and Effective Time which ever is narrower.
        /// We also use this number to trail back the amount of Bars
        /// </summary>
        public int MaximumTrailingIndex { get; }
    }
}
