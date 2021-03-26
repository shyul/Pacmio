/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// BarTable Data Types
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Pacmio
{
    [Serializable, DataContract]
    public enum TradeExecutionType : int
    {
        [EnumMember]
        None = 0,

        // Add Liquidation

        [EnumMember]
        Long = 3,

        [EnumMember]
        Short = -3,

        // Hold Liquidation

        [EnumMember]
        LongHold = 1,

        [EnumMember]
        ShortHold = -1,

        // Remove Liquidation

        [EnumMember]
        Sell = -2,

        [EnumMember]
        Cover = 2,
    }



}
