/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Reflection;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {
        public static bool IsReady_AccountUpdateMulti => Connected && !ActiveRequestContains(RequestType.RequestAccountUpdatesMulti);
        //private int RequestIdAccountUpdateMulti = -1;

        public static void SendRequest_AccountUpdateMulti(string account = "", string modelCode = "", bool ledgerAndNLV = false)
        {
            if (IsReady_AccountUpdateMulti)
            {
                (int requestId, string typeStr) = RegisterRequest(RequestType.RequestAccountUpdatesMulti);
                //RequestIdAccountUpdateMulti = requestId;
                SendRequest(new string[] {
                    typeStr, // 62
                    "1",
                    requestId.ToString(),
                    account,
                    modelCode,
                    ledgerAndNLV ? "1" : "0"
                });
            }
        }

        private static void Parse_AccountUpdateMulti(string[] fields)
        {
            Console.WriteLine(fields[3] + " >> Summary Item: " + fields.ToStringWithIndex());
            if (fields[1] == "1")
            {
                /*
                int msgVersion = ReadInt();
                int requestId = ReadInt();
                string account = ReadString();
                string modelCode = ReadString();
                string key = ReadString();
                string value = ReadString();
                string currency = ReadString();
                eWrapper.accountUpdateMulti(requestId, account, modelCode, key, value, currency);
                */
            }
        }
        private static void Parse_AccountUpdateMultiEnd(string[] fields)
        {
            /*
                int msgVersion = ReadInt();
                int requestId = ReadInt();
                eWrapper.accountUpdateMultiEnd(requestId);
            */
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
            if (fields.Length == 3 && fields[1] == "1")
            {
                int reqId = int.Parse(fields[2]);
                RemoveRequest(reqId);
                AccountPositionManager.UpdateTime = DateTime.Now;
            }
        }
    }
}
