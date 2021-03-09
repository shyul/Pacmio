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
        private static Dictionary<(Contract c, BarFreq barFreq, BarType type), BarDataFile> DataLUT { get; }
            = new Dictionary<(Contract c, BarFreq barFreq, BarType type), BarDataFile>();

        public static void Clear()
        {
            lock (DataLUT)
            {
                DataLUT.Clear();
            }

            GC.Collect();
        }

        public static BarDataFile GetOrCreateBarDataFile(this Contract c, BarFreq barFreq, BarType type = BarType.Trades)
        {
            if (barFreq > BarFreq.Daily) throw new Exception("We don't support storging BarFreq greater than daily as file");

            var key = (c, barFreq, type);

            lock (DataLUT)
            {
                if (!DataLUT.ContainsKey(key))
                {
                    DataLUT[key] = BarDataFile.LoadFile((c.Key, barFreq, type));
                }

                return DataLUT[key];
            }
        }

        public static BarDataFile GetOrCreateBarDataFile(this BarTable bt) => GetOrCreateBarDataFile(bt.Contract, bt.BarFreq, bt.Type);

        #region Download

        /// <summary>
        /// Only support download daily and smaller bars data file.
        /// </summary>
        /// <param name="bdf"></param>
        /// <param name="pd"></param>
        /// <param name="cts"></param>
        /// <returns></returns>
        private static bool Download(this BarDataFile bdf, Period pd, CancellationTokenSource cts)
        {
            if (bdf.BarFreq > BarFreq.Daily) throw new Exception("Only support download daily and smaller bars data file.");

            bool is_successful = false;

            if (bdf.HistoricalHeadTime == DateTime.MaxValue)
            {
                IB.Client.Fetch_HistoricalDataHeadTimestamp(bdf, cts, false, 1);
            }

            if (bdf.Contract.Status != ContractStatus.Error)
            {
                is_successful = Quandl.Download(bdf);

                if (!is_successful)
                {

                }
            }

            return is_successful;
        }

        #endregion Download

        #region File Operation

        public static void Save()
        {
            lock (DataLUT)
            {
                Parallel.ForEach(DataLUT.Values.Where(n => n.IsModified), bi => bi.SaveFile());
            }
        }

        #endregion File Operation
    }
}
