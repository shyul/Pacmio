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
    [Serializable, DataContract(Name = "SharesOut")]
    public class SharesOutDatum : FundamentalDatum
    {
        public SharesOutDatum(DateTime asOfDate) => AsOfDate = asOfDate;

        [IgnoreDataMember]
        public override double Value { get => Shares; set => Shares = value; }

        [DataMember]
        public double Shares { get; set; } = 1;
    }
}
