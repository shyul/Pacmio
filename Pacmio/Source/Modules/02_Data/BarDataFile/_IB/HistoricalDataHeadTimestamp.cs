/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Threading;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {
        private static BarDataFile ActiveBarDataFile_HistoricalDataHeadTimestamp;

        public static DateTime Fetch_HistoricalDataHeadTimestamp(BarDataFile bdf, CancellationTokenSource cts = null, bool includeExpired = false, int formatDate = 1)
        {
            lock (DataRequestLockObject)
            {
                if (cts.IsContinue() && DataRequestReady && HistoricalData_Connected)
                {
                    if (cts.Cancelled() || IsCancelled)
                        goto End;

                    StartDownload:
                    SendRequest_HistoricalDataHeadTimestamp(bdf, includeExpired, formatDate);

                    int time = 0;
                    while (!DataRequestReady)
                    {
                        time++;
                        Thread.Sleep(10);

                        if (time > Timeout) // Handle Time out here.
                        {
                            SendCancel_HistoricalHeadDataTimestamp();
                            Thread.Sleep(100);
                            goto StartDownload;
                        }
                        else if (cts.Cancelled() || IsCancelled)
                        {
                            SendCancel_HistoricalHeadDataTimestamp();
                            goto End;
                        }
                    }
                }
            }

        End:
            return bdf.HistoricalHeadTime;
        }


        /// <summary>
        /// Sample Request: (0)87-(1)858750582-(2)0-(3)AAPL-(4)STK-(5)-(6)0-(7)-(8)-(9)SMART-(10)-(11)USD-(12)-(13)-(14)0-(15)1-(16)TRADES-(17)1-(18)
        /// </summary>
        /// <param name="c"></param>
        /// <param name="useRTH"></param>
        /// <param name="type"></param>
        /// <param name="formatDate"></param>
        private static void SendRequest_HistoricalDataHeadTimestamp(BarDataFile bdf, bool includeExpired = false, int formatDate = 1)
        {
            Contract c = bdf.Contract;

            if (bdf.PriceType.Param() is string barTypeCode &&
                c.Exchange.Param() is string exchangeCode &&
                DataRequestReady) // Also please check the RHD is already active?
            {
                (int requestId, string requestType) = RegisterRequest(RequestType.RequestHeadTimestamp);
                DataRequestID = requestId;
                ActiveBarDataFile_HistoricalDataHeadTimestamp = bdf;

                string lastTradeDateOrContractMonth = "";
                double strike = 0;
                string right = "";
                string multiplier = "";

                if (c is IOption opt)
                {
                    lastTradeDateOrContractMonth = opt.LastTradeDateOrContractMonth;
                    strike = opt.Strike;
                    right = opt.Right;
                    multiplier = opt.Multiplier;
                }

                List<string> paramsList = new() {
                    requestType,
                    requestId.ParamPos(),

                    c.ConId.Param(),
                    c.Name,
                    c.TypeCode(),
                    lastTradeDateOrContractMonth,
                    (strike == 0) ? "0" : strike.ToString("0.0###"),
                    right, // 7
                    multiplier, // 8
                    c.SmartExchangeRoute ? "SMART" : exchangeCode, // "ISLAND" exchange,
                    exchangeCode, // primaryExch,
                    c.CurrencyCode, // currency,
                    "", // c.LocalSymbol, // 12
                    "", // c.TradingClass, // 13
                    includeExpired.Param(),
                    (bdf.BarFreq >= BarFreq.Daily).Param(), // bt.IsRegularTradeHoursOnly.Param(), // useRTH
                    barTypeCode, // whatToShow
                    formatDate.Param(), // "1"
                };

                SendRequest(paramsList);
            }
        }

        private static void SendCancel_HistoricalHeadDataTimestamp()
        {
            if (Connected)
            {
                RemoveRequest(DataRequestID, RequestType.RequestHeadTimestamp);
                ActiveBarDataFile_HistoricalDataHeadTimestamp = null;
                DataRequestID = -1; // Emit update cancelled.
            }
        }

        private static void ParseError_HistoricalHeadDataTimestamp(string[] fields)
        {
            int requestId = fields[2].ToInt32(-1);
            RemoveRequest(requestId, false);
            Contract c = ActiveBarDataFile_HistoricalDataHeadTimestamp.Contract;

            if (fields[3] == "200")
            {
                c.Status = ContractStatus.Error;
                c.UpdateTime = DateTime.Now;
            }

            ActiveBarDataFile_HistoricalDataHeadTimestamp.HistoricalHeadTime = c.HistoricalHeadTime;

            Console.WriteLine("Requesting HeadTimestamp errors: " + ActiveBarDataFile_HistoricalDataHeadTimestamp.ToString() + " | " + fields.ToStringWithIndex());
            Console.WriteLine("Using daily BDF's HeadTimestamp: " + ActiveBarDataFile_HistoricalDataHeadTimestamp.HistoricalHeadTime);
            DataRequestID = -1;
        }

        /// <summary>
        /// Sample Message HeadTimestamp: (0)"88"-(1)"1"-(2)"19801212  14:30:00"-
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_HistoricalHeadDataTimestamp(string[] fields)
        {
            int requestId = fields[1].ToInt32(-1);
            if (requestId == DataRequestID)
            {
                Console.WriteLine("HistoricalDataEarliestTime = " + fields[2]);

                ActiveBarDataFile_HistoricalDataHeadTimestamp.HistoricalHeadTime
                    = Util.ParseTime(fields[2], ActiveBarDataFile_HistoricalDataHeadTimestamp.Contract.TimeZone);

                ActiveBarDataFile_HistoricalDataHeadTimestamp.SaveFile();
            }

            RemoveRequest(requestId, false);
            DataRequestID = -1;
        }
    }
}