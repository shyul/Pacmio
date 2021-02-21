/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xu;

namespace Pacmio
{
    public class InteractiveBrokerWatchList : DynamicWatchList
    {
        public InteractiveBrokerWatchList(string name, int numberOfRows = 100)
        {
            Name = name;
            NumberOfRows = numberOfRows;
        }

        public override void Start()
        {
            if (IB.Client.Connected)
            {
                Stop();

                IsSnapshot = false;
                IsRunning = true;
                string configStr = ConfigurationString;
                IB.Client.SendRequest_ScannerSubscription(this);

                Console.WriteLine("#### Start IB WatchList: " + Name + " | " + configStr);
            }
            else
            {
                Console.WriteLine("#### No IB connection, unable to start WatchList: " + Name);
            }
        }

        public override void Stop()
        {
            if (IsRunning)
            {
                Console.WriteLine("#### Stop IB WatchList: " + Name);
                IB.Client.SendCancel_ScannerSubscription(RequestId);
                IsRunning = false;
            }
        }

        public override IEnumerable<Contract> SingleSnapshot()
        {
            if (IB.Client.Connected)
            {
                Stop();

                IsSnapshot = true;
                IsRunning = true;
                string configStr = ConfigurationString;
                IB.Client.SendRequest_ScannerSubscription(this);

                Console.WriteLine("#### Get a snapshot for : " + Name + " | " + configStr);

                while (IsRunning) { }

                return Contracts;
            }
            else
            {
                Console.WriteLine("#### No IB connection, unable to start WatchList: " + Name);
                return null;
            }
        }

        public override bool IsRunning { get => m_IsRunning && IB.Client.Connected; protected set => m_IsRunning = value; }
        private bool m_IsRunning = false;

        public int RequestId { get; set; } = -1;

        public override int NumberOfRows { get; set; } = 100;

        public IB.ScannerContractType ContractType { get; set; } = IB.ScannerContractType.USStocks;

        /*
        public string ContractTypeString => ContractType switch
        {
            IB.ScannerContractType.USStocks => "STK",
            IB.ScannerContractType.USEquityETFs => "ETF.EQ.US",
            IB.ScannerContractType.USFixedIncomeETF => "ETF.FI.US",
            IB.ScannerContractType.USFutures => "FUT.US",
            IB.ScannerContractType.AmericaNonUSStocks => "STOCK.NA",
            _ => "STK",
        };*/

        public string ContractLocation { get; set; } = "STK.US";

        public string ContractTypeFilter { get; set; } = "ALL";

        public IB.ScannerType ScannerType { get; set; } = IB.ScannerType.NONE;

        /*
        public string ScanTypeString { get; set; } = string.Empty; // "TOP_PERC_GAIN";
                                                             // "MOST_ACTIVE"
                                                             // "TOP_OPEN_PERC_GAIN"
                                                             // "HALTED"
        */

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
    }
}
