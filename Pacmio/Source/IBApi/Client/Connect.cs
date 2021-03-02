/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {
        #region TCP Client Settings

        public static string Hostname { get; private set; }

        public static int Port { get; private set; }

        public static int ClientId { get; private set; }

        public static int Timeout { get; private set; }

        public const int Pace = 50;

        public static int ServerVersion { get; private set; }
        public static DateTime ConnectTime { get; private set; }



        private static CancellationTokenSource TaskCancelTs { get; set; }

        private static bool IsCancelled => TaskCancelTs == null || TaskCancelTs.IsCancellationRequested;

        private const int bufferSize = 20 * 1024 * 1024;

        private static TcpClient TcpClient { get; set; }

        #endregion TCP Client Settings

        #region Connect and Disconnect

        public const int MIN_VERSION = ServerVersionHistory.PRICE_MGMT_ALGO;
        public const int MAX_VERSION = ServerVersionHistory.PRICE_MGMT_ALGO;
        public const string ConnectOptions = "";
        private static readonly string VERSION_STR = "v" + MIN_VERSION.ToString() + (MAX_VERSION != MIN_VERSION ? ".." + MAX_VERSION : string.Empty) + ((ConnectOptions.Length == 0) ? string.Empty : " " + ConnectOptions); //"v100..151";

        private static Task ConnectTask { get; set; }

        private static bool IsSocketConnected()
        {
            if (TcpClient is null) return false;
            else
                return TcpClient.Connected;
        }

        public static bool GatewayConnected { get; private set; } = true;

        /// <summary>
        /// usfarm
        /// </summary>
        public static readonly ConcurrentDictionary<string, ConnectionStatus> ServerList = new ConcurrentDictionary<string, ConnectionStatus>();

        public static ConnectionStatus ApiStatus { get; private set; } = ConnectionStatus.Disconnected;

        public static bool Connected => ApiStatus == ConnectionStatus.Connected && GatewayConnected && (!IsCancelled);

        public static void Connect(int clientId = 180, int port = 15062, string address = "127.0.0.1", int timeout = 1000)
        {
            if (ApiStatus == ConnectionStatus.Disconnected || Hostname != address || Port != port || ClientId != clientId)
            {
                ApiStatus = ConnectionStatus.Connecting;

                Hostname = address;
                Port = port;
                ClientId = clientId;
                Timeout = timeout;

                ConnectTask = new Task(() => ConnectWorker());
                ConnectTask.Start();
            }
            else
            {
                Root.NetConnectUpdate(ApiStatus, DateTime.Now, "Connect already established!");
            }
        }

        private static void ConnectWorker()
        {
            Root.NetConnectUpdate(ApiStatus = ConnectionStatus.Connecting, DateTime.Now, "Reset and prepare connectivity resource."); // If it is connected
            ResetAll();
            Root.NetConnectUpdate(ApiStatus = ConnectionStatus.Connecting, DateTime.Now, "Start connect: " + Hostname + ":" + Port); // If it is connected

            TcpClient = new TcpClient()
            {
                ReceiveBufferSize = bufferSize,
                ReceiveTimeout = Timeout,
                SendBufferSize = bufferSize,
                SendTimeout = Timeout,
            };

            try
            {
                TcpClient.Connect(Hostname, Port);
                Root.NetConnectUpdate(ApiStatus = ConnectionStatus.Connecting, DateTime.Now, "TCP Client is connected: " + Hostname + ":" + Port); // If it is connected

                int j = 0;
                while (!IsSocketConnected() && j < Timeout)
                {
                    Thread.Sleep(1);
                    j++;
                }

                if (j < Timeout)
                {
                    TcpReader = new BinaryReader(TcpClient.GetStream());

                    DecodeAsyncMessageTask = new Task(() => DecodeAsyncMessageWorker(), TaskCancelTs.Token);
                    DecodeSequentialMessageTask = new Task(() => DecodeSequentialMessageWorker(), TaskCancelTs.Token);

                    ReceiveTask = new Task(() => ReceiveWorker(), TaskCancelTs.Token);
                    SendTask = new Task(() => SendWorker(), TaskCancelTs.Token);
                    //DataRequestTask = new Task(() => DataRequestTaskWorker(), TaskCancelTs.Token);

                    DecodeAsyncMessageTask.Start();
                    DecodeSequentialMessageTask.Start();

                    ReceiveTask.Start();
                    SendTask.Start();
                    //DataRequestTask.Start();

                    try
                    {
                        // Confirm API version
                        byte[] SendBytes = Encoding.ASCII.GetBytes("API\0" + ToBigEndianString(VERSION_STR.Length) + VERSION_STR);
                        TcpClient.GetStream().Write(SendBytes, 0, SendBytes.Length); // Length = cnt + 4

                        // Start API
                        Send(new string[] { ((int)RequestType.StartApi).ToString(), "2", ClientId.ToString(), string.Empty }); // StartApi version is 2;                    
                    }
                    catch (Exception e) when (e is IOException || e is InvalidOperationException)
                    {
                        Root.NetConnectUpdate(ApiStatus, DateTime.Now, "Send Data Error, disconnecting.");
                        Disconnect.Start();
                    }

                    j = 0;
                    while (j < Timeout)
                    {
                        if (IsRequestIdValid) break;
                        Thread.Sleep(1);
                        j++;
                    }

                    if (j < Timeout)
                    {
                        Root.NetConnectUpdate(ApiStatus = ConnectionStatus.Connected, DateTime.Now, "Connect Success: " + ConnectTime); // If it is connected

                        //SendRequest_AccountSummary();
                        //SendRequest_Postion();
                    }
                }

                if (j > Timeout - 2)
                {
                    Root.NetConnectUpdate(ApiStatus = ConnectionStatus.Disconnected, DateTime.Now, "Timeout. Failed to connect.");
                    ResetAll();
                }
            }
            catch (SocketException e)
            {
                ResetAll();
                Root.NetConnectUpdate(ApiStatus = ConnectionStatus.Disconnected, DateTime.Now, "Socket Error: " + e.Message);
                return;
            }
        }

        public static Task Disconnect => new Task(() =>
        {
            WatchListManager.List.Where(n => n is InteractiveBrokerWatchList).ToList().ForEach(n => { if (n is InteractiveBrokerWatchList wt) wt.Stop(); });

            if (ApiStatus == ConnectionStatus.Connected || ApiStatus == ConnectionStatus.Connecting)
            {
                Root.NetConnectUpdate(ApiStatus = ConnectionStatus.Disconnecting, DateTime.Now);
                ResetAll();
                Root.NetConnectUpdate(ApiStatus = ConnectionStatus.Disconnected, DateTime.Now);
                /*
                foreach(var item in TicksList) 
                {
                    string path = Root.ResourcePath + "HistoricalData\\" + item.Key.Type.ToString() + "\\" + item.Key.Exchange.ToString() + "\\Ticks\\";

                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    File.WriteAllText(path + "$" + item.Key.Name + ".csv", TicksList[item.Key].ToString());
                }*/
            }
        });

        private static void ResetAll()
        {
            IsRequestIdValid = false;

            if (!(TaskCancelTs is null)) TaskCancelTs.Cancel(); // Cancel all tasks
                                                                //OnConnectedHandler?.Invoke(Status = ConnectionStatus.Connecting, DateTime.Now, "TaskCancelTs.Cancel();");

            int i = Timeout + 100;
            //while ((!(DataRequestTask is null)) && DataRequestTask?.Status == TaskStatus.Running && i > 0) { Thread.Sleep(1); i--; }
            //DataRequestTask?.Dispose();

            //i = Timeout + 100;
            while ((!(SendTask is null)) && SendTask?.Status == TaskStatus.Running && i > 0) { Thread.Sleep(1); i--; }
            SendTask?.Dispose();
            //OnConnectedHandler?.Invoke(Status = ConnectionStatus.Connecting, DateTime.Now, "SendTask?.Dispose();");


            i = Timeout + 100;
            while ((!(ReceiveTask is null)) && ReceiveTask?.Status == TaskStatus.Running && i > 0) { Thread.Sleep(1); i--; }
            ReceiveTask?.Dispose();
            //OnConnectedHandler?.Invoke(Status = ConnectionStatus.Connecting, DateTime.Now, "ReceiveTask?.Dispose();");


            i = Timeout + 100;
            while ((!(DecodeAsyncMessageTask is null)) && DecodeAsyncMessageTask?.Status == TaskStatus.Running && i > 0) { Thread.Sleep(1); i--; }
            DecodeAsyncMessageTask?.Dispose();
            // One exception error here.
            //OnConnectedHandler?.Invoke(Status = ConnectionStatus.Connecting, DateTime.Now, "DecodeTask?.Dispose();");

            i = Timeout + 100;
            while ((!(DecodeSequentialMessageTask is null)) && DecodeSequentialMessageTask?.Status == TaskStatus.Running && i > 0) { Thread.Sleep(1); i--; }
            DecodeSequentialMessageTask?.Dispose();

            TcpReader?.Dispose();

            TcpClient?.Close();
            //TcpClient?.Dispose();

            TaskCancelTs?.Dispose();

            lock (ServerList) ServerList.Clear();
            //OnConnectedHandler?.Invoke(Status = ConnectionStatus.Connecting, DateTime.Now, "ServerList.Clear();");

            ClearAllSubscription();

            // Flush Request IDs
            lock (ActiveRequestIds) ActiveRequestIds.Clear();
            // Flush send data
            FlushSendData();
            // Flush received data
            FlushAsyncMessageBuffer();
            FlushSequentialMessageBuffer();

            TaskCancelTs = new CancellationTokenSource();

            RequestId = 0;
            DataRequestID = -1;
            ReceivedMessageCount = 0;


            requestId_HistoricalTick = -1;
        }

        #endregion Connect and Disconnect

        //private static readonly string VERSION_STR = "v100.." + VERSION.ToString(); //"v100..136";

        private static string ToBigEndianString(int input) =>
            new string(new char[] {
            Convert.ToChar((input >> 24) & 0xFF),
            Convert.ToChar((input >> 16) & 0xFF),
            Convert.ToChar((input >> 8) & 0xFF),
            Convert.ToChar(input & 0xFF)});

        public static bool IsWorkHours => WorkHours.IsWorkTime();

        public static readonly WorkHours WorkHours = new WorkHours("Eastern Standard Time", new Dictionary<DayOfWeek, MultiTimePeriod>()
        {
            { DayOfWeek.Monday,     new MultiTimePeriod() { new TimePeriod(new Time(1), new Time(23, 30)) } },
            { DayOfWeek.Tuesday,    new MultiTimePeriod() { new TimePeriod(new Time(1), new Time(23, 30)) } },
            { DayOfWeek.Wednesday,  new MultiTimePeriod() { new TimePeriod(new Time(1), new Time(23, 30)) } },
            { DayOfWeek.Thursday,   new MultiTimePeriod() { new TimePeriod(new Time(1), new Time(23, 30)) } },
            { DayOfWeek.Friday,     new MultiTimePeriod() { new TimePeriod(new Time(1), new Time(22, 30)) } },
            { DayOfWeek.Saturday,   new MultiTimePeriod() { new TimePeriod(new Time(3, 30), new Time(23, 30)) } },
            { DayOfWeek.Sunday,     new MultiTimePeriod() { new TimePeriod(new Time(1), new Time(23, 30)) } }
        });
    }
}
