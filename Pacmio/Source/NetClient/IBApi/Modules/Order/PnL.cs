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
    public static partial class Client
    {



        //Send ReqPnL: (0)"92"-(1)"600065159"-(2)"U1564932"-
        //Received PnL: (0)"94"-(1)"600065159"-(2)"-81.00009719999798"-(3)"6053.1747039"-(4)"0.0"-
        //Received PnL: (0)"94"-(1)"600065159"-(2)"-97.20108764648467"-(3)"6036.9737134535135"-(4)"0.0"-
        private static void Parse_PnL(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
        }

        private static void Parse_PnLSingle(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
        }
    }
}
