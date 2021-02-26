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
    [Serializable, DataContract(Name = "SplitDatum")]
    public class SplitDatum : FundamentalDatum
    {
        public SplitDatum(DateTime asOfDate, double close, double split) : base(asOfDate, close)
        {
            Split = split;
        }

        [DataMember]
        public double Split { get; set; }

        [IgnoreDataMember]
        public override double Value => Split;
    }
}
