/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// https://interactivebrokers.github.io/tws-api/tick_types.html
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
        /// Send RequestHistoricalNews: (0)"86"-(1)"90020000"-(2)"8314"-(3)"BZ+FLY"-(4)"2020-01-28 16:21:07.0"-(5)"2020-01-29 16:21:07.0"-(6)"5"
        /// </summary>
        /// <param name="conId"></param>
        /// <param name="providerCodes"></param>
        public void RequestHistoricalNews(int conId, ICollection<string> providerCodes, Period period, int totalResults, ICollection<(string, string)> historicalNewsOptions = null)
        {
            if (Connected) // && !M_IsActiveSymbolSamples && value != m_ActiveSymbolSample)
            {
                (int requestId, string typeStr) = RegisterRequest(RequestType.RequestHistoricalNews);

                SendRequest(new string[] {
                    typeStr, // 81
                    requestId.Param(),
                    conId.Param(),
                    string.Join(",", providerCodes),
                    period.Start.ToString("yyyy-MM-dd HH:mm:ss.0"),
                    period.Stop.ToString("yyyy-MM-dd HH:mm:ss.0"),
                    totalResults.Param(),
                    historicalNewsOptions.Param()
                }); ;
            }
        }

        /// <summary>
        /// (0)"86"-(1)"90020000"-(2)"2019-10-17 14:46:06.0"-(3)"BRFG"-(4)"BRFG$0bc6e1ef"-(5)"{K:1.00}IBM is wearing a Red Hat but investors are disappointed with Q3 results"
        /// </summary>
        /// <param name="fields"></param>
        private void Parse_HistoricalNews(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());

            int requestId = fields[1].ToInt32(-1);





        }

        /// <summary>
        /// (0)"87"-(1)"90020000"-(2)"1"
        /// </summary>
        /// <param name="fields"></param>
        private void Parse_HistoricalNewsEnd(string[] fields)
        {

        }
    }
}
