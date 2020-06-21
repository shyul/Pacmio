/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// BarTable Data Types
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Pacmio.IB;

namespace Pacmio
{
    [Serializable, DataContract]
    public class OrderInfo
    {
        [DataMember]
        public (string name, Exchange exchange, string typeName, int conId) ContractInfo { get; set; }

        [IgnoreDataMember]
        public Contract Contract
        {
            get
            {
                return m_Contract;
            }
            set
            {
                int conId = value.ConId;
                if (conId < 1) throw new Exception("ConId can't be zero for order contract.");
                ContractInfo = (value.Name, value.Exchange, value.TypeName, conId);
                m_Contract = value;
            }
        }

        [IgnoreDataMember]
        private Contract m_Contract;

        [DataMember]
        public string AccountCode { get; set; } = string.Empty;

        [IgnoreDataMember]
        public Account Account => AccountManager.Get(AccountCode);

        [DataMember]
        public double Quantity { get; set; } = double.NaN;

        /// <summary>
        /// The LIMIT price.
        /// Used for limit, stop-limit and relative orders. In all other cases specify zero. 
        /// For relative orders with no limit price, also specify zero.
        /// </summary>
        [DataMember]
        public double LimitPrice { get; set; } = double.NaN;

        /// <summary>
        /// Generic field to contain the stop price for STP LMT orders, trailing amount, etc.
        /// </summary>
        [DataMember]
        public double AuxPrice { get; set; } = double.NaN;

        /// <summary>
        /// Trail stop price for TRAILIMIT orders.
        /// </summary>
        [DataMember]
        public double TrailStopPrice { get; set; } = double.NaN;

        /// <summary>
        /// Specifies the trailing amount of a trailing stop order as a percentage.
        /// Observe the following guidelines when using the trailingPercent field:
        ///     - This field is mutually exclusive with the existing trailing amount. That is, the API client can send one or the other but not both.
        ///     - This field is read AFTER the stop price (barrier price) as follows: deltaNeutralAuxPrice stopPrice, trailingPercent, scale order attributes.
        ///     - The field will also be sent to the API in the openOrder message if the API client version is >= 56. 
        ///         It is sent after the stopPrice field as follows: stopPrice, trailingPct, basisPoint
        /// </summary>
        [DataMember]
        public double TrailingPercent { get; set; } = double.NaN;


        [DataMember]
        public OrderType Type { get; set; } = OrderType.MarketLimit;

        [DataMember]
        public bool Transmit { get; set; } = true;

        /// <summary>
        /// Indicates whether or not all the order has to be filled on a single execution.
        /// </summary>
        [DataMember]
        public bool AllOrNone { get; set; } = false;

        /// <summary>
        /// If set to true, the order will not be visible when viewing the market depth.
        /// This option only applies to orders routed to the ISLAND exchange.
        /// </summary>
        [DataMember]
        public bool Hidden { get; set; } = false;

        /// <summary>
        /// The time in force.
        /// Valid values are:
        ///     DAY - Valid for the day only.
        ///     GTC - Good until canceled. The order will continue to work within the system and in the marketplace until it executes or is canceled. 
        ///         GTC orders will be automatically be cancelled under the following conditions:
        ///         If a corporate action on a security results in a stock split (forward or reverse), exchange for shares, or distribution of shares.
        ///         If you do not log into your IB account for 90 days.
        ///         At the end of the calendar quarter following the current quarter. For example, an order placed during the third quarter of 2011 will be canceled at the end of the first quarter of 2012. If the last day is a non-trading day, the cancellation will occur at the close of the final trading day of that quarter. 
        ///         For example, if the last day of the quarter is Sunday, the orders will be cancelled on the preceding Friday.
        ///         Orders that are modified will be assigned a new “Auto Expire” date consistent with the end of the calendar quarter following the current quarter.
        ///         Orders submitted to IB that remain in force for more than one day will not be reduced for dividends. To allow adjustment to your order price on ex-dividend date, 
        ///         consider using a Good-Til-Date/Time (GTD) or Good-after-Time/Date (GAT) order type, or a combination of the two.
        ///     IOC - Immediate or Cancel. Any portion that is not filled as soon as it becomes available in the market is canceled.
        ///     GTD - Good until Date. It will remain working within the system and in the marketplace until it executes or until the close of the market on the date specified.
        ///     OPG - Use OPG to send a market-on-open (MOO) or limit-on-open (LOO) order.
        ///     FOK - If the entire Fill-or-Kill order does not execute as soon as it becomes available, the entire order is canceled.
        ///     DTC - Day until Canceled.
        /// </summary>
        [DataMember]
        public OrderTimeInForce TimeInForce { get; set; } = OrderTimeInForce.Day;

        [DataMember]
        public DateTime EffectiveDateTime { get; set; }

        /// <summary>
        /// If set to true, allows orders to also trigger or fill outside of regular trading hours.
        /// </summary>
        [DataMember]
        public bool OutsideRegularTradeHours { get; set; } = true;

        #region Identification Numbers

        /// <summary>
        /// Order id.
        /// </summary>
        [DataMember]
        public int OrderId { get; set; } = -1;

        /// <summary>
        /// The Host order identifier.
        /// </summary>
        [DataMember]
        public int PermId { get; set; } = -1;

        /*
        [DataMember]
        public string ExecId { get; set; } = string.Empty;
        */

        /// <summary>
        /// The order ID of the parent order, used for bracket and auto trailing stop orders.
        /// </summary>
        [DataMember]
        public int ParentOrderId { get; set; } = 0;

        /// <summary>
        /// The parent host order identifier.
        /// </summary>
        [DataMember]
        public int ParentPermId { get; set; } = -1;

        [DataMember]
        public int ClientId { get; set; } = -1;

        #endregion Identification Numbers

        #region Status

        [DataMember]
        public OrderStatus Status { get; set; } = OrderStatus.Inactive;

        public bool IsEditable => Status == OrderStatus.PreSubmitted || Status == OrderStatus.PendingSubmit || Status == OrderStatus.Submitted;

        [DataMember]
        public double FilledQuantity { get; set; } = 0;

        [DataMember]
        public double RemainingQuantity { get; set; } = 0;

        [DataMember]
        public double LastFillPrice { get; set; } = double.NaN;

        [DataMember]
        public double AveragePrice { get; set; } = double.NaN;

        [DataMember]
        public double MarketCapPrice { get; set; } = double.NaN;

        [DataMember]
        public string HeldNotice { get; set; } = string.Empty;

        #endregion

        [DataMember, Browsable(true)]
        public string ModeCode { get; set; } = string.Empty;

        [DataMember, Browsable(true)]
        public string Description { get; set; }

        [DataMember]
        public DateTime OrderTime { get; set; }
    }
}
