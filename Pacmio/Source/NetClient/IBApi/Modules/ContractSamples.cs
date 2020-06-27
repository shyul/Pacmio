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
        /// <summary>
        /// It has to be be waited until you get the result. Or there won't be any return of the result. It won't pop error, just fail silently.
        /// </summary>
        /*
        public static bool IsReady_ContractSamples => Connected && requestId_ContractSamples == -1;
        private static int requestId_ContractSamples = -1;
        */

        private static string active_ContractSample = string.Empty;
        private static readonly List<Contract> active_ContractSamples = new List<Contract>();

        private static void SendRequest_ContractSamples(string symbol) // Valid control send
        {
            if (DataRequestReady)
            {
                Console.WriteLine("SendRequest_ContractSamples: " + symbol);

                (int requestId, string requestType) = RegisterRequest(RequestType.RequestMatchingSymbols);

                DataRequestID = requestId;
                active_ContractSamples.Clear();
                active_ContractSample = symbol;

                SendRequest(new string[] {
                    requestType, // 81
                    requestId.Param(),
                    symbol });
            }
        }

        private static void SendCancel_ContractSamples()
        {
            active_ContractSample = string.Empty;
            RemoveRequest(DataRequestID, true);
            DataRequestID = -1;
        }

        private static void Parse_ContractSamples(string[] fields)
        {
            int requestId = fields[1].ToInt32(-1);

            //if (requestId != DataRequestID) throw new Exception("DataRequestID miss aligned!");

            //if (requestId == DataRequestID)
            //{
            bool isUnknown = true;
            int num = fields[2].ToInt32();

            if (num > 0)
            {
                int pt = 3;
                while (pt < fields.Length)
                {
                    string conIdStr = fields[pt];
                    string symbolStr = fields[pt + 1];
                    string exchangeStr = fields[pt + 3];
                    string secTypeCode = fields[pt + 2]; // STK
                                                         //string currencyStr = fields[pt + 4];
                    int DerivativeSecTypeCount = fields[pt + 5].ToInt32(0);

                    if (symbolStr == active_ContractSample) isUnknown = false;

                    if (exchangeStr == "VALUE")
                    {
                        UnknownItemList.Add(symbolStr, "_" + secTypeCode, "", "", conIdStr.ToInt32(0), "", "", "_VALUE");
                    }
                    else
                    {
                        //Console.WriteLine("Item: " + symbolStr +" | " + exchangeStr + " | " + secTypeStr + " | " + conIdStr);

                        (bool symbolInfoValid, Contract c) = Util.GetContractByIbCode(symbolStr, exchangeStr, secTypeCode, conIdStr);

                        if (symbolInfoValid)
                        {


                            c.MarketData.DerivativeTypes.Clear();// = new List<ContractType>();

                            for (int i = 0; i < DerivativeSecTypeCount; i++)
                            {
                                string derSecTypeCode = fields[pt + 6 + i];
                                c.MarketData.DerivativeTypes.FromString(derSecTypeCode, ',');
                                /// Need to convert derSecTypeStr to ContractType here.
                                /// if (stIsValid && !c.MarketData.DerivativeSecTypes.Contains(st)) c.MarketData.DerivativeSecTypes.Add(st);
                            }

                            //Console.WriteLine("RequestMatching: " + symbolStr + " | " + exchangeStr + " | " + secTypeStr + " | " + DerivativeSecTypeCount.ToString());
                            active_ContractSamples.Add(c);
                            ContractList.GetOrAdd(c);
                        }
                        else
                        {
                            UnknownItemList.Add(symbolStr, "_" + secTypeCode, "", "", conIdStr.ToInt32(0), "", "", "_" + exchangeStr);
                        }
                    }

                    pt = pt + 6 + DerivativeSecTypeCount;
                }

                if (isUnknown)
                    UnknownItemList.Add(active_ContractSample, "");
            }
            //}

            active_ContractSample = string.Empty;

            if (requestId > -1) RemoveRequest(requestId, false);
            DataRequestID = -1;
        }
    }
}