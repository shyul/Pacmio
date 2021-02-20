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
        /// <summary>
        /// Requests position subscription for account and/or model
        /// Initially all positions are returned, and then updates are returned for any position changes in real time.
        /// </summary>
        internal static void SendRequest_PostionMulti(string account, string modelCode)
        {
            if (Connected) // !IsActiveAccountSummary &&
            {
                (int requestId, string typeStr) = RegisterRequest(RequestType.RequestPositionsMulti);

                SendRequest(new string[] {
                    typeStr,
                    "1",
                    requestId.Param(),
                    account,
                    modelCode
                });
            }
        }

        private static void Parse_PositionMulti(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
        }

        private static void Parse_PositionMultiEnd(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
        }
    }
}