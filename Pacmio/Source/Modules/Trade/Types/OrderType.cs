/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using Pacmio.IB;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Pacmio
{
    /// <summary>
    /// Basic Orders
    /// For more information: https://interactivebrokers.github.io/tws-api/basic_orders.html
    /// </summary>
    [Serializable, DataContract]
    public enum OrderType : int
    {
        [EnumMember, Description("Unknown")]
        UNKNOWN = 0,

        /// <summary>
        /// A Market order is an order to buy or sell at the market bid or offer price. A market order may increase the likelihood of a fill and the speed of execution, 
        /// but unlike the Limit order a Market order provides no price protection and may fill at a price far lower/higher than the current displayed bid/ask.
        /// </summary>
        [EnumMember, ApiCode("MKT"), Description("Market")]
        Market = 10,

        /// <summary>
        /// A Market-to-Limit (MTL) order is submitted as a market order to execute at the current best market price.
        /// If the order is only partially filled, the remainder of the order is canceled and re-submitted as a limit order with 
        /// the limit price equal to the price at which the filled portion of the order executed.
        /// </summary>
        [EnumMember, ApiCode("MTL"), Description("Market Limit")]
        MarketLimit = 11,

        /// <summary>
        /// A Market if Touched (MIT) is an order to buy (or sell) an instrument below (or above) the market.
        /// Its purpose is to take advantage of sudden or unexpected changes in share or other prices and provides investors with a trigger price to set an order in motion.
        /// Investors may be waiting for excessive strength (or weakness) to cease, which might be represented by a specific price point.
        /// MIT orders can be used to determine whether or not to enter the market once a specific price level has been achieved.
        /// This order is held in the system until the trigger price is touched, and is then submitted as a market order.
        /// An MIT order is similar to a stop order, except that an MIT sell order is placed above the current market price, and a stop sell order is placed below.
        /// </summary>
        [EnumMember, ApiCode("MIT"), Description("Market If Touched")]
        MarketIfTouched = 12,

        /// <summary>
        /// A Limit order is an order to buy or sell at a specified price or better. 
        /// The Limit order ensures that if the order fills, 
        /// it will not fill at a price less favorable than your limit price, but it does not guarantee a fill.
        /// </summary>
        [EnumMember, ApiCode("LMT"), Description("Limit")]
        Limit = 13,

        /// <summary>
        /// A Stop order is an instruction to submit a buy or sell market order if and when the user-specified stop trigger price is attained or penetrated.
        /// A Stop order is not guaranteed a specific execution price and may execute significantly away from its stop price.
        /// A Sell Stop order is always placed below the current market price and is typically used to limit a loss or protect a profit on a long stock position.
        /// A Buy Stop order is always placed above the current market price.
        /// It is typically used to limit a loss or help protect a profit on a short sale.
        /// </summary>
        [EnumMember, ApiCode("STP"), Description("Stop")]
        Stop = 14,

        /// <summary>
        /// A Stop-Limit order is an instruction to submit a buy or sell limit order when the user-specified stop trigger price is attained or penetrated. 
        /// The order has two basic components: the stop price and the limit price. When a trade has occurred at or through the stop price, 
        /// the order becomes executable and enters the market as a limit order, which is an order to buy or sell at a specified price or better.
        /// </summary>
        [EnumMember, ApiCode("STP LMT"), Description("Stop Limit")]
        StopLimit = 15,

        /// <summary>
        /// A Limit if Touched is an order to buy (or sell) a contract at a specified price or better, below (or above) the market. 
        /// This order is held in the system until the trigger price is touched. 
        /// An LIT order is similar to a stop limit order, except that an LIT sell order is placed above the current market price, and a stop limit sell order is placed below.
        /// </summary>
        [EnumMember, ApiCode("LIT"), Description("Limit If Touched")]
        LimitIfTouched = 16,

        /// <summary>
        /// A sell trailing stop order sets the stop price at a fixed amount below the market price with an attached "trailing" amount.
        /// As the market price rises, the stop price rises by the trail amount, but if the stock price falls, the stop loss price doesn't change, and a market order is submitted when the stop price is hit.
        /// This technique is designed to allow an investor to specify a limit on the maximum possible loss, without setting a limit on the maximum possible gain. 
        /// "Buy" trailing stop orders are the mirror image of sell trailing stop orders, and are most appropriate for use in falling markets.
        /// Note that Trailing Stop orders can have the trailing amount specified as a percent, as in the example below, or as an absolute amount which is specified in the auxPrice field.
        /// </summary>
        [EnumMember, ApiCode("TRAIL"), Description("Trailing Stop")]
        TrailingStop = 20,

        /// <summary>
        /// A trailing stop limit order is designed to allow an investor to specify a limit on the maximum possible loss, without setting a limit on the maximum possible gain. 
        /// A SELL trailing stop limit moves with the market price, and continually recalculates the stop trigger price at a fixed amount below the market price, 
        /// based on the user-defined "trailing" amount. The limit order price is also continually recalculated based on the limit offset. 
        /// As the market price rises, both the stop price and the limit price rise by the trail amount and limit offset respectively, 
        /// but if the stock price falls, the stop price remains unchanged, and when the stop price is hit a limit order is submitted at the last calculated limit price. 
        /// A "Buy" trailing stop limit order is the mirror image of a sell trailing stop limit, and is generally used in falling markets.
        /// 
        /// Trailing Stop Limit orders can be sent with the trailing amount specified as an absolute amount, as in the example below,
        /// or as a percentage, specified in the trailingPercent field.
        /// 
        /// Important: the 'limit offset' field is set by default in the TWS/IBG settings in v963+. This setting either needs to be changed in the Order Presets, 
        /// the default value accepted, or the limit price offset sent from the API as in the example below. 
        /// Not both the 'limit price' and 'limit price offset' fields can be set in TRAIL LIMIT orders.
        /// </summary>
        [EnumMember, ApiCode("TRAIL LIMIT"), Description("Trailing Stop Limit")]
        TrailingStopLimit = 21,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember, ApiCode("MIDPRICE"), Description("IB Mid-Price Algo")]
        MidPrice = 50,
    }
}
