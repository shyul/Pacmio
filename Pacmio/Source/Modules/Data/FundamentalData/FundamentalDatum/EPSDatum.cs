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
    [Serializable, DataContract(Name = "EPSDatum")]
    public class EPSDatum : ARPDatum
    {
        public EPSDatum(DateTime asOfDate, double close) : base(asOfDate, close) { }

        [IgnoreDataMember]
        public double EPS { get => Value; set => Value_P = value; }

        [IgnoreDataMember]
        public double PE => EPS != 0 ? Close_Price / EPS : double.NaN;
    }
}
