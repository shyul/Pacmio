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
    [Serializable, DataContract(Name = "SharesFloat")]
    public class SharesFloatDatum : FundamentalDatum
    {
        public SharesFloatDatum(DateTime asOfDate) => AsOfDate = asOfDate;

        [IgnoreDataMember]
        public override double Value { get => Float; set => Float = value; }

        [DataMember]
        public double Float { get; set; } = 1;
    }
}
