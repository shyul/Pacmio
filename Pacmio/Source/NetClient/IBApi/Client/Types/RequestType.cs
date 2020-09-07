/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;

namespace Pacmio.IB
{
    public enum RequestType : int
    {
        Empty = 0,

        // ---------------
        // Order
        // ---------------

        RequestGlobalCancel = 58,

        [RequestCancellationCode(CancelOrder)]
        PlaceOrder = 3,  // OK -
        CancelOrder = 4,  // OK -

        RequestOpenOrders = 5,  // OK -
        RequestAutoOpenOrders = 15,
        RequestAllOpenOrders = 16,

        ReqCompletedOrders = 99,

        // ---------------
        // Account
        // ---------------

        RequestAccountData = 6,

        [RequestCancellationCode(CancelAccountSummary)]
        RequestAccountSummary = 62,
        CancelAccountSummary = 63,

        [RequestCancellationCode(CancelPositionsMulti)]
        RequestPositionsMulti = 74,
        CancelPositionsMulti = 75,

        [RequestCancellationCode(CancelAccountUpdatesMulti)]
        RequestAccountUpdatesMulti = 76,
        CancelAccountUpdatesMulti = 77,

        [RequestCancellationCode(CancelPositions)]
        RequestPositions = 61,  // OK
        CancelPositions = 64,

        // ---------------
        // All Data Source
        // ---------------

        RequestMarketDataType = 59,

        [RequestCancellationCode(CancelMarketData)]
        RequestMarketData = 1,
        CancelMarketData = 2,

        RequestMktDepthExchanges = 82,

        [RequestCancellationCode(CancelMarketDepth)]
        RequestMarketDepth = 10,  // OK --
        CancelMarketDepth = 11,  // OK --

        RequestScannerParameters = 24,

        [RequestCancellationCode(CancelScannerSubscription)]
        RequestScannerSubscription = 22,
        CancelScannerSubscription = 23,

        [RequestCancellationCode(CancelRealTimeBars)]
        RequestRealTimeBars = 50,  // OK
        CancelRealTimeBars = 51,  // OK

        // ---------------
        // Historical Data
        // ---------------

        [RequestCancellationCode(CancelHistoricalData)]
        RequestHistoricalData = 20,  // OK - First
        CancelHistoricalData = 25,  // OK

        [RequestCancellationCode(CancelTickByTickData)]
        RequestTickByTickData = 97,
        CancelTickByTickData = 98,

        RequestHistoricalTicks = 96,

        [RequestCancellationCode(CancelHistogramData)]
        RequestHistogramData = 88,
        CancelHistogramData = 89,

        // ---------------
        // Fundamentals
        // ---------------

        [RequestCancellationCode(CancelFundamentalData)]
        RequestFundamentalData = 52,  // OK
        CancelFundamentalData = 53,

        [RequestCancellationCode(CancelNewsBulletin)]
        RequestNewsBulletins = 12,
        CancelNewsBulletin = 13,

        RequestMatchingSymbols = 81,
        RequestContractData = 9,

        RequestExecutions = 7,
        RequestManagedAccounts = 17,

        RequestFA = 18,
        ReplaceFA = 19,

        ExerciseOptions = 21,

        [RequestCancellationCode(CancelImpliedVolatility)]
        ReqCalcImpliedVolat = 54,
        CancelImpliedVolatility = 56,

        [RequestCancellationCode(CancelOptionPrice)]
        ReqCalcOptionPrice = 55,
        CancelOptionPrice = 57,

        VerifyRequest = 65,
        VerifyMessage = 66,
        QueryDisplayGroups = 67,

        UpdateDisplayGroup = 69,

        [RequestCancellationCode(UnsubscribeFromGroupEvents)]
        SubscribeToGroupEvents = 68,
        UnsubscribeFromGroupEvents = 70,

        // ---------------
        // System
        // ---------------

        RequestIds = 8,
        ChangeServerLog = 14,
        RequestCurrentTime = 49,  // OK
        StartApi = 71,  // Done

        VerifyAndAuthRequest = 72,
        VerifyAndAuthMessage = 73,

        RequestSecurityDefinitionOptionalParameters = 78,
        RequestSoftDollarTiers = 79,
        RequestFamilyCodes = 80,

        RequestSmartComponents = 83,
        RequestNewsArticle = 84,
        RequestNewsProviders = 85,
        RequestHistoricalNews = 86,

        [RequestCancellationCode(CancelHeadTimestamp)]
        RequestHeadTimestamp = 87,
        CancelHeadTimestamp = 90,

        RequestMarketRule = 91,

        [RequestCancellationCode(CancelPnL)]
        ReqPnL = 92,
        CancelPnL = 93,

        [RequestCancellationCode(CancelPnLSingle)]
        ReqPnLSingle = 94,
        CancelPnLSingle = 95,
    }

    [AttributeUsage(AttributeTargets.Field), Serializable]
    public sealed class RequestCancellationCode : Attribute
    {
        public RequestCancellationCode(RequestType type, int ver = 1) { Type = type; Version = ver; }
        public RequestType Type { get; private set; }
        public int Version { get; private set; }
    }
}
