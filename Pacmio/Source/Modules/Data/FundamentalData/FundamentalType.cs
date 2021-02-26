/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Runtime.Serialization;

namespace Pacmio
{
    /// <summary>
    /// Bar data from different source
    /// Prioritized by number value.
    /// </summary>
    [Serializable, DataContract]
    public enum FundamentalType : int
    {
        [EnumMember]
        Split = 10,

        [EnumMember]
        Dividend = 20,

        [EnumMember]
        EPS = 30,

        [EnumMember]
        Revenue = 40,


    }
}
