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
        /// Requests TWS's current time.
        /// </summary>
        /// <returns></returns>
        public static bool SendRequest_CurrentTime()
        {
            if (IsReady_MktDepthExchanges)
            {
                IsReady_MktDepthExchanges = false;

                SendRequest(new string[] {
                    ((int)RequestType.RequestCurrentTime).ToString(), // 62
                    "1"
                });

                return true;
            }
            return false;
        }

        /// <summary>
        /// Requests the next valid order ID at the current moment.
        /// </summary>
        /// <returns></returns>
        public static bool SendRequest_Ids(int numIds)
        {
            if (IsReady_MktDepthExchanges)
            {
                IsReady_MktDepthExchanges = false;

                SendRequest(new string[] {
                    ((int)RequestType.RequestIds).ToString(), // 62
                    "1",
                    numIds.Param(),
                });

                return true;
            }
            return false;
        }
    }
}
