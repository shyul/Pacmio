/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    public interface IOption
    {
        string Right { get; set; }

        double Strike { get; set; }

        string Multiplier { get; set; }

        string LastTradeDateOrContractMonth { get; set; }
    }
}
