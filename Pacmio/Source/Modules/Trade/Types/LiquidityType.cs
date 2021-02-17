/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Runtime.Serialization;

namespace Pacmio
{
    [Serializable, DataContract]
    public enum LiquidityType : int
    {
        [EnumMember]
        None = 0,

        [EnumMember]
        Added = 1,

        [EnumMember]
        Removed = 2,

        /// <summary>
        /// The initial use for this field was for markets that operated continuous limit order books.
        /// 
        /// I agree that the language of the field definition is awkward. Obviously, there are more than two enums, 
        /// so orders can’t be classified as only adding or taking liquidity.
        /// 
        /// Although I can see how it might not be explicit, my understanding is that LastLiquidityInd is determined from the perspective of the person placing the order.
        /// If I enter an order that stays on the book and adds liquidity, I’ll get a 1 back when it trades.
        /// The person who entered an order that hit my order on the book will have removed liquidity and will get a 2.
        /// 
        /// This can be of importance if the market’s pricing structure differentiates between adding or removing liquidity.
        /// Some markets charge a lower fee or provide a rebate for orders that add liquidity.
        /// 
        /// Within the US equities markets, Reg NMS generally prohibits one market from trading through another market’s published quote.
        /// So let’s say I send an order to Market A to buy at the national best offer. Market A doesn’t have any orders at that price.
        /// And my order is not an intermarket sweep. Market A may then send a buy order to Market B, who is displaying that price.
        /// Market A will have removed liquidity from Market B. But I will get a 3 back because my order was routed out of Market A to that other market.
        /// This is useful to know because Market A may charge me a routing fee or a higher rate.
        /// 
        /// In theory, for continuous limit books, orders should fall into one of these three categories.
        /// “Routing in” doesn’t have useful meaning to me. If I add an order to Market A’s book, it doesn’t matter to me if a member of Market A, or of Market B routing into Market A, traded against me.
        /// I’m still adding liquidity either way.
        /// Now there can occasionally be “gotcha” scenarios. E.g. one person pegs to buy at the bid, and another pegs to sell at the offer minus 1 cent.
        /// Both orders are entered when the spread was 2 cents, hence both stay on the book. The spread tightens to 1 cent, and the orders trade against each other.
        /// So who added and who removed liquidity?
        /// 
        /// Stepping outside continuous markets, more enumerations may be needed. Auction is one of these.
        /// A market might have an opening auction prior to continuous trading, where the market accumulates buy and sell orders,
        /// and at a given time the auction happens, determining a price and matching the buyers and sellers.
        /// In this case, it might not be appropriate to designate who added vs. who removed liquidity, as both parties were contributing orders to the auction, so a value of 4 would be appropriate.
        /// If you have a business need for more values, such as “negotiated cross”, these could be handled by submitting a Gap Analysis requesting them to FPL.
        /// 
        /// https://forum.fixtrading.org/t/liquidity-routed-out/5457/4
        /// </summary>
        [EnumMember]
        RoutedOut = 3
    }
}
