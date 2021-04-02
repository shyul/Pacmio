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
    public static class BarDataFileManager
    {
        public static BarDataFile GetBarDataFile(this Contract c, BarFreq barFreq = BarFreq.Daily, DataType type = DataType.Trades)
        {
            if (barFreq > BarFreq.Daily) throw new Exception("We don't support storging BarFreq greater than daily as file");

            return BarDataFile.LoadFile((c.Key, barFreq, type));
        }

        /*
        public static BarDataFile GetOrCreateBarDataFile(this BarTable bt) => GetBarDataFile(bt.Contract, bt.BarFreq, bt.Type);

        public static BarTable LoadBarTable(this BarTableSet bts, BarFreq barFreq, DataType dataType, Period pd, CancellationTokenSource cts = null)
            => LoadBarTable(bts, barFreq, dataType, new MultiPeriod(pd), cts);
        */

        public static BarTable LoadBarTable(this BarTableSet bts, BarFreq barFreq, DataType dataType, MultiPeriod mp = null, CancellationTokenSource cts = null)
        {
            BarDataFile bdf_daily_base = bts.Contract.GetBarDataFile(BarFreq.Daily);
            bdf_daily_base.Fetch(Period.Full, cts);

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
                return bt;
            }
            else
            {
                BarTable bt = new(bts, barFreq, dataType);
                bt.LoadBars(mp, bts.AdjustDividend);
                return bt;
            }
        }

        public static BarTable LoadBarTable(this Contract c, BarFreq barFreq, DataType dataType, Period pd, bool adjustDividend = false, CancellationTokenSource cts = null)
            => c.LoadBarTable(barFreq, dataType, new MultiPeriod(pd), adjustDividend, cts);

        public static BarTable LoadBarTable(this Contract c, BarFreq barFreq, DataType dataType, MultiPeriod mp = null, bool adjustDividend = false, CancellationTokenSource cts = null)
        {
            BarTableSet bts = new BarTableSet(c, adjustDividend);
            bts.SetPeriod(mp, cts);
            return bts[barFreq, dataType];

            /*
            BarDataFile bdf_daily_base = c.GetBarDataFile(BarFreq.Daily);
            bdf_daily_base.Fetch(Period.Full, cts);

            if (barFreq == BarFreq.Daily && dataType == DataType.Trades)
            {
                BarTable bt = new(c, BarFreq.Daily, DataType.Trades);
                bt.LoadBars(bdf_daily_base, adjustDividend);
                return bt;
            }
            else if (barFreq > BarFreq.Daily)
            {
                BarDataFile bdf_daily = dataType == DataType.Trades ? bdf_daily_base : c.GetBarDataFile(BarFreq.Daily, dataType);

                if (dataType != DataType.Trades)
                    bdf_daily.Fetch(Period.Full, cts);

                BarTable bt_daily = new(c, BarFreq.Daily, dataType);
                var sorted_daily_list = bdf_daily.LoadBars(bt_daily, adjustDividend);

                BarTable bt = new(c, barFreq, dataType);
                bt.LoadFromSmallerBar(sorted_daily_list);
                return bt;
            }
            else
            {
                BarTable bt = new(c, barFreq, dataType);
                bt.LoadBars(mp, adjustDividend);
                return bt;
            }*/
        }

        #region Download

        public static List<BarDataFile> DownloadingList { get; } = new();

        private static object DataLockObject { get; } = new();

        public static BarDataFile AddDownloadingBarDataFile(this Contract c, BarFreq barFreq = BarFreq.Daily, DataType type = DataType.Trades) 
        {
            lock (DataLockObject) 
            {

                return null;
            }
        }

        public static void RemoveDownloadingBarDataFile(BarDataFile bdf)
        {
            lock (DataLockObject)
            {
                if (DownloadingList.Contains(bdf)) DownloadingList.Remove(bdf);
            }
        }

        /// <summary>
        /// Only support download daily and smaller bars data file.
        /// </summary>
        /// <param name="bdf"></param>
        /// <param name="pd"></param>
        /// <param name="cts"></param>
        /// <returns></returns>
        public static bool Fetch(this BarDataFile bdf, Period pd, CancellationTokenSource cts = null)
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

                //Console.WriteLine("Historical Head Time = " + bdf.HistoricalHeadTime);

                if (bdf.Contract.Status != ContractStatus.Error)
                {
                    if (!Quandl.Fetch(bdf))
                    {
                        IB.Client.Fetch_HistoricalData(bdf, pd, cts);
                        RemoveDownloadingBarDataFile(bdf);
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
