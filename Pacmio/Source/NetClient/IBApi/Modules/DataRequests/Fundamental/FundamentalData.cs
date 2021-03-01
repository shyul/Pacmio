/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Reflection;
using System.Text;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {
        public static void SendRequest_FundamentalData(Contract c, FinancialDataRequestType type)
        {
            if (Connected && c.Exchange.Param() is string exchangeCode)
            {
                (int requestId, string requestType) = RegisterRequest(RequestType.RequestFundamentalData);

                SendRequest(new string[] {
                    requestType,
                    "3", // API Version 3
                    requestId.ToString(),
                    c.ConId.Param(), // ConId
                    c.Name, // EquityFactory.GetEquity(fdk.Key).Symbol,
                    c.TypeCode(),
                    c.SmartExchangeRoute ? "SMART" : exchangeCode, // "ISLAND" exchange,
                    exchangeCode, // primaryExch,
                    c.CurrencyCode, // Currency "USD",
                    string.Empty,
                    type.Param(), // FundamentalRequestType
                });
            }
        }

        private static void Parse_FundamentalData(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());

            StringBuilder sb = new StringBuilder(fields[3]);

            sb.ToFile("B:\\test.xml");
        }
    }
}
