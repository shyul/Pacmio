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
    [Serializable, DataContract(Name = "ARPDatum")]
    [KnownType(typeof(DividendDatum))]
    [KnownType(typeof(EPSDatum))]
    [KnownType(typeof(TotalRevenueDatum))]
    public abstract class ARPDatum : FundamentalDatum
    {
        public ARPDatum(DateTime asOfDate, double close) : base(asOfDate, close) { }

        /// <summary>
        /// Audited
        /// </summary>
        [DataMember]
        public double Value_A { get; set; } = double.NaN;

        /// <summary>
        /// Restated
        /// </summary>
        [DataMember]
        public double Value_R { get; set; } = double.NaN;

        /// <summary>
        /// Preliminary
        /// </summary>
        [DataMember]
        public double Value_P { get; set; } = double.NaN;

        [IgnoreDataMember]
        public override double Value
        {
            get
            {
                if (!double.IsNaN(Value_A)) return Value_A;
                else if (!double.IsNaN(Value_R)) return Value_R;
                else return Value_P;
            }
        }
    }
}
