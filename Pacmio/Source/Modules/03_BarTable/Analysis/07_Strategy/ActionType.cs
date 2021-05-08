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
    public enum ActionType : int
    {
        [EnumMember]
        None = 0,

        // Add Liquidation, Enter

        [EnumMember]
        Long = 3,

        [EnumMember]
        Short = -3,

        // Hold Liquidation

        [EnumMember]
        LongHold = 1,

        [EnumMember]
        ShortHold = -1,

        // Remove Liquidation, Exit

        [EnumMember]
        Sell = -2,

        [EnumMember]
        Cover = 2,
    }

    public enum ActionDirection : int 
    {
        None = 0,

        Add = 1,

        Remove = -1
    }
}
