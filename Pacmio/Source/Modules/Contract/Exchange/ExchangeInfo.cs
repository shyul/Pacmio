/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Xu;

namespace Pacmio
{
    [AttributeUsage(AttributeTargets.Field), Serializable, DataContract]
    public sealed class ExchangeInfo : Attribute
    {
        public ExchangeInfo(string regionName, string name, string fullName, ExchangeWorkHours workHours, ExchangeWorkHours workHoursExtended)
        {
            Region = new RegionInfo(regionName);
            Name = name;
            FullName = fullName;
            WorkHours = GetWorkHours(workHours);
            WorkHoursExtended = GetWorkHours(workHoursExtended);
        }

        public ExchangeInfo(string regionName, string name, string fullName, ExchangeWorkHours workHours)
        {
            Region = new RegionInfo(regionName);
            Name = name;
            FullName = fullName;
            WorkHours = GetWorkHours(workHours);
            WorkHoursExtended = WorkHours;
        }

        [DataMember]
        public RegionInfo Region { get; private set; }

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public string FullName { get; private set; }

        [IgnoreDataMember, XmlIgnore]
        public WorkHours WorkHours { get; private set; }

        [IgnoreDataMember, XmlIgnore]
        public WorkHours WorkHoursExtended { get; private set; }

        #region Work Hours

        public static WorkHours GetWorkHours(ExchangeWorkHours workHours)
        {
            return workHours switch
            {
                ExchangeWorkHours.Full => WorkHoursAll,
                ExchangeWorkHours.NorthAmerica => WorkHoursNorthAmerica,
                ExchangeWorkHours.NorthAmericaExtended => WorkHoursNorthAmericaExtended,
                ExchangeWorkHours.NorthAmericaExtended2 => WorkHoursNorthAmericaExtended2,
                ExchangeWorkHours.Amex => WorkHoursAmex,
                ExchangeWorkHours.ArcaEdge => WorkHoursArcaEdge,
                ExchangeWorkHours.Cboe => WorkHoursCboe,
                ExchangeWorkHours.CanadianVenture => WorkHoursCanadianVenture,
                ExchangeWorkHours.LSE => WorkHoursLSE,
                ExchangeWorkHours.FWB => WorkHoursFWB,
                ExchangeWorkHours.SWB => WorkHoursSWB,
                ExchangeWorkHours.CentralEurope => WorkHoursCentralEurope,
                ExchangeWorkHours.EBS => WorkHoursEBS,
                ExchangeWorkHours.SFB => WorkHoursSFB,
                ExchangeWorkHours.BM => WorkHoursBM,
                ExchangeWorkHours.China => WorkHoursChina,
                ExchangeWorkHours.HongKong => WorkHoursHongKong,
                ExchangeWorkHours.Singapore => WorkHoursSingapore,
                ExchangeWorkHours.India => WorkHoursIndia,
                ExchangeWorkHours.ASX => WorkHoursASX,
                _ => WorkHoursAll,
            };
        }

        [NonSerialized, IgnoreDataMember, XmlIgnore]
        public static readonly WorkHours WorkHoursAll = new WorkHours("GMT Standard Time", new Dictionary<DayOfWeek, MultiTimePeriod>()
        {
            { DayOfWeek.Sunday,     new MultiTimePeriod(new Time(0), new Time(23, 59, 59, 999)) },
            { DayOfWeek.Monday,     new MultiTimePeriod(new Time(0), new Time(23, 59, 59, 999)) },
            { DayOfWeek.Tuesday,    new MultiTimePeriod(new Time(0), new Time(23, 59, 59, 999)) },
            { DayOfWeek.Wednesday,  new MultiTimePeriod(new Time(0), new Time(23, 59, 59, 999)) },
            { DayOfWeek.Thursday,   new MultiTimePeriod(new Time(0), new Time(23, 59, 59, 999)) },
            { DayOfWeek.Friday,     new MultiTimePeriod(new Time(0), new Time(23, 59, 59, 999)) },
            { DayOfWeek.Saturday,   new MultiTimePeriod(new Time(0), new Time(23, 59, 59, 999)) },
        });

        [NonSerialized, IgnoreDataMember, XmlIgnore]
        public static readonly WorkHours WorkHoursNorthAmerica = new WorkHours("Eastern Standard Time", new Dictionary<DayOfWeek, MultiTimePeriod>()
        {
            { DayOfWeek.Monday,     new MultiTimePeriod(new Time(9, 30), new Time(16)) },
            { DayOfWeek.Tuesday,    new MultiTimePeriod(new Time(9, 30), new Time(16)) },
            { DayOfWeek.Wednesday,  new MultiTimePeriod(new Time(9, 30), new Time(16)) },
            { DayOfWeek.Thursday,   new MultiTimePeriod(new Time(9, 30), new Time(16)) },
            { DayOfWeek.Friday,     new MultiTimePeriod(new Time(9, 30), new Time(16)) },
        });

        [NonSerialized, IgnoreDataMember, XmlIgnore]
        public static readonly WorkHours WorkHoursNorthAmericaExtended = new WorkHours("Eastern Standard Time", new Dictionary<DayOfWeek, MultiTimePeriod>()
        {
            { DayOfWeek.Monday,     new MultiTimePeriod(new Time(4), new Time(20)) },
            { DayOfWeek.Tuesday,    new MultiTimePeriod(new Time(4), new Time(20)) },
            { DayOfWeek.Wednesday,  new MultiTimePeriod(new Time(4), new Time(20)) },
            { DayOfWeek.Thursday,   new MultiTimePeriod(new Time(4), new Time(20)) },
            { DayOfWeek.Friday,     new MultiTimePeriod(new Time(4), new Time(20)) },
        });

        [NonSerialized, IgnoreDataMember, XmlIgnore]
        public static readonly WorkHours WorkHoursNorthAmericaExtended2 = new WorkHours("Eastern Standard Time", new Dictionary<DayOfWeek, MultiTimePeriod>()
        {
            { DayOfWeek.Monday,     new MultiTimePeriod(new Time(8), new Time(17)) },
            { DayOfWeek.Tuesday,    new MultiTimePeriod(new Time(8), new Time(17)) },
            { DayOfWeek.Wednesday,  new MultiTimePeriod(new Time(8), new Time(17)) },
            { DayOfWeek.Thursday,   new MultiTimePeriod(new Time(8), new Time(17)) },
            { DayOfWeek.Friday,     new MultiTimePeriod(new Time(8), new Time(17)) },
        });

        [NonSerialized, IgnoreDataMember, XmlIgnore]
        public static readonly WorkHours WorkHoursAmex = new WorkHours("Eastern Standard Time", new Dictionary<DayOfWeek, MultiTimePeriod>()
        {
            { DayOfWeek.Monday,     new MultiTimePeriod() { new TimePeriod(new Time(7), new Time(20)) } },
            { DayOfWeek.Tuesday,    new MultiTimePeriod() { new TimePeriod(new Time(7), new Time(20)) } },
            { DayOfWeek.Wednesday,  new MultiTimePeriod() { new TimePeriod(new Time(7), new Time(20)) } },
            { DayOfWeek.Thursday,   new MultiTimePeriod() { new TimePeriod(new Time(7), new Time(20)) } },
            { DayOfWeek.Friday,     new MultiTimePeriod() { new TimePeriod(new Time(7), new Time(20)) } },
        });

        [NonSerialized, IgnoreDataMember, XmlIgnore]
        public static readonly WorkHours WorkHoursArcaEdge = new WorkHours("Eastern Standard Time", new Dictionary<DayOfWeek, MultiTimePeriod>()
        {
            { DayOfWeek.Monday,     new MultiTimePeriod() { new TimePeriod(new Time(7, 30), new Time(16, 1)) } },
            { DayOfWeek.Tuesday,    new MultiTimePeriod() { new TimePeriod(new Time(7, 30), new Time(16, 1)) } },
            { DayOfWeek.Wednesday,  new MultiTimePeriod() { new TimePeriod(new Time(7, 30), new Time(16, 1)) } },
            { DayOfWeek.Thursday,   new MultiTimePeriod() { new TimePeriod(new Time(7, 30), new Time(16, 1)) } },
            { DayOfWeek.Friday,     new MultiTimePeriod() { new TimePeriod(new Time(7, 30), new Time(16, 1)) } }
        });

        [NonSerialized, IgnoreDataMember, XmlIgnore]
        public static readonly WorkHours WorkHoursCboe = new WorkHours("Eastern Standard Time", new Dictionary<DayOfWeek, MultiTimePeriod>()
        {
            { DayOfWeek.Monday,     new MultiTimePeriod() { new TimePeriod(new Time(9, 30), new Time(16, 15)) } },
            { DayOfWeek.Tuesday,    new MultiTimePeriod() { new TimePeriod(new Time(9, 30), new Time(16, 15)) } },
            { DayOfWeek.Wednesday,  new MultiTimePeriod() { new TimePeriod(new Time(9, 30), new Time(16, 15)) } },
            { DayOfWeek.Thursday,   new MultiTimePeriod() { new TimePeriod(new Time(9, 30), new Time(16, 15)) } },
            { DayOfWeek.Friday,     new MultiTimePeriod() { new TimePeriod(new Time(9, 30), new Time(16, 15)) } }
        });

        [NonSerialized, IgnoreDataMember, XmlIgnore]
        public static readonly WorkHours WorkHoursCanadianVenture = new WorkHours("Eastern Standard Time", new Dictionary<DayOfWeek, MultiTimePeriod>()
        {
            { DayOfWeek.Monday,     new MultiTimePeriod() { new TimePeriod(new Time(9, 30), new Time(16)), new TimePeriod(new Time(16, 15), new Time(17)) } },
            { DayOfWeek.Tuesday,    new MultiTimePeriod() { new TimePeriod(new Time(9, 30), new Time(16)), new TimePeriod(new Time(16, 15), new Time(17)) } },
            { DayOfWeek.Wednesday,  new MultiTimePeriod() { new TimePeriod(new Time(9, 30), new Time(16)), new TimePeriod(new Time(16, 15), new Time(17)) } },
            { DayOfWeek.Thursday,   new MultiTimePeriod() { new TimePeriod(new Time(9, 30), new Time(16)), new TimePeriod(new Time(16, 15), new Time(17)) } },
            { DayOfWeek.Friday,     new MultiTimePeriod() { new TimePeriod(new Time(9, 30), new Time(16)), new TimePeriod(new Time(16, 15), new Time(17)) } }
        });

        [NonSerialized, IgnoreDataMember, XmlIgnore]
        public static readonly WorkHours WorkHoursLSE = new WorkHours("GMT Standard Time", new Dictionary<DayOfWeek, MultiTimePeriod>()
        {
            { DayOfWeek.Monday,     new MultiTimePeriod() { new TimePeriod(new Time(8), new Time(16, 50)) } },
            { DayOfWeek.Tuesday,    new MultiTimePeriod() { new TimePeriod(new Time(8), new Time(16, 50)) } },
            { DayOfWeek.Wednesday,  new MultiTimePeriod() { new TimePeriod(new Time(8), new Time(16, 50)) } },
            { DayOfWeek.Thursday,   new MultiTimePeriod() { new TimePeriod(new Time(8), new Time(16, 50)) } },
            { DayOfWeek.Friday,     new MultiTimePeriod() { new TimePeriod(new Time(8), new Time(16, 50)) } }
        });

        [NonSerialized, IgnoreDataMember, XmlIgnore]
        public static readonly WorkHours WorkHoursCentralEurope = new WorkHours("Central European Standard Time", new Dictionary<DayOfWeek, MultiTimePeriod>()
        {
            { DayOfWeek.Monday,     new MultiTimePeriod() { new TimePeriod(new Time(9), new Time(17, 40)) } },
            { DayOfWeek.Tuesday,    new MultiTimePeriod() { new TimePeriod(new Time(9), new Time(17, 40)) } },
            { DayOfWeek.Wednesday,  new MultiTimePeriod() { new TimePeriod(new Time(9), new Time(17, 40)) } },
            { DayOfWeek.Thursday,   new MultiTimePeriod() { new TimePeriod(new Time(9), new Time(17, 40)) } },
            { DayOfWeek.Friday,     new MultiTimePeriod() { new TimePeriod(new Time(9), new Time(17, 40)) } },
         });

        [NonSerialized, IgnoreDataMember, XmlIgnore]
        public static readonly WorkHours WorkHoursFWB = new WorkHours("Central European Standard Time", new Dictionary<DayOfWeek, MultiTimePeriod>()
        {
            { DayOfWeek.Monday,     new MultiTimePeriod() { new TimePeriod(new Time(8), new Time(20)) } },
            { DayOfWeek.Tuesday,    new MultiTimePeriod() { new TimePeriod(new Time(8), new Time(20)) } },
            { DayOfWeek.Wednesday,  new MultiTimePeriod() { new TimePeriod(new Time(8), new Time(20)) } },
            { DayOfWeek.Thursday,   new MultiTimePeriod() { new TimePeriod(new Time(8), new Time(20)) } },
            { DayOfWeek.Friday,     new MultiTimePeriod() { new TimePeriod(new Time(8), new Time(20)) } }
        });

        [NonSerialized, IgnoreDataMember, XmlIgnore]
        public static readonly WorkHours WorkHoursSWB = new WorkHours("Central European Standard Time", new Dictionary<DayOfWeek, MultiTimePeriod>()
        {
            { DayOfWeek.Monday,     new MultiTimePeriod() { new TimePeriod(new Time(8), new Time(22)) } },
            { DayOfWeek.Tuesday,    new MultiTimePeriod() { new TimePeriod(new Time(8), new Time(22)) } },
            { DayOfWeek.Wednesday,  new MultiTimePeriod() { new TimePeriod(new Time(8), new Time(22)) } },
            { DayOfWeek.Thursday,   new MultiTimePeriod() { new TimePeriod(new Time(8), new Time(22)) } },
            { DayOfWeek.Friday,     new MultiTimePeriod() { new TimePeriod(new Time(8), new Time(22)) } }
        });

        [NonSerialized, IgnoreDataMember, XmlIgnore]
        public static readonly WorkHours WorkHoursSFB = new WorkHours("Central European Standard Time", new Dictionary<DayOfWeek, MultiTimePeriod>()
        {
            { DayOfWeek.Monday,     new MultiTimePeriod() { new TimePeriod(new Time(9), new Time(17, 30)) } },
            { DayOfWeek.Tuesday,    new MultiTimePeriod() { new TimePeriod(new Time(9), new Time(17, 30)) } },
            { DayOfWeek.Wednesday,  new MultiTimePeriod() { new TimePeriod(new Time(9), new Time(17, 30)) } },
            { DayOfWeek.Thursday,   new MultiTimePeriod() { new TimePeriod(new Time(9), new Time(17, 30)) } },
            { DayOfWeek.Friday,     new MultiTimePeriod() { new TimePeriod(new Time(9), new Time(17, 30)) } },
        });

        [NonSerialized, IgnoreDataMember, XmlIgnore]
        public static readonly WorkHours WorkHoursEBS = new WorkHours("Central European Standard Time", new Dictionary<DayOfWeek, MultiTimePeriod>()
        {
            { DayOfWeek.Monday,     new MultiTimePeriod() { new TimePeriod(new Time(9), new Time(17, 32)) } },
            { DayOfWeek.Tuesday,    new MultiTimePeriod() { new TimePeriod(new Time(9), new Time(17, 32)) } },
            { DayOfWeek.Wednesday,  new MultiTimePeriod() { new TimePeriod(new Time(9), new Time(17, 32)) } },
            { DayOfWeek.Thursday,   new MultiTimePeriod() { new TimePeriod(new Time(9), new Time(17, 32)) } },
            { DayOfWeek.Friday,     new MultiTimePeriod() { new TimePeriod(new Time(9), new Time(17, 32)) } },
        });

        [NonSerialized, IgnoreDataMember, XmlIgnore]
        public static readonly WorkHours WorkHoursBM = new WorkHours("Central European Standard Time", new Dictionary<DayOfWeek, MultiTimePeriod>()
        {
            { DayOfWeek.Monday,     new MultiTimePeriod() { new TimePeriod(new Time(9), new Time(17, 35)) } },
            { DayOfWeek.Tuesday,    new MultiTimePeriod() { new TimePeriod(new Time(9), new Time(17, 35)) } },
            { DayOfWeek.Wednesday,  new MultiTimePeriod() { new TimePeriod(new Time(9), new Time(17, 35)) } },
            { DayOfWeek.Thursday,   new MultiTimePeriod() { new TimePeriod(new Time(9), new Time(17, 35)) } },
            { DayOfWeek.Friday,     new MultiTimePeriod() { new TimePeriod(new Time(9), new Time(17, 35)) } },
        });

        [NonSerialized, IgnoreDataMember, XmlIgnore]
        public static readonly WorkHours WorkHoursChina = new WorkHours("China Standard Time", new Dictionary<DayOfWeek, MultiTimePeriod>()
        {
            { DayOfWeek.Monday,     new MultiTimePeriod() { new TimePeriod(new Time(9, 30), new Time(11, 30)), new TimePeriod(new Time(13), new Time(15)) } },
            { DayOfWeek.Tuesday,    new MultiTimePeriod() { new TimePeriod(new Time(9, 30), new Time(11, 30)), new TimePeriod(new Time(13), new Time(15)) } },
            { DayOfWeek.Wednesday,  new MultiTimePeriod() { new TimePeriod(new Time(9, 30), new Time(11, 30)), new TimePeriod(new Time(13), new Time(15)) } },
            { DayOfWeek.Thursday,   new MultiTimePeriod() { new TimePeriod(new Time(9, 30), new Time(11, 30)), new TimePeriod(new Time(13), new Time(15)) } },
            { DayOfWeek.Friday,     new MultiTimePeriod() { new TimePeriod(new Time(9, 30), new Time(11, 30)), new TimePeriod(new Time(13), new Time(15)) } }
        });

        [NonSerialized, IgnoreDataMember, XmlIgnore]
        public static readonly WorkHours WorkHoursHongKong = new WorkHours("China Standard Time", new Dictionary<DayOfWeek, MultiTimePeriod>()
        {
            { DayOfWeek.Monday,     new MultiTimePeriod() { new TimePeriod(new Time(9, 30), new Time(12)), new TimePeriod(new Time(13), new Time(16)) } },
            { DayOfWeek.Tuesday,    new MultiTimePeriod() { new TimePeriod(new Time(9, 30), new Time(12)), new TimePeriod(new Time(13), new Time(16)) } },
            { DayOfWeek.Wednesday,  new MultiTimePeriod() { new TimePeriod(new Time(9, 30), new Time(12)), new TimePeriod(new Time(13), new Time(16)) } },
            { DayOfWeek.Thursday,   new MultiTimePeriod() { new TimePeriod(new Time(9, 30), new Time(12)), new TimePeriod(new Time(13), new Time(16)) } },
            { DayOfWeek.Friday,     new MultiTimePeriod() { new TimePeriod(new Time(9, 30), new Time(12)), new TimePeriod(new Time(13), new Time(16)) } }
        });

        [NonSerialized, IgnoreDataMember, XmlIgnore]
        public static readonly WorkHours WorkHoursSingapore = new WorkHours("Singapore Standard Time", new Dictionary<DayOfWeek, MultiTimePeriod>()
        {
            { DayOfWeek.Monday,     new MultiTimePeriod() { new TimePeriod(new Time(9), new Time(12)), new TimePeriod(new Time(13), new Time(17)) } },
            { DayOfWeek.Tuesday,    new MultiTimePeriod() { new TimePeriod(new Time(9), new Time(12)), new TimePeriod(new Time(13), new Time(17)) } },
            { DayOfWeek.Wednesday,  new MultiTimePeriod() { new TimePeriod(new Time(9), new Time(12)), new TimePeriod(new Time(13), new Time(17)) } },
            { DayOfWeek.Thursday,   new MultiTimePeriod() { new TimePeriod(new Time(9), new Time(12)), new TimePeriod(new Time(13), new Time(17)) } },
            { DayOfWeek.Friday,     new MultiTimePeriod() { new TimePeriod(new Time(9), new Time(12)), new TimePeriod(new Time(13), new Time(17)) } }
        });

        [NonSerialized, IgnoreDataMember, XmlIgnore]
        public static readonly WorkHours WorkHoursIndia = new WorkHours("India Standard Time", new Dictionary<DayOfWeek, MultiTimePeriod>()
        {
            { DayOfWeek.Monday,     new MultiTimePeriod() { new TimePeriod(new Time(9, 15), new Time(15, 30)) } },
            { DayOfWeek.Tuesday,    new MultiTimePeriod() { new TimePeriod(new Time(9, 15), new Time(15, 30)) } },
            { DayOfWeek.Wednesday,  new MultiTimePeriod() { new TimePeriod(new Time(9, 15), new Time(15, 30)) } },
            { DayOfWeek.Thursday,   new MultiTimePeriod() { new TimePeriod(new Time(9, 15), new Time(15, 30)) } },
            { DayOfWeek.Friday,     new MultiTimePeriod() { new TimePeriod(new Time(9, 15), new Time(15, 30)) } },
        });

        [NonSerialized, IgnoreDataMember, XmlIgnore]
        public static readonly WorkHours WorkHoursASX = new WorkHours("E. Australia Standard Time", new Dictionary<DayOfWeek, MultiTimePeriod>()
        {
            { DayOfWeek.Monday,     new MultiTimePeriod(new Time(10), new Time(16)) },
            { DayOfWeek.Tuesday,    new MultiTimePeriod(new Time(10), new Time(16)) },
            { DayOfWeek.Wednesday,  new MultiTimePeriod(new Time(10), new Time(16)) },
            { DayOfWeek.Thursday,   new MultiTimePeriod(new Time(10), new Time(16)) },
            { DayOfWeek.Friday,     new MultiTimePeriod(new Time(10), new Time(16)) },
        });

        #endregion

        /// <summary>
        /// This Exchange Lookup table has to be updated according to IbApi
        /// </summary>
        private static readonly Dictionary<string, Exchange> ApiToExchange = new Dictionary<string, Exchange>()
        {
            //{ "SMART",          Exchange.SMART },

            { "IDEALPRO",       Exchange.IDEALPRO },

            { "ISLAND",         Exchange.NASDAQ },
            { "NASDAQ",         Exchange.NASDAQ },

            //{ "NASDAQ.NMS",     Exchange.NASDAQ },
            //{ "NASDAQ.SCM",     Exchange.NASDAQ },

            { "NSDQ",           Exchange.NASDAQ },
            { "NYSE",           Exchange.NYSE },
            { "AMEX",           Exchange.AMEX },
            { "ARCA",           Exchange.ARCA },
            { "BATS",           Exchange.BATS },

            { "PSX",            Exchange.PSX },
            { "CHX",            Exchange.CHX },
            { "BYX",            Exchange.BYX },
            { "IEX",            Exchange.IEX },
            { "BEX",            Exchange.BEX },
            { "NSX",            Exchange.NSX },
            { "DRCTEDGE",       Exchange.DRCTEDGE },
            { "EDGEA",          Exchange.EDGEA },
            { "ISE",            Exchange.ISE },

            { "OTCBB",          Exchange.OTCBB },
            { "PINK",           Exchange.OTCMKT },

            //{ "PINK.CURRENT",   Exchange.PINK },
            //{ "PINK.PREMQX",    Exchange.PINK },
            //{ "PINK.INTPREMQX", Exchange.PINK },
            //{ "PINK.PRIMQX",    Exchange.PINK },
            //{ "PINK.INTPRIMQX", Exchange.PINK },
            //{ "PINK.LIMITED",   Exchange.PINK },
            //{ "PINK.NOINFO",    Exchange.PINK },

            { "TSE",            Exchange.TSE },
            { "VENTURE",        Exchange.VENTURE },
            { "AEQLIT",         Exchange.ALPHA },
            { "MEXI",           Exchange.MEXI },

            { "LSE",            Exchange.LSE },
            { "SBF",            Exchange.SBF },
            { "FWB",            Exchange.FWB },
            { "SWB",            Exchange.SWB },
            { "IBIS",           Exchange.XETRA },
            { "EBS",            Exchange.EBS },
            { "VSE",            Exchange.VSE },
            { "AEB",            Exchange.AEB },
            { "SFB",            Exchange.SFB },
            { "BM",             Exchange.BM },

            { "SHSE",           Exchange.SHSE },
            { "SEHKNTL",        Exchange.SHSE },
            { "SZSE",           Exchange.SZSE },
            { "SEHKSZSE",       Exchange.SZSE },
            { "SEHK",           Exchange.SEHK },
            { "SGX",            Exchange.SGX },
            { "NSE",            Exchange.NSE },

            { "ASX",            Exchange.ASX },
            { "MZHO",           Exchange.MZHO },
        };

        public static HashSet<string> UnknownExchangeCode { get; private set; } = new HashSet<string>();

        public static (bool valid, Exchange exchange, string suffix) GetEnum(string value)
        {
            string[] exparts = value.Split('.');

            if (exparts.Length > 0)
            {
                string exchangeCode = exparts[0];
                if (!ApiToExchange.ContainsKey(exchangeCode))
                {
                    lock (UnknownExchangeCode)
                        if (!UnknownExchangeCode.Contains(value))
                            UnknownExchangeCode.Add(value);

                    return (false, Exchange.NASDAQ, string.Empty);
                }
                else
                {
                    if (exparts.Length > 1)
                        return (true, ApiToExchange[exchangeCode], exparts[1]);
                    else
                        return (true, ApiToExchange[exchangeCode], string.Empty);
                }
            }
            else
                return (false, Exchange.NASDAQ, string.Empty);
        }

        // public static List<Exchange> US_Exchanges { get; } = new List<Exchange>() { Exchange.BATS, Exchange.NASDAQ, Exchange.NYSE, Exchange.ARCA, Exchange.AMEX, Exchange.OTCBB };
    }
}
