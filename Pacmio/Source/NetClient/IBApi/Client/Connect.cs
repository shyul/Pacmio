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
        public Client(int clientId = 180, int port = 15062, string address = "127.0.0.1", int timeout = 1000)
        {
            Hostname = address;
            Port = port;
            ClientId = clientId;
            Timeout = timeout;

            OnConnectedHandler += (ConnectionStatus status, DateTime time, string msg) => { Console.WriteLine("IBClient [ " + time.ToString("HH:mm:ss") + " - " + status.ToString() + " ]: " + msg); };
        }

        #region TCP Client Settings

        public string Hostname { get; private set; }

        public int Port { get; private set; }

        public int ClientId { get; private set; }

        public int Timeout { get; private set; }

        public int Pace { get; private set; } = 50;

        public int ServerVersion { get; private set; }
        public DateTime ConnectTime { get; private set; }

        public event ConnectionStatusEventHandler OnConnectedHandler;

        private CancellationTokenSource TaskCancelTs { get; set; }

        private bool IsCancelled => TaskCancelTs == null || TaskCancelTs.IsCancellationRequested;

        private const int bufferSize = 20 * 1024 * 1024;

        private TcpClient TcpClient { get; set; }

        #endregion TCP Client Settings

        #region Connect and Disconnect

        public const int MIN_VERSION = ServerVersionHistory.PRICE_MGMT_ALGO;
        public const int MAX_VERSION = ServerVersionHistory.PRICE_MGMT_ALGO;
        public const string ConnectOptions = "";
        private readonly string VERSION_STR = "v" + MIN_VERSION.ToString() + (MAX_VERSION != MIN_VERSION ? ".." + MAX_VERSION : string.Empty) + ((ConnectOptions.Length == 0) ? string.Empty : " " + ConnectOptions); //"v100..151";

        private Task ConnectTask { get; set; }

        private bool IsSocketConnected() 
        {
            if (TcpClient is null) return false;
            else
                return TcpClient.Connected;
        }

        public bool GatewayConnected { get; private set; } = true;

        /// <summary>
        /// usfarm
        /// </summary>
        public readonly ConcurrentDictionary<string, ConnectionStatus> ServerList = new ConcurrentDictionary<string, ConnectionStatus>();

        public ConnectionStatus ApiStatus { get; private set; } = ConnectionStatus.Disconnected;

        public bool Connected => ApiStatus == ConnectionStatus.Connected && GatewayConnected;

        public void Connect(int clientId = 180, int port = 15062, string address = "127.0.0.1", int timeout = 1000)
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
                OnConnectedHandler?.Invoke(ApiStatus, DateTime.Now, "Connect already established!");
            }
        }

        private void ConnectWorker()
        {
            OnConnectedHandler?.Invoke(ApiStatus = ConnectionStatus.Connecting, DateTime.Now, "Reset and prepare connectivity resource."); // If it is connected
            ResetAll();
            OnConnectedHandler?.Invoke(ApiStatus = ConnectionStatus.Connecting, DateTime.Now, "Start connect: " + Hostname + ":" + Port); // If it is connected

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
                OnConnectedHandler?.Invoke(ApiStatus = ConnectionStatus.Connecting, DateTime.Now, "TCP Client is connected: " + Hostname + ":" + Port); // If it is connected

                int j = 0;
                while (!IsSocketConnected() && j < Timeout)
                {
                    Thread.Sleep(1);
                    j++;
                }

                if (j < Timeout)
                {
                    TcpReader = new BinaryReader(TcpClient.GetStream());

                    DecodeTask = new Task(() => DecodeWorker(), TaskCancelTs.Token);
                    ReceiveTask = new Task(() => ReceiveWorker(), TaskCancelTs.Token);
                    SendTask = new Task(() => SendWorker(), TaskCancelTs.Token);

                    DecodeTask.Start();
                    ReceiveTask.Start();
                    SendTask.Start();

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
                        OnConnectedHandler?.Invoke(ApiStatus, DateTime.Now, "Send Data Error, disconnecting.");
                        Disconnect.Start();
                    }

                    j = 0;
                    while (j < Timeout)
                    {
                        if (IsRequestIdValid) break;
                        Thread.Sleep(1);
                        j++;
                    }
                    /*
                    if (IsReady_AccountSummary)
                        SendRequest_AccountSummary();

                    j = 0;
                    while (j < Timeout)
                    {
                        if (IsReady_AccountSummary) break;
                        Thread.Sleep(1);
                        j++;
                    }*/

                    if (j < Timeout)
                    {


                        OnConnectedHandler?.Invoke(ApiStatus = ConnectionStatus.Connected, DateTime.Now, "Connect Success: " + ConnectTime); // If it is connected
                    }
                }

                if (j > Timeout - 2)
                {
                    OnConnectedHandler?.Invoke(ApiStatus = ConnectionStatus.Disconnected, DateTime.Now, "Timeout. Failed to connect.");
                    ResetAll();
                }
            }
            catch (SocketException e) 
            {
                ResetAll();
                OnConnectedHandler?.Invoke(ApiStatus = ConnectionStatus.Disconnected, DateTime.Now, "Socket Error: " + e.Message);
                return;
            }
        }

        public Task Disconnect => new Task(() =>
        {
            ScannerManager.CancelAll();
            if (ApiStatus == ConnectionStatus.Connected || ApiStatus == ConnectionStatus.Connecting)
            {
                OnConnectedHandler?.Invoke(ApiStatus = ConnectionStatus.Disconnecting, DateTime.Now);
                ResetAll();
                OnConnectedHandler?.Invoke(ApiStatus = ConnectionStatus.Disconnected, DateTime.Now);
                /*
                foreach(var item in TicksList) 
                {
                    string path = Root.ResourcePath + "HistoricalBars\\" + item.Key.Type.ToString() + "\\" + item.Key.Exchange.ToString() + "\\Ticks\\";

                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    File.WriteAllText(path + "$" + item.Key.Name + ".csv", TicksList[item.Key].ToString());
                }*/
            }
        });

        private void ResetAll()
        {
            IsRequestIdValid = false;

            if (!(TaskCancelTs is null)) TaskCancelTs.Cancel(); // Cancel all tasks
            //OnConnectedHandler?.Invoke(Status = ConnectionStatus.Connecting, DateTime.Now, "TaskCancelTs.Cancel();");

            int i = Timeout + 100;
            while ((!(SendTask is null)) && SendTask?.Status == TaskStatus.Running && i > 0) { Thread.Sleep(1); i--; }
            SendTask?.Dispose();
            //OnConnectedHandler?.Invoke(Status = ConnectionStatus.Connecting, DateTime.Now, "SendTask?.Dispose();");


            i = Timeout + 100;
            while ((!(ReceiveTask is null)) && ReceiveTask?.Status == TaskStatus.Running && i > 0) { Thread.Sleep(1); i--; }
            ReceiveTask?.Dispose();
            //OnConnectedHandler?.Invoke(Status = ConnectionStatus.Connecting, DateTime.Now, "ReceiveTask?.Dispose();");


            i = Timeout + 100;
            while ((!(DecodeTask is null)) && DecodeTask?.Status == TaskStatus.Running && i > 0) { Thread.Sleep(1); i--; }
            DecodeTask?.Dispose();
            // One exception error here.
            //OnConnectedHandler?.Invoke(Status = ConnectionStatus.Connecting, DateTime.Now, "DecodeTask?.Dispose();");

            TcpReader?.Dispose();

            TcpClient?.Close();
            //TcpClient?.Dispose();

            TaskCancelTs?.Dispose();

            lock (ServerList) ServerList.Clear();
            //OnConnectedHandler?.Invoke(Status = ConnectionStatus.Connecting, DateTime.Now, "ServerList.Clear();");

            lock (ActiveTicks) ActiveTicks.Clear();

            // Flush Request IDs
            lock (ActiveRequestIds) ActiveRequestIds.Clear();
            // Flush send data
            FlushSendData();
            // Flush received data
            FlushReceiveData();

            TaskCancelTs = new CancellationTokenSource();

            RequestId = 0;
            ReceivedMessageCount = 0;
            requestId_HistoricalData = -1;
            requestId_ContractData = -1;
            requestId_ContractSamples = -1;
            requestId_HistoricalDataHeadTimestamp = -1;
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

        public static readonly WorkHours WorkHours = new WorkHours("Eastern Standard Time", new Dictionary<DayOfWeek, List<(Time Start, Time Stop)>>()
        {
            { DayOfWeek.Monday,     new List<(Time Start, Time Stop)>() { (Start: new Time(1), Stop: new Time(23, 30)) } },
            { DayOfWeek.Tuesday,    new List<(Time Start, Time Stop)>() { (Start: new Time(1), Stop: new Time(23, 30)) } },
            { DayOfWeek.Wednesday,  new List<(Time Start, Time Stop)>() { (Start: new Time(1), Stop: new Time(23, 30)) } },
            { DayOfWeek.Thursday,   new List<(Time Start, Time Stop)>() { (Start: new Time(1), Stop: new Time(23, 30)) } },
            { DayOfWeek.Friday,     new List<(Time Start, Time Stop)>() { (Start: new Time(1), Stop: new Time(22, 30)) } },
            { DayOfWeek.Saturday,   new List<(Time Start, Time Stop)>() { (Start: new Time(3, 30), Stop: new Time(23, 30)) } },
            { DayOfWeek.Sunday,     new List<(Time Start, Time Stop)>() { (Start: new Time(1), Stop: new Time(23, 30)) } }
        });
    }
}
