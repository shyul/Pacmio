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
using System.Reflection;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {

        internal static readonly ConcurrentDictionary<int, Contract> ActiveRealTimeBars = new ConcurrentDictionary<int, Contract>();

        /// <summary>
        /// 5 Seconds Realtime Bars, the period can not be changed.
        /// </summary>
        /// <param name="bt"></param>
        /// <param name="options"></param>
        internal static bool SendRequest_RealTimeBars(Contract c, ICollection<(string, string)> options = null)
        {
            var (valid_exchange, exchangeCode) = ApiCode.GetIbCode(c.Exchange);

            if (Connected && valid_exchange && !ActiveRealTimeBars.Values.Contains(c) && !SubscriptionOverflow)
            {
                (int requestId, string requestType) = RegisterRequest(RequestType.RequestRealTimeBars);
                ActiveRealTimeBars.CheckAdd(requestId, c);

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

                SendRequest(new string[] {
                    requestType,
                    "3", // VERSION
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
                    string.Empty, // LocalSymbol / 12 Judy the name
                    string.Empty, // TradingClass / 13 SCM, NMS
                    "5", // barFreqCode,
                    "TRADES",//barTypeCode, // whatToShow,
                    "0", //bt.IsRegularTradeHoursOnly.Param(), // useRTH
                    options.Param()
                });
                return true;
            }
            return false;
        }

        internal static bool SendCancel_RealTimeBars(int requestId)
        {
            RemoveRequest(requestId, RequestType.RequestRealTimeBars);
            lock (ActiveRealTimeBars)
            {
                ActiveRealTimeBars.TryRemove(requestId, out Contract c);
            }
            // Emit update cancelled.
            return false;
        }

        //         CancelRealTimeBars = 51,  // OK

        private static void Parse_RealTimeBars(string[] fields)
        {
            string msgVersion = fields[1];
            int requestId = fields[2].ToInt32(-1);

            if (msgVersion == "3" && ActiveRealTimeBars.ContainsKey(requestId) && fields.Length == 11)
            {
                Contract c = ActiveRealTimeBars[requestId];

                long epochTime = fields[3].ToInt64(); // This is Zulu (UTC) Time.
                /*
                double open = fields[4].ToDouble();
                double high = fields[4].ToDouble();
                double low = fields[4].ToDouble();
                double close = fields[4].ToDouble();
                long volume = fields[3].ToInt64();
                double wap = fields[4].ToDouble();
                int count = fields[3].ToInt32();
                */
                DateTime time = TimeZoneInfo.ConvertTimeFromUtc(TimeTool.FromEpoch(epochTime), c.TimeZone);
                //Period p = new Period(time, time.AddSeconds(5));

                // Emit BarTable update

                Console.WriteLine(MethodBase.GetCurrentMethod().Name + " | " + time + " : " + fields.ToStringWithIndex());
            }
            else
                SendCancel_RealTimeBars(requestId);



        }
    }
}
