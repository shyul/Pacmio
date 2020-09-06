﻿/// ***************************************************************************
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
    public class PatternColumn : Column//, IEquatable<PatternColumn>, IEquatable<IChartPattern>, IEquatable<IArea>, IEquatable<string>
    {
        public PatternColumn(IChartPattern source)
        {
            Source = source;
            Name = Label = Source.Name;
        }

        public IChartPattern Source { get; }

        public string AreaName => Source.AreaName;

        /// <summary>
        /// Future Index and Effective Time which ever is narrower.
        /// We also use this number to trail back the amount of Bars
        /// </summary>
        public int MaximumTrailingIndex { get; set; } = int.MaxValue;
    }
}
