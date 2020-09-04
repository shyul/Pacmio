/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Trade-Ideas API
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xu;
using TradeIdeas.TIProData;
using TradeIdeas.TIProData.Configuration;

namespace Pacmio.TIProData
{
    public abstract class Filter : Scanner
    {
        public override bool IsActive { get => m_IsActive && Client.Connected; set => m_IsActive = value; }
        protected bool m_IsActive = false;

        public override string Name { get => GetConfigString("WN"); set => SetConfig("WN", value); }

        public override int NumberOfRows { get => GetConfigInt("count"); set => SetConfig("count", value); }

        public override bool IsSnapshot { get => m_IsSnapshot || IsHistory; set => m_IsSnapshot = value; }

        private bool m_IsSnapshot = false;

        public bool IsHistory { get => GetConfigBool("hist", "1"); set { SetConfig("hist", value, "1"); } }

        public DateTime HistoricalTime
        {
            get => m_HistoricalTime;

            set
            {
                DateTime time = value;

                if ((DateTime.Now - time).TotalDays > 90)
                    time = DateTime.Now.AddDays(-90);

                m_HistoricalTime = time;
            }
        }

        private DateTime m_HistoricalTime;

        public override (double Min, double Max) Price
        {
            get => (GetConfigDouble("MinPrice"), GetConfigDouble("MaxPrice"));

            set
            {
                SetConfig("MinPrice", value.Min);
                SetConfig("MaxPrice", value.Max);
            }
        }

        public (double Min, double Max) Volume
        {
            get => (GetConfigDouble("MinTV"), GetConfigDouble("MaxTV"));

            set
            {
                SetConfig("MinTV", value.Min);
                SetConfig("MaxTV", value.Max);
            }
        }

        public override (double Min, double Max) MarketCap
        {
            get => (GetConfigDouble("MinMCap"), GetConfigDouble("MaxMCap"));

            set
            {
                SetConfig("MinMCap", value.Min);
                SetConfig("MaxMCap", value.Max);
            }
        }

        public override (double Min, double Max) GainPercent
        {
            get => (GetConfigDouble("MinFCP"), GetConfigDouble("MaxFCP"));

            set
            {
                SetConfig("MinFCP", value.Min);
                SetConfig("MaxFCP", value.Max);
            }
        }



        public (double Min, double Max) Gap
        {
            get => (GetConfigDouble("MinGUD"), GetConfigDouble("MaxGUD"));

            set
            {
                SetConfig("MinGUD", value.Min);
                SetConfig("MaxGUD", value.Max);
            }
        }

        public override (double Min, double Max) GapPercent
        {
            get => (GetConfigDouble("MinGUP"), GetConfigDouble("MaxGUP"));

            set
            {
                SetConfig("MinGUP", value.Min);
                SetConfig("MaxGUP", value.Max);
            }
        }

        public (double Min, double Max) GapBars
        {
            get => (GetConfigDouble("MinGUR"), GetConfigDouble("MaxGUR"));

            set
            {
                SetConfig("MinGUR", value.Min);
                SetConfig("MaxGUR", value.Max);
            }
        }




        public (double Min, double Max) AverageTrueRange
        {
            get => (GetConfigDouble("MinATR"), GetConfigDouble("MaxATR"));

            set
            {
                SetConfig("MinATR", value.Min);
                SetConfig("MaxATR", value.Max);
            }
        }

        public (double Min, double Max) Volatility
        {
            get => (GetConfigDouble("MinVWV"), GetConfigDouble("MaxVWV"));

            set
            {
                SetConfig("MinVWV", value.Min);
                SetConfig("MaxVWV", value.Max);
            }
        }

        public (double Min, double Max) VolatilityPercent
        {
            get => (GetConfigDouble("MinVWVP"), GetConfigDouble("MaxVWVP"));

            set
            {
                SetConfig("MinVWVP", value.Min);
                SetConfig("MaxVWVP", value.Max);
            }
        }

        public (double Min, double Max) Wiggle
        {
            get => (GetConfigDouble("MinWiggle"), GetConfigDouble("MaxWiggle"));

            set
            {
                SetConfig("MinWiggle", value.Min);
                SetConfig("MaxWiggle", value.Max);
            }
        }

        public List<Exchange> Exchanges { get; } = new List<Exchange>() { Exchange.NYSE, Exchange.NASDAQ, Exchange.ARCA, Exchange.AMEX, Exchange.BATS };

        public override string ConfigString
        {
            get
            {
                if (Name.Length > 1) ConfigList["WN"] = Name;

                SetConfig("X_NYSE", Exchanges.Contains(Exchange.NYSE), "on");
                SetConfig("XN", Exchanges.Contains(Exchange.NASDAQ), "on");
                SetConfig("X_ARCA", Exchanges.Contains(Exchange.ARCA), "on");
                SetConfig("X_AMEX", Exchanges.Contains(Exchange.AMEX), "on");
                SetConfig("X_BATS", Exchanges.Contains(Exchange.BATS), "on");
                SetConfig("X_PINK", Exchanges.Contains(Exchange.OTCMKT), "on");

                if (IsHistory)
                {
                    long epoch = HistoricalTime.ToEpoch().ToInt64();
                    ConfigList["exact_time"] = epoch.ToString();
                }
                else if (ConfigList.ContainsKey("exact_time"))
                    ConfigList.Remove("exact_time");

                string extraConfig = ExtraConfig;
                if (!extraConfig.EndsWith("&")) extraConfig += "&";
                return extraConfig + string.Join("&", ConfigList.OrderBy(n => n.Key).Select(n => n.Key + "=" + n.Value).ToArray());
            }
        }
    }
}
