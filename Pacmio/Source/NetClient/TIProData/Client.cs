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

        public static TimeSpan Ping { get; private set; }

        public static DateTime LastStatusCheckTime { get; private set; } = DateTime.MinValue;

        public static TimeSpan LastStatusCheck => (DateTime.Now - LastStatusCheckTime);

        public static AccountStatus AccountStatus { get; private set; }

        public static DateTime? NextPayment { get; private set; }

        public static int OddsmakerAvailable { get; private set; }

        public static bool Connected { get; private set; }

        public static TcpIpConnectionFactory TcpIpConnectionFactory { get; private set; }

        //public static IConnection TcpIpConnection { get; private set; }

        public static void Connect()
        {
            Connected = false;
            Connection = new ConnectionMaster();
            Connection.PingManager.PingUpdate += PingUpdate_Handler;
            Connection.LoginManager.AccountStatusUpdate += AccountStatusUpdate_Handler;
            Connection.ConnectionBase.ConnectionStatusUpdate += ConnectionBaseConnectionStatusUpdate_Handler;
            Connection.ConnectionBase.Preview += ConnectionBasePreview_Handler;
            Connection.LoginManager.Username = Root.Settings.TIUsername;
            Connection.LoginManager.Password = Root.Settings.TIPassword;
            Connection.ConnectionBase.ConnectionFactory = TcpIpConnectionFactory = new TcpIpConnectionFactory(Root.Settings.TIServerAddress, Root.Settings.TIServerPort);
            /*
            TcpIpConnectionFactory.Address = Root.Settings.TIServerAddress;
            TcpIpConnectionFactory.Port = Root.Settings.TIServerPort;
            TcpIpConnection = TcpIpConnectionFactory.CreateConnection();*/
        }

        public static void Disconnect()
        {
            if (TcpIpConnectionFactory is TcpIpConnectionFactory icf)
            {
                Connected = false;
                Connection?.Dispose();
                //icf.CreateConnection(null).Disconnect();
            }
        }

        public static ConnectionMaster Connection { get; private set; } 

        private static void PingUpdate_Handler(TimeSpan ping)
        {
            Ping = ping;

            Console.WriteLine("[TIProData] Ping: " + Ping.TotalMilliseconds.ToString() + "ms");
        }

        private static void AccountStatusUpdate_Handler(LoginManager source, AccountStatusArgs args)
        {
            AccountStatus = args.accountStatus;
            NextPayment = args.nextPayment;
            OddsmakerAvailable = args.oddsmakerAvailable;

            Console.WriteLine("[TIProData] Account Status:  " + AccountStatus.ToString() + " | Next Payment: " + ((NextPayment is DateTime pt) ? pt.ToString() : "Pay Now")
                + " | OddsMaker Remaining: " + OddsmakerAvailable.ToString());
        }

        private static void ConnectionBaseConnectionStatusUpdate_Handler(ConnectionBase source, ConnectionStatusCallbackArgs args)
        {
            Connected = !args.isServerDisconnect;
            string message = args.message;

            if (message.Contains("Working:  "))
            {
                message = message.Replace("Working:  ", "");
                LastStatusCheckTime = DateTime.Parse(message);
            }

            Console.WriteLine("[TIProData] Status: " + Connected + " | LastStatusCheck: " + LastStatusCheck.TotalSeconds.ToString() + " secs ago | Message: " + args.message);
        }

        private static void ConnectionBasePreview_Handler(ConnectionBase source, PreviewArgs args)
        {
            //Console.WriteLine("GoodMessage: " + args.goodMessage.ToString());
            /*
            if (args.goodMessage)
            {
                Console.WriteLine(Encoding.ASCII.GetString(args.messageBody));
            }*/
        }
    }
}
