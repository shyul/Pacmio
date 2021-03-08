/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// Duration            Allowed Bar Sizes
/// =====================================
/// 60 S	            1 sec - 1 mins
/// 120 S	            1 sec - 2 mins
/// 1800 S(30 mins)     1 sec - 30 mins
/// 3600 S(1 hr)        5 secs - 1 hr
/// 14400 S(4hr)        10 secs - 3 hrs
/// 28800 S(8 hrs)      30 secs - 8 hrs
/// 1 D                 1 min - 1 day
/// 2 D                 2 mins - 1 day
/// 1 W                 3 mins - 1 week
/// 1 M                 30 mins - 1 month
/// 1 Y                 1 day - 1 month
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {
        #region Fetch Historical Data



        public static void Fetch_HistoricalData(BarTable bt, Period period, CancellationTokenSource cts = null)
        {
            if (cts.IsContinue() && HistoricalData_Connected && bt.BarFreq.GetAttribute<BarFreqInfo>() is BarFreqInfo bfi && period.Start < DateTime.Now)
            {
                // We will download, but won't log the period if the stop may extended to the future.
                IsLoggingLastRequestedHistoricalDataPeriod = period.Stop < DateTime.Now.AddDays(-1);
                LastRequestedHistoricalDataPeriod = period;

                // RequestLockObject and DataRequestReady must happen in pairs
                lock (DataRequestLockObject)
                    if (DataRequestReady)
                    {
                        if (cts.Cancelled() || IsCancelled) return;

                        StartDownload:
                        SendRequest_HistoricalData(bt, bfi.DurationString, period.Stop);

                        int time = 0; // Wait the last transmit is over.
                        while (!DataRequestReady)
                        {
                            time++;
                            Thread.Sleep(50);

                            if (time > Timeout) // Handle Time out here.
                            {
                                SendCancel_HistoricalData();
                                Thread.Sleep(2000);
                                goto StartDownload;
                            }
                            else if (cts.Cancelled() || IsCancelled)
                            {
                                SendCancel_HistoricalData();
                                return;
                            }
                        }
                    }

            }
        }

        #endregion Fetch Historical Data

        public static DateTime LastDataRequestTime { get; private set; } = DateTime.MinValue;

        public static bool HistoricalData_Connected { get; private set; } = true;

        public static readonly List<string> HistoricalData_Servers = new List<string>();

        /**
         * @brief Requests contracts' historical data.
         * When requesting historical data, a finishing time and date is required along with a duration string. For example, having: 
         *      - endDateTime: 20130701 23:59:59 GMT
         *      - durationStr: 3 D
         * will return three days of data counting backwards from July 1st 2013 at 23:59:59 GMT resulting in all the available bars of the last three days until the date and time specified. It is possible to specify a timezone optionally. The resulting bars will be returned in EWrapper::historicalData
         * @param tickerId the request's unique identifier.
         * @param contract the contract for which we want to retrieve the data.
         * @param endDateTime request's ending time with format yyyyMMdd HH:mm:ss {TMZ}
         * @param durationString the amount of time for which the data needs to be retrieved:
         *      - " S (seconds)
         *      - " D (days)
         *      - " W (weeks)
         *      - " M (months)
         *      - " Y (years)
         * @param barSizeSetting the size of the bar:
         *      - 1 sec
         *      - 5 secs
         *      - 15 secs
         *      - 30 secs
         *      - 1 min
         *      - 2 mins
         *      - 3 mins
         *      - 5 mins
         *      - 15 mins
         *      - 30 mins
         *      - 1 hour
         *      - 1 day
         * @param whatToShow the kind of information being retrieved:
         *      - TRADES
         *      - MIDPOINT
         *      - BID
         *      - ASK
         *      - BID_ASK
         *      - HISTORICAL_VOLATILITY
         *      - OPTION_IMPLIED_VOLATILITY
         *	    - FEE_RATE
         *	    - REBATE_RATE
         * @param useRTH set to 0 to obtain the data which was also generated outside of the Regular Trading Hours, set to 1 to obtain only the RTH data
         * @param formatDate set to 1 to obtain the bars' time as yyyyMMdd HH:mm:ss, set to 2 to obtain it like system time format in seconds
         * @param keepUpToDate set to True to received continuous updates on most recent bar data. If True, and endDateTime cannot be specified.
         * @sa EWrapper::historicalData
         */

        #region Historical Data

        private static Period LastRequestedHistoricalDataPeriod { get; set; }  // Start Time and Stop Time
        private static bool IsLoggingLastRequestedHistoricalDataPeriod { get; set; } = false;

        private static BarTable active_HistoricalDataBarTable;
        private static TimeZoneInfo ActiveTimeZone_HistoricalData => active_HistoricalDataBarTable.Contract.TimeZone;

        /// <summary>
        /// At this time Historical Data Limitations for barSize = "1 mins" and greater have been lifted. 
        /// However, please use caution when requesting large amounts of historical data or sending historical data requests too frequently. 
        /// Though IB has lifted the "hard" limit, we still implement a "soft" slow to load-balance client requests vs. server response. 
        /// Requesting too much historical data can lead to throttling and eventual disconnect of the API client. 
        /// If a request requires more than several minutes to return data, it would be best to cancel the request using the IBApi.EClient.cancelHistoricalData function.
        /// </summary>
        /// <param name="bt"></param>
        /// <param name="durationString"></param>
        /// <param name="endTime"></param>
        /// <param name="keepUpToDate"></param>
        /// <param name="includeExpired"></param>
        /// <param name="formatDate"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        private static void SendRequest_HistoricalData(BarTable bt, string durationString, DateTime endTime,
            bool keepUpToDate = false, bool includeExpired = false, int formatDate = 1,
            ICollection<(string, string)> options = null)
        {
            Contract c = bt.Contract;

            endTime = TimeZoneInfo.ConvertTimeFromUtc(TimeZoneInfo.ConvertTimeToUtc(endTime, c.TimeZone), TimeZoneInfo.Local);

            if (bt.BarFreq.Param() is string barFreqCode &&
                bt.Type.Param() is string barTypeCode &&
                c.Exchange.Param() is string exchangeCode &&
                DataRequestReady)
            {
                (int requestId, string requestType) = RegisterRequest(RequestType.RequestHistoricalData);
                DataRequestID = requestId;
                active_HistoricalDataBarTable = bt;

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
                    c.TypeCode(),
                    lastTradeDateOrContractMonth,
                    (strike == 0) ? "0" : strike.ToString("0.0###"),
                    right, // 7
                    multiplier, // 8
                    c.SmartExchangeRoute ? "SMART" : exchangeCode, // "ISLAND" exchange,
                    exchangeCode, // primaryExch,
                    c.CurrencyCode, // currency,
                    "", // LocalSymbol / 12 Judy the name
                    "", // TradingClass / 13 SCM, NMS
                    includeExpired.Param(),
                    keepUpToDate ? "" : endTime.ToString("yyyyMMdd HH:mm:ss"), // endDateTime, // "20180720 17:00:00", 
                    barFreqCode,
                    durationString,
                    (bt.BarFreq >= BarFreq.Daily).Param(), // bt.IsRegularTradeHoursOnly.Param(), // useRTH
                    barTypeCode, // whatToShow,
                    formatDate.Param() // "1"
                };

                // Insert Bag Data Here.
                // if (string.Compare(secTypeCode, "BAG", true) == 0)
                if (c is ICombo cmb)
                {
                    if (cmb.ComboLegs is null)
                    {
                        paramsList.Add("0");
                    }
                    else
                    {
                        paramsList.Add(cmb.ComboLegs.Count.ParamPos());
                        foreach (ComboLeg leg in cmb.ComboLegs)
                        {
                            paramsList.Add(leg.ConId.ParamPos());
                            paramsList.Add(leg.Ratio.Param());
                            paramsList.Add(leg.Action);
                            paramsList.Add(leg.Exchange);
                        }
                    }
                }

                paramsList.Add(keepUpToDate.Param());
                paramsList.Add(options.Param());

                while ((DateTime.Now - LastDataRequestTime).TotalSeconds < 3) { Thread.Sleep(200); }
                SendRequest(paramsList);
                LastDataRequestTime = DateTime.Now;
            }
        }

        private static void SendCancel_HistoricalData()
        {
            if (Connected)
            {
                RemoveRequest(DataRequestID, RequestType.RequestHistoricalData);
                DataRequestID = -1; // Emit update cancelled.
            }
        }

        private static void ParseError_HistoricalData(string[] fields)
        {
            int requestId = fields[2].ToInt32(-1);
            string message = fields[4];

            Console.WriteLine("Historical Data Error !!!!!!!!!!!!! " + fields[2] + ": " + message);

            if (requestId > -1)
            {
                RemoveRequest(requestId, RequestType.RequestHistoricalData);
                if (DataRequestID == requestId)
                {
                    DataRequestID = -1;
                }

                //ActiveBarTable_HistoricalData = null;
                if (fields[3] == "200")
                {
                    active_HistoricalDataBarTable.Contract.Status = ContractStatus.Error;
                    active_HistoricalDataBarTable.Contract.UpdateTime = DateTime.Now;
                }
                else if (fields[3] == "366")
                {
                    // Unable to find the table for the contract
                }
            }
        }

        private static void Parse_HistoricalData(string[] fields)
        {
            int requestId = fields[1].ToInt32(-1);
            int num = fields[4].ToInt32();

            Console.WriteLine(ActiveTimeZone_HistoricalData.DisplayName);

            if (fields.Length == 5 + num * 8 && requestId == DataRequestID)
            {
                /*
                LastHistoricalDataPeriod = new Period(Util.ParseTime(fields[2], ActiveTimeZone_HistoricalData), 
                                                        Util.ParseTime(fields[3], ActiveTimeZone_HistoricalData)); */

                Console.WriteLine("Start Parsing Historical Data... " + LastRequestedHistoricalDataPeriod + ", num: " + num);

                TimeSpan ts = active_HistoricalDataBarTable.Frequency.Span;
                DateTime time = DateTime.MinValue;

                for (int i = 0; i < num; i++)
                {
                    int pt = (i * 8) + 5;

                    time = Util.ParseTime(fields[pt], ActiveTimeZone_HistoricalData);
                    double open = fields[pt + 1].ToDouble(-1);
                    double high = fields[pt + 2].ToDouble(-1);
                    double low = fields[pt + 3].ToDouble(-1);
                    double close = fields[pt + 4].ToDouble(-1);
                    double volume = fields[pt + 5].ToDouble(-1) * 100;

                    if (open != -1 && high != -1 && low != -1 && close != -1 && volume > 0)
                        active_HistoricalDataBarTable.Add(DataSourceType.IB, time, ts, open, high, low, close, volume, true);
                }

                if (IsLoggingLastRequestedHistoricalDataPeriod)
                    active_HistoricalDataBarTable.DataSourceSegments.Add(LastRequestedHistoricalDataPeriod, DataSourceType.IB);
                else if (time > LastRequestedHistoricalDataPeriod.Start)
                    active_HistoricalDataBarTable.DataSourceSegments.Add(new Period(LastRequestedHistoricalDataPeriod.Start, time), DataSourceType.IB);
            }

            RemoveRequest(requestId, false); // false means the task is ended with success
            DataRequestID = -1;
        }

        private static void Parse_HistoricalDataUpdate(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());

            /*

            private void HistoricalDataUpdateEvent()
            {
                int requestId = ReadInt(); 1
                int barCount = ReadInt(); 2
                string date = ReadString(); 3 
                double open = ReadDouble(); 4
                double close = ReadDouble(); 5
                double high = ReadDouble(); 6
                double low = ReadDouble(); 7
                double WAP = ReadDouble(); 8
                long volume = ReadLong(); 9

                eWrapper.historicalDataUpdate(requestId, new Bar(date, open, high, low,
                                        close, volume, barCount, WAP));
            }

            Received HistoricalDataUpdate: (0)"90"-(1)"30000001"-(2)"2"-(3)"20200415  16:01:00"-(4)"176.10"-(5)"176.10"-(6)"176.10"-(7)"176.10"-(8)"176.1"-(9)"8"
            Received HistoricalDataUpdate: (0)"90"-(1)"30000001"-(2)"2"-(3)"20200415  16:01:00"-(4)"176.10"-(5)"176.10"-(6)"176.10"-(7)"176.10"-(8)"176.1"-(9)"8"
            Received HistoricalDataUpdate: (0)"90"-(1)"30000001"-(2)"2"-(3)"20200415  16:01:00"-(4)"176.10"-(5)"176.10"-(6)"176.10"-(7)"176.10"-(8)"176.1"-(9)"8"
            Received HistoricalDataUpdate: (0)"90"-(1)"30000001"-(2)"2"-(3)"20200415  16:01:00"-(4)"176.10"-(5)"176.10"-(6)"176.10"-(7)"176.10"-(8)"176.1"-(9)"8"
            Received HistoricalDataUpdate: (0)"90"-(1)"30000001"-(2)"-1"-(3)"20200415  16:02:00"-(4)"176.00"-(5)"176.00"-(6)"176.00"-(7)"176.00"-(8)"176.1"-(9)"-1"
            Received HistoricalDataUpdate: (0)"90"-(1)"30000001"-(2)"-1"-(3)"20200415  16:02:00"-(4)"176.00"-(5)"176.00"-(6)"176.00"-(7)"176.00"-(8)"176.1"-(9)"-1"
            Received HistoricalDataUpdate: (0)"90"-(1)"30000001"-(2)"1"-(3)"20200415  16:02:00"-(4)"176.00"-(5)"176.00"-(6)"176.00"-(7)"176.00"-(8)"176.0"-(9)"1"
            Received HistoricalDataUpdate: (0)"90"-(1)"30000001"-(2)"1"-(3)"20200415  16:02:00"-(4)"176.00"-(5)"176.00"-(6)"176.00"-(7)"176.00"-(8)"176.0"-(9)"1"
            Received HistoricalDataUpdate: (0)"90"-(1)"30000001"-(2)"1"-(3)"20200415  16:02:00"-(4)"176.00"-(5)"175.93"-(6)"176.00"-(7)"175.93"-(8)"176.0"-(9)"1"
            Received HistoricalDataUpdate: (0)"90"-(1)"30000001"-(2)"2"-(3)"20200415  16:02:00"-(4)"176.00"-(5)"175.93"-(6)"176.00"-(7)"175.93"-(8)"175.95333333333335"-(9)"3"

             */
        }

        #endregion Historical Data
    }
}