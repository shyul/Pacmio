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

        public static BarDataFile GetOrCreateBarDataFile(this BarTable bt) => GetBarDataFile(bt.Contract, bt.BarFreq, bt.Type);

        #region Download

        /// <summary>
        /// Only support download daily and smaller bars data file.
        /// </summary>
        /// <param name="bdf"></param>
        /// <param name="pd"></param>
        /// <param name="cts"></param>
        /// <returns></returns>
        public static bool Fetch(this BarDataFile bdf, Period pd, CancellationTokenSource cts = null)
        {
            if (bdf.BarFreq > BarFreq.Daily)
                throw new Exception("Only support download daily and smaller bars data file.");
            else //if (bdf.BarFreq == BarFreq.Daily)
                Console.WriteLine("Last Closing Date: " + bdf.Contract.LatestExtendedClosingDateTime.ToString("MMM-dd-yyyy"));

            if (bdf.HistoricalHeadTime.IsInvalid())
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
                }
                else
                    return true;
            }

            return false;
        }

        #endregion Download
    }
}
