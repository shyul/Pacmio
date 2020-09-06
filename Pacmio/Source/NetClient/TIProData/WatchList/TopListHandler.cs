﻿/// ***************************************************************************
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
using System.Threading;
using Xu;
using TradeIdeas.TIProData;
using TradeIdeas.TIProData.Configuration;

namespace Pacmio.TIProData
{
    public class TopListHandler : TopListConfig, IWatchList
    {
        public TopListHandler(string name, int numberOfRows = 100)
        {
            Name = name;
            NumberOfRows = numberOfRows;
            ConfigList["form"] = "1";
            ConfigList["omh"] = "1";
            ConfigList["col_ver"] = "1";

            ShowColumns.AddRange(new string[] { "D_Symbol", "Price", "TV", "Float", "SFloat", "STP", "STH", "EarningD", "D_Name" });
        }

        private TopList TopList { get; set; }

        public override void Start() => Start(false, false, DateTime.Now);

        public void Start(bool isSnapshot, bool isHistorical, DateTime historicalTime)
        {
            if (Client.Connected)
            {
                Stop();

                SetConfig("hist", isHistorical, "1");
                IsSnapshot = isSnapshot || isHistorical;

                if (isHistorical)
                {
                    long epoch = historicalTime.ToEpoch().ToInt64();
                    ConfigList["exact_time"] = epoch.ToString();
                }
                else
                {
                    if (ConfigList.ContainsKey("exact_time"))
                        ConfigList.Remove("exact_time");
                }

                IsActive = true;
                string configStr = ConfigString;

                var toplist = Client.Connection.TopListManager.GetTopList(configStr);
                TopList = toplist is TopList tl ? tl : null;
                TopList.TopListStatus += new TopListStatus(TopListStatus_Handler);
                TopList.TopListData += new TopListData(TopListData_Handler);
                TopList.Start();

                Console.WriteLine("#### Start TopList: " + Name + " | " + configStr);
            }
            else
            {
                Console.WriteLine("#### No TI connection, unable to start TopList: " + Name);
            }
        }

        public override void Stop()
        {
            if (m_IsActive)
            {
                Console.WriteLine("#### Stop TopList: " + Name);

                if (TopList is TopList)
                    TopList.Stop();

                IsActive = false;
            }
        }

        public ICollection<Contract> Snapshot()
        {
            Start(true, false, DateTime.Now);

            int timeout = 200;
            while (IsActive)
            {
                Thread.Sleep(10);
                timeout--;

                if (timeout < 0)
                {
                    return null;
                }
            }

            lock (List)
            {
                return List.ToArray();
            }
        }

        public ICollection<Contract> Snapshot(DateTime historicalTime)
        {
            Start(false, true, historicalTime);

            int timeout = 200;
            while (IsActive)
            {
                Thread.Sleep(10);
                timeout--;

                if (timeout < 0)
                {
                    return null;
                }
            }

            lock (List)
            {
                return List.ToArray();
            }
        }

        public string SortColumn { get => GetConfigString("sort"); set => SetConfig("sort", value); }

        void TopListStatus_Handler(TopList sender)
        {
            if (sender == TopList)
            {
                Console.WriteLine("WindowName: " + sender.TopListInfo.WindowName);
                ConfigColumns(sender.TopListInfo.Columns);
            }
        }

        public ICollection<Contract> List { get; private set; } = new List<Contract>();

        private void TopListData_Handler(List<RowData> rows, DateTime? start, DateTime? end, TopList sender)
        {
            LastRefreshTime = DateTime.Now;
            if (IsSnapshot) Stop();

            lock (List)
            {
                List = PrintAllRows(rows);
            }
        }
    }
}
