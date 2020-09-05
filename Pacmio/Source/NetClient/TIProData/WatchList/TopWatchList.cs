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
    public class TopWatchList : Filter, IWatchList
    {
        public TopWatchList(string name, int numberOfRows = 100)
        {
            Name = name;
            NumberOfRows = numberOfRows;
        }

        private TopList TopList { get; set; }

        public override void Start()
        {
            if (!IsActive && Client.Connected)
            {
                Stop();

                IsActive = true;
                string configStr = ConfigString;
                Console.WriteLine(Name + " | " + configStr);

                var toplist = Client.Connection.TopListManager.GetTopList(configStr);
                TopList = toplist is TopList tl ? tl : null;
                TopList.TopListStatus += new TopListStatus(TopListStatus_Handler);
                TopList.TopListData += new TopListData(TopListData_Handler);
                TopList.Start();
            }
        }

        public override void Stop()
        {
            if (m_IsActive)
            {
                if (TopList is TopList)
                    TopList.Stop();
                IsActive = false;
            }
        }

        public string SortColumn { get => GetConfigString("sort"); set => SetConfig("sort", value); }





        void TopListStatus_Handler(TopList sender)
        {
            if (sender == TopList)
            {
                Console.WriteLine("WindowName: " + sender.TopListInfo.WindowName);
                ConfigColumns(sender.TopListInfo.Columns);
                /*
                lock (Columns)
                {
                    Columns.Clear();
                    foreach (ColumnInfo c in sender.TopListInfo.Columns)
                    {
                        Columns[c.WireName] = c;
                        Console.WriteLine(c.WireName + " | " + c.Description + " | " + c.Format + " | " + c.Units + " | " + c.InternalCode + " | " + c.Graphics + " | " + c.TextHeader + " | " + c.PreferredWidth);
                    }
                }*/
                //Console.WriteLine(sender.TopListInfo.ToString());
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
