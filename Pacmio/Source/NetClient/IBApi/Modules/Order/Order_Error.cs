/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Globalization;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Xu;
using Pacmio;

namespace Pacmio.IB
{
    public static partial class Client
    {
        private static void ParseError_PlaceOrder(string[] fields)
        {
            int orderId = fields[2].ToInt32(-1);
            string code = fields[3];
            string message = fields[4];

            if(code == "2102" || code == "2109") 
            {
                Console.WriteLine(">>>>> PlaceOrder Warning: " + message);
            }
            else
            {
                Console.WriteLine(">>>>> PlaceOrder Error: " + fields.ToStringWithIndex());
                if (orderId > -1) RemoveRequest(orderId, false);
            }
        }
    }
}
