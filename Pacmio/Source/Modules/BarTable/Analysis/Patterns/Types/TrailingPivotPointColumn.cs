/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using Xu;

namespace Pacmio
{
    public class TrailingPivotPointColumn : Column
    {
        public TrailingPivotPointColumn(string name) => Name = Label = name;

        public TrailingPivotPointColumn(string name, string label)
        {
            Name = name;
            Label = label;
        }
    }
}
