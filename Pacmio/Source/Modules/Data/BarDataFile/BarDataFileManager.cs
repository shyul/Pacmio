/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Xu;

namespace Pacmio
{
    public static class BarDataFileManagerTest
    {
        // Obsolete, Test Only
        public static BarTable LoadBarTable(this Contract c, BarFreq barFreq, DataType dataType, Period pd, bool adjustDividend = false, CancellationTokenSource cts = null)
            => c.LoadBarTable(barFreq, dataType, new MultiPeriod(pd), adjustDividend, cts);

        // Obsolete, Test Only
        public static BarTable LoadBarTable(this Contract c, BarFreq barFreq, DataType dataType, MultiPeriod mp = null, bool adjustDividend = false, CancellationTokenSource cts = null)
        {
            BarTableSet bts = new BarTableSet(c, adjustDividend);
            bts.SetPeriod(mp, cts);
            return bts[barFreq, dataType];
        }
    }

    public static class BarDataFileManager
    {
        public static BarTable LoadBarTable(this BarTableSet bts, BarFreq barFreq, DataType dataType, MultiPeriod mp = null, CancellationTokenSource cts = null)
        {
            BarDataFile bdf_daily_base = GetBarDataFile(bts.Contract, BarFreq.Daily);
            bdf_daily_base.Fetch(Period.Full, cts);
            RemoveDownloadingLUT(bdf_daily_base);

            if (barFreq == BarFreq.Daily && dataType == DataType.Trades)
            {
                BarTable bt = new(bts, BarFreq.Daily, DataType.Trades);
                bt.LoadBars(bdf_daily_base, bts.AdjustDividend);
                return bt;
            }
            else if (barFreq > BarFreq.Daily)
            {
                BarDataFile bdf_daily =
                    dataType == DataType.Trades ?
                    bdf_daily_base :
                    bts.Contract.GetBarDataFile(BarFreq.Daily, dataType);

                if (dataType != DataType.Trades)
                    bdf_daily.Fetch(Period.Full, cts);

                var sorted_daily_list = bdf_daily.LoadBars(new BarTable(bts.Contract, BarFreq.Daily, dataType), bts.AdjustDividend);

                BarTable bt = new(bts, barFreq, dataType);
                bt.LoadFromSmallerBar(sorted_daily_list);

                RemoveDownloadingLUT(bdf_daily);
                return bt;
            }
            else
            {
                BarTable bt = new(bts, barFreq, dataType);
                bt.LoadBars(mp);
                return bt;
            }
        }

        public static void LoadBars(this BarTable bt, MultiPeriod mp, CancellationTokenSource cts = null)
        {
            if (mp is not null)
            {
                bool adjustDividend = bt.AdjustDividend;

                BarDataFile bdf = bt.Contract.GetBarDataFile(bt.BarFreq, bt.Type);

                MultiPeriod new_mp = new MultiPeriod();

                foreach (var period in mp)
                {
                    Period pd = new Period(period);
                    DateTime newStart = pd.Start.AddSeconds(-1000 * bt.Frequency.Span.TotalSeconds);
                    pd.Insert(newStart);
                    bdf.Fetch(pd, cts);
                    new_mp.Add(pd);
                }

                var sorted_list = bdf.LoadBars(bt, new_mp, adjustDividend);
                bt.LoadBars(sorted_list);

                RemoveDownloadingLUT(bdf);
            }
        }

        #region Download

        private static Dictionary<(Contract c, BarFreq barFreq, DataType dataType), BarDataFile> DownloadingLUT { get; } = new();

        private static object DataLockObject { get; } = new();

        private static void RemoveDownloadingLUT(BarDataFile bdf)
        {
            var key = (bdf.Contract, bdf.BarFreq, bdf.Type);

            lock (DataLockObject)
                if (DownloadingLUT.ContainsKey(key))
                {
                    DownloadingLUT.Remove(key);
                }
        }

        private static BarDataFile GetBarDataFile(this Contract c, BarFreq barFreq = BarFreq.Daily, DataType type = DataType.Trades)
        {
            if (barFreq > BarFreq.Daily)
                throw new Exception("We don't support storging BarFreq greater than daily as file");

            var key = (c, barFreq, type);

            BarDataFile bdf = null;

            lock (DataLockObject)
                if (DownloadingLUT.ContainsKey(key))
                {
                    bdf = DownloadingLUT[key];
                }
                else
                {
                    bdf = BarDataFile.LoadFile(key);
                    DownloadingLUT.Add(key, bdf);
                }

            return bdf;
        }

        /// <summary>
        /// Only support download daily and smaller bars data file.
        /// </summary>
        /// <param name="bdf"></param>
        /// <param name="pd"></param>
        /// <param name="cts"></param>
        /// <returns></returns>
        private static bool Fetch(this BarDataFile bdf, Period pd, CancellationTokenSource cts = null)
        {
            lock (bdf)
            {
                if (bdf.BarFreq > BarFreq.Daily)
                    throw new Exception("Only support download daily and smaller bars data file.");
                else //if (bdf.BarFreq == BarFreq.Daily)
                    Console.WriteLine("Last Closing Date: " + bdf.Contract.LatestExtendedClosingDateTime.ToString("MMM-dd-yyyy"));


                if (bdf.Contract.HistoricalHeadTime.IsInvalid())
                {
                    IB.Client.Fetch_HistoricalDataHeadTimestamp(bdf, cts, false, 1);
                }
                else
                {
                    Console.WriteLine("Historical Head Time = " + bdf.HistoricalHeadTime);
                }

                if (bdf.Contract.Status != ContractStatus.Error)
                {
                    if (!Quandl.Fetch(bdf))
                    {
                        IB.Client.Fetch_HistoricalData(bdf, pd, cts);

                        //RemoveDownloadingBarDataFile(bdf);
                    }
                    else
                        return true;
                }
                else
                {
                    Console.WriteLine("Contract has errors!");
                }

                return false;
            }
        }

        #endregion Download
    }
}
