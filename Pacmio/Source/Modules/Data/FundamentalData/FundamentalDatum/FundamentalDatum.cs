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
    [Serializable, DataContract(Name = "FundamentalDatum")]
    [KnownType(typeof(ARPDatum))]
    [KnownType(typeof(SplitDatum))]
    [KnownType(typeof(DividendDatum))]
    [KnownType(typeof(EPSDatum))]
    [KnownType(typeof(TotalRevenueDatum))]
    public abstract class FundamentalDatum : IEquatable<FundamentalDatum>
    {
        protected FundamentalDatum(DateTime asOfDate, double close)
        {
            Close_Price = close;
            AsOfDate = asOfDate;
        }

        [DataMember]
        public DataSourceType DataSource { get; set; } = DataSourceType.Invalid;

        [DataMember]
        public DateTime AsOfDate { get; }

        [DataMember]
        public virtual double Value { get; }

        [DataMember]
        public double Close_Price { get; }

        [IgnoreDataMember]
        public (Type, DateTime) Key => (GetType(), AsOfDate);

        #region Equality

        public override int GetHashCode() => Key.GetHashCode();

        public bool Equals(FundamentalDatum other) => GetType() == other.GetType() && AsOfDate == other.AsOfDate;

        public override bool Equals(object other) => other is FundamentalDatum && Equals(other);

        public static bool operator ==(FundamentalDatum left, FundamentalDatum right) => left.Equals(right);
        public static bool operator !=(FundamentalDatum left, FundamentalDatum right) => !left.Equals(right);

        #endregion Equality
    }
}
