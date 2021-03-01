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
    public enum DataSourceType : int
    {
        [EnumMember]
        Manual = int.MinValue, // Highest Priority

        [EnumMember]
        Consolidated = Manual + 5, // High Priority

        [EnumMember]
        Quandl = Manual + 10,

        [EnumMember] // data source with both unadj and adj 
        FullData = Manual + 20,

        [EnumMember]
        Norgate = Manual + 25,

        [EnumMember]
        IB = Manual + 100,

        [EnumMember]
        Yahoo = Manual + 1000,

        [EnumMember]
        Google = Manual + 1010,

        [EnumMember]
        Realtime = -100,

        [EnumMember]
        Tick = -10,

        [EnumMember]
        Unknown = -1,

        [EnumMember]
        Invalid = 0,
    }
}
