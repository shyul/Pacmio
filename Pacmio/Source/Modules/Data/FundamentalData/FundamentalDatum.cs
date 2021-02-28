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


    //         public double PE => EPS != 0 ? Close_Price / EPS : double.NaN;

    /*
        [Serializable, DataContract]
        [KnownType(typeof(ARPDatum))]
        [KnownType(typeof(SplitDatum))]
        [KnownType(typeof(DividendDatum))]
        [KnownType(typeof(EPSDatum))]
        [KnownType(typeof(TotalRevenueDatum))]
        public class FundamentalDatum : IEquatable<FundamentalDatum>
        {
            protected FundamentalDatum(DateTime asOfDate, double close)
            {
                Close_Price = close;
                AsOfDate = asOfDate;
            }

            [DataMember]
            public DataSourceType DataSource { get; set; } = DataSourceType.Invalid;

            [DataMember]
            public DateTime AsOfDate { get; private set; }

            [IgnoreDataMember]
            public virtual double Value { get; }

            [DataMember]
            public double Close_Price { get; set; }

            [IgnoreDataMember]
            public (Type, DateTime) Key => (GetType(), AsOfDate);

            #region Equality

            public override int GetHashCode() => Key.GetHashCode();

            public bool Equals(FundamentalDatum other) => GetType() == other.GetType() && AsOfDate == other.AsOfDate;

            public override bool Equals(object other) => other is FundamentalDatum && Equals(other);

            public static bool operator ==(FundamentalDatum left, FundamentalDatum right) => left.Equals(right);
            public static bool operator !=(FundamentalDatum left, FundamentalDatum right) => !left.Equals(right);

            #endregion Equality
        }*/
}
