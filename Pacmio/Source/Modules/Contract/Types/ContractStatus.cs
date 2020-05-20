/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
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
        [EnumMember, Description("Test")]
        Test = -1000,

        [EnumMember, Description("Delist")]
        Delist = -100,

        [EnumMember, Description("Phase out")]
        Value = -50,

        [EnumMember, Description("Unknown")]
        Unknown = 0,

        [EnumMember, Description("Alive")]
        Alive = 10
    }
}
