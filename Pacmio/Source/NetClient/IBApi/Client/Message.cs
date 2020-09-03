/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
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
        // => new Task(() => ReceiveWork(), taskCancellationTokenSource.Token);
        private static Task ReceiveTask { get; set; }
        // => new BinaryReader(tcpClient.GetStream());
        private static BinaryReader TcpReader { get; set; }
        private static bool IsReceiveDataAvailable => IsSocketConnected() && TcpClient.GetStream().DataAvailable;

        private static readonly ConcurrentQueue<string[]> receiveDataBuffer = new ConcurrentQueue<string[]>();
        private static int ReceiveDataBufferCount => receiveDataBuffer.Count;
        private static bool IsReceiveDataEmpty => receiveDataBuffer.IsEmpty;
        private static void FlushReceiveData() { lock (receiveDataBuffer) while (!IsReceiveDataEmpty) receiveDataBuffer.TryDequeue(out _); } //(out string[] fields); }
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
                        string[] raw = Encoding.ASCII.GetString(TcpReader.ReadBytes(msgSize), 0, msgSize).Trim(MSG_EOL).Split(MSG_EOL);

                        if (ReceivedMessageCount == 0)
                        {
                            // The time would be the local time, example PST for the west coast.
                            ConnectTime = DateTime.ParseExact(raw[1].Substring(0, raw[1].Length - 4), "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);
                            ServerVersion = raw[0].ToInt32(-1);
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
                            receiveDataBuffer.Enqueue(raw);
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
