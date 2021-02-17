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
    /// The security's status
    /// </summary>
    [Serializable, DataContract]
    public enum ContractStatus : int
    {
        [EnumMember, Description("Error")]
        Error = -2000,

        [EnumMember, Description("Test")]
        Test = -1000,

        [EnumMember, Description("Delist")]
        Delist = -100,

        [EnumMember, Description("Phase out")]
        Incomplete = -50,

        [EnumMember, Description("Unknown")]
        Unknown = 0,

        [EnumMember, Description("Alive")]
        Alive = 10
    }
}
