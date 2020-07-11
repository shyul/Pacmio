/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using Pacmio.IB;
using System;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    /// <summary>
    /// Bar's period (presets)
    /// </summary>
    [Serializable, DataContract]
    public enum BarFreq : int
    {
        [EnumMember, BarFreqInfo(TimeUnit.Years, 1), ApiCode("")]
        Annually = 31536000, // 365 days

        [EnumMember, BarFreqInfo(TimeUnit.Months, 3), ApiCode("")]
        Quarterly = 7603200, // 88 days

        [EnumMember, BarFreqInfo(TimeUnit.Months, 1), ApiCode("1 month")]
        Monthly = 2419200, // 28 days

        [EnumMember, BarFreqInfo(TimeUnit.Weeks, 1), ApiCode("1 week")]
        Weekly = 604800,

        [EnumMember, BarFreqInfo(TimeUnit.Days, 1), ApiCode("1 day")]
        Daily = 86400,

        [EnumMember, BarFreqInfo(TimeUnit.Hours, 8), ApiCode("8 hours")]
        Hours_8 = 28800,

        [EnumMember, BarFreqInfo(TimeUnit.Hours, 4), ApiCode("4 hours")]
        Hours_4 = 14400,

        [EnumMember, BarFreqInfo(TimeUnit.Hours, 4), ApiCode("3 hours")]
        Hours_3 = 10800,

        [EnumMember, BarFreqInfo(TimeUnit.Hours, 2), ApiCode("2 hours")]
        Hours_2 = 7200,

        [EnumMember, BarFreqInfo(TimeUnit.Hours, 1), ApiCode("1 hour")]
        Hourly = 3600,

        [EnumMember, BarFreqInfo(TimeUnit.Minutes, 30), ApiCode("30 mins")]
        HalfHour = 1800,

        [EnumMember, BarFreqInfo(TimeUnit.Minutes, 20), ApiCode("20 mins")]
        Minutes_20 = 1200,

        [EnumMember, BarFreqInfo(TimeUnit.Minutes, 15), ApiCode("15 mins")]
        Minutes_15 = 900,

        [EnumMember, BarFreqInfo(TimeUnit.Minutes, 10), ApiCode("10 mins")]
        Minutes_10 = 600,

        [EnumMember, BarFreqInfo(TimeUnit.Minutes, 5), ApiCode("5 mins")]
        Minutes_5 = 300,

        [EnumMember, BarFreqInfo(TimeUnit.Minutes, 3), ApiCode("3 mins")]
        Minutes_3 = 180,

        [EnumMember, BarFreqInfo(TimeUnit.Minutes, 2), ApiCode("2 mins")]
        Minutes_2 = 120,

        [EnumMember, BarFreqInfo(TimeUnit.Minutes, 1), ApiCode("1 min")]
        Minute = 60,

        [EnumMember, BarFreqInfo(TimeUnit.Seconds, 30), ApiCode("30 secs")]
        HalfMinute = 30,

        [EnumMember, BarFreqInfo(TimeUnit.Seconds, 15), ApiCode("15 secs")]
        Seconds_15 = 15,

        [EnumMember, BarFreqInfo(TimeUnit.Seconds, 10), ApiCode("10 secs")]
        Seconds_10 = 10,

        [EnumMember, BarFreqInfo(TimeUnit.Seconds, 5), ApiCode("5 secs")]
        Seconds_5 = 5,

        [EnumMember, BarFreqInfo(TimeUnit.Seconds, 1), ApiCode("1 secs")]
        Second = 1
    }
}
