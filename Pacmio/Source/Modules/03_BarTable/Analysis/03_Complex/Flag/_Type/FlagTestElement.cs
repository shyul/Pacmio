/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio
{
    public class FlagTestElement
    {
        public FlagTestElement(Bar b, double rangeOfTotal)
        {
            Bar = b;
            RangeOfTotal = rangeOfTotal;
        }

        public Bar Bar { get; }

        public double RangeOfTotal { get; }

        public double RangeLocation { get; set; }

        public bool IsRunning { get; set; } = false;
    }
}
