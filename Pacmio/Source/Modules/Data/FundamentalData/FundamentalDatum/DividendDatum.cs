/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    [Serializable, DataContract(Name = "DividendDatum")]
    public class DividendDatum : ARPDatum
    {
        public DividendDatum(DateTime asOfDate, double close) : base(asOfDate, close) { }

        [IgnoreDataMember]
        public double Divident { get => Value; set => Value_P = value; }

        [IgnoreDataMember]
        public double Percent => Close_Price > 0 ? Divident / Close_Price : 0;
    }
}
