/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xu;

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
        public static string Param(this Enum eu) => eu.GetAttribute<ApiCode>() is ApiCode ac ? ac.Code : null;
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

        //private const string FORMAT_FILTERDATE = "yyyymmdd-hh:mm:ss";
        private const string FORMAT_DATEONLY = "yyyyMMdd";
        private const string FORMAT_DATETIME = "yyyyMMdd  HH:mm:ss";

        public static DateTime ParseTime(string value, TimeZoneInfo tzi)
        {
            value = value.Trim();
            if (value.Length == FORMAT_DATEONLY.Length)
                return DateTime.ParseExact(value, FORMAT_DATEONLY, CultureInfo.InvariantCulture);
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

        /*
        private static readonly Dictionary<string, Type> ApiToSecType = new Dictionary<string, Type>()
        {
            { "STK",    typeof(Stock) },
            { "FUND",   typeof(MutualFund) },
            { "CASH",   typeof(Forex) },
            { "IND",    typeof(Index) },
            //{ "CMDTY",  ContractType.COMMODITY },
            //{ "BOND",   ContractType.BOND },
            { "FUT",    typeof(Future) },
            { "OPT",    typeof(Option) },
            //{ "FOP",    ContractType.FUTURE_OPTION },
            //{ "CFD",    ContractType.CFD },
            //{ "WAR",    ContractType.WAR },
            //{ "IOPT",   ContractType.IOPT },
        };*/

        public static string TypeCode(this Contract c)
        {
            return c switch
            {
                Stock _ => "STK",
                Index _ => "IND",
                Future _ => "FUT",
                Option _ => "OPT",
                MutualFund _ => "FUND",
                Forex _ => "CASH",
                _ => throw new NotImplementedException(),
            };
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
                            Stock c = ContractManager.GetOrAdd(new Stock(symbolName, exchange));
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
                            Index c = ContractManager.GetOrAdd(new Index(symbolName, exchange));
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

        /*
        public static T GetEnum<T>(string code) where T : Enum
        {
            var res = ReflectionTool.ToArray<T>().Where(n => n.GetAttribute<ApiCode>() is ApiCode res && res.Code == code);

            if (res.Count() > 0)
                return res.First();
            else
            {
                Console.WriteLine("???? Unknown IB API Code: " + code);
                return default(T);
            }
        }*/

        /*
        foreach (T item in Enum.GetValues(typeof(T)) as T[])
        {
            (bool IsValid, ApiCode Result) = item.GetAttribute<ApiCode>();
            if (IsValid && Result.Code == code) return (true, item);
        }*/


        private static void InitializeEnumLookUpTable<T>(Dictionary<string, T> table) where T : Enum
        {
            lock (table) 
            {
                table.Clear(); 
                ReflectionTool.ToArray<T>().ToList().ForEach(n => {
                    if (n.GetAttribute<ApiCode>() is ApiCode res) 
                        table[res.Code] = n;
                });
            }
        }

        public static void InitializeLookUpTables() 
        {
            InitializeEnumLookUpTable(OrderTypeLUT);
            InitializeEnumLookUpTable(OrderTimeInForceLUT);
            InitializeEnumLookUpTable(OrderStatusLUT);
        }

        private static Dictionary<string, OrderType> OrderTypeLUT { get; } = new Dictionary<string, OrderType>();
        private static Dictionary<string, OrderTimeInForce> OrderTimeInForceLUT { get; } = new Dictionary<string, OrderTimeInForce>();
        private static Dictionary<string, OrderStatus> OrderStatusLUT { get; } = new Dictionary<string, OrderStatus>();

        public static OrderType ToOrderType(this string value)
        {
            lock (OrderTypeLUT)
                if (OrderTypeLUT.ContainsKey(value))
                    return OrderTypeLUT[value];
                else
                    throw new Exception("Unknown OrderType Code: " + value);
            /*
            return value switch
            {
                "MKT" => OrderType.Market,
                "MTL" => OrderType.MarketLimit,
                "MIT" => OrderType.MarketIfTouched,
                "LMT" => OrderType.Limit,
                "STP" => OrderType.Stop,
                "STP LMT" => OrderType.StopLimit,
                "TRAIL" => OrderType.TrailingStop,
                "TRAIL LIMIT" => OrderType.TrailingStopLimit,
                "MIDPRICE" => OrderType.MidPrice,

                _ => OrderType.UNKNOWN
            };*/
        }

        public static OrderTimeInForce ToOrderTimeInForce(this string value)
        {
            lock (OrderTimeInForceLUT)
                if (OrderTimeInForceLUT.ContainsKey(value))
                    return OrderTimeInForceLUT[value];
                else
                    throw new Exception("Unknown OrderTimeInForce Code: " + value);

            /*
            return value switch
            {
                "DAY" => OrderTimeInForce.Day,
                _ => OrderTimeInForce.UNKNOWN
            };*/
        }

        public static OrderStatus ToOrderStatus(this string value)
        {
            lock (OrderStatusLUT)
                if (OrderStatusLUT.ContainsKey(value))
                    return OrderStatusLUT[value];
                else
                    throw new Exception("Unknown OrderStatus Code: " + value);

            /*
            return value switch
            {
                "Inactive" => OrderStatus.Inactive,
                "ApiPending" => OrderStatus.ApiPending,
                "PendingSubmit" => OrderStatus.PendingSubmit,
                "PreSubmitted" => OrderStatus.PreSubmitted,
                "Submitted" => OrderStatus.Submitted,
                "Filled" => OrderStatus.Filled,
                "PendingCancel" => OrderStatus.PendingCancel,
                "ApiCancelled" => OrderStatus.ApiCancelled,
                "Cancelled" => OrderStatus.Cancelled,

                _ => OrderStatus.UNKNOWN
            };*/
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
