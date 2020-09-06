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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TradeIdeas.TIProData;
using TradeIdeas.TIProData.Configuration;
using TradeIdeas.ServerConnection;
using Xu;

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
            string message = args.message;

            if (message.Contains("Working:  "))
            {
                message = message.Replace("Working:  ", "");
                LastStatusCheckTime = DateTime.Parse(message);
            }

            Connected = !args.isServerDisconnect;
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

        public static string GetValue(this RowData row, ColumnInfo ci)
        {
            if (row.GetAsString(ci.WireName) is string dataString && dataString.Length > 0)
            {
                string format = ci.Format.Trim();

                switch (format)
                {
                    case (""): return dataString;

                    case ("p"):
                        {
                            double d = dataString.ToDouble();
                            if (!double.IsNaN(d))
                            {
                                return (row.GetAsString("four_digits") == "1") ? d.ToString("N4") : d.ToString("N2");
                            }
                            else
                                return null;
                        }
                    default:
                        {
                            double d = dataString.ToDouble();
                            if (!double.IsNaN(d))
                            {
                                int digits = format.ToInt32();

                                if (digits > 7)
                                    digits = 7;
                                else if (digits < 0)
                                    digits = 0;

                                return d.ToString("N" + digits);
                            }
                            else
                                return null;
                        }
                }
            }
            else
                return null;
        }

        public static string GetString(this RowData row, string key) => (row.GetAsString(key) is string value) ? value : null;

        public static int GetInt(this RowData row, string key)
        {
            string s = row.GetString(key);

            if (string.IsNullOrWhiteSpace(s))
                return 0;
            else
                return s.ToInt32();
        }

        public static double GetDouble(this RowData row, string key)
        {
            string s = row.GetString(key);

            if (string.IsNullOrWhiteSpace(s))
                return double.NaN;
            else
                return s.ToDouble();
        }

        public static string GetSymbol(this RowData row, string key = "symbol")
        {
            string symbol = GetString(row, key);

            if (symbol.Length > 0 && Regex.IsMatch(symbol, @"^[a-zA-Z]+$"))
                return symbol;
            else
                return null;
        }

        public static Contract GetContract(this RowData row, string symbolColumnName = "symbol", string exchangeColumnName = "EXCHANGE") 
        {
            if (row.GetSymbol(symbolColumnName) is string symbolName && 
                symbolName.Length > 0 && 
                Regex.IsMatch(symbolName, @"^[a-zA-Z]+$") && 
                GetString(row, exchangeColumnName) is string exchangeCode)
            {
                Exchange exchange = exchangeCode switch
                {
                    "NYSE" => Exchange.NYSE,
                    "NASD" => Exchange.NASDAQ,
                    "AMEX" => Exchange.AMEX,
                    "ARCA" => Exchange.ARCA,
                    "BATS" => Exchange.BATS,
                    "PINK" => Exchange.OTCMKT,
                    //null => Exchange.UNKNOWN,
                    _ => Exchange.UNKNOWN
                };

                var list = ContractList.Fetch(symbolName, exchange);

                if (list.Length == 1) 
                    return list.First();
                else
                {

                    // Add UnknowItem

                }
            }
            else
            {

                // Add UnknowItem
            }






            return null;
        }


        public static (Exchange Exchange, string Code) GetExchange(this RowData row)
        {
            if (GetString(row, "EXCHANGE") is string exCode)
            {
                switch (exCode)
                {
                    case ("NYSE"): return (Exchange.NYSE, exCode);
                    case ("NASD"): return (Exchange.NASDAQ, exCode);
                    case ("AMEX"): return (Exchange.AMEX, exCode);
                    case ("ARCA"): return (Exchange.ARCA, exCode);
                    case ("BATS"): return (Exchange.BATS, exCode);
                    case ("PINK"): return (Exchange.OTCMKT, exCode);

                    default:
                        {
                            Console.WriteLine(">>>>> Unregistered Exchange Code: " + exCode);
                            return (Exchange.UNKNOWN, exCode);
                        }
                }
            }

            return (Exchange.UNKNOWN, "_");
        }
    }
}
