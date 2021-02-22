/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Xu;

namespace Pacmio
{
    public abstract class DynamicWatchList : WatchList
    {
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

        public void ImportConfig(DynamicWatchList s)
        {
            foreach (var item in s.ConfigList)
            {
                ConfigList[item.Key] = item.Value;
            }
        }

        public string ExtraConfig { get; set; } = string.Empty;

        public override string ConfigurationString
        {
            get
            {
                string extraConfig = ExtraConfig;
                if (extraConfig.Length > 0 && !extraConfig.EndsWith(";")) extraConfig += ";";
                return extraConfig + string.Join(";", ConfigList.OrderBy(n => n.Key).Select(n => n.Key + "=" + n.Value).ToArray());
            }
        }

        public abstract void Start();

        public abstract void Stop();

        public abstract IEnumerable<Contract> SingleSnapshot();

        public virtual bool IsRunning { get; protected set; } = false;

        public virtual bool IsSnapshot { get; protected set; } = false;

        public override int NumberOfRows
        {
            get
            {
                if (m_Contracts is null) m_Contracts = new List<Contract>();
                lock (m_Contracts)
                {
                    return m_Contracts.Count();
                }
            }
        }

        public override IEnumerable<Contract> Contracts
        {
            get
            {
                if (m_Contracts is null) m_Contracts = new List<Contract>();
                lock (m_Contracts)
                {
                    return m_Contracts.ToArray();
                }
            }
        }

        protected IEnumerable<Contract> m_Contracts = null;

        public void Update(IEnumerable<Contract> list)
        {
            if (m_Contracts is null) m_Contracts = new List<Contract>();
            lock (m_Contracts)
            {
                m_Contracts = list;
                UpdateTime = DateTime.Now;
                OnUpdateHandler?.Invoke(0, UpdateTime, "");
            }
        }

        public DateTime UpdateTime { get; protected set; }

        public event StatusEventHandler OnUpdateHandler;
    }
}
