/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Xu;
using Xu.GridView;

namespace Pacmio
{
    [Serializable, DataContract]
    public class FundamentalData
    {
        [DataMember]
        public (string name, Exchange exchange, string typeName) ContractInfo { get; set; }

        //[IgnoreDataMember]
        //public Contract Contract => ContractManager

        [DataMember]
        private List<FundamentalDatum> DatumList { get; } = new List<FundamentalDatum>();


    }

    [Serializable, DataContract(Name = "FundamentalDatum")]
    [KnownType(typeof(ARPDatum))]
    [KnownType(typeof(SplitDatum))]
    [KnownType(typeof(DividendDatum))]
    [KnownType(typeof(EPSDatum))]
    [KnownType(typeof(TotalRevenueDatum))]
    public abstract class FundamentalDatum
    {
        protected FundamentalDatum(DateTime asOfDate, double close)
        {
            Close = close;
            AsOfDate = asOfDate;
        }

        [DataMember]
        public double Close { get; }

        [DataMember]
        public DateTime AsOfDate { get; }
    }

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
        public double Value
        {
            get
            {
                if (!double.IsNaN(Value_A)) return Value_A;
                else if (!double.IsNaN(Value_R)) return Value_R;
                else return Value_P;
            }
        }
    }

    [Serializable, DataContract(Name = "SplitDatum")]
    public class SplitDatum : FundamentalDatum
    {
        public SplitDatum(DateTime asOfDate, double close, double split) : base(asOfDate, close)
        {
            Split = split;
        }

        [DataMember]
        public double Split { get; }
    }

    [Serializable, DataContract(Name = "Dividend")]
    public class DividendDatum : ARPDatum
    {
        public DividendDatum(DateTime asOfDate, double close) : base(asOfDate, close) { }

        [IgnoreDataMember]
        public double Divident { get => Value; set => Value_P = value; }

        [IgnoreDataMember]
        public double Percent => Close > 0 ? Divident / Close : 0;
    }

    [Serializable, DataContract(Name = "EPSDatum")]
    public class EPSDatum : ARPDatum
    {
        public EPSDatum(DateTime asOfDate, double close) : base(asOfDate, close) { }

        [IgnoreDataMember]
        public double EPS { get => Value; set => Value_P = value; }

        [IgnoreDataMember]
        public double PE => EPS != 0 ? Close / EPS : double.NaN;
    }

    [Serializable, DataContract(Name = "TotalRevenueDatum")]
    public class TotalRevenueDatum : ARPDatum
    {
        public TotalRevenueDatum(DateTime asOfDate, double close) : base(asOfDate, close) { }

        [IgnoreDataMember]
        public double Revenue { get => Value; set => Value_P = value; }
    }
}
