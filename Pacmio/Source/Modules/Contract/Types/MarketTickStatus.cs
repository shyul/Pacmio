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
    public enum MarketTickStatus : int
    {
        [EnumMember]
        Unknown = 0,

        [EnumMember]
        RealTime = 1,

        [EnumMember]
        Delayed = 3,

        [EnumMember]
        Frozen = 2,

        [EnumMember]
        DelayedFrozen = 4,
    }
}
