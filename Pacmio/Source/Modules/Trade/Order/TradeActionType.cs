/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Runtime.Serialization;
using Xu;
using Pacmio.IB;

namespace Pacmio
{
    [Serializable, DataContract]
    public enum TradeActionType : int
    {
        [EnumMember]
        None = 0,

        // Hold Liquidation

        [EnumMember]
        LongHold = 1,

        [EnumMember]
        ShortHold = 2,

        // Add Liquidation

        [EnumMember]
        Long = 10,

        [EnumMember]
        Short = 11,


        // Remove Liquidation

        [EnumMember]
        Sell = 20,

        [EnumMember]
        Cover = 21,
    }
}
