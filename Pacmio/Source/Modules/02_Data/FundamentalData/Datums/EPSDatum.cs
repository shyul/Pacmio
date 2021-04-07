/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Runtime.Serialization;

namespace Pacmio
{
    [Serializable, DataContract(Name = "EPS")]
    public class EPSDatum : RevenueDatum
    {
        public EPSDatum(DateTime asOfDate) : base(asOfDate) { }

        [IgnoreDataMember]
        public double PE_Ratio => Value != 0 ? Close_Price / Value : double.NaN;

        [IgnoreDataMember]
        public override string Comment => double.IsNaN(PE_Ratio) ? string.Empty : "PE Ratio = " + PE_Ratio.ToString("0.##");
    }
}
