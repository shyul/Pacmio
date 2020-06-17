/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Xu;
using System.Linq;

namespace Pacmio
{
    public class ContractColumn : Column
    {
        public ContractColumn(string name) => Name = name;

        public ContractColumn(string name, string label)
        {
            Name = name;
            Label = label;
        }
    }
}
