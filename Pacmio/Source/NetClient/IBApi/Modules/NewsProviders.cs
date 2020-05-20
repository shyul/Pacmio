/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// https://interactivebrokers.github.io/tws-api/tick_types.html
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Xu;

namespace Pacmio.IB
{
    public partial class Client
    {
        public readonly ConcurrentDictionary<string, string> NewsProviders = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Send RequestNewsProviders: (0)"85"
        /// </summary>
        /// <param name="value"></param>
        public void RequestNewsProviders() // Valid control send
        {
            if (Connected)
            {
                SendRequest(new string[] { RequestType.RequestNewsProviders.Param() });
            }
        }

        /// <summary>
        /// Received NewsProviders: (0)"85"-(1)"4"-(2)"BRFG"-(3)"Briefing.com General Market Columns"-(4)"BRFUPDN"-(5)"Briefing.com Analyst Actions"-(6)"DJNL"-(7)"Dow Jones Newsletters"-(8)"BZ"-(9)"Benzinga"
        /// </summary>
        /// <param name="fields"></param>
        private void Parse_NewsProviders(string[] fields)
        {
            string msgVersion = fields[1];
            if (msgVersion == "4")
            {
                for (int i = 2; i < fields.Length; i += 2)
                    NewsProviders.CheckAdd(fields[i], fields[i + 1]);
            }
        }
    }
}
