/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Runtime.Serialization;

namespace Pacmio
{
    [Serializable, DataContract(Name = "Revenue")]
    public class RevenueDatum : FundamentalDatum
    {
        public RevenueDatum(DateTime asOfDate) => AsOfDate = asOfDate;

        /// <summary>
        /// Audited
        /// </summary>
        [DataMember]
        public double Audited { get; set; } = double.NaN;

        /// <summary>
        /// Restated
        /// </summary>
        [DataMember]
        public double Restated { get; set; } = double.NaN;

        /// <summary>
        /// Preliminary
        /// </summary>
        [DataMember]
        public double Preliminary { get; set; } = double.NaN;

        [IgnoreDataMember]
        public override double Value
        {
            get
            {
                if (!double.IsNaN(Audited)) return Audited;
                else if (!double.IsNaN(Restated)) return Restated;
                else return Preliminary;
            }
            set
            {
                Preliminary = value;
                if (double.IsNaN(Audited)) Audited = value;
                if (double.IsNaN(Restated)) Restated = value;
            }
        }
    }
}
