/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Trade-Ideas API: 
/// 1. https://www.trade-ideas.com/Help.html
/// 2. https://pro.trade-ideas.com/professional/tidocumentation/
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
using System.Collections.Concurrent;

namespace Pacmio.TIProData
{
    public class AlertHandler : AlertConfig, ISignalSource
    {
        public AlertHandler(string name)
        {
            Name = name;

            ConfigList["O"] = "2000000000000000000000000000001_1D_0";
            ConfigList["SL"] = "X1o5";
            ConfigList["col_ver"] = "1";

            ShowColumns.AddRange(new string[] { "D_Symbol", "D_Type", "D_Time", "D_Desc", "Count", "Price", "TV", "Float", "SFloat", "STP", "STH", "EarningD", "D_Name" });
        }

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
                StreamingAlerts.StreamingAlertsData += new StreamingAlertsData(AlertsData_Handler);
                StreamingAlerts.StreamingAlertsConfig += new StreamingAlertsConfig(AlertsConfig_Handler);
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

        void AlertsConfig_Handler(StreamingAlerts sender)
        {
            if (sender == StreamingAlerts)
            {
                Console.WriteLine("Name = " + StreamingAlerts.WindowName + ", ShortForm = " + StreamingAlerts.Config + "\r\n");
                ConfigColumns(sender.Columns);
            }
        }

        ConcurrentQueue<Contract> ISignalSource.Queue { get; } = new ConcurrentQueue<Contract>();

        void AlertsData_Handler(List<RowData> data, StreamingAlerts source)
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

                PrintAllRows(data, "SYMBOL");
                /*
                foreach (RowData alert in data)
                {
                    Console.WriteLine(alert.ToString());
                }*/
            }

        }



    }
}
