/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;

namespace Pacmio
{
    public enum EntryType : int
    {
        None = 0,

        Buy = 1,

        BuyLimit = 2,

        BuyStop = 3,

        Sell = -1,

        SellStop = -2,

        SellLimit = -3,
    }
}
