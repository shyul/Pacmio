/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Xu;
using Pacmio.IB;

namespace Pacmio
{
    public enum ExchangeWorkHours
    {
        Full,

        NorthAmerica,
        NorthAmericaExtended,
        NorthAmericaExtended2,
        Amex,
        ArcaEdge,
        Cboe,

        CanadianVenture,

        LSE,
        CentralEurope,
        FWB,
        SWB,
        EBS,
        SFB,
        BM,

        China,
        HongKong,
        Singapore,
        India,

        ASX,
    }
}
