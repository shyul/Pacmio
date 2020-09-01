/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// 1. Pre-Market Gapper (10%, total pre-market volume, social media sentiment)
/// 2. Halted and Resumed
/// 3. Low Share out-standing / Small-cap / and high *** volume spike ***
/// 4. Reversal??
/// 5. Social Mention
/// 
/// ***************************************************************************

using Pacmio.IB;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Pacmio
{
    public class ScannerConfig : IEquatable<ScannerConfig>
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

        public bool Equals(ScannerConfig other) => other is ScannerConfig sc &&
            (Type, Location, StockTypeFilter, Code, FilterOptions) == (sc.Type, sc.Location, sc.StockTypeFilter, sc.Code, sc.FilterOptions);

        // https://stackoverflow.com/questions/4219261/overriding-operator-how-to-compare-to-null
        public override bool Equals(object other) => other is ScannerConfig sc && Equals(sc);

        public static bool operator ==(ScannerConfig s1, ScannerConfig s2) => s1.Equals(s2);
        public static bool operator !=(ScannerConfig s1, ScannerConfig s2) => !s1.Equals(s2);

        public override int GetHashCode() =>
                            Type.GetHashCode() ^
                            Location.GetHashCode() ^
                            StockTypeFilter.GetHashCode() ^
                            Code.GetHashCode() ^
                            FilterOptions.GetHashCode();
    }
}
