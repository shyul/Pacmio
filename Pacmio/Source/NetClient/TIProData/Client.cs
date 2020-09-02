/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Trade-Ideas API
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeIdeas.TIProData;
using TradeIdeas.TIProData.Configuration;
using TradeIdeas.ServerConnection;

namespace Pacmio.TIProData
{
    public static partial class Client
    {
        public static ConnectionMaster Connection { get; private set; }

        public static IConnectionFactory ConnectionFactory => Connection.ConnectionBase.ConnectionFactory;

        public static void Connect(string userName, string password) 
        {
            Connection.LoginManager.Username = userName;
            Connection.LoginManager.Password = password;

        }
    }
}
