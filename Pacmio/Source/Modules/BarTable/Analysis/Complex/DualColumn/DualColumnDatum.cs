/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio.Analysis
{
    public class DualColumnDatum : IDatum
    {
        public List<DualColumnType> List { get; } = new List<DualColumnType>();

        public double Ratio { get; set; }

        public double Difference { get; set; }

        public double DifferenceRatio { get; set; }
    }
}
