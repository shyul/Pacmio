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
            DateTime time = new DateTime(2020, 06, 10, 06, 30, 00);
            long epoch = time.ToEpoch().ToInt64();

            Console.WriteLine("Epoch = " + epoch);


            tlg = new TopListGroup("form=1&sort=MaxGUP&count=100&MinGUP=5&MinPrice=1.5&MinTV=50000&MaxGUP=-5&MaxPrice=25&X_NYSE=on&XN=on&WN=Small+Gappers&show0=D_Symbol&show1=Price&show2=Float&show3=SFloat&show4=GUP&show5=TV&show6=EarningD&show7=Vol5&show8=STP&show9=RV&show10=D_Name&show11=RD&show12=FCP&show13=D_Sector&show14=&col_ver=1&omh=1&hist=1&exact_time=" + epoch);


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

            var toplist = Client.Connection.TopListManager.GetTopList(Configuration);

            TopList = toplist is TopList tl ? tl : null;


            TopList.TopListStatus += new TopListStatus(TopListStatus_Handler);
            TopList.TopListData += new TopListData(TopListData_Handler);
            TopList.Start();

            //Console.WriteLine("########################################"  + TopList.GetType().FullName);
        }

        

        public string Configuration { get; private set; }

        public void Start() => TopList.Start();

        public void Stop() => TopList.Stop();

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

        HashSet<Stock> UnknownStock { get; } = new HashSet<Stock>();

        void TopListData_Handler(List<RowData> rows, DateTime? start, DateTime? end, TopList sender)
        {
            LastUpdateTime = DateTime.Now;

            lock (Columns)
            {
                foreach (RowData row in rows)
                {
                    if (row.GetAsString("symbol") is string symbol && symbol.Length > 0 && Regex.IsMatch(symbol, @"^[a-zA-Z]+$"))
                    {
                        var list = ContractList.GetList(symbol, US_Exchanges);

                        if (list.Where(n => n is Stock).Count() > 0)
                        {
                            Stock stk = list.First() as Stock;

                            if (list.Count() == 1 && row.GetAsString("c_D_Name") is string fullname && fullname.Length > 0)
                                stk.FullName = fullname;

                            if(stk.ISIN.Length < 8 && !UnknownStock.Contains(stk)) 
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
