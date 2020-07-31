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
        public PatternColumn(string name) => Name = Label = name;

        public PatternColumn(string name, string label)
        {
            Name = name;
            Label = label;
        }
    }
}
