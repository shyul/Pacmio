/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Xu;
using Pacmio.IB;

namespace Pacmio
{
    public class ScannerInfo : IEquatable<ScannerInfo>
    {
        public int RequestId { get; set; } = 0;

        public int NumberOfRows { get; set; } = 100;

        public string Type { get; set; } = "STK";

        public string Location { get; set; } = "STK.US";

        public string StockTypeFilter { get; set; } = "ALL";

        public string Code { get; set; } = string.Empty; // "TOP_PERC_GAIN";
                                                         // "MOST_ACTIVE"
                                                         // "TOP_OPEN_PERC_GAIN"
                                                         // "HALTED"

        public string FilterOptions { get; set; } = string.Empty; // (23)"marketCapAbove1e6=10000;marketCapBelow1e6=100000;stkTypes=inc:CORP;"
                                                                  // (23)"priceAbove=5;avgVolumeAbove=500000;marketCapAbove1e6=1000;"
                                                                  // stkTypes: All, inc:CORP, inc:ADR, inc:ETF, inc:ETN, inc:REIT, inc:CEF, inc:ETMF (Exchange Traded Managed Fund)
                                                                  // exc:CORP, exc:ADR, exc:ETF, exc:ETN, exc:REIT, exc:CEF

        public bool Equals(ScannerInfo other)
        {
            if (this is null || other is null)
                return false;
            else return (Type, Location, StockTypeFilter, Code, FilterOptions) ==
                    (other.Type, other.Location, other.StockTypeFilter, other.Code, other.FilterOptions);
        }

        public override bool Equals(object other)
        {
            if (this is null || other is null) // https://stackoverflow.com/questions/4219261/overriding-operator-how-to-compare-to-null
                return false;
            else if (other is ScannerInfo sc)
                return Equals(sc);
            else
                return false;
        }

        public static bool operator ==(ScannerInfo s1, ScannerInfo s2) => s1.Equals(s2);
        public static bool operator !=(ScannerInfo s1, ScannerInfo s2) => !s1.Equals(s2);

        public override int GetHashCode() =>
                            Type.GetHashCode() ^
                            Location.GetHashCode() ^
                            StockTypeFilter.GetHashCode() ^
                            Code.GetHashCode() ^
                            FilterOptions.GetHashCode();
    }

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
