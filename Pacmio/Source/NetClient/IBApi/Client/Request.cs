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
using System.Text.RegularExpressions;

namespace Pacmio.IB
{
    public partial class Client
    {
        #region Request
        private void SendRequest(string[] paramsList)
        {
            if (ApiStatus == ConnectionStatus.Connected)
                sendDataBuffer.Enqueue(paramsList);
        }

        private void SendRequest(List<string> paramsList) => SendRequest(paramsList.ToArray());

        public int RequestId { get; private set; }
        public bool IsRequestIdValid { get; private set; } = false;

        private readonly ConcurrentDictionary<int, RequestType> ActiveRequestIds = new ConcurrentDictionary<int, RequestType>();

        private bool ActiveRequestContains(RequestType type) => ActiveRequestIds.Values.Contains(type);
        private bool ActiveRequestContains(int reqId) => ActiveRequestIds.ContainsKey(reqId);

        private (int ReqId, string ReqTypeNum) RegisterRequest(RequestType type)
        {
            if (IsRequestIdValid)
            {
                int reqId = -1;
                lock (ActiveRequestIds)
                {
                    while (ActiveRequestIds.ContainsKey(RequestId)) RequestId++;
                    reqId = RequestId;
                    ActiveRequestIds[reqId] = type;
                    RequestId++;
                }
                return (reqId, ((int)type).ToString());
            }
            else
                throw new Exception("Invalid RequestId, Please ask IB to get the next valid id.");
        }

        private (bool valid, RequestType type) RemoveRequest(int requestId, bool cancel = true)
        {
            RequestType type = RequestType.RequestGlobalCancel;
            lock (ActiveRequestIds)
            {
                if (ActiveRequestIds.ContainsKey(requestId))
                {
                    type = ActiveRequestIds[requestId];

                    Log.Print(DateTime.Now.ToString("HH:mm:ss") + " Removing " + type + "Request >> " + requestId);

                    (bool hasCancellationCode, RequestCancellationCode Result) = type.GetAttribute<RequestCancellationCode>();
                    if (hasCancellationCode && cancel)
                    {
                        RequestType cancelRequest = Result.Type;
                        Send(new string[] {
                            ((int)cancelRequest).ToString(),
                            Result.Version.ToString(),
                            requestId.ToString(),
                        });
                    }
                    ActiveRequestIds.TryRemove(requestId, out _);
                    return (true, type);
                }
            }
            return (false, type);
        }

        private void RemoveRequest(int requestId, RequestType type)
        {
            lock (ActiveRequestIds)
            {
                if (ActiveRequestIds.ContainsKey(requestId) && ActiveRequestIds[requestId] == type)
                {
                    Log.Print(DateTime.Now.ToString("HH:mm:ss") + " Removing " + type + "Request >> " + requestId);

                    (bool hasCancellationCode, RequestCancellationCode Result) = type.GetAttribute<RequestCancellationCode>();
                    if (hasCancellationCode)
                    {
                        RequestType cancelRequest = Result.Type;
                        Send(new string[] {
                            ((int)cancelRequest).ToString(),
                            Result.Version.ToString(),
                            requestId.ToString(),
                        });
                    }
                    ActiveRequestIds.TryRemove(requestId, out _);
                }
            }
        }

        private void RemoveRequest(RequestType type)
        {
            lock (ActiveRequestIds)
            {
                List<int> removeList = new List<int>();

                foreach (int reqId in ActiveRequestIds.Keys)
                    if (ActiveRequestIds[reqId] == type)
                        removeList.Add(reqId);

                foreach (int reqId in removeList)
                    RemoveRequest(reqId);
            }
        }

        #endregion Request

        #region Send Data

        private const char MSG_EOL = '\0';
        private Task SendTask { get; set; }
        private readonly ConcurrentQueue<string[]> sendDataBuffer = new ConcurrentQueue<string[]>();
        private bool SendDataEmpty => sendDataBuffer.IsEmpty;
        private void FlushSendData() { lock (sendDataBuffer) while (!SendDataEmpty) sendDataBuffer.TryDequeue(out _); }

        private void Send(string[] paramsList) // 50 request per second
        {
            StringBuilder sb = new StringBuilder("0000"); // Clear 4 byte (octet) for request length.

            for (int i = 0; i < paramsList.Length; i++)
                sb.Append(paramsList[i] + MSG_EOL);

            byte[] sendBytes = Encoding.ASCII.GetBytes(sb.ToString());

            int length = sb.Length - 4;
            sendBytes[0] = Convert.ToByte((length >> 24) & 0xFF);
            sendBytes[1] = Convert.ToByte((length >> 16) & 0xFF);
            sendBytes[2] = Convert.ToByte((length >> 8) & 0xFF);
            sendBytes[3] = Convert.ToByte((length) & 0xFF);

            TcpClient.GetStream().Write(sendBytes, 0, sendBytes.Length); // Length = cnt + 4  
        }

        /// <summary>
        /// =============================================================> Need to make a better pace
        /// </summary>
        private void SendWorker()
        {
            int paceCount = 0, timer = 0;
            while (!IsCancelled)
            {
                // Only dequeue when the pace count is below the threshold
                if (paceCount < Pace && IsSocketConnected() && sendDataBuffer.Count > 0)
                {
                    try
                    {
                        sendDataBuffer.TryDequeue(out string[] values);

                        try
                        {
                            if (values[0] != "API")
                            {
                                RequestType type = (RequestType)int.Parse(Regex.Replace(values[0].Trim(), @"[^\d]", ""));
                                Console.WriteLine("Send " + type + ": " + values.ToStringWithIndex());
                            }
                            else
                                Console.WriteLine("Send " + ": " + values.ToStringWithIndex());
                        }
                        catch (Exception e) when (e is ArgumentException || e is FormatException || e is OverflowException)
                        {
                            Console.WriteLine("Send " + ": " + values.ToStringWithIndex());
                        }

                        Send(values); paceCount++;
                        Thread.Sleep(5);
                    }
                    catch (Exception e) when (e is IOException || e is InvalidOperationException)
                    {
                        OnConnectedHandler?.Invoke(ApiStatus, DateTime.Now, "Send Data Error, disconnecting.");
                        Disconnect.Start();
                        break;
                    }
                }
                else if (!IsSocketConnected())
                {
                    OnConnectedHandler?.Invoke(ApiStatus, DateTime.Now, "Socket Error, disconnecting.");
                    Disconnect.Start();
                    break;
                }
                else
                {
                    timer++;
                    Thread.Sleep(1); // 1ms
                    if (timer > 1000 / Pace) // if pace = 50, the 20ms per pace
                    {
                        timer = 0;
                        if (paceCount > 0) paceCount--;
                    }
                }
            }
        }

        #endregion Send Data
    }
}
