﻿/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Trade-Ideas API: 
/// 1. https://www.trade-ideas.com/Help.html
/// 2. https://pro.trade-ideas.com/professional/tidocumentation/
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xu;
using TradeIdeas.TIProData;
using TradeIdeas.TIProData.Configuration;

namespace Pacmio.TIProData
{
    public class AlertHandler : AlertConfig, IAlert
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
            if (Client.Connected)
            {
                Stop();

                MessageCount = 0;
                AlertCount = 0;

                IsRunning = true;
                string configStr = ConfigurationString;

                StreamingAlerts = Client.Connection.StreamingAlertsManager.GetAlerts(configStr);
                StreamingAlerts.StreamingAlertsData += new StreamingAlertsData(AlertsData_Handler);
                StreamingAlerts.StreamingAlertsConfig += new StreamingAlertsConfig(AlertsConfig_Handler);
                StreamingAlerts.Start();

                Console.WriteLine("#### Start Alert: " + Name + " | " + configStr);
            }
            else
            {
                Console.WriteLine("#### No TI connection, unable to start Alert: " + Name);
            }
        }

        public override void Stop()
        {
            if (m_IsRunning)
            {
                if (StreamingAlerts is StreamingAlerts)
                    StreamingAlerts.Stop();

                IsRunning = false;
            }
        }

        void AlertsConfig_Handler(StreamingAlerts sender)
        {
            if (sender == StreamingAlerts)
            {
                Console.WriteLine("Alert Handler Name = " + StreamingAlerts.WindowName + ", Config = " + StreamingAlerts.Config + "\n\n");
                ConfigColumns(sender.Columns);
            }
        }

        public ConcurrentQueue<Contract> Queue { get; } = new ConcurrentQueue<Contract>();

        void AlertsData_Handler(List<RowData> rows, StreamingAlerts sender)
        {
            MessageCount++;
            if (sender == StreamingAlerts && UpdateTime < DateTime.Now)
            {
                UpdateTime = DateTime.Now;
                if (rows.Count > 0)
                {
                    AlertCount += rows.Count;
                    Console.WriteLine("\n\n######## TI Alert " + rows.Count + " Result Received for [ " + Name + " ] | MessageCount = " + MessageCount + " | AlertCount = " + AlertCount + " | " + UpdateTime + "\n\n");
                    Task.Run(() => {
                        GetContractList(rows, "SYMBOL");
                        if (IsSnapshot) Stop();
                        Console.WriteLine("\n\n######## TI Alert Result End.\n\n");
                    });
                }
            }
            else
            {
                Console.WriteLine("######## Ignoring TI StreamingAlertsData.\n\n");
            }

        }

        public override IEnumerable<Contract> SingleSnapshot()
        {
            throw new NotImplementedException();
        }
    }
}
