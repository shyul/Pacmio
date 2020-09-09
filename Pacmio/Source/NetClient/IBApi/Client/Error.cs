/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {
        private static void Parse_Errors(string[] fields)
        {
            int requestId = fields[2].ToInt32(-1000);
            if (ActiveRequestContains(requestId))
            {
                RequestType type = ActiveRequestIds[requestId];
                switch (type)
                {
                    case RequestType.PlaceOrder:
                        RemoveRequest(requestId, false);
                        ParseError_PlaceOrder(fields);
                        break;

                    case RequestType.RequestContractData:
                        ParseError_ContractData(fields);
                        DataRequestID = -1;
                        break;

                    case RequestType.RequestScannerSubscription:
                        ParseError_ScannerSubscription(fields);
                        break;

                    case RequestType.RequestHistoricalData:
                        ParseError_HistoricalData(fields);
                        DataRequestID = -1;
                        break;

                    case RequestType.RequestHeadTimestamp:
                        ParseError_HistoricalHeadDataTimestamp(fields);
             
                        break;

                    case RequestType.RequestHistoricalTicks:
                        RemoveRequest(requestId);
                        requestId_HistoricalTick = -1;
                        break;

                    case RequestType.RequestMarketData:
                        ParseError_HistoricalTick(fields);
                        break;

                    case RequestType.RequestMarketDepth:
                        ParseError_MarketDepth(fields);
                        break;

                    case RequestType.RequestFundamentalData:
                    default:
                        Console.WriteLine(type + " returned with errors: " + fields.ToStringWithIndex());
                        RemoveRequest(requestId);
                        break;
                }

            }
            else if (requestId == -1)
            {
                switch (fields[3])
                {
                    case ("2103"): // Received Error: (0)"4"-(1)"2"-(2)"-1"-(3)"2103"-(4)"Market data farm connection is broken:usfarm.nj"
                        Console.WriteLine(fields[4]);
                        break;

                    case ("2104"): // 20:54:49:921 -> ---54-2--1-2104-Market data farm connection is OK:usfarm-
                        Console.WriteLine(fields[4]);
                        break;

                    case ("2105"): // Parse Errors: (0)"4"-(1)"2"-(2)"-1"-(3)"2105"-(4)"HMDS data farm connection is broken:ushmds"
                        HistoricalData_Servers.CheckRemove(fields[4].Replace("HMDS data farm connection is broken:", ""));
                        if (HistoricalData_Servers.Count == 0)
                        {
                            RemoveRequest(RequestType.RequestHeadTimestamp);
                            //requestId_HistoricalDataHeadTimestamp = -1;
                            RemoveRequest(RequestType.RequestHistoricalData);
                            //requestId_HistoricalData = -1;
                            DataRequestID = -1;
                            HistoricalData_Connected = false;
                        }
                        break;

                    case ("2106"): // 20:54:49:921 -> ---34-2--1-2106-HMDS data farm connection is OK:ushmds-
                        HistoricalData_Servers.CheckAdd(fields[4].Replace("HMDS data farm connection is OK:", ""));
                        HistoricalData_Connected = true;


                        break;

                    case ("2107"):

                    default:
                        Console.WriteLine("System Info: " + fields.ToStringWithIndex());
                        break;
                }
            }
            else
            {
                // Received Error: (0)"4" - (1)"2" - (2)"20000001" - (3)"2152" - (4)"Exchanges - Depth: BATS; ARCA; ISLAND; BEX; IEX; Top: BYX; AMEX; CHX; NYSENAT; PSX; EDGEA; ISE; DRCTEDGE; Need additional market data permissions - Depth: NYSE; "
                // Received Error: (0)"4" - (1)"2" - (2)"10000001" - (3)"10197" - (4)"No market data during competing live session"

                Console.WriteLine("Unable to locate this request Id: " + requestId + " in the table, maybe this message just tells your something has been properly removed.");
                Console.WriteLine("Parse Errors: " + fields.ToStringWithIndex());
                RemoveRequest(requestId);
            }

        }

        /// <summary>
        /// Error: (0)"4"-(1)"2"-(2)"-1"-(3)"1100"-(4)"Connectivity between IB and Trader Workstation has been lost."
        /// Error: (0)"4"-(1)"2"-(2)"-1"-(3)"1100"-(4)"Connectivity between IB and Trader Workstation has been lost."
        /// Error: (0)"4"-(1)"2"-(2)"-1"-(3)"1102"-(4)"Connectivity between IB and Trader Workstation has been restored - data maintained."
        /// Error: (0)"4"-(1)"2"-(2)"-1"-(3)"2104"-(4)"Market data farm connection is OK:usfarm"
        /// Parse Errors: (0)"4"-(1)"2"-(2)"-1"-(3)"1102"-(4)"Connectivity between IB and Trader Workstation has been restored - data maintained. The following farms are connected: usfarm; secdefnj. The following farms are not connected: ushmds."
        /// </summary>
        /// 
        /*

        Parse Errors: (0)"4"-(1)"2"-(2)"1"-(3)"321"-(4)"Error validating request:-'bN' : cause - The API interface is currently in Read-Only mode."

        20:45:51:394 -> ---74-2--1-2105-HMDS data farm connection is broken:ushmds-
        20:46:11:733 -> ---74-2--1-2105-HMDS data farm connection is broken:ushmds-
        20:46:32:650 -> ---74-2--1-2105-HMDS data farm connection is broken:ushmds-
        20:54:49:920 -> ---15-1-DU332281-
        20:54:49:921 -> ---9-1-6-


        20:54:49:921 -> ---84-2--1-2158-Sec-def data farm connection is OK:secdefnj-


        System Info: (0)"4"-(1)"2"-(2)"-1"-(3)"2107"-(4)"HMDS data farm connection is inactive but should be available upon demand.ushmds"
        System Info: (0)"4"-(1)"2"-(2)"-1"-(3)"2157"-(4)"Sec-def data farm connection is broken:secdefnj"
        Market data farm connection is broken:usfarm
        System Info: (0)"4"-(1)"2"-(2)"-1"-(3)"1100"-(4)"Connectivity between IB and Trader Workstation has been lost."
        System Info: (0)"4"-(1)"2"-(2)"-1"-(3)"2158"-(4)"Sec-def data farm connection is OK:secdefnj"
        Market data farm connection is OK:usfarm
        System Info: (0)"4"-(1)"2"-(2)"-1"-(3)"1102"-(4)"Connectivity between IB and Trader Workstation has been restored - data maintained. The following farms are connected: usfarm; secdefnj. The following farms are not connected: ushmds."
        */
        /*


        Received Error: (0)"4"-(1)"2"-(2)"-1"-(3)"2119"-(4)"Market data farm is connecting:usfarm"
        Received Error: (0)"4"-(1)"2"-(2)"-1"-(3)"2119"-(4)"Market data farm is connecting:usfarm"
        Received Error: (0)"4"-(1)"2"-(2)"-1"-(3)"2119"-(4)"Market data farm is connecting:usfarm"
        Received Error: (0)"4"-(1)"2"-(2)"-1"-(3)"2119"-(4)"Market data farm is connecting:usfarm.nj"

        Parse Errors:   (0)"4"-(1)"2"-(2)"-1"-(3)"2104"-(4)"Market data farm connection is OK:usfarm"
        Received Error: (0)"4"-(1)"2"-(2)"-1"-(3)"2104"-(4)"Market data farm connection is OK:usfarm.nj"
        Received Error: (0)"4"-(1)"2"-(2)"-1"-(3)"2104"-(4)"Market data farm connection is OK:usfarm.nj"
        Received Error: (0)"4"-(1)"2"-(2)"-1"-(3)"2104"-(4)"Market data farm connection is OK:usfarm"
        Received Error: (0)"4"-(1)"2"-(2)"-1"-(3)"2104"-(4)"Market data farm connection is OK:usfarm"
        Received Error: (0)"4"-(1)"2"-(2)"-1"-(3)"2104"-(4)"Market data farm connection is OK:usfarm"

        Received Error: (0)"4"-(1)"2"-(2)"-1"-(3)"2158"-(4)"Sec-def data farm connection is OK:secdefnj"
          Parse Errors: (0)"4"-(1)"2"-(2)"-1"-(3)"2158"-(4)"Sec-def data farm connection is OK:secdefnj"
          Parse Errors: (0)"4"-(1)"2"-(2)"-1"-(3)"1102"-(4)"Connectivity between IB and Trader Workstation has been restored - data maintained. The following farms are connected: usfarm; secdefnj. The following farms are not connected: ushmds."

        Parse Errors: (0)"4"-(1)"2"-(2)"-1"-(3)"2110"-(4)"Connectivity between Trader Workstation and server is broken. It will be restored automatically."
        Parse Errors: (0)"4"-(1)"2"-(2)"-1"-(3)"1100"-(4)"Connectivity between IB and Trader Workstation has been lost."

        public static void ParseServerConnection(string[] fields)
        {
            string[] vals; // = string.Empty;

            // https://interactivebrokers.github.io/tws-api/message_codes.html#gsc.tab=0

            switch (fields[3])
            {
                case ("1100"): // Connectivity between IB and the TWS has been lost.
                    {

                    }
                    break;
                case ("1101"): // Connectivity between IB and TWS has been restored- data lost.*
                    {

                    }
                    break;
                case ("1102"): // Connectivity between IB and TWS has been restored- data maintained.
                    {

                    }
                    break;
                case ("1300"): // TWS socket port has been reset and this connection is being dropped. Please reconnect on the new port - <port_num>
                    {

                    }
                    break;
                case ("2100"): // New account data requested from TWS. API client has been unsubscribed from account data.
                    {

                    }
                    break;
                case ("2101"): // Unable to subscribe to account as the following clients are subscribed to a different account.
                    {

                    }
                    break;
                case ("2102"): // Unable to modify this order as it is still being processed.
                    {

                    }
                    break;
                case ("2103"): // A market data farm is disconnected. // Market data farm connection is broken:usfarm.us
                    {
                        //vals = fields[4].Replace("Market data farm connection is broken:", string.Empty);
                        vals = fields[4].Split(':');
                        if (vals.Length == 2) Servers.CheckRemove(vals[1]);

                        Log.Print("Market data: " + vals[1] + " is disconnected.");
                    }
                    break;
                case ("2104"): // Market data farm connection is OK // Market data farm connection is OK:usfarm.us
                    {
                        //vals = fields[4].Replace("Market data farm connection is OK:", string.Empty);
                        vals = fields[4].Split(':');
                        if (vals.Length == 2) Servers.CheckAdd(vals[1]);

                        Log.Print("Market data: " + vals[1] + " is connected!");
                    }
                    break;
                case ("2105"): // A historical data farm is disconnected.
                    {
                        vals = fields[4].Split(':');
                        if (vals.Length == 2) Servers.CheckRemove(vals[1]);

                        Log.Print("HMDS data: " + vals[1] + " is disconnected.");
                    }
                    break;
                case ("2106"): // A historical data farm is connected. // HMDS data farm connection is OK:
                    {
                        //vals = fields[4].Replace("HMDS data farm connection is OK:", string.Empty);
                        //vals = Regex.Replace(rawInput[4], "[^0-9A-Za-z]", " ").Trim(new char[] { ' ' }).Split(' ').LastOrDefault();
                        vals = fields[4].Split(':');
                        if (vals.Length == 2) Servers.CheckAdd(vals[1]);

                        Log.Print("HMDS data: " + vals[1] + " is connected!");
                    }
                    break;
                case ("2107"): // HMDS data farm connection is inactive but should be available upon demand.fundfarm
                    {
                        //vals = fields[4].Replace("HMDS data farm connection is inactive but should be available upon demand.", string.Empty);
                        //vals = Regex.Replace(rawInput[4], "[^0-9A-Za-z]", " ").Trim(new char[] { ' ' }).Split(' ').LastOrDefault();
                        vals = fields[4].Split('.');
                        if (vals.Length == 2) Servers.CheckRemove(vals[1]);

                        Log.Print(vals[1] + " is idle!");
                    }
                    break;
                case ("2108"): // A market data farm connection has become inactive but should be available upon demand.
                    {

                    }
                    break;
                case ("2109"): // Order Event Warning: Attribute "Outside Regular Trading Hours" is ignored based on the order type and destination. PlaceOrder is now processed.
                    {

                    }
                    break;
                case ("2110"): // Connectivity between TWS and server is broken. It will be restored automatically.
                    {

                    }
                    break;
                case ("2137"): // Cross Side Warning
                    {

                    }
                    break;
                default:
                    break;
            }
        }*/
    }
}
