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
    public static class BarTableManager
    {
        private static Dictionary<(Contract c, BarFreq freq), BarTable> ContractDailyBarTableLUT { get; } = new Dictionary<(Contract c, BarFreq freq), BarTable>();

        public static BarTable GetOrCreateDailyBarTable(this Contract c, BarFreq freq = BarFreq.Daily)
        {
            var key = (c, freq);

            lock (ContractDailyBarTableLUT)
            {
                if (!ContractDailyBarTableLUT.ContainsKey(key))
                {
                    BarTable bt = c.LoadBarTable(freq, BarType.Trades, false);
                    ContractDailyBarTableLUT[key] = bt;
                }
                return ContractDailyBarTableLUT[key];
            }
        }

        public static BarTable LoadBarTable(this Contract c, BarFreq barFreq, BarType barType, bool adjustDividend)
        {
            // Request Download??

            if (barFreq > BarFreq.Daily)
            {
                BarDataFile bdf_daily = c.GetOrCreateBarDataFile(BarFreq.Daily, barType);
                BarTable bt_daily = bdf_daily.GetBarTable();
                var sorted_daily_list = bdf_daily.LoadBars(bt_daily, adjustDividend);
                BarTable bt = new BarTable(c, barFreq, barType);
                bt.LoadFromSmallerBar(sorted_daily_list);
                return bt;
            }
            else
            {
                BarDataFile bdf = c.GetOrCreateBarDataFile(barFreq, barType);
                BarTable bt = bdf.GetBarTable();
                var sorted_list = bdf.LoadBars(bt, adjustDividend);
                bt.LoadFromSmallerBar(sorted_list);
                return bt;
            }
        }

        public static BarTable LoadBarTable(this Contract c, Period period, BarFreq barFreq, BarType barType, bool adjustDividend)
        {
            // Request Download??

            if (barFreq > BarFreq.Daily)
            {
                BarDataFile bdf_daily = c.GetOrCreateBarDataFile(BarFreq.Daily, barType);
                BarTable bt_daily = bdf_daily.GetBarTable();
                var sorted_daily_list = bdf_daily.LoadBars(bt_daily, period, adjustDividend);
                BarTable bt = new BarTable(c, barFreq, barType);
                bt.LoadFromSmallerBar(sorted_daily_list);
                return bt;
            }
            else
            {
                BarDataFile bdf = c.GetOrCreateBarDataFile(barFreq, barType);
                BarTable bt = bdf.GetBarTable();
                var sorted_list = bdf.LoadBars(bt, period, adjustDividend);
                bt.LoadFromSmallerBar(sorted_list);
                return bt;
            }
        }
    }
}
