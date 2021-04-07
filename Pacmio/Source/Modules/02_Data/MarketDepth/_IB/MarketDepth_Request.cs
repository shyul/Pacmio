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

            if (mdt.RequestId == -1 && Connected && c.Exchange.Param() is string exchangeCode)
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

                List<string> paramsList = new() {
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
    }
}
