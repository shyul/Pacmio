/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xu;

namespace Pacmio
{
    public static class BarTableManager
    {
        private static Dictionary<(Contract c, BarFreq freq), BarTable> ContractDailyBarTableLUT { get; } = new();

        public static void Clear()
        {
            lock (ContractDailyBarTableLUT)
            {
                ContractDailyBarTableLUT.Values.ToList().ForEach(n => n.Dispose());
                ContractDailyBarTableLUT.Clear();
            }
        }

        public static BarTable GetOrCreateDailyBarTable(this Contract c, BarFreq barFreq = BarFreq.Daily)
        {
            if (barFreq >= BarFreq.Daily)
            {
                var key = (c, barFreq);

                lock (ContractDailyBarTableLUT)
                {
                    if (!ContractDailyBarTableLUT.ContainsKey(key))
                    {
                        BarTable bt = c.LoadDailyBarTable(barFreq, false);
                        ContractDailyBarTableLUT[key] = bt;
                    }
                    return ContractDailyBarTableLUT[key];
                }
            }
            else
                throw new("This function does not support intra-day bars.");
        }

        private static BarTable LoadDailyBarTable(this Contract c, BarFreq barFreq, bool adjustDividend = false, CancellationTokenSource cts = null)
        {
            if (barFreq < BarFreq.Daily)
                throw new("This function does not support intra-day bars.");

            BarDataFile bdf_daily = c.GetOrCreateBarDataFile(BarFreq.Daily, DataType.Trades);
            bdf_daily.Fetch(Period.Full, cts);

            //BarTable bt_daily = bdf_daily.GetBarTable();
            BarTable bt_daily = new(bdf_daily);//.GetBarTable();

            var sorted_daily_list = bdf_daily.LoadBars(bt_daily, adjustDividend);

            if (barFreq > BarFreq.Daily)
            {
                BarTable bt = new(c, barFreq, DataType.Trades);
                bt.LoadFromSmallerBar(sorted_daily_list);
                return bt;
            }
            else if (barFreq == BarFreq.Daily)
            {
                bt_daily.LoadBars(sorted_daily_list);
                return bt_daily;
            }
            else
                return null;
        }

        public static BarTable LoadBarTable(this Contract c, Period period, BarFreq barFreq, DataType dataType, bool adjustDividend, CancellationTokenSource cts = null)
        {
            Console.WriteLine("LoadBarTable Period = " + period);

            BarDataFile bdf_daily_base = c.GetOrCreateBarDataFile(BarFreq.Daily);
            bdf_daily_base.Fetch(Period.Full, cts);

            if (barFreq == BarFreq.Daily && dataType == DataType.Trades)
            {
                BarTable bt = new(bdf_daily_base, period, adjustDividend);
                /*
                BarTable bt = bdf_daily_base.GetBarTable();
                var sorted_list = bdf_daily_base.LoadBars(bt, period, adjustDividend);
                bt.LoadBars(sorted_list);*/
                return bt;
            }
            else if (barFreq > BarFreq.Daily)
            {
                BarDataFile bdf_daily = dataType == DataType.Trades ? bdf_daily_base : c.GetOrCreateBarDataFile(BarFreq.Daily, dataType);

                if (dataType != DataType.Trades)
                    bdf_daily.Fetch(Period.Full, cts);

                BarTable bt_daily = new(bdf_daily);//, period, adjustDividend);
                //BarTable bt_daily = bdf_daily.GetBarTable();
                var sorted_daily_list = bdf_daily.LoadBars(bt_daily, period, adjustDividend);
                BarTable bt = new(c, barFreq, dataType);
                bt.LoadFromSmallerBar(sorted_daily_list);
                return bt;
            }
            else
            {
                BarDataFile bdf = c.GetOrCreateBarDataFile(barFreq, dataType);
                bdf.Fetch(period, cts);
                BarTable bt = new(bdf, period, adjustDividend);
                //BarTable bt = bdf.GetBarTable();
                //var sorted_list = bdf.LoadBars(bt, period, adjustDividend);
                //bt.LoadBars(sorted_list);
                return bt;
            }
        }

        public static void SetPeriod(this BarTable bt, Period pd, bool adjustDividend, CancellationTokenSource cts)
        {
            bt.Clear();
            var bdf = bt.GetOrCreateBarDataFile();
            bdf.Fetch(pd, cts);
            var sorted_list = bdf.LoadBars(bt, pd, adjustDividend);
            bt.LoadBars(sorted_list);
        }
    }
}
