/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************


namespace Pacmio.IB
{
    /// <summary>
    /// https://interactivebrokers.github.io/tws-api/tick_types.html
    /// </summary>
    public enum TickType : int
    {
        Unknown = -1,

        BidSize = 0, // Number of contracts or lots offered at the bid price.	IBApi.EWrapper.tickSize	-
        BidPrice = 1, // Highest priced bid for the contract.	IBApi.EWrapper.tickPrice	-
        AskPrice = 2, // Lowest price offer on the contract.	IBApi.EWrapper.tickPrice	-
        AskSize = 3, // Number of contracts or lots offered at the ask price.	IBApi.EWrapper.tickSize	-
        LastPrice = 4, // Last price at which the contract traded (does not include some trades in RTVolume).	IBApi.EWrapper.tickPrice	-
        LastSize = 5, // Number of contracts or lots traded at the last price.	IBApi.EWrapper.tickSize	-
        High = 6, // High price for the day.	IBApi.EWrapper.tickPrice	-
        Low = 7, // Low price for the day.	IBApi.EWrapper.tickPrice	-
        Volume = 8, // Trading volume for the day for the selected contract (US Stocks: multiplier 100).	IBApi.EWrapper.tickSize	-
        LastClose = 9, // The last available closing price for the previous day. For US Equities, we use corporate action processing to get the closing price, so the close price is adjusted to reflect forward and reverse splits and cash and stock dividends.	IBApi.EWrapper.tickPrice	-
        BidOptionComputation = 10, // Computed Greeks and implied volatility based on the underlying stock price and the option bid price. See Option Greeks	IBApi.EWrapper.tickOptionComputation	-
        AskOptionComputation = 11, // Computed Greeks and implied volatility based on the underlying stock price and the option ask price. See Option Greeks	IBApi.EWrapper.tickOptionComputation	-
        LastOptionComputation = 12, // Computed Greeks and implied volatility based on the underlying stock price and the option last traded price. See Option Greeks	IBApi.EWrapper.tickOptionComputation	-
        ModelOptionComputation = 13, // Computed Greeks and implied volatility based on the underlying stock price and the option model price. Correspond to greeks shown in TWS. See Option Greeks	IBApi.EWrapper.tickOptionComputation	-
        Open = 14, // Today's opening price. The official opening price requires a market data subscription to the native exchange of a contract.	IBApi.EWrapper.tickPrice	-
        Low13Weeks = 15, // Lowest price for the last 13 weeks. For stocks only.	IBApi.EWrapper.tickPrice	165
        High13Weeks = 16, // Highest price for the last 13 weeks. For stocks only.	IBApi.EWrapper.tickPrice	165
        Low26Weeks = 17, // Lowest price for the last 26 weeks. For stocks only.	IBApi.EWrapper.tickPrice	165
        High26Weeks = 18, // Highest price for the last 26 weeks. For stocks only.	IBApi.EWrapper.tickPrice	165
        Low52Weeks = 19, // Lowest price for the last 52 weeks. For stocks only.	IBApi.EWrapper.tickPrice	165
        High52Weeks = 20, // Highest price for the last 52 weeks. For stocks only.	IBApi.EWrapper.tickPrice	165
        AverageVolume = 21, // The average daily trading volume over 90 days. Multiplier of 100. For stocks only.	IBApi.EWrapper.tickSize	165
        OpenInterest = 22, // (Deprecated, not currently in use) Total number of options that are not closed.	IBApi.EWrapper.tickSize	-
        OptionHistoricalVolatility = 23, // The 30-day historical volatility (currently for stocks).	IBApi.EWrapper.tickGeneric	104
        OptionImpliedVolatility = 24, // A prediction of how volatile an underlying will be in the future. The IB 30-day volatility is the at-market volatility estimated for a maturity thirty calendar days forward of the current trading day, and is based on option prices from two consecutive expiration months.	IBApi.EWrapper.tickGeneric	106
        OptionBidExchange = 25, // Not Used.	IBApi.EWrapper.tickString	-
        OptionAskExchange = 26, // Not Used.	IBApi.EWrapper.tickString	-
        OptionCallOpenInterest = 27, // Call option open interest.	IBApi.EWrapper.tickSize	101
        OptionPutOpenInterest = 28, // Put option open interest.	IBApi.EWrapper.tickSize	101
        OptionCallVolume = 29, // Call option volume for the trading day.	IBApi.EWrapper.tickSize	100
        OptionPutVolume = 30, // Put option volume for the trading day.	IBApi.EWrapper.tickSize	100
        IndexFuturePremium = 31, // The number of points that the index is over the cash index.	IBApi.EWrapper.tickGeneric	162
        BidExchange = 32, // For stock and options, identifies the exchange(s) posting the bid price. See Component Exchanges	IBApi.EWrapper.tickString	-
        AskExchange = 33, // For stock and options, identifies the exchange(s) posting the ask price. See Component Exchanges	IBApi.EWrapper.tickString	-
        AuctionVolume = 34, // The number of shares that would trade if no new orders were received and the auction were held now.	IBApi.EWrapper.tickSize	225
        AuctionPrice = 35, // The price at which the auction would occur if no new orders were received and the auction were held now- the indicative price for the auction. Typically received after Auction imbalance (tick type 36)	IBApi.EWrapper.tickPrice	225
        AuctionImbalance = 36, // The number of unmatched shares for the next auction; returns how many more shares are on one side of the auction than the other. Typically received after Auction Volume (tick type 34)	IBApi.EWrapper.tickSize	225
        MarkPrice = 37, // The mark price is the current theoretical calculated value of an instrument. Since it is a calculated value, it will typically have many digits of precision.	IBApi.EWrapper.tickPrice	221 or 232
        BidEFPComputation = 38, // Computed EFP bid price	IBApi.EWrapper.tickEFP	-
        AskEFPComputation = 39, // Computed EFP ask price	IBApi.EWrapper.tickEFP	-
        LastEFPComputation = 40, // Computed EFP last price	IBApi.EWrapper.tickEFP	-
        OpenEFPComputation = 41, // Computed EFP open price	IBApi.EWrapper.tickEFP	-
        HighEFPComputation = 42, // Computed high EFP traded price for the day	IBApi.EWrapper.tickEFP	-
        LowEFPComputation = 43, // Computed low EFP traded price for the day	IBApi.EWrapper.tickEFP	-
        CloseEFPComputation = 44, // Computed closing EFP price for previous day	IBApi.EWrapper.tickEFP	-
        LastTimestamp = 45, // Time of the last trade (in UNIX time).	IBApi.EWrapper.tickString	-
        Shortable = 46, // Describes the level of difficulty with which the contract can be sold short. See Shortable	IBApi.EWrapper.tickGeneric	236

        FundamentalRatios = 47, // Provides the available Reuter's Fundamental Ratios. See Fundamental Ratios 
                                // Note: For some tags the value returned may be -99999.99. This is Reuters' way of indicating data is not available for that tag for the company.
                                // Also see Fundamental Data for requesting other fundamental data type using IBApi.EClient.reqFundamentalData.	IBApi.EWrapper.tickString	258

        RTVolumeTimeSales = 48, // Last trade details (Including both "Last" and "Unreportable Last" trades). See RT Volume	IBApi.EWrapper.tickString	233
        Halted = 49, // Indicates if a contract is halted. See Halted	IBApi.EWrapper.tickGeneric	-
        BidYield = 50, // Implied yield of the bond if it is purchased at the current bid.	IBApi.EWrapper.tickPrice	-
        AskYield = 51, // Implied yield of the bond if it is purchased at the current ask.	IBApi.EWrapper.tickPrice	-
        LastYield = 52, // Implied yield of the bond if it is purchased at the last price.	IBApi.EWrapper.tickPrice	-
        CustomOptionComputation = 53, // Greek values are based off a user customized price.	IBApi.EWrapper.tickOptionComputation	-
        TradeCount = 54, // Trade count for the day.	IBApi.EWrapper.tickGeneric	293
        TradeRate = 55, // Trade count per minute.	IBApi.EWrapper.tickGeneric	294
        VolumeRate = 56, // Volume per minute.	IBApi.EWrapper.tickGeneric	295
        LastRTHTrade = 57, // Last Regular Trading Hours traded price.	IBApi.EWrapper.tickPrice	318
        RTHistoricalVolatility = 58, // 30-day real time historical volatility.	IBApi.EWrapper.tickGeneric	411
        IBDividends = 59, // Contract's dividends. See IB Dividends	IBApi.EWrapper.tickString	456
        BondFactorMultiplier = 60, // Not currently implemented.		-
        RegulatoryImbalance = 61, // The imbalance that is used to determine which at-the-open or at-the-close orders can be entered following the publishing of the regulatory imbalance.	IBApi.EWrapper.tickSize	-
        News = 62, // Contract's news feed. IBApi.EWrapper.tickString	292
        ShortTermVolume3Minutes = 63, // The past three minutes volume. Interpolation may be applied. For stocks only.	IBApi.EWrapper.tickSize	595
        ShortTermVolume5Minutes = 64, // The past five minutes volume. Interpolation may be applied. For stocks only.	IBApi.EWrapper.tickSize	595
        ShortTermVolume10Minutes = 65, // The past ten minutes volume. Interpolation may be applied. For stocks only.	IBApi.EWrapper.tickSize	595
        DelayedBid = 66, // Delayed bid price. See Market Data Types	IBApi.EWrapper.tickPrice	-
        DelayedAsk = 67, // Delayed ask price. See Market Data Types	IBApi.EWrapper.tickPrice	-
        DelayedLast = 68, // Delayed last traded price. See Market Data Types	IBApi.EWrapper.tickPrice	-
        DelayedBidSize = 69, // Delayed bid size. See Market Data Types	IBApi.EWrapper.tickSize	-
        DelayedAskSize = 70, // Delayed ask size. See Market Data Types	IBApi.EWrapper.tickSize	-
        DelayedLastSize = 71, // Delayed last size. See Market Data Types	IBApi.EWrapper.tickSize	-
        DelayedHighPrice = 72, // Delayed highest price of the day. See Market Data Types	IBApi.EWrapper.tickPrice	-
        DelayedLowPrice = 73, // Delayed lowest price of the day. See Market Data Types	IBApi.EWrapper.tickPrice	-
        DelayedVolume = 74, // Delayed traded volume of the day. See Market Data Types	IBApi.EWrapper.tickSize	-
        DelayedClose = 75, // The prior day's closing price.	IBApi.EWrapper.tickPrice	-
        DelayedOpen = 76, // Not typically available	IBApi.EWrapper.tickPrice	-
        RTTradeVolume = 77, // Last trade details that excludes "Unreportable Trades". See RT Trade Volume	IBApi.EWrapper.tickString	375
        CreditmanMarkPrice = 78, // Not currently available	IBApi.EWrapper.tickPrice	
        CreditmanSlowMarkPrice = 79, // Slower mark price update used in system calculations	IBApi.EWrapper.tickPrice	619
        DelayedBidOption = 80, // Computed greeks based on delayed bid price. See Market Data Types and Option Greeks	IBApi.EWrapper.tickPrice	
        DelayedAskOption = 81, // Computed greeks based on delayed ask price. See Market Data Types and Option Greeks	IBApi.EWrapper.tickPrice	
        DelayedLastOption = 82, // Computed greeks based on delayed last price. See Market Data Types and Option Greeks	IBApi.EWrapper.tickPrice	
        DelayedModelOption = 83, // Computed Greeks and model's implied volatility based on delayed stock and option prices.	IBApi.EWrapper.tickPrice	
        LastExchange = 84, // Exchange of last traded price	IBApi.EWrapper.tickString	
        LastRegulatoryTime = 85, // Timestamp (in Unix ms time) of last trade returned with regulatory snapshot	IBApi.EWrapper.tickString	
        FuturesOpenInterest = 86, // Total number of outstanding futures contracts (TWS v965+). *HSI open interest requested with generic tick 101	IBApi.EWrapper.tickSize	588
        AverageOptionVolume = 87, // Average volume of the corresponding option contracts(TWS Build 970+ is required)	IBApi.EWrapper.tickSize	105
        DelayedLastTimeStamp = 88, // Delayed time of the last trade (in UNIX time) (TWS Build 970+ is required)	IBApi.EWrapper.tickString	
        ShortableShares = 89, // Number of shares available to short (TWS Build 974+ is required)
    }
}
