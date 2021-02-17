/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

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

        private static string ActiveContractSample { get; set; } = string.Empty;
        private static List<Contract> ActiveContractSampleList { get; } = new List<Contract>();

        private static void SendRequest_ContractSamples(string symbol) // Valid control send
        {
            if (DataRequestReady)
            {
                Console.WriteLine("SendRequest_ContractSamples: " + symbol);

                (int requestId, string requestType) = RegisterRequest(RequestType.RequestMatchingSymbols);

                DataRequestID = requestId;
                ActiveContractSampleList.Clear();
                ActiveContractSample = symbol;

                SendRequest(new string[] {
                    requestType, // 81
                    requestId.Param(),
                    symbol });
            }
        }

        private static void SendCancel_ContractSamples()
        {
            ActiveContractSample = string.Empty;
            RemoveRequest(DataRequestID, true);
            DataRequestID = -1;
        }

        private static void Parse_ContractSamples(string[] fields)
        {
            int requestId = fields[1].ToInt32(-1);

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

                    Console.WriteLine("Sample: " + symbolStr + " | " + exchangeStr + " | " + secTypeCode + " | " + conIdStr);

                    int DerivativeSecTypeCount = fields[pt + 5].ToInt32(0);

                    if (symbolStr == ActiveContractSample) isUnknown = false;

                    if (exchangeStr == "VALUE")
                    {
                        //UnknownContractList.CheckIn(DateTime.Now, symbolStr, secTypeCode, "", "", conIdStr.ToInt32(0), "", "", "_VALUE");

                        var uc = UnknownContractList.CheckIn(symbolStr, "_VALUE", secTypeCode);
                        uc.ConId = conIdStr.ToInt32(0);
                        uc.LastCheckedTime = DateTime.Now;
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
                            ActiveContractSampleList.Add(c);
                            ContractList.GetOrAdd(c);
                            c.UpdateTime = DateTime.Now;
                        }
                        else
                        {
                            var uc = UnknownContractList.CheckIn(symbolStr, "_" + exchangeStr, secTypeCode);
                            uc.ConId = conIdStr.ToInt32(0);
                            uc.LastCheckedTime = DateTime.Now;

                            //UnknownContractList.CheckIn(DateTime.Now, symbolStr, secTypeCode, "", "", conIdStr.ToInt32(0), "", "", "_" + exchangeStr);
                        }
                    }

                    pt = pt + 6 + DerivativeSecTypeCount;
                }
            }

            if (isUnknown || ActiveContractSampleList.Where(n => n.Name == ActiveContractSample).Count() < 1)
            {
                var uc = UnknownContractList.CheckIn(ActiveContractSample);
                uc.LastCheckedTime = DateTime.Now;
            }
            else
            {
                UnknownContractList.Remove(ActiveContractSample);
                /*
                if (UnknownContractList.Contains(ActiveContractSample)) 
                {

          
                }*/
            }


            ActiveContractSample = string.Empty;

            if (requestId > -1) RemoveRequest(requestId, false);
            DataRequestID = -1;
        }
    }
}