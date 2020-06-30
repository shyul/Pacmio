/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    /// <summary>
    /// Master List of all symbols
    /// </summary>
    public static class TableList
    {
        private static readonly ConcurrentDictionary<(Contract c, BarFreq barFreq, BarType type), BarTable> List
            = new ConcurrentDictionary<(Contract c, BarFreq barFreq, BarType type), BarTable>();

        private static int Count => List.Count;
        public static bool IsEmpty => Count == 0; 
        public static IEnumerable<BarTable> Values => List.Values;

        public static bool Remove((Contract c, BarFreq barFreq, BarType type) info)
        {
            bool successful = List.TryRemove(info, out _);
            return successful;
        }


        /// <summary>
        /// Use Download Worker for this one please....
        /// </summary>
        /// <param name="c"></param>
        /// <param name="barFreq"></param>
        /// <param name="type"></param>
        public static BarTable GetTableOld(this Contract c, BarFreq barFreq, BarType type)
        {
            if (!List.ContainsKey((c, barFreq, type)))
                List.TryAdd((c, barFreq, type), new BarTable(c, barFreq, type));

            BarTable bt = List[(c, barFreq, type)];

            Console.WriteLine("TableList Get Table: " + bt.Name);

            return bt;
        }

        public static BarTable GetTable(this Contract c, BarFreq barFreq, BarType type, Period period, CancellationTokenSource cts, IProgress<float> progress)
        {
            BarTable bt = GetTableOld(c, barFreq, type);
            bt.Reset(period, cts, progress);
            return bt;
        }



        public static void Reset(BarFreq barFreq, BarType type, Period pd, CancellationTokenSource cts, IProgress<float> progress)
        {
            int i = 0;
            List.Values.Where(n => n.BarFreq == barFreq && n.Type == type).ToList().ForEach(bt =>
            {
                if (!cts.IsCancellationRequested)
                {
                    bt.Reset(pd, cts, progress);
                }
                i++;
            });
        }



        /// <summary>
        /// TODO: Table Download using IB.CLient.DataRequest
        /// </summary>
        /// <param name="bt"></param>
        /// <param name="period"></param>
        /// <param name="cts"></param>
        /// <param name="progress"></param>
        public static void Reset(this BarTable bt, Period period, CancellationTokenSource cts, IProgress<float> progress)
        {
            Console.WriteLine("Reset Table: " + bt.Name + " Period = " + period);

            bt.ReadyForTickCalculation = false;
            DownloadCancellationTokenSource = cts;
            DownloadProgress = progress;

            if (period.IsCurrent && IB.Client.Connected)
                bt.Contract.Request_MarketTicks("236,375");

            LocalRequestAction.Enqueue(new Action(() => {
                bt.LoadJsonFileToFileData(); // Blocking the process first
                // Then add the Bar to the Data Object
                InboundDataActions.Enqueue(new Action(() => {
                    bt.TransferActualValuesFromBarsToFileData();
                    bt.TransferActualValuesFromFileDataToBars(period);
                    // The table is loaded but not sorted, with only Actual Values
                    LocalRequestAction.Enqueue(new BarDataRequest(bt, period));
                }));
            }));
        }







        public static void Save()
        {
            // This is happening when the inbound task is over.

            if (!IsEmpty)
                Parallel.ForEach(Values, bt => {
                    bt.LoadJsonFileToFileData();
                    bt.TransferActualValuesFromBarsToFileData();
                    bt.SaveFileDataToJsonFile();
                });
        }
    }
}
