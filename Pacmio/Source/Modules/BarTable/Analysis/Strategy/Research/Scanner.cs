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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.UI.WebControls;
using Xu;

namespace Pacmio
{
    public abstract class Scanner : IEquatable<Scanner>
    {


        public abstract void Start();

        public abstract void Stop();

        public virtual bool IsSnapshot { get; set; } = false;

        public virtual string Name { get; set; }

        public virtual int NumberOfRows { get; set; } = 100;

        public virtual double PriceMin { get; set; }

        public virtual double PriceMax { get; set; }

        public virtual double VolumeMin { get; set; }

        public virtual double VolumeMax { get; set; }

        public virtual double MarketCapMin { get; set; }

        public virtual double MarketCapMax { get; set; }

        public virtual double GapPercentMin { get; set; }

        public virtual double GapPercentMax { get; set; }

        public bool GetConfigBool(string key, string expectedValue) => ConfigList.ContainsKey(key) && ConfigList[key] == expectedValue;

        public void SetConfig(string key, bool isSet, string value)
        {
            if (isSet)
                ConfigList[key] = value;
            else if (ConfigList.ContainsKey(key))
                ConfigList.Remove(key);
        }

        public double GetConfigDouble(string key) => ConfigList.ContainsKey(key) ? ConfigList[key].ToDouble() : double.NaN;

        public void SetConfig(string key, double value) 
        {
            if (double.IsNaN(value) && ConfigList.ContainsKey(key))
                ConfigList.Remove(key);
            else
                ConfigList[key] = value.ToString("0.#######");
        }

        public int GetConfigInt(string key) => ConfigList.ContainsKey(key) ? ConfigList[key].ToInt32() : 0;

        public void SetConfig(string key, int value) => ConfigList[key] = value.ToString();

        public string GetConfigString(string key) => ConfigList.ContainsKey(key) ? ConfigList[key] : string.Empty;

        public void SetConfig(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(value) && ConfigList.ContainsKey(key))
                ConfigList.Remove(key);
            else
                ConfigList[key] = value;
        }

        public Dictionary<string, string> ConfigList { get; } = new Dictionary<string, string>();

        public virtual string ConfigString => string.Join(";", ConfigList.OrderBy(n => n.Key).Select(n => n.Key + "=" + n.Value).ToArray());


        #region Output

        protected HashSet<string> UnknownSymbols { get; } = new HashSet<string>();

        public virtual List<Contract> List { get; } = new List<Contract>();

        #endregion Output


        #region Equality

        public bool Equals(Scanner other) => other is Scanner sc && GetType() == other.GetType() && ConfigString == sc.ConfigString;

        public override bool Equals(object other) => other is Scanner sc && Equals(sc);

        public static bool operator ==(Scanner s1, Scanner s2) => s1.Equals(s2);
        public static bool operator !=(Scanner s1, Scanner s2) => !s1.Equals(s2);

        public override int GetHashCode() => ConfigString.GetHashCode() ^ GetType().GetHashCode();

        public override string ToString() => Name + ": " + ConfigString;

        #endregion Equality
    }


    public class ScannerConfigOld : IEquatable<ScannerConfigOld>
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

        //public string FilterOptions { get; set; } = string.Empty; // (23)"marketCapAbove1e6=10000;marketCapBelow1e6=100000;stkTypes=inc:CORP;"
                                                                  // (23)"priceAbove=5;avgVolumeAbove=500000;marketCapAbove1e6=1000;"
                                                                  // stkTypes: All, inc:CORP, inc:ADR, inc:ETF, inc:ETN, inc:REIT, inc:CEF, inc:ETMF (Exchange Traded Managed Fund)
                                                                  // exc:CORP, exc:ADR, exc:ETF, exc:ETN, exc:REIT, exc:CEF

        public Dictionary<string, string> ConfigList { get; } = new Dictionary<string, string>();

        public string FilterOptions => string.Join(";", ConfigList.OrderBy(n => n.Key).Select(n => n.Key + "=" + n.Value).ToArray());

        public bool Equals(ScannerConfigOld other) => other is ScannerConfigOld sc &&
            (Type, Location, StockTypeFilter, Code, FilterOptions) == (sc.Type, sc.Location, sc.StockTypeFilter, sc.Code, sc.FilterOptions);

        // https://stackoverflow.com/questions/4219261/overriding-operator-how-to-compare-to-null
        public override bool Equals(object other) => other is ScannerConfigOld sc && Equals(sc);

        public static bool operator ==(ScannerConfigOld s1, ScannerConfigOld s2) => s1.Equals(s2);
        public static bool operator !=(ScannerConfigOld s1, ScannerConfigOld s2) => !s1.Equals(s2);

        public override int GetHashCode() =>
                            Type.GetHashCode() ^
                            Location.GetHashCode() ^
                            StockTypeFilter.GetHashCode() ^
                            Code.GetHashCode() ^
                            FilterOptions.GetHashCode();

        public static ScannerConfigOld GapUp => new ScannerConfigOld() 
        {
        
        
        };
    }
}
