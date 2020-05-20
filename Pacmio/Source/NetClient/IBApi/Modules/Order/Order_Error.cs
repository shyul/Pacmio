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
    public partial class Client
    {
        private void ParseError_PlaceOrder(string[] fields)
        {
            int requestId = fields[2].ToInt32(-1);
            string message = fields[4];

            Console.WriteLine("PlaceOrder returned with errors: " + fields.ToStringWithIndex());
            //Console.WriteLine("PlaceOrder Error !!!!!!!!!!!!! " + fields[2] + ": " + message);
            

            if (requestId > -1) RemoveRequest(requestId, false);
        }
    }
}
