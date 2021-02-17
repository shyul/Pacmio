/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// 1. Pre-Market Gapper (10%, total pre-market volume, social media sentiment)
/// 2. Halted and Resumed
/// 3. Low Share out-standing / Small-cap / and high volume spike
/// 4. Reversal??
/// 
/// ***************************************************************************

using Pacmio.IB;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Pacmio
{
    /// <summary>
    /// Scan Types
    /// </summary>
    [Serializable, DataContract]
    public enum ScannerType : int
    {
        [DataMember, ApiCode("MOST_ACTIVE"), Description("Most Active")]
        MOST_ACTIVE = 10000,

        [DataMember, ApiCode("TOP_PERC_GAIN"), Description("Top % Gainers")]
        TOP_PERC_GAIN = 5000,

        [DataMember, ApiCode("TOP_PERC_LOSE"), Description("Top % Losers")]
        TOP_PERC_LOSE = 5010,

        [DataMember, ApiCode("TOP_OPEN_PERC_GAIN"), Description("Top % Gainers at Open")]
        TOP_OPEN_PERC_GAIN = 1260,

        [DataMember, ApiCode("TOP_OPEN_PERC_LOSE"), Description("Top % Losers at Open")]
        TOP_OPEN_PERC_LOSE = 1270,

        [DataMember, ApiCode("TOP_TRADE_COUNT"), Description("Top Trade Count")]
        TOP_TRADE_COUNT = 1170,

        [DataMember, ApiCode("TOP_TRADE_RATE"), Description("Top Trade Rate")]
        TOP_TRADE_RATE = 1180,

        [DataMember, ApiCode("TOP_PRICE_RANGE"), Description("Top Price Range")]
        TOP_PRICE_RANGE = 1190,

        [DataMember, ApiCode("HOT_BY_PRICE_RANGE"), Description("Hot by Price Range")]
        HOT_BY_PRICE_RANGE = 1200,

        [DataMember, ApiCode("HIGH_OPEN_GAP"), Description("High Open Gap")]
        HIGH_OPEN_GAP = 1280,

        [DataMember, ApiCode("LOW_OPEN_GAP"), Description("Low Open Gap")]
        LOW_OPEN_GAP = 1290,

        [DataMember, ApiCode("HIGH_OPT_IMP_VOLAT_OVER_HIST"), Description("High Option Implied Vol Over Historical")]
        HIGH_OPT_IMP_VOLAT_OVER_HIST = 1010,

        [DataMember, ApiCode("LOW_OPT_IMP_VOLAT_OVER_HIST"), Description("Low Option Implied Vol Over Historical")]
        LOW_OPT_IMP_VOLAT_OVER_HIST = 1020,

        [DataMember, ApiCode("HIGH_OPT_IMP_VOLAT"), Description("High Option Implied Volatilities")]
        HIGH_OPT_IMP_VOLAT = 1030,

        [DataMember, ApiCode("LOW_OPT_IMP_VOLAT"), Description("Low Option Implied Volatilities")]
        LOW_OPT_IMP_VOLAT = 1220,

        [DataMember, ApiCode("TOP_OPT_IMP_VOLAT_GAIN"), Description("Top Option Implied Volatilities Gain")]
        TOP_OPT_IMP_VOLAT_GAIN = 1040,

        [DataMember, ApiCode("TOP_OPT_IMP_VOLAT_LOSE"), Description("Top Option Implied Volatilities Lose")]
        TOP_OPT_IMP_VOLAT_LOSE = 1050,

        [DataMember, ApiCode("HIGH_OPT_VOLUME_PUT_CALL_RATIO"), Description("High Option Volume P/C Ratio")]
        HIGH_OPT_VOLUME_PUT_CALL_RATIO = 1060,

        [DataMember, ApiCode("LOW_OPT_VOLUME_PUT_CALL_RATIO"), Description("Low Option Volume P/C Ratio")]
        LOW_OPT_VOLUME_PUT_CALL_RATIO = 1070,

        [DataMember, ApiCode("LOW_OPT_VOL_PUT_CALL_RATIO"), Description("Low Option Volume P/C Ratio")]
        LOW_OPT_VOL_PUT_CALL_RATIO = 1000,

        [DataMember, ApiCode("OPT_VOLUME_MOST_ACTIVE"), Description("Most Active Option Volume")]
        OPT_VOLUME_MOST_ACTIVE = 1080,

        [DataMember, ApiCode("HOT_BY_OPT_VOLUME"), Description("Hot by Option Volume")]
        HOT_BY_OPT_VOLUME = 1090,

        [DataMember, ApiCode("HIGH_OPT_OPEN_INTEREST_PUT_CALL_RATIO"), Description("High Option Open Interest P/C Ratio")]
        HIGH_OPT_OPEN_INTEREST_PUT_CALL_RATIO = 1100,

        [DataMember, ApiCode("LOW_OPT_OPEN_INTEREST_PUT_CALL_RATIO"), Description("Low Option Open Interest P/C Ratio")]
        LOW_OPT_OPEN_INTEREST_PUT_CALL_RATIO = 1110,

        [DataMember, ApiCode("HOT_BY_VOLUME"), Description("Hot Contracts by Volume")]
        HOT_BY_VOLUME = 1150,

        [DataMember, ApiCode("HOT_BY_PRICE"), Description("Hot Contracts by Price")]
        HOT_BY_PRICE = 1160,

        [DataMember, ApiCode("TOP_VOLUME_RATE"), Description("Top Volume Rate")]
        TOP_VOLUME_RATE = 1210,

        [DataMember, ApiCode("OPT_OPEN_INTEREST_MOST_ACTIVE"), Description("Most Active Option Open Interest")]
        OPT_OPEN_INTEREST_MOST_ACTIVE = 1230,

        [DataMember, ApiCode("HIGH_VS_13W_HL"), Description("13-Week High")]
        HIGH_VS_13W_HL = 1300,

        [DataMember, ApiCode("LOW_VS_13W_HL"), Description("13-Week Low")]
        LOW_VS_13W_HL = 1310,

        [DataMember, ApiCode("HIGH_VS_26W_HL"), Description("26-Week High")]
        HIGH_VS_26W_HL = 1320,

        [DataMember, ApiCode("LOW_VS_26W_HL"), Description("26-Week Low")]
        LOW_VS_26W_HL = 1330,

        [DataMember, ApiCode("HIGH_VS_52W_HL"), Description("52-Week High")]
        HIGH_VS_52W_HL = 1340,

        [DataMember, ApiCode("LOW_VS_52W_HL"), Description("52-Week Low")]
        LOW_VS_52W_HL = 1350,

        [DataMember, ApiCode("HIGH_SYNTH_BID_REV_NAT_YIELD"), Description("HIGH_SYNTH_BID_REV_NAT_YIELD")]
        HIGH_SYNTH_BID_REV_NAT_YIELD = 1360,

        [DataMember, ApiCode("LOW_SYNTH_BID_REV_NAT_YIELD"), Description("LOW_SYNTH_BID_REV_NAT_YIELD")]
        LOW_SYNTH_BID_REV_NAT_YIELD = 1370,

        [DataMember, ApiCode("NOT_OPEN"), Description("Not Open")]
        NOT_OPEN = -11000,

        [DataMember, ApiCode("HALTED"), Description("Halted")]
        HALTED = -20000,
    }
}
