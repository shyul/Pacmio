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

    [Serializable, DataContract]
    [KnownType(typeof(SplitDatum))]
    [KnownType(typeof(DividendDatum))]
    public abstract class FundamentalDatum
    {
        protected FundamentalDatum(int conId, DateTime time)
        {
            ConId = conId;
            DateTime = time;
        }

        [DataMember]
        public int ConId { get; }

        [DataMember]
        public DateTime DateTime { get; }
    }

    [Serializable, DataContract(Name = "Split")]
    public class SplitDatum : FundamentalDatum
    {
        public SplitDatum(int conId, DateTime time, double split) : base(conId, time)
        {
            Split = split;
        }

        [DataMember]
        public double Split { get; }

        /*
        [DataMember]
        public double Close { get; }

        [IgnoreDataMember]
        public double DividentPercent { get; set; }
        */
    }

    [Serializable, DataContract(Name = "Dividend")]
    public class DividendDatum : FundamentalDatum
    {
        public DividendDatum(int conId, DateTime time, double dividend, double close) : base(conId, time)
        {
            Divident = dividend;
            Close = close;
        }

        [DataMember]
        public double Divident { get; }

        [DataMember]
        public double Close { get; }

        [IgnoreDataMember]
        public double DividentPercent => Close > 0 ? Divident / Close : 0;
    }

    [Serializable, DataContract(Name = "EPS")]
    public class EPSDatum : FundamentalDatum
    {
        public EPSDatum(int conId, DateTime time, double eps, double close) : base(conId, time)
        {
            EPS = eps;
            Close = close;
        }

        [DataMember]
        public double EPS { get; }

        [DataMember]
        public double Close { get; }

        /*
        [IgnoreDataMember]
        public double DividentPercent { get; set; }
        */
    }


}
