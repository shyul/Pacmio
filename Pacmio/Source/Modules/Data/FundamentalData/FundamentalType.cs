/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Pacmio
{
    /// <summary>
    /// 
    /// </summary>
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
    }
}
