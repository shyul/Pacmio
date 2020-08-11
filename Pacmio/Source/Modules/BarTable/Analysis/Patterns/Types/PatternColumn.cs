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

namespace Pacmio
{
    public class PatternColumn : Column
    {
        public PatternColumn(IChartPattern source) 
        {
            Source = source;
            Name = Label = Source.Name;
        }

        public IChartPattern Source { get; }
    }
}
