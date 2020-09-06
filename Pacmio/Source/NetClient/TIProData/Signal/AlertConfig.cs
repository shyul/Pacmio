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
    public abstract class AlertConfig : TopListConfig
    {
        #region Price Actions

        public int NewHigh { get => GetConfigIntQ("NHP"); set => SetConfigQ("NHP", value); }
        public int NewLow { get => GetConfigIntQ("NLP"); set => SetConfigQ("NLP", value); }

        public int PreMarketHigh { get => GetConfigIntQ("HPRE"); set => SetConfigQ("HPRE", value); }
        public int PreMarketLow { get => GetConfigIntQ("LPRE"); set => SetConfigQ("LPRE", value); }

        public double RunUp { get => GetConfigDoubleQ("RUN"); set => SetConfigQ("RUN", value); }
        public double RunDown { get => GetConfigDoubleQ("RDN"); set => SetConfigQ("RDN", value); }

        #endregion Price Actions



        public double StockTwitsSpikeRatio { get => GetConfigDoubleQ("STAS"); set => SetConfigQ("STAS", value); }

        public bool Halt { get => GetConfigBool("Sh_HALT"); set { SetConfig("Sh_HALT", value); } }
        public bool Resume { get => GetConfigBool("Sh_RESUME"); set { SetConfig("Sh_RESUME", value); } }
        public bool MarketLocked { get => GetConfigBool("Sh_ML"); set { SetConfig("Sh_ML", value); } }




        #region Volume

        public int BlockTrade { get => GetConfigIntQ("BP"); set => SetConfigQ("BP", value); }
        public double StrongVolumeRatio { get => GetConfigDoubleQ("SV"); set => SetConfigQ("SV", value); }

        #endregion Volume

        #region Hourly Time Frame



        #endregion Hourly Time Frame

        #region Daily Time Frame

        public bool CrossDailySMA200Above { get => GetConfigBool("Sh_CA200"); set { SetConfig("Sh_CA200", value); } }
        public bool CrossDailySMA200Below { get => GetConfigBool("Sh_CB200"); set { SetConfig("Sh_CB200", value); } }

        public bool CrossDailySMA50Above { get => GetConfigBool("Sh_CA50"); set { SetConfig("Sh_CA50", value); } }
        public bool CrossDailySMA50Below { get => GetConfigBool("Sh_CB50"); set { SetConfig("Sh_CB50", value); } }

        public bool CrossDailySMA20Above { get => GetConfigBool("Sh_CA20"); set { SetConfig("Sh_CA20", value); } }
        public bool CrossDailySMA20Below { get => GetConfigBool("Sh_CB20"); set { SetConfig("Sh_CB20", value); } }

        #endregion Daily Time Frame

        #region Intraday Time Frame

        public bool CrossAboveVWAP { get => GetConfigBool("Sh_CAVC"); set { SetConfig("Sh_CAVC", value); } }
        public bool CrossBelowVWAP { get => GetConfigBool("Sh_CBVC"); set { SetConfig("Sh_CBVC", value); } }

        public bool High5Min { get => GetConfigBool("Sh_IDH5"); set { SetConfig("Sh_IDH5", value); } }
        public bool Low5Min { get => GetConfigBool("Sh_IDL5"); set { SetConfig("Sh_IDL5", value); } }

        public bool High10Min { get => GetConfigBool("Sh_IDH10"); set { SetConfig("Sh_IDH10", value); } }
        public bool Low10Min { get => GetConfigBool("Sh_IDL10"); set { SetConfig("Sh_IDL10", value); } }

        public bool High15Min { get => GetConfigBool("Sh_IDH15"); set { SetConfig("Sh_IDH15", value); } }
        public bool Low15Min { get => GetConfigBool("Sh_IDL15"); set { SetConfig("Sh_IDL15", value); } }

        public bool High30Min { get => GetConfigBool("Sh_IDH30"); set { SetConfig("Sh_IDH30", value); } }
        public bool Low30Min { get => GetConfigBool("Sh_IDL30"); set { SetConfig("Sh_IDL30", value); } }

        public bool High60Min { get => GetConfigBool("Sh_IDH60"); set { SetConfig("Sh_IDH60", value); } }
        public bool Low60Min { get => GetConfigBool("Sh_IDL60"); set { SetConfig("Sh_IDL60", value); } }

        public bool Breakout1MinOpenRange { get => GetConfigBool("Sh_ORU1"); set { SetConfig("Sh_ORU1", value); } }
        public bool Breakdown1MinOpenRange { get => GetConfigBool("Sh_ORD1"); set { SetConfig("Sh_ORD1", value); } }

        public bool Breakout2MinOpenRange { get => GetConfigBool("Sh_ORU2"); set { SetConfig("Sh_ORU2", value); } }
        public bool Breakdown2MinOpenRange { get => GetConfigBool("Sh_ORD2"); set { SetConfig("Sh_ORD2", value); } }

        public bool Breakout5MinOpenRange { get => GetConfigBool("Sh_ORU5"); set { SetConfig("Sh_ORU5", value); } }
        public bool Breakdown5MinOpenRange { get => GetConfigBool("Sh_ORD5"); set { SetConfig("Sh_ORD5", value); } }

        public bool Breakout10MinOpenRange { get => GetConfigBool("Sh_ORU10"); set { SetConfig("Sh_ORU10", value); } }
        public bool Breakdown10MinOpenRange { get => GetConfigBool("Sh_ORD10"); set { SetConfig("Sh_ORD10", value); } }

        public bool Breakout15MinOpenRange { get => GetConfigBool("Sh_ORU15"); set { SetConfig("Sh_ORU15", value); } }
        public bool Breakdown15MinOpenRange { get => GetConfigBool("Sh_ORD15"); set { SetConfig("Sh_ORD15", value); } }

        public bool Breakout30MinOpenRange { get => GetConfigBool("Sh_ORU30"); set { SetConfig("Sh_ORU30", value); } }
        public bool Breakdown30MinOpenRange { get => GetConfigBool("Sh_ORD30"); set { SetConfig("Sh_ORD30", value); } }

        public bool Breakout60MinOpenRange { get => GetConfigBool("Sh_ORU60"); set { SetConfig("Sh_ORU60", value); } }
        public bool Breakdown60MinOpenRange { get => GetConfigBool("Sh_ORD60"); set { SetConfig("Sh_ORD60", value); } }

        #endregion Intraday Time Frame

        #region Utilities

        protected int GetConfigIntQ(string key)
        {
            string keyOn = "Sh_" + key;
            string keyQ = "Q" + key;

            if (ConfigList.ContainsKey(keyOn))
            {
                return (ConfigList.ContainsKey(keyQ)) ? GetConfigInt(keyQ) : 0;
            }

            return -1;
        }

        protected void SetConfigQ(string key, int value)
        {
            string keyOn = "Sh_" + key;
            string keyQ = "Q" + key;

            if (value < 0) // Remove  
            {
                if (ConfigList.ContainsKey(keyOn))
                    ConfigList.Remove(keyOn);
                if (ConfigList.ContainsKey(keyQ))
                    ConfigList.Remove(keyQ);
            }
            else
            {
                ConfigList[keyOn] = "on";
                if (value > 0) ConfigList[keyQ] = value.ToString();
            }
        }

        protected double GetConfigDoubleQ(string key)
        {
            string keyOn = "Sh_" + key;
            string keyQ = "Q" + key;

            if (ConfigList.ContainsKey(keyOn))
            {
                return (ConfigList.ContainsKey(keyQ)) ? GetConfigDouble(keyQ) : 0;
            }

            return double.NaN;
        }

        protected void SetConfigQ(string key, double value)
        {
            string keyOn = "Sh_" + key;
            string keyQ = "Q" + key;

            if (double.IsNaN(value)) // Remove  
            {
                if (ConfigList.ContainsKey(keyOn))
                    ConfigList.Remove(keyOn);
                if (ConfigList.ContainsKey(keyQ))
                    ConfigList.Remove(keyQ);
            }
            else
            {
                ConfigList[keyOn] = "on";
                if (value > 0) ConfigList[keyQ] = value.ToString("0.#######");
            }
        }

        #endregion Utilities
    }
}
