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
    public abstract class SignalFilter : Filter
    {
        public bool Halt { get => GetConfigBool("Sh_HALT", "on"); set { SetConfig("Sh_HALT", value, "on"); } }

        public int NewHigh
        {
            get => GetConfigQ("NHP");
            set => SetConfigQ("NHP", value);
        }

        public int NewLow
        {
            get => GetConfigQ("NLP");
            set => SetConfigQ("NLP", value);
        }

        protected int GetConfigQ(string key)
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
    }
}
