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
using Xu;

namespace Pacmio.IB
{
    public partial class Client
    {
        /// <summary>
        /// Requests position subscription for account and/or model
        /// Initially all positions are returned, and then updates are returned for any position changes in real time.
        /// </summary>
        public void RequestRequestPostionMulti(string account, string modelCode)
        {
            UpdatedPositions.Clear();
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

        private void Parse_PositionMulti(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
        }

        private void Parse_PositionMultiEnd(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
        }
    }
}