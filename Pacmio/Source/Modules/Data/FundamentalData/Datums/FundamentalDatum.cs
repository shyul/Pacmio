/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Runtime.Serialization;

namespace Pacmio
{
    [Serializable, DataContract]
    public class FundamentalDatum
    {
        public FundamentalDatum((FundamentalType, DateTime) key)
        {
            Type = key.Item1;
            AsOfDate = key.Item2;
        }

        public FundamentalDatum(FundamentalType type, DateTime asOfDate)
        {
            Type = type;
            AsOfDate = asOfDate;
        }

        [IgnoreDataMember]
        public (FundamentalType, DateTime) Key => (Type, AsOfDate);

        [DataMember]
        public DataSourceType DataSource { get; set; } = DataSourceType.Invalid;

        [DataMember]
        public DateTime AsOfDate { get; private set; }

        [DataMember]
        public FundamentalType Type { get; private set; }

        [DataMember]
        public double Close_Price { get; set; } = double.NaN;

        /// <summary>
        /// Audited
        /// </summary>
        [DataMember]
        public double Value_Audited { get; set; } = double.NaN;

        /// <summary>
        /// Restated
        /// </summary>
        [DataMember]
        public double Value_Restated { get; set; } = double.NaN;

        /// <summary>
        /// Preliminary
        /// </summary>
        [DataMember]
        public double Value_Preliminary { get; set; } = double.NaN;

        [IgnoreDataMember]
        public double Value
        {
            get
            {
                if (!double.IsNaN(Value_Audited)) return Value_Audited;
                else if (!double.IsNaN(Value_Restated)) return Value_Restated;
                else return Value_Preliminary;
            }
            set
            {
                Value_Preliminary = value;
                if (double.IsNaN(Value_Audited)) Value_Audited = value;
                if (double.IsNaN(Value_Restated)) Value_Restated = value;
            }
        }
    }
}
