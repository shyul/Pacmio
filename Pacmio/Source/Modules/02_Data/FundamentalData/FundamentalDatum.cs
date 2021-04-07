/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using Xu;
using Xu.GridView;

namespace Pacmio
{
    [Serializable, DataContract]
    [KnownType(typeof(SplitDatum))]
    [KnownType(typeof(DividendDatum))]
    [KnownType(typeof(RevenueDatum))]
    [KnownType(typeof(EPSDatum))]
    public abstract class FundamentalDatum
    {
        [IgnoreDataMember]
        public virtual string TypeName => GetType().GetAttribute((DataContractAttribute d) => d.Name);

        [DataMember]
        public virtual DateTime AsOfDate { get; protected set; }

        [IgnoreDataMember]
        public (string, DateTime) Key => (TypeName, AsOfDate);

        [DataMember]
        public DataSourceType DataSource { get; set; } = DataSourceType.Invalid;

        [DataMember]
        public double Close_Price { get; set; } = double.NaN;

        [IgnoreDataMember]
        public virtual double Value { get; set; } = double.NaN;

        [IgnoreDataMember]
        public virtual string Comment { get; } = string.Empty;
    }

    /*
    [Serializable, DataContract]
    public enum FundamentalType : int
    {
        [EnumMember, Description("Unknown")]
        Unknown = 0,

        [EnumMember, Description("Split")]
        Split = 10,

        [EnumMember, Description("Dividend")]
        Dividend = 20,

        [EnumMember, Description("EPS")]
        EPS = 30,

        [EnumMember, Description("Total Revenue")]
        Revenue = 40,

        [EnumMember, Description("Outstanding Shares")]
        ShareOut = 1000,

        [EnumMember, Description("Floating Shares")]
        ShareFloat = 1100,

        [EnumMember, Description("Number Employees")]
        NumberEmployees = 10000,

        [EnumMember, Description("Number Shareholders")]
        NumberShareholders = 20000,
    }*/
}
