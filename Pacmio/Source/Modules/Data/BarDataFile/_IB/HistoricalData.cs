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
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {
        public static DateTime LastDataRequestTime { get; private set; } = DateTime.MinValue;

        public static bool HistoricalData_Connected { get; private set; } = true;

        public static List<string> HistoricalData_Servers { get; } = new List<string>();

        private static BarDataFile ActiveBarDataFile_HistoricalData { get; set; } = null;

        private static TimeZoneInfo ActiveTimeZone_HistoricalData => ActiveBarDataFile_HistoricalData.Contract.TimeZone;

        #region Fetch Historical Data

        /// <summary>
        /// If a request requires more than several minutes to return data, it would be best to cancel the request using the IBApi.EClient.cancelHistoricalData function.
        /// https://interactivebrokers.github.io/tws-api/historical_limitations.html
        /// </summary>
        /// <param name="bt"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static void Fetch_HistoricalData(BarDataFile bdf, Period period, CancellationTokenSource cts)
        {
            if (bdf.Contract.Status != ContractStatus.Error &&
                Connected &&
                HistoricalData_Connected &&
                bdf.BarFreq.GetAttribute<BarFreqInfo>() is BarFreqInfo bfi) // && HistoricalData_Connected)
            {
                Console.WriteLine(MethodBase.GetCurrentMethod().Name + " | Initial Request Period: " + period);

                if (bdf.HistoricalHeadTime.IsInvalid()) // If EarliestTime is unset, then request it here.
                {
                    if (cts.Cancelled()) goto End;

                    Console.WriteLine(MethodBase.GetCurrentMethod().Name + " | Historical Head Time Is Invalid, need to check with IB.");

                    Fetch_HistoricalDataHeadTimestamp(bdf, cts);
                }

                if (bdf.GetMissingPeriods(period, DataSourceType.IB) is MultiPeriod missing_period_list && !missing_period_list.IsEmpty)
                {
                    Console.WriteLine(MethodBase.GetCurrentMethod().Name + " | Rectified Period: \n" + missing_period_list.ToString());

                    var api_request_endTime_list = missing_period_list.Split(bfi.Duration).OrderBy(n => n.Stop).Select(n => n.Stop);

                    foreach (DateTime api_request_endTime in api_request_endTime_list)
                    {
                        if (cts.Cancelled() || IsCancelled)
                            goto End;
                        else
                            Thread.Sleep(2000);

                        lock (DataRequestLockObject)
                            if (DataRequestReady)
                            {
                            StartDownload:
                                Console.WriteLine("\n" + MethodBase.GetCurrentMethod().Name + " | Sending Api Request: " + api_request_endTime);
                                SendRequest_HistoricalData(bdf, bfi.DurationString, api_request_endTime);

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

                                        // OR goto Segment or so......
                                        goto End;
                                    }
                                }
                            }
                    }
                }
            }

        End:
            return;
        }

        #endregion Fetch Historical Data

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
        private static void SendRequest_HistoricalData(BarDataFile bdf, string durationString, DateTime endTime,
            bool keepUpToDate = false, bool includeExpired = false, int formatDate = 1,
            ICollection<(string, string)> options = null)
        {
            Contract c = bdf.Contract;

            //TODO: Test this one...
            endTime = TimeZoneInfo.ConvertTimeFromUtc(TimeZoneInfo.ConvertTimeToUtc(endTime, c.TimeZone), TimeZoneInfo.Local);

            if (bdf.BarFreq.Param() is string barFreqCode &&
                bdf.Type.Param() is string barTypeCode &&
                c.Exchange.Param() is string exchangeCode &&
                DataRequestReady)
            {
                (int requestId, string requestType) = RegisterRequest(RequestType.RequestHistoricalData);
                DataRequestID = requestId;
                ActiveBarDataFile_HistoricalData = bdf;

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
                    "", // LocalSymbol / 12 Judy the name
                    "", // TradingClass / 13 SCM, NMS
                    includeExpired.Param(),
                    keepUpToDate ? "" : endTime.ToString("yyyyMMdd HH:mm:ss"), // endDateTime, // "20180720 17:00:00", 
                    barFreqCode,
                    durationString,
                    (bdf.BarFreq >= BarFreq.Daily).Param(), // bt.IsRegularTradeHoursOnly.Param(), // useRTH
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
                ActiveBarDataFile_HistoricalData = null;
                DataRequestID = -1; // Emit update cancelled.
            }
        }

        private static void Parse_HistoricalData(string[] fields)
        {
            int requestId = fields[1].ToInt32(-1);
            int num = fields[4].ToInt32();

            Console.WriteLine(ActiveTimeZone_HistoricalData.DisplayName);
            Console.WriteLine("Start Parsing Historical Data... " + ActiveBarDataFile_HistoricalData.Contract + ", num: " + num);

            if (fields.Length == 5 + num * 8 && requestId == DataRequestID)
            {
                var rows = new List<(DateTime time, double O, double H, double L, double C, double V)>();
                Period data_pd = new();

                for (int i = 0; i < num; i++)
                {
                    int pt = (i * 8) + 5;

                    DateTime time = Util.ParseTime(fields[pt], ActiveTimeZone_HistoricalData);
                    double open = fields[pt + 1].ToDouble(-1);
                    double high = fields[pt + 2].ToDouble(-1);
                    double low = fields[pt + 3].ToDouble(-1);
                    double close = fields[pt + 4].ToDouble(-1);
                    double volume = fields[pt + 5].ToDouble(-1) * 100;

                    rows.Add((time, open, high, low, close, volume));
                    data_pd.Insert(time);

                    //if (open != -1 && high != -1 && low != -1 && close != -1 && volume > 0)
                    //ActiveBarDataFile_HistoricalData.Add(DataSourceType.IB, time, ts, open, high, low, close, volume, true);
                }

                data_pd.Insert(data_pd.Stop + ActiveBarDataFile_HistoricalData.Frequency.Span);
                ActiveBarDataFile_HistoricalData.AddRows(rows, DataSourceType.IB, data_pd);
                ActiveBarDataFile_HistoricalData.SaveFile();
            }

            ActiveBarDataFile_HistoricalData = null;
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
                    ActiveBarDataFile_HistoricalData.Contract.Status = ContractStatus.Error;
                    ActiveBarDataFile_HistoricalData.Contract.UpdateTime = DateTime.Now;

                }
                else if (fields[3] == "366")
                {
                    // Unable to find the table for the contract
                }

                ActiveBarDataFile_HistoricalData = null;
            }
        }

        /*
         
        Send RequestHeadTimestamp: (0)"87"-(1)"3"-(2)"72539702"-(3)"TQQQ"-(4)"STK"-(5)""-(6)"0"-(7)""-(8)""-(9)"SMART"-(10)"ISLAND"-(11)"USD"-(12)""-(13)""-(14)"0"-(15)"0"-(16)"TRADES"-(17)"1"
        Removing [RequestHeadTimestamp] Request only from the ActiveRequestIds list >> 08:00:09 | requestId = 3
        Requesting HeadTimestamp errors: Pacmio.BarDataFile | (0)"4"-(1)"2"-(2)"3"-(3)"162"-(4)"Historical Market Data Service error message:Trading TWS session is connected from a different IP address"
        Fetch_HistoricalData | Initial Request Period: 02-28-2021 00:00:00|03-11-2021 08:00:09
        Fetch_HistoricalData | Historical Head Time Is Invalid, need to check with IB.
        Send RequestHeadTimestamp: (0)"87"-(1)"4"-(2)"72539702"-(3)"TQQQ"-(4)"STK"-(5)""-(6)"0"-(7)""-(8)""-(9)"SMART"-(10)"ISLAND"-(11)"USD"-(12)""-(13)""-(14)"0"-(15)"0"-(16)"TRADES"-(17)"1"
        Removing [RequestHeadTimestamp] Request only from the ActiveRequestIds list >> 08:00:09 | requestId = 4
        Requesting HeadTimestamp errors: Pacmio.BarDataFile | (0)"4"-(1)"2"-(2)"4"-(3)"162"-(4)"Historical Market Data Service error message:Trading TWS session is connected from a different IP address"
        Requesting HeadTimestamp errors: Pacmio.BarDataFile | (0)"4"-(1)"2"-(2)"7"-(3)"162"-(4)"Historical Market Data Service error message:No historical market data for PLTR/STK@VALUE Last 0"

        Fetch_HistoricalData | Initial Request Period: 02-28-2021 00:00:00|03-11-2021 18:27:15
        Fetch_HistoricalData | Historical Head Time Is Invalid, need to check with IB.
        Send RequestHeadTimestamp: (0)"87"-(1)"8"-(2)"444857009"-(3)"PLTR"-(4)"STK"-(5)""-(6)"0"-(7)""-(8)""-(9)"SMART"-(10)"NYSE"-(11)"USD"-(12)""-(13)""-(14)"0"-(15)"0"-(16)"TRADES"-(17)"1"
        ###### Inbound Tick Ignored, because source = Quandl
        Added inbound tick[ Realtime | 3/11/2021 6:27:14 PM | 3/11/2021 6:27:03 PM -> 3/11/2021 6:27:14 PM, IsCurrent = False | 03-11-2021 18:27:00|03-11-2021 18:28:00] to existing bar: 03-11-2021 18:27:03|03-11-2021 18:27:14 | 03-11-2021 18:27:00|03-11-2021 18:28:00
        
         */

        #endregion Historical Data
    }
}