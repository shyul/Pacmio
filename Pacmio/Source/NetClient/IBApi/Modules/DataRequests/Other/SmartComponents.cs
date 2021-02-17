/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// 1. https://interactivebrokers.github.io/tws-api/smart_components.html
/// 2. https://interactivebrokers.github.io/tws-api/classIBApi_1_1EClient.html#aa5de1b3f68e143c6b10d5eda83d2ffe3
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
        internal static bool IsReady_SmartComponents => Connected && !ActiveRequestContains(RequestType.RequestSmartComponents);
        internal static string CurrentSmartComponentsList { get; private set; }

        // RequestSmartComponents = 83,
        // Received TickReqParams: (0)"81"-(1)"10000001"-(2)"0.01"-(3)"9c0001"-(4)"3"
        // Received TickPrice: (0)"1"-(1)"6"-(2)"10000001"-(3)"9"-(4)"271.16"-(5)"0"-(6)"0"
        // Received TickPrice: (0)"1"-(1)"6"-(2)"10000001"-(3)"1"-(4)"-1.00"-(5)"0"-(6)"1"
        // Received TickPrice: (0)"1"-(1)"6"-(2)"10000001"-(3)"2"-(4)"-1.00"-(5)"0"-(6)"1"
        // Send RequestSmartComponents: (0)"§83"-(1)"2073336869"-(2)"9c0001"
        // Received Error: (0)"4"-(1)"2"-(2)"2073336869"-(3)"321"-(4)"Error validating request.-'cb' : cause - Invalid BBO exchange/security type code"

        public static bool SendRequest_SmartComponents(string bboExchange = "9c0001")
        {
            if (IsReady_SmartComponents)
            {
                (int requestId, string requestType) = RegisterRequest(RequestType.RequestSmartComponents);
                CurrentSmartComponentsList = bboExchange;

                SendRequest(new string[] {
                    requestType, // 62
                    requestId.ToString(),
                    bboExchange
                });

                return true;
            }
            return false;
        }

        // Send RequestSmartComponents: (0)"83"-(1)"1"-(2)"9c0001"
        // Parse_SmartComponents: (0)"82"-(1)"1"-(2)"16"-(3)"0"-(4)"AMEX"-(5)"A"-(6)"1"-(7)"BEX"-(8)"B"-(9)"2"-(10)"NYSENAT"-(11)"C"-(12)"3"-(13)"FINRA"-(14)"D"-(15)"4"-(16)"ISE"-(17)"I"-(18)"5"-(19)"EDGEA"-(20)"J"-(21)"6"-(22)"DRCTEDGE"-(23)"K"-(24)"7"-(25)"CHX"-(26)"M"-(27)"8"-(28)"ARCA"-(29)"P"-(30)"9"-(31)"ISLAND"-(32)"Q"-(33)"10"-(34)"IEX"-(35)"V"-(36)"12"-(37)"PSX"-(38)"X"-(39)"13"-(40)"BYX"-(41)"Y"-(42)"14"-(43)"BATS"-(44)"Z"-(45)"15"-(46)"NYSE"-(47)"N"-(48)"16"-(49)"LTSE"-(50)"L"

        // Send RequestSmartComponents: (0)"83"-(1)"1"-(2)"a60001"
        // Parse_SmartComponents: (0)"82"-(1)"1"-(2)"16"-(3)"0"-(4)"AMEX"-(5)"A"-(6)"1"-(7)"BEX"-(8)"B"-(9)"2"-(10)"NYSENAT"-(11)"C"-(12)"3"-(13)"NYSE"-(14)"N"-(15)"4"-(16)"ISE"-(17)"I"-(18)"5"-(19)"EDGEA"-(20)"J"-(21)"6"-(22)"DRCTEDGE"-(23)"K"-(24)"7"-(25)"LTSE"-(26)"L"-(27)"8"-(28)"CHX"-(29)"M"-(30)"9"-(31)"ARCA"-(32)"P"-(33)"10"-(34)"ISLAND"-(35)"T"-(36)"11"-(37)"IEX"-(38)"V"-(39)"13"-(40)"PSX"-(41)"X"-(42)"14"-(43)"BYX"-(44)"Y"-(45)"15"-(46)"BATS"-(47)"Z"-(48)"17"-(49)"FINRA"-(50)"D"
        private static void Parse_SmartComponents(string[] fields)
        {
            int requestId = fields[1].ToInt32(-1);
            RemoveRequest(requestId, false); // false means the task is ended with success
            int num = fields[2].ToInt32();

            lock (Parameters)
            {
                Dictionary<string, string> list = Parameters.GetSmartComponents(CurrentSmartComponentsList);
                list.Clear();

                for (int i = 0; i < num; i++)
                {
                    int pt = (i * 3) + 3;
                    int sn = fields[pt].ToInt32();
                    string exchangeCode = fields[pt + 1];
                    string exchangeLetter = fields[pt + 2];
                    list[exchangeLetter] = exchangeCode;
                    Console.WriteLine(exchangeLetter + ": " + exchangeCode);
                }
            }
            //Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
        }
    }


}
