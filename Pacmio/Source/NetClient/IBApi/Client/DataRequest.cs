/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// Data Request Regulator for 
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
using System.Windows.Forms;

namespace Pacmio.IB
{
    public static partial class Client
    {
        private static bool DataRequestReady => Connected && DataRequestID == -1;

        private static int DataRequestID { get; set; } = -1;

        private static object RequestLockObject { get; } = new object();

        #region Fetch

        public static DateTime Fetch_HistoricalDataHeadTimestamp(BarTable bt, CancellationTokenSource cts = null)
        {
            lock (RequestLockObject)
                if (DataRequestReady && HistoricalData_Connected)
                {
                StartDownload:
                    SendRequest_HistoricalDataHeadTimestamp(bt);

                    int time = 0;
                    while (!DataRequestReady)
                    {
                        time++;
                        Thread.Sleep(10);

                        if (time > Timeout) // Handle Time out here.
                        {
                            SendCancel_HistoricalHeadDataTimestamp();
                            Thread.Sleep(100);
                            goto StartDownload;
                        }
                        else if (IsCancelled || (cts is CancellationTokenSource cs && cs.IsCancellationRequested))
                        {
                            SendCancel_HistoricalHeadDataTimestamp();
                            goto End;
                        }
                    }
                }

            End:
            return bt.Contract.BarTableEarliestTime;
        }

        public static void Fetch_HistoricalData(BarTable bt, Period period, CancellationTokenSource cts = null)
        {
            var (bfi_valid, bfi) = bt.BarFreq.GetAttribute<BarFreqInfo>();

            lock (RequestLockObject)
                if (DataRequestReady && HistoricalData_Connected && bfi_valid)
                {
                    // We will download, but won't log the period if the stop may extended to the future.
                    IsLoggingLastRequestedHistoricalDataPeriod = period.Stop < DateTime.Now.AddDays(-1);
                    LastRequestedHistoricalDataPeriod = period;

                StartDownload:
                    SendRequest_HistoricalData(bt, bfi.DurationString, period.Stop);

                    int time = 0; // Wait the last transmit is over.
                    while (!DataRequestReady)
                    {
                        time++;
                        Thread.Sleep(10);

                        if (time > Timeout) // Handle Time out here.
                        {
                            SendCancel_HistoricalData();
                            Thread.Sleep(100);
                            goto StartDownload;
                        }
                        else if (IsCancelled || (cts is CancellationTokenSource cs && cs.IsCancellationRequested))
                        {
                            SendCancel_HistoricalData();
                            return;
                        }
                    }
                }
        }

        #endregion Fetch

        #region Task
        /*
        private static readonly ConcurrentQueue<Action> DataRequestActionQueue = new ConcurrentQueue<Action>();

        private static readonly ConcurrentQueue<Action> DataRequestActionQueue_Background = new ConcurrentQueue<Action>();

        private static Task DataRequestTask { get; set; }

        private static void DataRequestTaskWorker()
        {
            while (true)
            {
                if (IsCancelled)
                {
                    while (DataRequestActionQueue.Count > 0)
                        DataRequestActionQueue.TryDequeue(out _);

                    break;
                }
                else if (DataRequestActionQueue.Count > 0 || DataRequestActionQueue_Background.Count > 0)
                {
                    if(DataRequestActionQueue.Count > 0) 
                    {
                        DataRequestActionQueue.TryDequeue(out Action ax);
                        ax?.Invoke();
                    }
                    else if (DataRequestActionQueue_Background.Count > 0)
                    {
                        DataRequestActionQueue_Background.TryDequeue(out Action ax_background);
                        ax_background?.Invoke();
                    }
                }
                else
                    Thread.Sleep(10);
            }
        }
        */
        #endregion Task
    }
}
