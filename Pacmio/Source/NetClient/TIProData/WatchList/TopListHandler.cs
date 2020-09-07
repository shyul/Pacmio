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
using System.Threading;
using System.Threading.Tasks;
using Xu;
using TradeIdeas.TIProData;
using TradeIdeas.TIProData.Configuration;

namespace Pacmio.TIProData
{
    public class TopListHandler : TopListConfig, IWatchList
    {
        public TopListHandler(string name)
        {
            Name = name;
            ConfigList["form"] = "1";
            ConfigList["omh"] = "1";
            ConfigList["col_ver"] = "1";

            ShowColumns.AddRange(new string[] { "D_Symbol", "Price", "TV", "Float", "SFloat", "STP", "STH", "EarningD", "D_Name" });
        }

        private TopList StreamingTopList { get; set; }

        public override void Start() => Start(false, false, DateTime.Now);

        public void Start(bool isSnapshot, bool isHistorical, DateTime historicalTime)
        {
            if (Client.Connected)
            {
                Stop();

                MessageCount = 0;

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

                StreamingTopList = Client.Connection.TopListManager.GetTopList(configStr);
                StreamingTopList.TopListStatus += new TopListStatus(TopListStatus_Handler);
                StreamingTopList.TopListData += new TopListData(TopListData_Handler);
                StreamingTopList.Start();

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

                if (StreamingTopList is TopList)
                    StreamingTopList.Stop();

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
            if (sender == StreamingTopList)
            {
                Console.WriteLine("TopList Handler Name: " + sender.TopListInfo.WindowName + ", Config = " + sender.TopListInfo.Config);
                ConfigColumns(sender.TopListInfo.Columns);
            }
        }

        public ICollection<Contract> List { get; private set; } = new List<Contract>();

        private void TopListData_Handler(List<RowData> rows, DateTime? start, DateTime? end, TopList sender)
        {
            MessageCount++;
            if (sender == StreamingTopList && LastRefreshTime < DateTime.Now)
            {
                LastRefreshTime = DateTime.Now;
                if (rows.Count > 0)
                {
                    Console.WriteLine("\n\n######## TI TopList " + rows.Count + " Result Received for [ " + Name + " ] | MessageCount = " + MessageCount + " | " + LastRefreshTime + "\n\n");
                    Task.Run(() =>
                    {
                        lock (List)
                        {
                            List = GetContractList(rows);
                        }
                        if (IsSnapshot) Stop();
                        Console.WriteLine("\n\n######## TI TopList Result End.\n\n");
                    });
                }
            }
            else
            {
                Console.WriteLine("######## Ignoring TI TopList Result!!\n\n");
            }

        }
    }
}
