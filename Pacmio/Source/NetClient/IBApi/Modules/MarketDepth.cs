/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
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
        /// <summary>
        /// Send MarketDepth: (0)"10"-(1)"5"-(2)"20000001"-(3)"0"-(4)"FB"-(5)"STK"-(6)""-(7)"0"-(8)""-(9)""-(10)"SMART"-(11)"ISLAND"-(12)"USD"-(13)""-(14)""-(15)"20"-(16)"1"
        /// Exchanges - Depth: BATS; ARCA; ISLAND; BEX; IEX; 
        ///             Top: BYX; AMEX; CHX; NYSENAT; PSX; EDGEA; ISE; DRCTEDGE;
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        internal static bool SendRequest_MarketDepth(Contract c, int numRows = 20, bool isSmartDepth = true, ICollection<(string, string)> options = null)
        {
            var (valid_exchange, exchangeCode) = ApiCode.GetIbCode(c.Exchange);

            if (Connected && valid_exchange)
            {
                (int requestId, string requestType) = RegisterRequest(RequestType.RequestMarketDepth);

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
                    "5",
                    requestId.ParamPos(),
                    c.ConId.Param(),
                    c.Name,
                    c.TypeCode(),
                    lastTradeDateOrContractMonth,
                    (strike == 0) ? "0" : strike.ToString("0.0###"),
                    right, // Right
                    multiplier, // Multiplier
                    useSmart ? "SMART" : exchangeCode, // "ISLAND" exchange,
                    exchangeCode, // primaryExch,
                    c.CurrencyCode, // USD / currency,
                    string.Empty, // LocalSymbol
                    string.Empty, // TradingClass
                    numRows.ParamPos(),
                    isSmartDepth.Param(),
                    options.Param()
                };

                SendRequest(paramsList);
                return true;
            }
            return false;
        }


        private static void Parse_MarketDepth(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
        }

        private static void Parse_MarketDepthL2(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
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
*/
