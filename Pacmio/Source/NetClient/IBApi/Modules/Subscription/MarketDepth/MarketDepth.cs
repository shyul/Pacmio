/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Reflection;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {
        /// <summary>
        /// Maximum Request: 3
        /// </summary>
        public static ConcurrentDictionary<int, MarketDepth> ActiveMarketDepth { get; } = new ConcurrentDictionary<int, MarketDepth>();

        /// <summary>
        /// Send MarketDepth: (0)"10"-(1)"5"-(2)"20000001"-(3)"0"-(4)"FB"-(5)"STK"-(6)""-(7)"0"-(8)""-(9)""-(10)"SMART"-(11)"ISLAND"-(12)"USD"-(13)""-(14)""-(15)"20"-(16)"1"
        /// Exchanges - Depth: BATS; ARCA; ISLAND; BEX; IEX; 
        ///             Top: BYX; AMEX; CHX; NYSENAT; PSX; EDGEA; ISE; DRCTEDGE;
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        internal static bool SendRequest_MarketDepth(this MarketDepth mdt)
        {
            Contract c = mdt.Contract;

            if (Connected && c.Exchange.Param() is string exchangeCode)
            {
                (int requestId, string requestType) = RegisterRequest(RequestType.RequestMarketDepth);

                mdt.RequestId = requestId;
                mdt.StartTime = DateTime.Now;

                ActiveMarketDepth.TryAdd(requestId, mdt);

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
                    "5",
                    requestId.ParamPos(),
                    c.ConId.Param(),
                    c.Name,
                    c.TypeCode(),
                    lastTradeDateOrContractMonth,
                    (strike == 0) ? "0" : strike.ToString("0.0###"),
                    right, // Right
                    multiplier, // Multiplier
                    c.SmartExchangeRoute ? "SMART" : exchangeCode, // "ISLAND" exchange,
                    exchangeCode, // primaryExch,
                    c.CurrencyCode, // USD / currency,
                    string.Empty, // LocalSymbol
                    string.Empty, // TradingClass
                    mdt.NumberOfRows.ParamPos(),
                    mdt.IsSmartDepth.Param(),
                    mdt.IbOptions.Param()
                };

                SendRequest(paramsList);
                return true;
            }
            return false;
        }

        public static MarketDepth SendCancel_MarketDepth(int requestId)
        {
            if (requestId > 0 && ActiveMarketDepth.ContainsKey(requestId))
            {
                RemoveRequest(requestId, true);
                ActiveMarketDepth.TryRemove(requestId, out MarketDepth mdt);
                if (mdt is not null) mdt.RequestId = -1;
                return mdt;
            }
            else
                return null;
        }

        private static void Parse_MarketDepth(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
        }

        private static void Parse_MarketDepthL2(string[] fields)
        {
            string msgVersion = fields[1];
            int requestId = fields[2].ToInt32(-1);

            if (msgVersion == "1" && ActiveMarketDepth.ContainsKey(requestId)) 
            {
                if (ActiveMarketDepth[requestId] is MarketDepth mdt)
                {
                    int depth = fields[3].ToInt32(-1);
                    MarketDepthDatum md = mdt[depth];

                    if (fields[6] == "0") 
                    {
                        md.AskExchangeCode = fields[4];
                        md.Ask = fields[7].ToDouble();
                        md.AskSize = fields[8].ToDouble() * 100;
                        md.AskTime = DateTime.Now;
                        md.AskIsSmartDepth = fields[9] == "1";
                        md.AskOperation = fields[5].ToInt32();
                    }
                    else
                    {
                        md.BidExchangeCode = fields[4];
                        md.Bid = fields[7].ToDouble();
                        md.BidSize = fields[8].ToDouble() * 100;
                        md.BidTime = DateTime.Now;
                        md.BidIsSmartDepth = fields[9] == "1";
                        md.BidOperation = fields[5].ToInt32();
                    }

                    mdt.Updated();
                }
            }
            //Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
        }

        /// <summary>
        /// Request Market Depth fatal errors: (0)"4"-(1)"2"-(2)"4"-(3)"309"-(4)"Max number (3) of market depth requests has been reached"
        /// RequestMarketDepth returned with errors: (0)"4"-(1)"2"-(2)"1"-(3)"2152"-(4)"Exchanges - Depth: BATS; ARCA; ISLAND; BEX; IEX; Top: BYX; AMEX; CHX; NYSENAT; PSX; EDGEA; ISE; DRCTEDGE; Need additional market data permissions - Depth: NYSE; LTSE; "
        /// </summary>
        /// <param name="fields"></param>
        private static void ParseError_MarketDepth(string[] fields)
        {
            if (fields[3] == "2152")
            {
                Console.WriteLine("Request Market Depth: " + fields[4]);
            }
            else
            {
                int requestId = fields[2].ToInt32(-1);
                RemoveRequest(requestId, false);
                ActiveMarketDepth.TryRemove(requestId, out MarketDepth mdt);
                Console.WriteLine("Request Market Depth fatal errors: " + fields.ToStringWithIndex());
            }
        }
    }
}

/*
Received MarketDepthL2: (0)"13"-(1)"1"-(2)"20000001"-(3)"0"-(4)"AMEX"-(5)"0"-(6)"1"-(7)"213.50"-(8)"1"-(9)"1"
Received MarketDepthL2: (0)"13"-(1)"1"-(2)"20000001"-(3)"0"-(4)"AMEX"-(5)"0"-(6)"0"-(7)"220.77"-(8)"2"-(9)"1"
Received Error: (0)"4"-(1)"2"-(2)"20000001"-(3)"2152"-(4)"Exchanges - Depth: BATS; ARCA; ISLAND; BEX; IEX; Top: BYX; AMEX; CHX; NYSENAT; PSX; EDGEA; ISE; DRCTEDGE; Need additional market data permissions - Depth: NYSE; "
Received MarketDepthL2: (0)"13"-(1)"1"-(2)"20000001"-(3)"1"-(4)"AMEX"-(5)"0"-(6)"1"-(7)"213.50"-(8)"1"-(9)"1"
Received MarketDepthL2: (0)"13"-(1)"1"-(2)"20000001"-(3)"2"-(4)"IEX"-(5)"0"-(6)"1"-(7)"213.50"-(8)"3"-(9)"1"
Received MarketDepthL2: (0)"13"-(1)"1"-(2)"20000001"-(3)"3"-(4)"IEX"-(5)"0"-(6)"1"-(7)"210.00"-(8)"1"-(9)"1"
Received MarketDepthL2: (0)"13"-(1)"1"-(2)"20000001"-(3)"4"-(4)"IEX"-(5)"0"-(6)"1"-(7)"180.00"-(8)"1"-(9)"1"

MarketDepth: [AAPL] STOCK USD @ NASDAQ
Send RequestMarketDepth: (0)"10"-(1)"5"-(2)"1"-(3)"265598"-(4)"AAPL"-(5)"STK"-(6)""-(7)"0"-(8)""-(9)""-(10)"SMART"-(11)"ISLAND"-(12)"USD"-(13)""-(14)""-(15)"20"-(16)"1"-(17)""
RequestMarketDepth returned with errors: (0)"4"-(1)"2"-(2)"1"-(3)"2152"-(4)"Exchanges - Depth: BATS; ARCA; ISLAND; BEX; IEX; Top: BYX; AMEX; CHX; NYSENAT; PSX; EDGEA; ISE; DRCTEDGE; Need additional market data permissions - Depth: NYSE; LTSE; "
12:20:37 Removing RequestMarketDepth and Cancel: 1 | CancelMarketDepth
*/
