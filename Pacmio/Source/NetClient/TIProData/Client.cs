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

        public static Stock GetContract(this RowData row, string symbolColumnName = "symbol", string exchangeColumnName = "EXCHANGE")
        {
            string symbolName = GetString(row, symbolColumnName);

            if (!string.IsNullOrWhiteSpace(symbolName))
            {
                if (symbolName.EndsWith(".U"))
                {
                    symbolName = symbolName.ReplaceEnd(".U", " U");
                }
                else
                {

                }

                if (Regex.IsMatch(symbolName, @"^[a-zA-Z]+$"))
                {
                    string exchangeCode = GetString(row, exchangeColumnName);
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

                    if (exchange != Exchange.UNKNOWN)
                    {
                        bool tried = false;

                    StartLoad:
                        var list = ContractList.GetList(symbolName, exchange).Where(n => n is Stock && n.Status == ContractStatus.Alive && (DateTime.Now - n.UpdateTime).TotalDays < 100);

                        if (list.Count() == 1)
                        {
                            Stock stk = list.First() as Stock;






                            return stk;
                        }
                        else if(!tried)
                        {
                            tried = true;
                            var uc = UnknownContractList.CheckIn(symbolName, exchangeCode);
                            if ((DateTime.Now - uc.LastCheckedTime).TotalDays > 100)
                            {
                                uc.LastCheckedTime = DateTime.Now;
                                var fetchList = ContractList.Fetch(symbolName).Where(n => n is Stock && n.Name == symbolName && n.Exchange == exchange);
                                if (fetchList.Count() > 0)
                                {
                                    UnknownContractList.Remove(uc);
                                    fetchList.ToList().ForEach(n => { if (n is Stock stk && stk.ISIN.Length < 8) ContractList.Fetch(stk); });
                                }

                                goto StartLoad;
                            }
                        }
                    }
                    else
                    {
                        UnknownContractList.CheckIn(symbolName, exchangeCode).LastCheckedTime = DateTime.Now;
                    }
                }
            }

            return null;
        }
    }
}
