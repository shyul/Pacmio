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
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.UI.WebControls;
using Xu;

namespace Pacmio
{
    public abstract class Scanner : IEquatable<Scanner>, IDisposable
    {
        public abstract void Start();

        public abstract void Stop();



        public virtual bool IsActive { get; set; } = false;

        public virtual bool IsSnapshot { get; set; } = false;

        public virtual DateTime LastRefreshTime { get; protected set; } = DateTime.MinValue;

        public virtual string Name { get; set; }

        public virtual (double Min, double Max) Price { get; set; }

        public virtual (double Min, double Max) MarketCap { get; set; }

        public virtual (double Min, double Max) GainPercent { get; set; }

        public virtual (double Min, double Max) GapPercent { get; set; }

        protected bool GetConfigBool(string key, string expectedValue = "on") => ConfigList.ContainsKey(key) && ConfigList[key] == expectedValue;

        protected void SetConfig(string key, bool isSet, string value = "on")
        {
            if (isSet)
                ConfigList[key] = value;
            else if (ConfigList.ContainsKey(key))
                ConfigList.Remove(key);
        }

        protected double GetConfigDouble(string key) => ConfigList.ContainsKey(key) ? ConfigList[key].ToDouble() : double.NaN;

        protected void SetConfig(string key, double value, double min = double.MinValue, double max = double.MaxValue)
        {
            if (double.IsNaN(value))
            {
                if (ConfigList.ContainsKey(key))
                    ConfigList.Remove(key);
            }
            else
            {
                if (value < min) value = min;
                else if (value > max) value = max;

                ConfigList[key] = value.ToString("0.#######");
            }
        }

        protected void SetConfigPercent(string key, double value) => SetConfig(key, value, 0D, 100D);

        protected int GetConfigInt(string key) => ConfigList.ContainsKey(key) ? ConfigList[key].ToInt32() : 0;

        protected void SetConfig(string key, int value) => ConfigList[key] = value.ToString();

        protected string GetConfigString(string key) => ConfigList.ContainsKey(key) ? ConfigList[key] : string.Empty;

        protected void SetConfig(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (ConfigList.ContainsKey(key))
                    ConfigList.Remove(key);
            }
            else
                ConfigList[key] = value;
        }

        protected Dictionary<string, string> ConfigList { get; } = new Dictionary<string, string>();

        public void ImportConfig(Scanner s)
        {
            foreach (var item in s.ConfigList)
            {
                ConfigList[item.Key] = item.Value;
            }
        }

        public string ExtraConfig { get; set; } = string.Empty;

        public virtual string ConfigString
        {
            get
            {
                string extraConfig = ExtraConfig;
                if (extraConfig.Length > 0 && !extraConfig.EndsWith(";")) extraConfig += ";";
                return extraConfig + string.Join(";", ConfigList.OrderBy(n => n.Key).Select(n => n.Key + "=" + n.Value).ToArray());
            }
        }

        #region Equality 
        
        // https://stackoverflow.com/questions/4219261/overriding-operator-how-to-compare-to-null
        public bool Equals(Scanner other) => other is Scanner sc && GetType() == other.GetType() && ConfigString == sc.ConfigString;

        public override bool Equals(object other) => other is Scanner sc && Equals(sc);

        public static bool operator ==(Scanner s1, Scanner s2) => s1.Equals(s2);
        public static bool operator !=(Scanner s1, Scanner s2) => !s1.Equals(s2);

        public override int GetHashCode() => ConfigString.GetHashCode() ^ GetType().GetHashCode();

        public override string ToString() => Name + ": " + ConfigString;

        #endregion Equality

        public void Dispose() => Stop();
    }



}
