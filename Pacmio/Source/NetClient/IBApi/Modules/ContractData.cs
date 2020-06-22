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
using System.Text;
using Xu;
using Pacmio;

namespace Pacmio.IB
{
    public static partial class Client
    {
        public static bool IsReady_ContractData => Connected && requestId_ContractData == -1;
        private static int requestId_ContractData = -1;
        private static Contract active_ContractData;

        internal static void SendRequest_ContractData(Contract c, bool includeExpired = false)
        {
            var (valid_exchange, exchangeCode) = ApiCode.GetIbCode(c.Exchange);

            if (IsReady_ContractData && valid_exchange)
            {
                (int requestId, string requestType) = RegisterRequest(RequestType.RequestContractData);
                requestId_ContractData = requestId;
                active_ContractData = c;

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

                SendRequest(new string[] {
                    requestType, // 9
                    "8",
                    requestId.ToString(),
                    c.ConId.Param(), // "0"
                    c.Name,
                    c.TypeApiCode,
                    lastTradeDateOrContractMonth,
                    (strike == 0) ? "0" : strike.ToString("0.0###"),
                    right, // Right
                    multiplier, // Multiplier
                    useSmart ? "SMART" : exchangeCode, // "ISLAND" exchange,
                    exchangeCode, // primaryExch,
                    c.CurrencyCode, // currency,
                    string.Empty, // LocalSymbol
                    string.Empty, // TradingClass
                    includeExpired.Param(),
                    string.Empty, // SecIdType
                    string.Empty, // SecId
                });
            }
        }

        internal static void SendRequest_ContractData(int conId, bool includeExpired = false)
        {
            if (IsReady_ContractData)
            {
                (int requestId, string requestType) = RegisterRequest(RequestType.RequestContractData);
                requestId_ContractData = requestId;
                active_ContractData = null;

                SendRequest(new string[] {
                    requestType, // 9
                    "8",
                    requestId.ToString(),
                    conId.Param(), // "0"
                    string.Empty, // c.Name,
                    string.Empty, // c.TypeApiCode,
                    string.Empty, // lastTradeDateOrContractMonth,
                    "0", // (strike == 0) ? "0" : strike.ToString("0.0###"),
                    string.Empty, // Right
                    string.Empty, // Multiplier
                    "SMART", // exchange,
                    string.Empty, // primaryExch,
                    string.Empty, // currency,
                    string.Empty, // LocalSymbol
                    string.Empty, // TradingClass
                    includeExpired.Param(),
                    string.Empty, // SecIdType
                    string.Empty, // SecId
                });
            }
        }

        public static void Cancel_ContractData()
        {
            RemoveRequest(requestId_ContractData, true);
            requestId_ContractData = -1;
        }

        private static void Parse_ContractData(string[] fields) // 10
        {
            int requestId = fields[2].ToInt32(-1);

            if (fields[1] == "8" && requestId == requestId_ContractData)
            {
                Contract c = active_ContractData;

                if(c is null) 
                {
                    // TODO: Add Contract
                
                }

                string symbolStr = fields[3];
                string conIdStr = fields[13];

                //(bool symbolInfoValid, SymbolInfo si) = SymbolList.GetSymbol(symbolStr, exchangeStr, secTypeStr, conIdStr);

                if (c.ConId == conIdStr.ToInt32(-10) && c.Name == symbolStr)
                {
                    c.MarketData.OrderTypes.FromString(fields[17], ',');
                    c.MarketData.ValidExchanges.FromString(fields[18], ',');

                    //si.IndustryInfo.Industry = rawInput[24];
                    //si.IndustryInfo.Category = rawInput[25];
                    //si.IndustryInfo.Subcategory = rawInput[26];

                    // Get ISIN here.
                    int tagNum = fields[32].ToInt32(0);

                    int pt = 33;

                    Dictionary<string, string> tags = new Dictionary<string, string>();

                    for (int i = 0; i < tagNum; i++)
                    {
                        tags.Add(fields[pt], fields[pt + 1]);
                        pt += 2;
                    }

                    if (tags.ContainsKey("ISIN") && c is ITradable it)
                    {
                        it.ISIN = tags["ISIN"];

                        //string isin_header = si.ISIN.Substring(0, 2);
                        //if (isin_header == "US")
                        //   si.CUSIP = si.ISIN.Substring(2, 9);
                    }

                    c.MarketData.MarketRules.FromString(fields[pt + 3], ','); // 38

                    if (c.FullName.Length < 5)
                        c.FullName = fields[21].Replace("\"", "");

                    c.UpdateTime = DateTime.Now;
                }
            }
        }

        private static void Parse_ContractDataEnd(string[] fields) // 52
        {
            int requestId = fields[2].ToInt32(-1);
            if (requestId > -1) RemoveRequest(requestId);
            requestId_ContractData = -1;
        }

        private static void ParseError_ContractData(string[] fields)
        {
            int requestId = fields[2].ToInt32(-1);
            string message = fields[4];

            Console.WriteLine("Symbol Data Error !!!!!!!!!!!!! " + fields[2] + ": " + message);
            if (message.Contains("No security definition has been found for the request"))
            {
                active_ContractData.UpdateTime = DateTime.Now;
            }

            if (requestId > -1) RemoveRequest(requestId);
            requestId_ContractData = -1;
        }
    }
}
