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

        public static void Download(this BarDataFile bd, Period pd)
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
