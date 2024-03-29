﻿/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Threading;
using System.Threading.Tasks;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {
        #region Fetch Fundamental Data

        public static void Fetch_FundamentalData(this Contract c, FinancialDataRequestType type, CancellationTokenSource cts = null)
        {
            lock (DataRequestLockObject)
            {
                if (cts.IsContinue() && DataRequestReady)
                {
                    if (cts.Cancelled() || IsCancelled) return;

                    StartDownload:
                    SendRequest_FinancialData(c, type);

                    int time = 0; // Wait the last transmit is over.
                    while (!DataRequestReady)
                    {
                        time++;
                        Thread.Sleep(50);

                        if (time > Timeout) // Handle Time out here.
                        {
                            SendCancel_FundamentalData();
                            Thread.Sleep(2000);
                            goto StartDownload;
                        }
                        else if (cts.Cancelled() || IsCancelled)
                        {
                            SendCancel_FundamentalData();
                            return;
                        }
                    }
                }
            }
        }

        #endregion Fetch Fundamental Data


        private static void SendRequest_FinancialData(Contract c, FinancialDataRequestType type)
        {
            if (Connected && c.Exchange.Param() is string exchangeCode)
            {
                FinancialDataTools.IB_RequestContract = c;
                FinancialDataTools.IB_RequestType = type;

                (int requestId, string requestType) = RegisterRequest(RequestType.RequestFundamentalData);
                DataRequestID = requestId;

                SendRequest(new string[] {
                    requestType,
                    "3", // API Version 3
                    requestId.ToString(),
                    c.ConId.Param(), // ConId
                    c.Name, // EquityFactory.GetEquity(fdk.Key).Symbol,
                    c.TypeCode(),
                    c.SmartExchangeRoute ? "SMART" : exchangeCode, // "ISLAND" exchange,
                    exchangeCode, // primaryExch,
                    c.CurrencyCode, // Currency "USD",
                    string.Empty,
                    type.Param(), // FundamentalRequestType
                });
            }
        }

        private static void SendCancel_FundamentalData()
        {
            if (Connected)
            {
                RemoveRequest(DataRequestID, RequestType.RequestFundamentalData);
                DataRequestID = -1; // Emit update cancelled.
            }
        }

        private static void Parse_FundamentalData(string[] fields)
        {
            int requestId = fields[2].ToInt32(-1);
            if (requestId > -1 && requestId == DataRequestID)
            {
                Task.Run(() =>
                {
                    FinancialDataTools.ApplyData(
                        FinancialDataTools.IB_RequestContract,
                        FinancialDataTools.IB_RequestType,
                        fields[3],
                        true);
                });
                RemoveRequest(requestId, false);
                DataRequestID = -1;
            }
            else
                throw new Exception("Miss-matched Request Id for FundamentalData: requestId = " + requestId + " DataRequestID = " + DataRequestID);
        }
    }
}
