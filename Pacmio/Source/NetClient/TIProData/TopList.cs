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
using System.Threading.Tasks;
using Xu;
using TradeIdeas.TIProData;
using TradeIdeas.TIProData.Configuration;
using TradeIdeas.ServerConnection;

namespace Pacmio.TIProData
{
    public static partial class Client
    {
        public static TopListGroup tlg { get; private set; }

        public static void TestTopList()
        {
            tlg = new TopListGroup("form=1&sort=MaxGUP&count=100&MinGUP=5&MinPrice=1.5&MinTV=50000&MaxGUP=-5&MaxPrice=25&X_NYSE=on&XN=on&WN=Small+Gappers&show0=D_Symbol&show1=Price&show2=Float&show3=SFloat&show4=GUP&show5=TV&show6=EarningD&show7=Vol5&show8=STP&show9=RV&show10=D_Name&show11=RD&show12=FCP&show13=D_Sector&show14=&col_ver=1&omh=1");


            if (Connected)
            {


            }
        }
    }

    public class TopListGroup
    {

        public TopListGroup(string config)
        {
            Configuration = config;
            TopList = Client.Connection.TopListManager.GetTopList(Configuration);
            TopList.TopListStatus += new TopListStatus(TopListStatus_Handler);
            TopList.TopListData += new TopListData(TopListData_Handler);
            TopList.Start();
        }



        public string Configuration { get; private set; }

        private TopList TopList { get; set; }

        public Dictionary<string, ColumnInfo> Columns { get; } = new Dictionary<string, ColumnInfo>();


        public static DateTime LastUpdateTime { get; private set; } = DateTime.MinValue;

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

        List<Exchange> US_Exchanges { get; } = new List<Exchange>() { Exchange.NYSE, Exchange.NASDAQ, Exchange.ARCA, Exchange.AMEX, Exchange.BATS };

        HashSet<string> UnknownSymbols { get; } = new HashSet<string>();

        void TopListData_Handler(List<RowData> rows, DateTime? start, DateTime? end, TopList sender)
        {
            LastUpdateTime = DateTime.Now;

            lock (Columns)
            {
                foreach (RowData row in rows)
                {
                    string symbol = row.GetAsString("symbol");

                    if (Regex.IsMatch(symbol, @"^[a-zA-Z]+$"))
                    {
                        var list = ContractList.GetList(symbol, US_Exchanges);

                        if (list.Count() > 0)
                        {
                            string rowString = list.First().ToString() + " >> ";
                            foreach (string key in Columns.Keys)
                            {
                                ColumnInfo c = Columns[key];

                                string dataString = row.GetAsString(key);
                                string format = c.Format.Trim();

                                if (format == string.Empty)
                                {
                                    rowString += c.Description + "=" + dataString + "; ";
                                }
                                else if (format == "p")
                                {
                                    if (row.GetAsString("four_digits") == "1")
                                    {
                                        rowString += c.Description + "=" + dataString.ToDouble().ToString("N4") + "; ";
                                    }
                                    else
                                    {
                                        rowString += c.Description + "=" + dataString.ToDouble().ToString("N2") + "; ";
                                    }
                                }
                                else
                                {
                                    int digits = c.Format.ToInt32();

                                    if (digits > 7)
                                        digits = 7;
                                    else if (digits < 0)
                                        digits = 0;

                                    rowString += c.Description + "=" + dataString.ToDouble().ToString("N" + digits) + "; ";
                                }
                            }
                            Console.WriteLine(rowString);
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
        }
    }
}
