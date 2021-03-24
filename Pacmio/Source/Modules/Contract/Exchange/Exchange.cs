/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using Pacmio.IB;
using System;
using System.Runtime.Serialization;

namespace Pacmio
{
    /// <summary>
    /// Exchange List
    /// </summary>
    [Serializable, DataContract]
    public enum Exchange : int
    {
        [EnumMember]
        UNKNOWN = 0,
        //[EnumMember, IbApiCode("SMART", "SMART")]
        //[ExchangeInfo("US", "SMART", "IB Smart Routing Service", ExchangeWorkHours.Full)]
        //SMART = 0,

        [EnumMember, ApiCode("IDEALPRO")]
        [ExchangeInfo("US", "IDEALPRO", "Ideal Pro Forex Service", ExchangeWorkHours.Full)]
        IDEALPRO = 100,

        #region US Exchanges

        [EnumMember, ApiCode("NYSE")] // Code = Primary Exchange / Code2 = Destination Exchange
        [ExchangeInfo("US", "NYSE", "New York Stock Exchange", ExchangeWorkHours.NorthAmerica, ExchangeWorkHours.NorthAmericaExtended)]
        NYSE = 1000,

        [EnumMember, ApiCode("AMEX")]
        //[ExchangeInfo("US", "AMEX", "NYSE MKT", ExchangeWorkHours.Amex, ExchangeWorkHours.NorthAmericaExtended)]
        [ExchangeInfo("US", "AMEX", "NYSE MKT", ExchangeWorkHours.NorthAmerica, ExchangeWorkHours.NorthAmericaExtended)]
        AMEX = 1001,

        [EnumMember, ApiCode("ARCA")]
        //[ExchangeInfo("US", "ARCA", "NYSE Arca", ExchangeWorkHours.NorthAmericaExtended, ExchangeWorkHours.NorthAmericaExtended)]
        [ExchangeInfo("US", "ARCA", "NYSE Arca", ExchangeWorkHours.NorthAmerica, ExchangeWorkHours.NorthAmericaExtended)]
        ARCA = 1002,

        [EnumMember, ApiCode("ISLAND")]
        [ExchangeInfo("US", "NASDAQ", "NASDAQ Stock Market", ExchangeWorkHours.NorthAmerica, ExchangeWorkHours.NorthAmericaExtended)]
        NASDAQ = 1020,

        [EnumMember, ApiCode("PSX")]
        [ExchangeInfo("US", "PSX", "NASDAQ OMX PSX", ExchangeWorkHours.NorthAmerica, ExchangeWorkHours.NorthAmericaExtended)]
        PSX = 1022,

        [EnumMember, ApiCode("BEX")]
        [ExchangeInfo("US", "BEX", "NASDAQ OMX BX", ExchangeWorkHours.NorthAmerica, ExchangeWorkHours.NorthAmericaExtended)]
        BEX = 1023,

        [EnumMember, ApiCode("BATS")]
        [ExchangeInfo("US", "BATS", "BATS Global Markets", ExchangeWorkHours.NorthAmerica, ExchangeWorkHours.NorthAmericaExtended)]
        BATS = 1040,

        [EnumMember, ApiCode("BYX")]
        [ExchangeInfo("US", "BYX", "BATS BYX", ExchangeWorkHours.NorthAmericaExtended2)]
        BYX = 1041,

        [EnumMember, ApiCode("CBOE")]
        [ExchangeInfo("US", "CBOE", "Chicago Board Options Exchange", ExchangeWorkHours.Cboe)]
        CBOE = 1050,

        [EnumMember, ApiCode("DRCTEDGE")]
        [ExchangeInfo("US", "DRCTEDGE", "CBOE Direct Edge", ExchangeWorkHours.NorthAmerica)]
        DRCTEDGE = 1051,

        [EnumMember, ApiCode("EDGEA")]
        [ExchangeInfo("US", "EDGEA", "CBOE Direct Edge (EDGEA)", ExchangeWorkHours.NorthAmerica)]
        EDGEA = 1052,

        [EnumMember, ApiCode("CHX")]
        [ExchangeInfo("US", "CHX", "Chicago Stock Exchange", ExchangeWorkHours.NorthAmerica)]
        CHX = 1060,

        [EnumMember, ApiCode("NSX")]
        [ExchangeInfo("US", "NSX", "National Stock Exchange", ExchangeWorkHours.NorthAmerica)]
        NSX = 1061,

        [EnumMember, ApiCode("IEX")]
        [ExchangeInfo("US", "IEX", "Investors Exchange", ExchangeWorkHours.NorthAmericaExtended2)]
        IEX = 1062,

        [EnumMember, ApiCode("ARCAEDGE")]
        [ExchangeInfo("US", "OTCBB", "NASD OTC Bulletin Board", ExchangeWorkHours.ArcaEdge)]
        OTCBB = 1091,

        [EnumMember, ApiCode("SMART")]
        [ExchangeInfo("US", "OTCMKT", "OTC Markets Pink Sheets", ExchangeWorkHours.ArcaEdge)]
        OTCMKT = 1095,

        /*
        [EnumMember, IBApiCode("SMART")]
        [ExchangeInfo("US", "PINK", "Pink Sheet Other", ExchangeWorkHours.ArcaEdge)]
        PINK = 1096,
        */
        [EnumMember, ApiCode("ISE")]
        [ExchangeInfo("US", "ISE", "ISE Options Exchange", ExchangeWorkHours.NorthAmerica)]
        ISE = 1200,

        #endregion US Exchanges

        #region Canadian Exchanges

        [EnumMember, ApiCode("TSE")]
        [ExchangeInfo("CA", "TSE", "Toronto Stock Exchange", ExchangeWorkHours.NorthAmerica)]
        TSE = 2000,

        [EnumMember, ApiCode("VENTURE")]
        [ExchangeInfo("CA", "VENTURE", "TSX Venture Exchange", ExchangeWorkHours.CanadianVenture)]
        VENTURE = 2001,

        [EnumMember, ApiCode("SMART")]
        [ExchangeInfo("CA", "ALPHA", "TSX Alpha Exchange", ExchangeWorkHours.NorthAmericaExtended2)]
        ALPHA = 2002,

        #endregion Canadian Exchanges

        #region America / Other

        [EnumMember, ApiCode("MEXI")]
        [ExchangeInfo("MX", "MEXI", "Mexican Stock Exchange", ExchangeWorkHours.NorthAmerica)]
        MEXI = 2100,

        #endregion America / Other

        #region Europe

        [EnumMember, ApiCode("LSE")]
        [ExchangeInfo("GB", "LSE", "London Stock Exchange", ExchangeWorkHours.LSE)]
        LSE = 3000,

        [EnumMember, ApiCode("SBF")]
        [ExchangeInfo("ES", "SBF", "Euronext France", ExchangeWorkHours.CentralEurope)]
        SBF = 3050,

        [EnumMember, ApiCode("FWB")]
        [ExchangeInfo("DE", "FWB", "Frankfurt Stock Exchange", ExchangeWorkHours.FWB)]
        FWB = 3100,

        [EnumMember, ApiCode("SWB")]
        [ExchangeInfo("DE", "SWB", "Stuttgart Stock Exchange", ExchangeWorkHours.SWB)]
        SWB = 3110,

        [EnumMember, ApiCode("IBIS")]
        [ExchangeInfo("DE", "XETRA", "Deutsche Börse Xetra", ExchangeWorkHours.CentralEurope)]
        XETRA = 3120,

        [EnumMember, ApiCode("EBS")]
        [ExchangeInfo("CH", "EBS", "SIX Swiss Exchange", ExchangeWorkHours.EBS)]
        EBS = 3200,

        [EnumMember, ApiCode("VSE")]
        [ExchangeInfo("AT", "VSE", "Vienna Stock Exchange", ExchangeWorkHours.CentralEurope)]
        VSE = 3250,

        [EnumMember, ApiCode("AEB")]
        [ExchangeInfo("NL", "AEB", "Euronext NL Stocks", ExchangeWorkHours.CentralEurope)]
        AEB = 3260,

        [EnumMember, ApiCode("SFB")]
        [ExchangeInfo("SE", "SFB", "Swedish Stock Exchange", ExchangeWorkHours.SFB)]
        SFB = 3400,

        [EnumMember, ApiCode("BM")]
        [ExchangeInfo("ES", "BM", "Bolsa de Madrid", ExchangeWorkHours.BM)]
        BM = 3700,

        #endregion Europe

        #region Asia / Pacific

        [EnumMember, ApiCode("SEHKNTL")]
        [ExchangeInfo("HK", "SHSE", "Shanghai Security Exchange", ExchangeWorkHours.China)]
        SHSE = 10000,

        [EnumMember, ApiCode("SEHKSZSE")]
        [ExchangeInfo("HK", "SZSE", "Shenzhen Security Exchange", ExchangeWorkHours.China)]
        SZSE = 10010,

        [EnumMember, ApiCode("SEHK")]
        [ExchangeInfo("HK", "SEHK", "Hong Kong Stock Exchange", ExchangeWorkHours.HongKong)]
        SEHK = 10020,

        [EnumMember, ApiCode("SGX")]
        [ExchangeInfo("SG", "SGX", "Singapore Exchange", ExchangeWorkHours.Singapore)]
        SGX = 11000,

        [EnumMember, ApiCode("NSE")]
        [ExchangeInfo("IN", "NSE", "National Stock Exchange of India", ExchangeWorkHours.India)]
        NSE = 11100,

        [EnumMember, ApiCode("ASX")]
        [ExchangeInfo("AU", "ASX", "Australian Stock Exchange", ExchangeWorkHours.ASX)]
        ASX = 5000,

        [EnumMember, ApiCode("MZHO")]
        [ExchangeInfo("JP", "MIZUHO", "Mizuho Securities / みずほ証券", ExchangeWorkHours.Full)]
        MZHO = 5100,

        #endregion Asia / Pacific
    }
}
