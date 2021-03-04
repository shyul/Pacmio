/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
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

        public static List<(DateTime time, DataSourceType SRC, double O, double H, double L, double C, double V)>
            GetRows(this Contract c, Period pd, BarFreq barFreq, BarType type = BarType.Trades, bool adjustDividend = false)
        {
            if (barFreq == BarFreq.Daily)
            {
                if (type == BarType.Trades)
                {
                    // Download from Quandl
                }
                else
                {

                }
            }
            else if (barFreq > BarFreq.Daily)
            {
                // always merge from Daily...
            }
            else
            {
                /*
                var freqList = ReflectionTool.ToArray<BarFreq>().Where(n => n <= barFreq && ((int)barFreq / (int)n) % 1 == 0).OrderByDescending(n => n);
                foreach (var freq in freqList)
                {
                    // load the bar file,
                    BarDataFile bdf = c.GetOrCreateBarDataFile(freq, type);
                    // load the list within the period range
                    var smaller_list = bdf.GetRows(pd);
                    // apply the list, verify a full list of each bar, and disregard if it is not full,  apply the DataSourceSegment 
                    // save the file for future use
                }
                */

                // Download the still missing part...
            }



            List<(DateTime time, DataSourceType SRC, double O, double H, double L, double C, double V)> sortedList = null;

            return null;
        }

        public static void Download_IB(this BarDataFile bd, Period pd)
        {

        }



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
