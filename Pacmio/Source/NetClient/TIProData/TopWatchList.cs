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
                TopList.Stop();
                IsActive = false;
            }
        }

        public string SortColumn { get => GetConfigString("sort"); set => SetConfig("sort", value); }

        public Dictionary<string, ColumnInfo> Columns { get; } = new Dictionary<string, ColumnInfo>();

        public HashSet<Stock> UnknownStock { get; } = new HashSet<Stock>();

        void TopListStatus_Handler(TopList sender)
        {
            if (sender == TopList)
            {
                Console.WriteLine("WindowName: " + sender.TopListInfo.WindowName);
                lock (Columns)
                {
                    Columns.Clear();
                    foreach (ColumnInfo c in sender.TopListInfo.Columns)
                    {
                        Columns[c.WireName] = c;
                        Console.WriteLine(c.WireName + " | " + c.Description + " | " + c.Format + " | " + c.Units + " | " + c.InternalCode + " | " + c.Graphics + " | " + c.TextHeader + " | " + c.PreferredWidth);
                    }
                }
                //Console.WriteLine(sender.TopListInfo.ToString());
            }
        }

        public ICollection<Contract> List { get; } = new List<Contract>();

        private void TopListData_Handler(List<RowData> rows, DateTime? start, DateTime? end, TopList sender)
        {
            LastRefreshTime = DateTime.Now;
            if (IsSnapshot) Stop();

            lock (Columns)
            {
                lock (List)
                {
                    List.Clear();

                    foreach (RowData row in rows)
                    {
                        if (row.GetAsString("symbol") is string symbol && symbol.Length > 0 && Regex.IsMatch(symbol, @"^[a-zA-Z]+$"))
                        {
                            var list = ContractList.GetList(symbol, Exchanges);

                            if (list.Where(n => n is Stock).Count() > 0)
                            {
                                Stock stk = list.First() as Stock;

                                List.Add(stk);

                                if (list.Count() == 1 && row.GetAsString("c_D_Name") is string fullname && fullname.Length > 0)
                                    stk.FullName = fullname;

                                if (stk.ISIN.Length < 8 && !UnknownStock.Contains(stk))
                                {
                                    UnknownStock.Add(stk);
                                    ContractList.Fetch(stk);
                                }

                                string rowString = stk.ToString() + " >> " + stk.ISIN + " >> " + stk.FullName + " >> ";
                                foreach (string key in Columns.Keys)
                                {
                                    ColumnInfo ci = Columns[key];

                                    string dataString = row.GetAsString(key);
                                    string format = ci.Format.Trim();

                                    if (format == string.Empty)
                                    {
                                        rowString += ci.Description + "=" + dataString + "; ";
                                    }
                                    else if (format == "p")
                                    {
                                        if (row.GetAsString("four_digits") == "1")
                                        {
                                            rowString += ci.Description + "=" + dataString.ToDouble().ToString("N4") + "; ";
                                        }
                                        else
                                        {
                                            rowString += ci.Description + "=" + dataString.ToDouble().ToString("N2") + "; ";
                                        }
                                    }
                                    else
                                    {
                                        int digits = ci.Format.ToInt32();

                                        if (digits > 7)
                                            digits = 7;
                                        else if (digits < 0)
                                            digits = 0;

                                        rowString += ci.Description + "=" + dataString.ToDouble().ToString("N" + digits) + "; ";
                                    }
                                }
                                Console.WriteLine(rowString);

                                // See if the stk has live data subscription !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! before override the Last and volume

                                //Console.WriteLine(row.ToString());
                            }
                            else if (IB.Client.Connected && !UnknownSymbols.Contains(symbol))
                            {
                                UnknownSymbols.Add(symbol);
                                ContractList.Fetch(symbol);
                            }
                        }
                    }
                }

                // Trigger the list is updated event!!
            }
        }
    }

    /*
    public static partial class Client
    {
        public static WatchList tls { get; private set; }

        public static void TestTopList()
        {
            DateTime time = new DateTime(2020, 09, 03, 06, 30, 00);

            Console.WriteLine("TotalDays = " + (DateTime.Now - time).TotalDays);

            long epoch = time.ToEpoch().ToInt64();

            Console.WriteLine("Epoch = " + epoch);

            tls = new WatchList("Low Price Gappers")
            {
                Price = (1.5, 25),
                Volume = (50e3, double.NaN),
                GapPercent = (5, -5),
                AverageTrueRange = (0.25, double.NaN),
                ExtraConfig = "form=1&sort=MaxGUP&omh=1&col_ver=1&show0=D_Symbol&show1=Price&show2=Float&show3=SFloat&show4=GUP&show5=TV&show6=EarningD&show7=Vol5&show8=STP&show9=RV&show10=D_Name&show11=RD&show12=FCP&show13=D_Sector&show14=",
            };

            tls.IsSnapshot = true;
            //tls.IsHistory = true;
            //tls.HistoricalTime = new DateTime(2020, 06, 10, 06, 30, 00);

            tls.Start();





        }
    }
    */
}
