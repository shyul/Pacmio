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
    public abstract class Filter : Scanner
    {
        public override bool IsActive { get => m_IsActive && Client.Connected; set => m_IsActive = value; }
        protected bool m_IsActive = false;

        public override string Name { get => GetConfigString("WN"); set => SetConfig("WN", value); }

        public override int NumberOfRows { get => GetConfigInt("count"); set => SetConfig("count", value); }

        public override bool IsSnapshot { get => m_IsSnapshot || IsHistory; set => m_IsSnapshot = value; }

        private bool m_IsSnapshot = false;

        public bool IsHistory { get => GetConfigBool("hist", "1"); set { SetConfig("hist", value, "1"); } }

        public DateTime HistoricalTime
        {
            get => m_HistoricalTime;

            set
            {
                DateTime time = value;

                if ((DateTime.Now - time).TotalDays > 90)
                    time = DateTime.Now.AddDays(-90);

                m_HistoricalTime = time;
            }
        }

        private DateTime m_HistoricalTime;

        public override (double Min, double Max) Price
        {
            get => (GetConfigDouble("MinPrice"), GetConfigDouble("MaxPrice"));

            set
            {
                SetConfig("MinPrice", value.Min);
                SetConfig("MaxPrice", value.Max);
            }
        }

        public (double Min, double Max) Volume
        {
            get => (GetConfigDouble("MinTV"), GetConfigDouble("MaxTV"));

            set
            {
                SetConfig("MinTV", value.Min);
                SetConfig("MaxTV", value.Max);
            }
        }

        public override (double Min, double Max) MarketCap
        {
            get => (GetConfigDouble("MinMCap"), GetConfigDouble("MaxMCap"));

            set
            {
                SetConfig("MinMCap", value.Min);
                SetConfig("MaxMCap", value.Max);
            }
        }

        public override (double Min, double Max) GainPercent
        {
            get => (GetConfigDouble("MinFCP"), GetConfigDouble("MaxFCP"));

            set
            {
                SetConfig("MinFCP", value.Min);
                SetConfig("MaxFCP", value.Max);
            }
        }



        public (double Min, double Max) Gap
        {
            get => (GetConfigDouble("MinGUD"), GetConfigDouble("MaxGUD"));

            set
            {
                SetConfig("MinGUD", value.Min);
                SetConfig("MaxGUD", value.Max);
            }
        }

        public override (double Min, double Max) GapPercent
        {
            get => (GetConfigDouble("MinGUP"), GetConfigDouble("MaxGUP"));

            set
            {
                SetConfig("MinGUP", value.Min);
                SetConfig("MaxGUP", value.Max);
            }
        }

        public (double Min, double Max) GapBars
        {
            get => (GetConfigDouble("MinGUR"), GetConfigDouble("MaxGUR"));

            set
            {
                SetConfig("MinGUR", value.Min);
                SetConfig("MaxGUR", value.Max);
            }
        }




        public (double Min, double Max) AverageTrueRange
        {
            get => (GetConfigDouble("MinATR"), GetConfigDouble("MaxATR"));

            set
            {
                SetConfig("MinATR", value.Min);
                SetConfig("MaxATR", value.Max);
            }
        }

        public (double Min, double Max) Volatility
        {
            get => (GetConfigDouble("MinVWV"), GetConfigDouble("MaxVWV"));

            set
            {
                SetConfig("MinVWV", value.Min);
                SetConfig("MaxVWV", value.Max);
            }
        }

        public (double Min, double Max) VolatilityPercent
        {
            get => (GetConfigDouble("MinVWVP"), GetConfigDouble("MaxVWVP"));

            set
            {
                SetConfig("MinVWVP", value.Min);
                SetConfig("MaxVWVP", value.Max);
            }
        }

        public (double Min, double Max) Wiggle
        {
            get => (GetConfigDouble("MinWiggle"), GetConfigDouble("MaxWiggle"));

            set
            {
                SetConfig("MinWiggle", value.Min);
                SetConfig("MaxWiggle", value.Max);
            }
        }

        public List<Exchange> Exchanges { get; } = new List<Exchange>() { Exchange.NYSE, Exchange.NASDAQ, Exchange.ARCA, Exchange.AMEX, Exchange.BATS };

        public override string ConfigString
        {
            get
            {
                if (Name.Length > 1) ConfigList["WN"] = Name;

                SetConfig("X_NYSE", Exchanges.Contains(Exchange.NYSE), "on");
                SetConfig("XN", Exchanges.Contains(Exchange.NASDAQ), "on");
                SetConfig("X_ARCA", Exchanges.Contains(Exchange.ARCA), "on");
                SetConfig("X_AMEX", Exchanges.Contains(Exchange.AMEX), "on");
                SetConfig("X_BATS", Exchanges.Contains(Exchange.BATS), "on");
                SetConfig("X_PINK", Exchanges.Contains(Exchange.OTCMKT), "on");

                if (IsHistory)
                {
                    long epoch = HistoricalTime.ToEpoch().ToInt64();
                    ConfigList["exact_time"] = epoch.ToString();
                }
                else if (ConfigList.ContainsKey("exact_time"))
                    ConfigList.Remove("exact_time");

                string extraConfig = ExtraConfig;
                if (!extraConfig.EndsWith("&")) extraConfig += "&";
                return extraConfig + string.Join("&", ConfigList.OrderBy(n => n.Key).Select(n => n.Key + "=" + n.Value).ToArray());
            }
        }

        public Dictionary<string, ColumnInfo> Columns { get; } = new Dictionary<string, ColumnInfo>();

        public void ConfigColumns(IList<ColumnInfo> list)
        {
            lock (Columns)
            {
                Columns.Clear();
                foreach (ColumnInfo c in list)
                {
                    Columns[c.WireName] = c;
                    Console.WriteLine(c.WireName + " | " + c.Description + " | " + c.Format + " | " + c.Units + " | " + c.InternalCode + " | " + c.Graphics + " | " + c.TextHeader + " | " + c.PreferredWidth);
                }
            }
        }

        public HashSet<Stock> UnknownStock { get; } = new HashSet<Stock>();

        
        public void InitColumns() 
        {
            ConfigList["form"] = "1";
            ConfigList["omh"] = "1";
            ConfigList["col_ver"] = "1";
            ConfigList["show0"] = "D_Symbol"; // Important
            ConfigList["show1"] = "Price";
            ConfigList["show2"] = "TV";
            ConfigList["show3"] = "Float"; // Important
            ConfigList["show4"] = "SFloat"; // Important
            ConfigList["show5"] = "STP"; // Important
            ConfigList["show6"] = "RV";
            ConfigList["show7"] = "EarningD";
            
        }






        public List<Contract> PrintAllRows(List<RowData> rows, string symbolColumnName = "symbol") 
        {
            List<Contract> List = new List<Contract>();
            lock (Columns)
            {
                foreach (RowData row in rows)
                {
                    if (row.GetAsString(symbolColumnName) is string symbol && symbol.Length > 0 && Regex.IsMatch(symbol, @"^[a-zA-Z]+$"))
                    {
                        var list = ContractList.GetList(symbol, Exchanges);

                        if (list.Where(n => n is Stock).Count() > 0)
                        {
                            Stock stk = list.First() as Stock;

                            if((!IsSnapshot) && (!stk.IsActiveMarketTick) && stk.HistoricalData is HistoricalData hd)
                            {
                                hd.Status = MarketTickStatus.Delayed;

                                hd.LastPrice = row.GetAsString("c_Price") is string s && s.Length > 0 ? s.ToDouble() : double.NaN; // c_Price
                                hd.Volume = row.GetAsString("c_TV") is string s1 && s1.Length > 0 ? s1.ToDouble() : double.NaN;
                                // Price

                                // Volume

                                // 

                                // Social

                                // Float Short

                                // Float
                            }

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

                                switch (key) 
                                {
                                    case ("c_D_Name") when row.GetAsString("c_D_Name") is string fn && fn.Length > 0: stk.FullName = fn; break;
                                    case ("DESCRIPTION"): break;
                                }





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
                            
                            if (row.GetAsString("DESCRIPTION") is string desc && desc.Length > 0)
                            {
                                rowString += " | desc = " + desc;
                            }

                            if (row.GetAsString("ALT_DESCRIPTION") is string adesc && adesc.Length > 0)
                            {
                                rowString += " | adesc = " + adesc;
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


                // Trigger the list is updated event!!
            }

            return List;
        }
    }
}
