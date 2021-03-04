/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// Data Request Regulator for 
/// 
/// ***************************************************************************

using System;
using System.Linq;
using System.Threading;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {
        /// <summary>
        /// Check list:
        /// 1. DataRequestID must be set in SendRequest_ function
        /// 2. DataRequestID must be set to -1 when the data is being parsed
        /// 2. DataRequestID must be set to -1 in SendCancel_ function
        /// 
        /// TODO: TQQQ 3 minutes chart won't receive updated ticks, when 1 minute chart is loaded.
        /// 
        /// </summary>

        private static bool DataRequestReady => Connected && DataRequestID == -1;

        private static int DataRequestID { get; set; } = -1;

        private static object DataRequestLockObject { get; } = new object();

        #region Account




        #endregion Account

        #region Other




        #endregion Other

        #region Order History




        #endregion Order History

        #region Fetch Contract

        public static Contract[] Fetch_ContractSamples(string symbol, CancellationTokenSource cts = null)
        {
            lock (DataRequestLockObject)
                if (DataRequestReady)
                {
                    Console.WriteLine("Fetch_ContractSamples: " + symbol);

                    StartDownload:
                    SendRequest_ContractSamples(symbol);

                    int time = 0;
                    while (!DataRequestReady)
                    {
                        time++;
                        Thread.Sleep(10);
                        if (time > Timeout) // Handle Time out here.
                        {
                            SendCancel_ContractSamples();
                            Thread.Sleep(100);
                            goto StartDownload;
                        }
                        else if (IsCancelled || (cts is CancellationTokenSource cs && cs.IsCancellationRequested))
                        {
                            SendCancel_ContractSamples();
                            goto End;
                        }
                    }

                    return ActiveContractSampleList.ToArray();
                }

            End:
            return null;
        }

        public static Contract[] Fetch_ContractData(string name, string exchangeCode, string typeCode = "STK", CancellationTokenSource cts = null)
        {
            lock (DataRequestLockObject)
                if (DataRequestReady)
                {
                    StartDownload:
                    SendRequest_ContractData(0, name, exchangeCode, typeCode);

                    int time = 0;
                    while (!DataRequestReady)
                    {
                        time++;
                        Thread.Sleep(10);
                        if (time > Timeout) // Handle Time out here.
                        {
                            SendCancel_ContractData();
                            Thread.Sleep(100);
                            goto StartDownload;
                        }
                        else if (IsCancelled || (cts is CancellationTokenSource cs && cs.IsCancellationRequested))
                        {
                            SendCancel_ContractData();
                            goto End;
                        }
                    }

                    return active_ContractData_ResultList.ToArray();
                }

            End:
            return null;
        }

        public static Contract Fetch_ContractData(int conId, CancellationTokenSource cts = null)
        {
            lock (DataRequestLockObject)
            {
                if (cts.Continue() && DataRequestReady)
                {

                    StartDownload:
                    SendRequest_ContractData(conId);

                    int time = 0;
                    while (!DataRequestReady)
                    {
                        time++;
                        Thread.Sleep(10);
                        if (time > Timeout) // Handle Time out here.
                        {
                            SendCancel_ContractData();
                            Thread.Sleep(100);
                            goto StartDownload;
                        }
                        else if (cts.Cancelled() || IsCancelled)
                        {
                            SendCancel_ContractData();
                            goto End;
                        }
                    }

                    var list = active_ContractData_ResultList.Where(n => n.ConId == conId);
                    return (list.Count() > 0) ? list.First() : null;
                }
            }

            End:
            return null;
        }

        public static void Fetch_ContractData(Contract c, CancellationTokenSource cts = null)
        {
            lock (DataRequestLockObject)
            {
                if (cts.Continue() && DataRequestReady)
                {

                    StartDownload:
                    SendRequest_ContractData(c);

                    int time = 0;
                    while (!DataRequestReady)
                    {
                        time++;
                        Thread.Sleep(10);
                        if (time > Timeout) // Handle Time out here.
                        {
                            SendCancel_ContractData();
                            Thread.Sleep(100);
                            goto StartDownload;
                        }
                        else if (cts.Cancelled() || IsCancelled)
                        {
                            SendCancel_ContractData();
                            return;
                        }
                    }
                }
            }
        }

        #endregion Fetch Contract




        #region News




        #endregion News
    }
}
