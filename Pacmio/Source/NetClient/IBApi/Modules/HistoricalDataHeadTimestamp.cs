/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {
        public static bool IsReady_HistoricalDataHeadTimestamp => Connected && requestId_HistoricalDataHeadTimestamp == -1;
        private static int requestId_HistoricalDataHeadTimestamp = -1;
        private static BarTable activeBarTable_HistoricalDataHeadTimestamp;

        /// <summary>
        /// Sample Request: (0)87-(1)858750582-(2)0-(3)AAPL-(4)STK-(5)-(6)0-(7)-(8)-(9)SMART-(10)-(11)USD-(12)-(13)-(14)0-(15)1-(16)TRADES-(17)1-(18)
        /// </summary>
        /// <param name="c"></param>
        /// <param name="useRTH"></param>
        /// <param name="type"></param>
        /// <param name="formatDate"></param>
        internal static int SendRequest_HistoricalDataHeadTimestamp(BarTable bt, bool includeExpired = false, int formatDate = 1)
        {
            Contract c = bt.Contract;
            var (valid_barFreq, _) = ApiCode.GetIbCode(bt.BarFreq);
            var (valid_barType, barTypeCode) = ApiCode.GetIbCode(bt.Type);
            var (valid_exchange, exchangeCode) = ApiCode.GetIbCode(c.Exchange);

            if (valid_barFreq && valid_barType && valid_exchange && requestId_HistoricalDataHeadTimestamp == -1) // Also please check the RHD is already active?
            {
                (int requestId, string requestType) = RegisterRequest(RequestType.RequestHeadTimestamp);
                requestId_HistoricalDataHeadTimestamp = requestId;
                activeBarTable_HistoricalDataHeadTimestamp = bt;

                bool useSmart = c is ITradable it && it.AutoExchangeRoute;

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

                List<string> paramsList = new List<string>() {
                    requestType,
                    requestId.ParamPos(),

                    c.ConId.Param(),
                    c.Name,
                    c.TypeApiCode,
                    lastTradeDateOrContractMonth,
                    (strike == 0) ? "0" : strike.ToString("0.0###"),
                    right, // 7
                    multiplier, // 8
                    useSmart ? "SMART" : exchangeCode, // "ISLAND" exchange,
                    exchangeCode, // primaryExch,
                    c.CurrencyCode, // currency,
                    "", // c.LocalSymbol, // 12
                    "", // c.TradingClass, // 13
                    includeExpired.Param(),
                    (bt.BarFreq >= BarFreq.Daily).Param(), // bt.IsRegularTradeHoursOnly.Param(), // useRTH
                    barTypeCode, // whatToShow
                    formatDate.Param(), // "1"
                };

                SendRequest(paramsList);

                return requestId;
            }

            return -1;
        }

        /// <summary>
        /// Sample Message HeadTimestamp: (0)"88"-(1)"1"-(2)"19801212  14:30:00"-
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_HistoricalHeadDataTimestamp(string[] fields)
        {
            int requestId = fields[1].ToInt32(-1);
            if (requestId == requestId_HistoricalDataHeadTimestamp)
            {
                Console.WriteLine("HistoricalDataEarliestTime = " + fields[2]);
                activeBarTable_HistoricalDataHeadTimestamp.Contract.BarTableEarliestTime = Util.ParseTime(fields[2], activeBarTable_HistoricalDataHeadTimestamp.Contract.TimeZone);
            }
            RemoveRequest(requestId, false);
            requestId_HistoricalDataHeadTimestamp = -1;
        }

        public static void SendCancel_HistoricalHeadDataTimestamp() => SendCancel_HistoricalHeadDataTimestamp(requestId_HistoricalDataHeadTimestamp);
        /*
            if (Connected)
            {
                RemoveRequest(DataRequestID, RequestType.RequestHeadTimestamp);
                DataRequestID = -1; // Emit update cancelled.
            }*/

    private static void SendCancel_HistoricalHeadDataTimestamp(int requestId)
        {
            if (Connected && requestId > -1)
            {
                RemoveRequest(requestId, RequestType.RequestHeadTimestamp);
                requestId_HistoricalDataHeadTimestamp = -1; // Emit update cancelled.
            }
        }
    }
}