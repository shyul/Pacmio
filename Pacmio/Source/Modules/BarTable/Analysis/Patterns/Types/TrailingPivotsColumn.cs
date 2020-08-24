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
    public class TrailingPivotsColumn : Column
    {
        public TrailingPivotsColumn(string name) => Name = Label = name;

        public TrailingPivotsColumn(string name, string label)
        {
            Name = name;
            Label = label;
        }
    }
}
