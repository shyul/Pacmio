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
using System.Threading;
using System.Linq;
using Xu;
using Pacmio;

namespace Pacmio.IB
{
    public class WatchList : Scanner, IWatchList
    {
        public WatchList(string name, int numberOfRows = 100)
        {
            Name = name;
            NumberOfRows = numberOfRows;
        }

        public override void Start() => Start(false);

        public void Start(bool isSnapshot)
        {
            if (Client.Connected)
            {
                Stop();

                IsSnapshot = isSnapshot;
                IsActive = true;
                string configStr = ConfigString;
                Client.SendRequest_ScannerSubscription(this);

                Console.WriteLine("#### Start IB WatchList: " + Name + " | " + configStr);
            }
            else
            {
                Console.WriteLine("#### No IB connection, unable to start WatchList: " + Name);
            }
        }

        public override void Stop()
        {
            if (m_IsActive)
            {
                Console.WriteLine("#### Stop IB WatchList: " + Name);

                if (Client.Connected)
                    Client.SendCancel_ScannerSubscription(RequestId);

                IsActive = false;
            }
        }

        public ICollection<Contract> Snapshot()
        {
            Start(true);

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

        public override bool IsActive { get => m_IsActive && Client.Connected; set => m_IsActive = value; }

        private bool m_IsActive = false;

        public int NumberOfRows { get; set; }

        public int RequestId { get; set; } = 0;

        public ContractTypes ContractType { get; set; } = ContractTypes.USStocks;

        public enum ContractTypes
        {
            USStocks,
            USEquityETFs,
            USFixedIncomeETF,
            USFutures,
            AmericaNonUSStocks,
        }

        public string ContractTypeString
        {
            get
            {
                return ContractType switch
                {
                    ContractTypes.USStocks => "STK",
                    ContractTypes.USEquityETFs => "ETF.EQ.US",
                    ContractTypes.USFixedIncomeETF => "ETF.FI.US",
                    ContractTypes.USFutures => "FUT.US",
                    ContractTypes.AmericaNonUSStocks => "STOCK.NA",

                    _ => "STK",
                };
            }
        }

        public string ContractLocation { get; set; } = "STK.US";

        public string ContractTypeFilter { get; set; } = "ALL";

        public string ScanType { get; set; } = string.Empty; // "TOP_PERC_GAIN";
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


        public Contract GetContract(int conId, string symbolName)
        {
            bool tried = false;

        StartLoad:
            var list = ContractList.GetList(conId).Where(n => n.Name == symbolName && n.Status == ContractStatus.Alive && (DateTime.Now - n.UpdateTime).TotalDays < 100);

            if (list.Count() == 1)
            {
                return list.First();
            }
            else if (!tried)
            {
                tried = true;
                var fetchList = ContractList.Fetch(symbolName).Where(n => n.Name == symbolName && n.ConId == conId && n is Stock);
                if (fetchList.Count() > 0)
                {
                    fetchList.ToList().ForEach(n => { if (n is Stock stk && stk.ISIN.Length < 8) ContractList.Fetch(stk); });
                }
                goto StartLoad;
            }

            return null;
        }


        public void ScannerData_Handler(string[] fields)
        {
            LastRefreshTime = DateTime.Now;
            if (IsSnapshot) { Stop(); }

            lock (List)
            {
                List.Clear();

                for (int i = 4; i < fields.Length; i += 16)
                {
                    // int rank = fields[i].ToInt32(-1);
                    int conId = fields[i + 1].ToInt32(-1);
                    string symbolName = fields[i + 2];


                    if (GetContract(conId, symbolName) is Contract c)
                    {
                        List.Add(c);
                    }
                }

                //ScannerManager.Updated(info); 

            }

            lock (List)
            {
                int j = 0;
                foreach (Contract c in List)
                {
                    if (c is Stock stk)
                        Console.WriteLine("Rank " + j + ": " + c.Name + "\t" + "\t" + stk.ISIN + "\t" + c.ExchangeName + "\t" + c.FullName);
                    else
                        Console.WriteLine("Rank " + j + ": " + c.Name + "\t" + "\t" + "NoISIN" + "\t" + c.ExchangeName + "\t" + c.FullName);
                    j++;
                }
            }

            Console.WriteLine("Scanner Result End.\n\n");
        }
    }
}
