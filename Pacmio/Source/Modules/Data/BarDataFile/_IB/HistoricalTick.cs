/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {
        public static bool IsReady_HistoricalTick => Connected && requestId_HistoricalTick == -1;
        private static int requestId_HistoricalTick = -1;
        private static Contract active_HistoricalTick;
        public static DateTime LastTime_HistoricalTick { get; private set; }

        public static void Request_HistoricalTick(Contract c, Period pd)
        {
            if (!TickList.ContainsKey(c)) TickList[c] = new List<(DateTime time, double price, double size)>();

            if (TickList[c].Count > 0)
            {
                // Save the existing ticks
            }

            while (!IsReady_HistoricalTick)
            {
                Thread.Sleep(10);
                // Handle Time out here.
            }

            DateTime time = pd.Start;

            while (LastTime_HistoricalTick < pd.Stop)
            {
                SendRequest_HistoricalTick(c, time);
                while (!IsReady_HistoricalTick)
                {
                    Thread.Sleep(10); // Handle Time out here.
                }

                time = LastTime_HistoricalTick.AddSeconds(1);
            }
        }

        internal static void SendRequest_HistoricalTick(Contract c, DateTime startTime, DataType dataType = DataType.Trades,
            int numberOfTicks = 1000, bool useRTH = true, bool ignoreSize = false, bool includeExpired = false,
            ICollection<(string, string)> options = null)
        {
            if (IsReady_HistoricalTick && 
                dataType.Param() is string barTypeCode && 
                c.Exchange.Param() is string exchangeCode)
            {
                (int requestId, string requestType) = RegisterRequest(RequestType.RequestHistoricalTicks);
                requestId_HistoricalTick = requestId;
                active_HistoricalTick = c;

                startTime = TimeZoneInfo.ConvertTimeFromUtc(TimeZoneInfo.ConvertTimeToUtc(startTime, c.TimeZone), TimeZoneInfo.Local);

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
                    "", // c.LocalSymbol, // 12
                    "", // c.TradingClass, // 13
                    includeExpired.Param(),

                    startTime.ToString("yyyyMMdd HH:mm:ss"), // Start of Time,
                    string.Empty, // End Time
                    numberOfTicks.Param(),
                    barTypeCode, // whatToShow,
                    useRTH.Param(),
                    ignoreSize.Param(),
                    options.Param()
                };

                while ((DateTime.Now - LastDataRequestTime).TotalSeconds < 3) { Thread.Sleep(200); }
                SendRequest(paramsList);
                LastDataRequestTime = DateTime.Now;
            }
        }

        private static void Parse_HistoricalTick(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
        }

        private static void Parse_HistoricalTickBidAsk(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
        }

        private static void Parse_HistoricalTickLast(string[] fields)
        {
            int requestId = fields[1].ToInt32(-1);
            int num = fields[2].ToInt32();
            if (requestId == requestId_HistoricalTick)
            {
                Contract c = active_HistoricalTick;
                int pt = 3;
                for (int i = 0; i < num; i++)
                {
                    long epochSec = fields[pt].ToInt64();
                    DateTime time = TimeZoneInfo.ConvertTimeFromUtc(TimeTool.Epoch.AddSeconds(epochSec), c.TimeZone);
                    //int mask = fields[pt + 1].ToInt32(); // Unreported | PastLimit
                    double last = fields[pt + 2].ToDouble();
                    double vol = fields[pt + 3].ToDouble();
                    string exchange = fields[pt + 4];
                    string speical_condition = fields[pt + 5];

                    InboundTick(c, time, last, vol);
                    Console.WriteLine(c.Name + " | " + time.ToString("yyyyMMdd HH:mm:ss") + " : " + last + " / " + vol + " / " + exchange + " / " + speical_condition);

                    if (i == num - 1) LastTime_HistoricalTick = time;

                    pt += 6;
                }
                Console.WriteLine(c.Name + " | Ticks are ended");
            }
            requestId_HistoricalTick = -1;
        }

        public static void InboundTick(Contract c, DateTime time, double price, double size) => TickList[c].Add((time, price, size));

        private static readonly ConcurrentDictionary<Contract, List<(DateTime time, double price, double size)>> TickList = new ConcurrentDictionary<Contract, List<(DateTime time, double price, double size)>>();


    }
}
