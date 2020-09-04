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
    public class AlertList : Filter
    {
        private StreamingAlerts StreamingAlerts { get; set; }

        public override void Start()
        {
            if (!IsActive && Client.Connected)
            {
                Stop();

                IsActive = true;
                string configStr = ConfigString;
                Console.WriteLine(Name + " | " + configStr);

                StreamingAlerts = Client.Connection.StreamingAlertsManager.GetAlerts(configStr);
                StreamingAlerts.StreamingAlertsData += new StreamingAlertsData(_streamingAlerts_StreamingAlertsData);
                StreamingAlerts.StreamingAlertsConfig += new StreamingAlertsConfig(_streamingAlerts_StreamingAlertsConfig);
                StreamingAlerts.Start();
            }
        }

        public override void Stop()
        {
            if (m_IsActive)
            {
                if (StreamingAlerts is StreamingAlerts)
                    StreamingAlerts.Stop();

                IsActive = false;
            }
        }

        private int alertCount;
        private int messageCount;

        void _streamingAlerts_StreamingAlertsConfig(StreamingAlerts source)
        {

            if (source != StreamingAlerts)
                Console.WriteLine("Ignoring StreamingAlertsConfig.\r\n");
            else
                Console.WriteLine("Name = " + StreamingAlerts.WindowName + ", ShortForm = " + StreamingAlerts.Config + "\r\n");

        }

        void _streamingAlerts_StreamingAlertsData(List<RowData> data, StreamingAlerts source)
        {

            if (source != StreamingAlerts)
                Console.WriteLine("Ignoring StreamingAlertsData.\r\n");
            else
            {
                alertCount += data.Count;
                Console.WriteLine("Alert Count:  " + alertCount);
                messageCount++;
                Console.WriteLine("Message Count:  " + messageCount);
                Console.WriteLine("Received " + data.Count + " alerts.\r\n");

                foreach (RowData alert in data)
                {
                    Console.WriteLine(alert.ToString());
                }
            }

        }


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
