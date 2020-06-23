/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Globalization;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Xu;
using Pacmio;

namespace Pacmio.IB
{
    public static partial class Util
    {
        public static string Param(this RequestType type) => ((int)type).ToString();
        public static string Param(this bool value) => value ? "1" : "0";
        public static string Param(this bool? value) => (value.HasValue) ? (value.Value ? "1" : "0") : string.Empty;
        public static string Param(this double value) => (double.IsNaN(value) || value == double.MaxValue || value == double.MinValue) ? string.Empty : value.ToString();
        public static string ParamPos(this int value) => (value < 0 || value == int.MaxValue) ? string.Empty : value.ToString(CultureInfo.InvariantCulture);
        public static string Param(this int value) => value.ToString(CultureInfo.InvariantCulture);
        public static string Param(this ICollection<(string, string)> options)
        {
            StringBuilder tagValuesStr = new StringBuilder();

            if (!(options is null))
                foreach ((string Tag, string Value) in options)
                {
                    tagValuesStr.Append(Tag).Append("=").Append(Value).Append(";");
                }

            return tagValuesStr.ToString();
        }

        private const string FORMAT_FILTERDATE = "yyyymmdd-hh:mm:ss";
        private const string FORMAT_DATEONLY = "yyyyMMdd";
        private const string FORMAT_DATETIME = "yyyyMMdd  HH:mm:ss";

        public static DateTime ParseTime(string value, TimeZoneInfo tzi)
        {
            value = value.Trim();
            if (value.Length == FORMAT_DATEONLY.Length)
                return DateTime.ParseExact(value, FORMAT_DATEONLY, CultureInfo.InvariantCulture);//.ToDestination(tzi);
            else if (value.Length == FORMAT_DATETIME.Length)
                return DateTime.ParseExact(value, FORMAT_DATETIME, CultureInfo.InvariantCulture).ToDestination(tzi);
            else
                throw new Exception("Unable to parse field into DateTime: " + value);
        }

        public static (string, TimeSpan) GetMaximumDuration(this BarFreq freq)
        {
            switch (freq)
            {
                case (BarFreq.Second):
                    return ("1800 S", new TimeSpan(0, 30, 0));

                case (BarFreq.Seconds_5):
                    return ("3600 S", new TimeSpan(1, 0, 0));

                case (BarFreq.Seconds_10):
                    return ("14400 S", new TimeSpan(4, 0, 0));

                case (BarFreq.Seconds_15):
                case (BarFreq.HalfMinute):   // Has small bars limitation
                    return ("28800 S", new TimeSpan(8, 0, 0));

                case (BarFreq.Minute):
                    return ("1 D", new TimeSpan(1, 0, 0, 0));

                case (BarFreq.Minutes_2):
                    return ("2 D", new TimeSpan(2, 0, 0, 0));

                case (BarFreq.Minutes_3):
                case (BarFreq.Minutes_5):
                case (BarFreq.Minutes_10):
                case (BarFreq.Minutes_15):
                case (BarFreq.Minutes_20):
                    return ("1 W", new TimeSpan(5, 0, 0, 0));

                case (BarFreq.HalfHour):
                case (BarFreq.Hourly):
                case (BarFreq.Hours_2):
                case (BarFreq.Hours_3):
                case (BarFreq.Hours_4):
                case (BarFreq.Hours_8):
                    return ("28 D", new TimeSpan(28, 0, 0, 0));

                default:
                case (BarFreq.Daily):
                case (BarFreq.Weekly):
                case (BarFreq.Monthly):
                case (BarFreq.Quarterly):
                case (BarFreq.Annually):
                    return ("360 D", new TimeSpan(360, 0, 0, 0));
            }
        }

        public static (bool, Contract) GetContractByIbCode(string symbolName, string exchangeStr, string secTypeCode, string conIdStr)
        {
            (bool validExchange, Exchange exchange, string suffix) = ExchangeInfo.GetEnum(exchangeStr);

            if (validExchange)
            {
                switch (secTypeCode)
                {
                    case ("STK"):
                        {
                            Stock c = ContractList.GetOrAdd(new Stock(symbolName, exchange));
                            if (!string.IsNullOrEmpty(suffix)) c.ExchangeSuffix = suffix;
                            c.ConId = conIdStr.ToInt32(0);
                            if (c.ConId == 0)
                            {
                                c.Status = ContractStatus.Unknown;
                            }
                            else if (c.Status == ContractStatus.Unknown && c.ConId > 0)
                            {
                                c.Status = ContractStatus.Alive;
                            }
                            return (true, c);
                        }
                    case ("IND"):
                        {
                            Index c = ContractList.GetOrAdd(new Index(symbolName, exchange));
                            c.ConId = conIdStr.ToInt32(0);
                            if (c.ConId == 0)
                            {
                                c.Status = ContractStatus.Unknown;
                            }
                            else if (c.Status == ContractStatus.Unknown && c.ConId > 0)
                            {
                                c.Status = ContractStatus.Alive;
                            }
                            return (true, c);
                        }
                    case ("OPT"):
                    case ("FUT"):
                    case ("FUND"):
                    case ("CASH"):
                    default: throw new Exception("GetSymbolByIbCode: Unknown secTypeCode");
                }
            }
            else
                return (false, null);
        }



        public static TickType ToTickType(this string value)
        {
            int tickType = value.ToInt32(-1);

            if (tickType >= 0 && tickType <= 89)
                return (TickType)tickType;
            else
                return TickType.Unknown;
        }
    }
}
