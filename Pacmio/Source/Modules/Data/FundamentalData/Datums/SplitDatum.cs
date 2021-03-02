/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    [Serializable, DataContract(Name = "Split")]
    public class SplitDatum : FundamentalDatum
    {
        public SplitDatum(DateTime asOfDate)
        {
            AsOfDate = asOfDate;
        }

        [IgnoreDataMember]
        public override double Value { get => Split; set => Split = value; }

        [DataMember]
        public double Split { get; set; } = 1;
    }
}
