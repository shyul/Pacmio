/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio.IB
{
    public class WatchList : Scanner, IWatchList
    {
        public WatchList(string name, int numberOfRows = 25)
        {
            Name = name;
            NumberOfRows = numberOfRows;
        }

        public override void Start()
        {
            if (!IsActive && Client.Connected)
            {
                IsActive = true;
                string configStr = ConfigString;
                Console.WriteLine(Name + " | " + configStr);
                Client.SendRequest_ScannerSubscription(this);
            }
        }

        public override void Stop()
        {
            if (m_IsActive)
            {
                if(Client.Connected) 
                    Client.SendCancel_ScannerSubscription(RequestId);

                IsActive = false;
            }
        }

        public override bool IsActive { get => m_IsActive && Client.Connected; set => m_IsActive = value; }

        private bool m_IsActive = false;

        public int RequestId { get; set; } = 0;

        public string ContractType { get; set; } = "STK";

        public string ContractLocation { get; set; } = "STK.US";

        public string ContractTypeFilter { get; set; } = "ALL";

        public string SortType { get; set; } = string.Empty; // "TOP_PERC_GAIN";
                                                         // "MOST_ACTIVE"
                                                         // "TOP_OPEN_PERC_GAIN"
                                                         // "HALTED"

        //public string FilterOptions { get; set; } = string.Empty; // (23)"marketCapAbove1e6=10000;marketCapBelow1e6=100000;stkTypes=inc:CORP;"
        // (23)"priceAbove=5;avgVolumeAbove=500000;marketCapAbove1e6=1000;"
        // stkTypes: All, inc:CORP, inc:ADR, inc:ETF, inc:ETN, inc:REIT, inc:CEF, inc:ETMF (Exchange Traded Managed Fund)
        // exc:CORP, exc:ADR, exc:ETF, exc:ETN, exc:REIT, exc:CEF

        public override (double Min, double Max) Price
        {
            get => (GetConfigDouble("priceAbove"), GetConfigDouble("priceBelow"));

            set
            {
                SetConfig("priceAbove", value.Min);
                SetConfig("priceBelow", value.Max);
            }
        }

        public double VolumeMinimum { get => GetConfigDouble("volumeAbove"); set => SetConfig("volumeAbove", value); }

        public override (double Min, double Max) MarketCap
        {
            get => (GetConfigDouble("marketCapAbove1e6") * 1e6, GetConfigDouble("marketCapBelow1e6") * 1e6);

            set
            {
                SetConfig("marketCapAbove1e6", value.Min / 1e6);
                SetConfig("marketCapBelow1e6", value.Max / 1e6);
            }
        }

        public override (double Min, double Max) GainPercent
        {
            get => (GetConfigDouble("changePercAbove"), GetConfigDouble("changePercBelow"));

            set
            {
                SetConfig("changePercAbove", value.Min);
                SetConfig("changePercBelow", value.Max);
            }
        }

        public override (double Min, double Max) GapPercent
        {
            get => (GetConfigDouble("openGapPercAbove"), GetConfigDouble("openGapPercBelow"));

            set
            {
                SetConfig("openGapPercAbove", value.Min);
                SetConfig("openGapPercBelow", value.Max);
            }
        }



        public ICollection<Contract> List { get; } = new List<Contract>();

        public HashSet<Contract> UnknownContract { get; } = new HashSet<Contract>();

        public void ScannerData_Handler(string[] fields)
        {
            LastRefreshTime = DateTime.Now;
            if (IsSnapshot) Stop();

            lock (List) 
            {
                List.Clear();

                for (int i = 4; i < fields.Length; i += 16)
                {
                    int rank = fields[i].ToInt32(-1);
                    int conId = fields[i + 1].ToInt32(-1);
                    string symbolName = fields[i + 2];

                    var list = ContractList.GetList(symbolName, "US");

                    if(list.Count() == 1 && list.First() is Stock stk) 
                    {
                        List.Add(stk);
                    }
                    else if(list.Count() < 1)
                    {
                        if (!UnknownSymbols.Contains(symbolName)) 
                        {
                            ContractList.Fetch(symbolName);
                            UnknownSymbols.Add(symbolName);
                        }
                    }
                    else
                    {
                        foreach(Contract c in list) 
                        {
                            if (!UnknownContract.Contains(c)) 
                            {
                                ContractList.Fetch(c);
                                UnknownContract.Add(c);
                            }
                        }
                    }

                    /*
                    string typeName = fields[i + 3];
                    string lastTradeDateOrContractMonth = fields[i + 4];
                    double strike = fields[i + 5].ToDouble();
                    string right = fields[i + 6];
                    string exchange = fields[i + 7]; // SMART, which is useless
                    string currency = fields[i + 8];
                    string localSymbol = fields[i + 9];
                    string MarketName = fields[i + 10];
                    string tradingClass = fields[i + 11]; // "SCM"
                    string distance = fields[i + 12];
                    string benchmark = fields[i + 13];
                    string projection = fields[i + 14];
                    string legsStr = fields[i + 15];
                    */
                }

                //ScannerManager.Updated(info); 

            }


            int j = 0;
            foreach (Contract c in List)
            {
                if (c is Stock stk)
                    Console.WriteLine("Rank " + j + ": " + c.Name + "\t" + "\t" + stk.ISIN + "\t" + c.ExchangeName + "\t" + c.FullName);
                else
                    Console.WriteLine("Rank " + j + ": " + c.Name + "\t" + "\t" + "NoISIN" + "\t" + c.ExchangeName + "\t" + c.FullName);
                j++;
            }

            Console.WriteLine("Scanner Result End.\n\n");
        }
    }
}
