/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// 1. https://interactivebrokers.github.io/tws-api/smart_components.html
/// 2. https://interactivebrokers.github.io/tws-api/classIBApi_1_1EClient.html#aa5de1b3f68e143c6b10d5eda83d2ffe3
/// 
/// ***************************************************************************

using System;
using System.Reflection;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {
        private static void Parse_SmartComponents(string[] fields)
        {
            Console.WriteLine(MethodBase.GetCurrentMethod().Name + ": " + fields.ToStringWithIndex());
        }
    }
}
