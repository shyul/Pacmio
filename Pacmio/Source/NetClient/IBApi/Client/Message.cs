/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;

using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {
        #region Receive Data

        private static Task ReceiveTask { get; set; }

        private static BinaryReader TcpReader { get; set; }

        private static bool IsReceiveDataAvailable => IsSocketConnected() && TcpClient.GetStream().DataAvailable;

        public static int ReceivedMessageCount { get; private set; } = 0;

        private const int MaximumReceivedMessageSize = 0x00FFFFFF;

        private static void ReceiveWorker()
        {
            while (!IsCancelled)
            {
                if (IsReceiveDataAvailable && IsSocketConnected())
                {
                    int msgSize = IPAddress.NetworkToHostOrder(TcpReader.ReadInt32());

                    if (msgSize > MaximumReceivedMessageSize)
                    {
                        throw new Exception("IBClient Receive Overflow!");
                    }
                    else
                    {
                        string[] fields = Encoding.ASCII.GetString(TcpReader.ReadBytes(msgSize), 0, msgSize).Trim(MSG_EOL).Split(MSG_EOL);

                        if (ReceivedMessageCount == 0)
                        {
                            // The time would be the local time, example PST for the west coast.
                            ConnectTime = DateTime.ParseExact(fields[1].Substring(0, fields[1].Length - 4), "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);
                            ServerVersion = fields[0].ToInt32(-1);
                            Root.NetConnectUpdate(ApiStatus, DateTime.Now, "API Server Ver: " + ServerVersion.ToString());

                            if (ServerVersion < MIN_VERSION)
                            {
                                Root.NetConnectUpdate(ApiStatus = ConnectionStatus.Disconnecting, DateTime.Now, "IB Gateway version: " + ServerVersion.ToString() + " is too old, requiring minimum version: " + MIN_VERSION.ToString());
                                Disconnect.Start();
                                break;
                            }
                        }
                        else
                        {
                            int messageType = fields[0].ToInt32(-1);
                            
                            switch (messageType)
                            {
                                case IncomingMessage.NextValidId:
                                    if (fields.Length == 3 && fields[1] == "1")
                                    {
                                        RequestId = fields[2].ToInt32(0);
                                        while (ActiveRequestIds.ContainsKey(RequestId)) RequestId++;
                                        IsRequestIdValid = true;
                                        Console.WriteLine("NextValidId is: " + RequestId); // TBD
                                    }
                                    break;

                                case IncomingMessage.AccountSummary:
                                case IncomingMessage.AccountSummaryEnd:
                                case IncomingMessage.AccountUpdateMulti:
                                case IncomingMessage.AccountUpdateMultiEnd:
                                case IncomingMessage.ContractData:
                                case IncomingMessage.ContractDataEnd:
                                case IncomingMessage.OpenOrder:
                                case IncomingMessage.OpenOrderEnd:
                                case IncomingMessage.CommissionsReport:
                                case IncomingMessage.ExecutionData:
                                case IncomingMessage.ExecutionDataEnd:
                                case IncomingMessage.CompletedOrder:
                                case IncomingMessage.CompletedOrdersEnd:
                                case IncomingMessage.Position:
                                case IncomingMessage.PositionEnd:
                                case IncomingMessage.PositionMulti:
                                case IncomingMessage.PositionMultiEnd:
                                case IncomingMessage.HistoricalNews:
                                case IncomingMessage.HistoricalNewsEnd:
                                case IncomingMessage.SecurityDefinitionOptionParameter:
                                case IncomingMessage.SecurityDefinitionOptionParameterEnd:
                                    SequentialMessageBuffer.Enqueue(fields);
                                    break;

                                default:
                                    FastMessageBuffer.Enqueue(fields);
                                    break;
                            }
                        }
                    }

                    ReceivedMessageCount++;
                }
                else if (!IsSocketConnected())
                {
                    Root.NetConnectUpdate(ApiStatus, DateTime.Now, "Socket Error, disconnecting.");
                    Disconnect.Start();
                }
                else
                {
                    Thread.Sleep(50);
                }
            }
        }

        #endregion Receive Data
    }
}
