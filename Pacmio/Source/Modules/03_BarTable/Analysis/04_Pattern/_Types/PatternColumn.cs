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
    public class PatternColumn : DatumColumn
    {
        public PatternColumn(PatternAnalysis source, Type datumType, int maximumTrailingIndex = int.MaxValue)
            : base(source.Name, datumType)
        {
            Source = source;
            MaximumTrailingIndex = maximumTrailingIndex;
        }

        public PatternAnalysis Source { get; }

        public string AreaName => Source.AreaName;

        /// <summary>
        /// Future Index and Effective Time which ever is narrower.
        /// We also use this number to trail back the amount of Bars
        /// </summary>
        public int MaximumTrailingIndex { get; }
    }
}
