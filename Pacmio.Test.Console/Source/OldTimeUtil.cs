using System;
using System.Collections.Generic;
using Xu;

namespace Pacmio
{
    internal static class OldTimeUtil
    {
        public static DateTime GetNextTickTime(DateTime time, bool IsForward, bool IsRTH, Frequency freq)
        {
            Int32 cnt = Convert.ToInt32(freq.Length);
            if (!IsForward) cnt = -cnt;
            switch (freq.Unit)
            {
                case (TimeUnit.Years): return AlignTradingYear(time, cnt);
                case (TimeUnit.Months): return AlignTradingMonth(time, cnt);
                case (TimeUnit.Weeks): return AlignTradingWeek(time, cnt);
                case (TimeUnit.Days): return AlignTradingDay(time, cnt);
                case (TimeUnit.Hours): return AlignTradingHour(time, cnt, IsRTH);
                case (TimeUnit.Minutes): return AlignTradingMinute(time, cnt, IsRTH);
                case (TimeUnit.Seconds): return AlignTradingSecond(time, cnt, IsRTH);
                default: throw new Exception("Invalid TimeInterval Type!");
            }
        }

        internal static Frequency ToFreq(this BarFreq size)
        {
            if (BarSizeToFreq.ContainsKey(size)) return BarSizeToFreq[size];
            else throw new Exception("Invalid BarSize");
        }

        internal static string ToString(this BarFreq size)
        {
            if (BarSizeToFreq.ContainsKey(size)) return BarSizeToFreq[size].ToString();
            else throw new Exception("Invalid BarSize");
        }

        internal static readonly Dictionary<BarFreq, Frequency> BarSizeToFreq = new()
        {
            { BarFreq.Annually,     new Frequency(TimeUnit.Years,   1) },
            { BarFreq.Quarterly,    new Frequency(TimeUnit.Months,  3) },
            { BarFreq.Monthly,      new Frequency(TimeUnit.Months,  1) },
            { BarFreq.Weekly,       new Frequency(TimeUnit.Weeks,   1) },
            { BarFreq.Daily,        new Frequency(TimeUnit.Days,    1) },
            { BarFreq.Hours_8,      new Frequency(TimeUnit.Hours,   8) },
            { BarFreq.Hours_4,      new Frequency(TimeUnit.Hours,   4) },
            { BarFreq.Hours_3,      new Frequency(TimeUnit.Hours,   3) },
            { BarFreq.Hours_2,      new Frequency(TimeUnit.Hours,   2) },
            { BarFreq.Hourly,       new Frequency(TimeUnit.Hours,   1) },
            { BarFreq.HalfHour,     new Frequency(TimeUnit.Minutes, 30) },
            { BarFreq.Minutes_20,   new Frequency(TimeUnit.Minutes, 20) },
            { BarFreq.Minutes_15,   new Frequency(TimeUnit.Minutes, 15) },
            { BarFreq.Minutes_10,   new Frequency(TimeUnit.Minutes, 10) },
            { BarFreq.Minutes_5,    new Frequency(TimeUnit.Minutes, 5) },
            { BarFreq.Minutes_3,    new Frequency(TimeUnit.Minutes, 3) },
            { BarFreq.Minutes_2,    new Frequency(TimeUnit.Minutes, 2) },
            { BarFreq.Minute,       new Frequency(TimeUnit.Minutes, 1) },
            { BarFreq.HalfMinute,   new Frequency(TimeUnit.Seconds, 30) },
            { BarFreq.Seconds_15,   new Frequency(TimeUnit.Seconds, 15) },
            { BarFreq.Seconds_10,   new Frequency(TimeUnit.Seconds, 10) },
            { BarFreq.Seconds_5,    new Frequency(TimeUnit.Seconds, 5) },
            { BarFreq.Second,       new Frequency(TimeUnit.Seconds, 1) },
        };

        /// <summary>
        /// Time and Holiday Describers
        /// </summary>
        /// 
        /*
        public static Period FitBarPeriod(DateTime time, Frequency freq)
        {
            switch (freq.Unit)
            {
                case (TimeUnit.Years):
                    return new Period(new DateTime(time.Year, 1, 1), new DateTime(time.Year + freq.Length, 1, 1));
                case (TimeUnit.Months): // Need to set the datum
                    int month = time.Month + freq.Length;
                    int year = time.Year;
                    while (month > 12)
                    {
                        month -= 12;
                        year++;
                    }
                    return new Period(new DateTime(time.Year, time.Month, 1), new DateTime(year, month, 1));
                case (TimeUnit.Weeks):
                    // Find Monday of the week

                    break;
                case (TimeUnit.Days):


                    break;
                case (TimeUnit.Hours):


                    break;
                case (TimeUnit.Minutes):

                    break;
                case (TimeUnit.Seconds):

                    break;
            }
        }*/

        public static bool IsTradingDate(DateTime time) // Simple
        {
            if (time.DayOfWeek == DayOfWeek.Saturday || time.DayOfWeek == DayOfWeek.Sunday) return false;
            else if (time.Month == 1 && time.Day == 1) return false;
            else if (time.Month == 7 && time.Day == 4) return false;
            else if (time.Month == 12 && time.Day == 25) return false;
            else return true;
        }

        public static DateTime AlignTradingYear(DateTime time, int cnt)
        {
            time = new DateTime(time.Year, 1, 2);
            if (cnt != 0) time = time.AddYears(cnt);
            while (!IsTradingDate(time))
            {
                time = time.AddDays(1);
            }
            return time;
        }

        public static DateTime AlignTradingMonth(DateTime time, int cnt)
        {
            time = new DateTime(time.Year, time.Month, 1);
            if (cnt != 0) time = time.AddMonths(cnt);
            while (!IsTradingDate(time))
            {
                time = time.AddDays(1);
            }
            return time;
        }

        public static DateTime AlignTradingWeek(DateTime time, int cnt) // Align to Monday
        {
            time = new DateTime(time.Year, time.Month, time.Day);

            if (time.DayOfWeek == DayOfWeek.Sunday) time = time.AddDays(1);
            else if (time.DayOfWeek == DayOfWeek.Tuesday) time = time.AddDays(-1);
            else if (time.DayOfWeek == DayOfWeek.Wednesday) time = time.AddDays(-2);
            else if (time.DayOfWeek == DayOfWeek.Thursday) time = time.AddDays(-3);
            else if (time.DayOfWeek == DayOfWeek.Friday) time = time.AddDays(-4);
            else if (time.DayOfWeek == DayOfWeek.Saturday) time = time.AddDays(-5);

            if (cnt != 0) time = time.AddDays(7 * cnt);
            while (!IsTradingDate(time))
            {
                time = time.AddDays(1);
            }
            return time;
        }

        public static DateTime AlignTradingDay(DateTime time, int cnt)
        {
            time = new DateTime(time.Year, time.Month, time.Day);
            if (cnt != 0) time = time.AddDays(cnt);
            while (!IsTradingDate(time))
            {
                if (cnt >= 0)
                    time = time.AddDays(1);
                else
                    time = time.AddDays(-1);
            }
            return time;
        }

        public static bool IsTradingTime(DateTime time, bool IsRTH)
        {
            if (!IsTradingDate(time)) return false;
            else
            {
                if (IsRTH)
                {
                    if (time.Hour >= 10 && time.Hour <= 15) return true;
                    else if (time.Hour == 9 && time.Minute >= 30) return true;
                    else return false;
                }
                else
                {
                    if (time.Hour >= 4 && time.Hour <= 20) return true;
                    else return false;
                }
            }
        }

        public static DateTime AlighTradingIntraday(DateTime time, int cnt, bool IsRTH)
        {
            if (!IsTradingTime(time, IsRTH))
            {
                if (cnt >= 0)
                {
                    time.AddDays(1); // go to tomorrow
                    if (IsRTH) time = new DateTime(time.Year, time.Month, time.Day, 9, 30, 0);
                    else time = new DateTime(time.Year, time.Month, time.Day, 4, 0, 0);
                }
                else
                {
                    time.AddDays(-1); // back yesterday
                    if (IsRTH) time = new DateTime(time.Year, time.Month, time.Day, 15, 0, 0);
                    else time = new DateTime(time.Year, time.Month, time.Day, 20, 0, 0);
                }
            }
            while (!IsTradingDate(time))
            {
                if (cnt >= 0)
                    time.AddDays(1); // go to next day
                else
                    time.AddDays(-1); // back previous day
            }
            return time;
        }

        public static DateTime AlignTradingHour(DateTime time, int cnt, bool IsRTH)
        {
            time = new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0);
            if (cnt != 0) time.AddHours(cnt);
            return AlighTradingIntraday(time, cnt, IsRTH);
        }

        public static DateTime AlignTradingMinute(DateTime time, int cnt, bool IsRTH)
        {
            time = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0);
            if (cnt != 0) time.AddMinutes(cnt);
            return AlighTradingIntraday(time, cnt, IsRTH);
        }

        public static DateTime AlignTradingSecond(DateTime time, int cnt, bool IsRTH)
        {
            if (cnt != 0) time.AddSeconds(cnt);
            return AlighTradingIntraday(time, cnt, IsRTH);
        }
    }
}
