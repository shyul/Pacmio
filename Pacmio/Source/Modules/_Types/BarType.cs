/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using Pacmio.IB;
using System;
using System.Runtime.Serialization;

namespace Pacmio
{
    /// <summary>
    /// Bar data types
    /// </summary>
    [Serializable, DataContract]
    public enum BarType : int
    {
        [EnumMember, ApiCode("TRADES")]
        Trades = 0,

        [EnumMember, ApiCode("MIDPOINT")]
        MiddlePoint = 1,

        [EnumMember, ApiCode("BID")]
        Bid = 2,

        [EnumMember, ApiCode("ASK")]
        Ask = 3,

        [EnumMember, ApiCode("BID_ASK")]
        BidAsk = 4,

        [EnumMember, ApiCode("HISTORICAL_VOLATILITY")]
        HistoricalVolatility = 5,

        [EnumMember, ApiCode("OPTION_IMPLIED_VOLATILITY")]
        OptionImpliedVolatility = 6,

        [EnumMember, ApiCode("YIELD_BID")]
        YieldBid = 7,

        [EnumMember, ApiCode("YIELD_ASK")]
        YieldASk = 8,

        [EnumMember, ApiCode("YIELD_BID_ASK")]
        YieldBidAsk = 9,

        [EnumMember, ApiCode("YIELD_LAST")]
        YieldLast = 10,

        [EnumMember, ApiCode("ADJUSTED_LAST")]
        AdjustedLast = 11
    }
}
