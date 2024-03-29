﻿/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// BarTable Data Types
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Xu;
using Xu.GridView;

namespace Pacmio
{
    [Serializable, DataContract]
    public class OrderInfo : IDataProvider, IEquatable<OrderInfo>, IEquatable<ExecutionInfo>, IEquatable<Contract>, IEquatable<(string name, Exchange exchange, string typeName)>
    {
        public OrderInfo()//StrategyDatum sd) 
        {


        }

        public OrderInfo(StrategyDatum sd, double absoluteQty)
        {

            StrategyDatum = sd;
            Contract = StrategyDatum.Contract;


        }

        [IgnoreDataMember]
        public StrategyDatum StrategyDatum { get; }

        #region Data Provider

        [DataMember]
        public DateTime UpdateTime { get; protected set; } = DateTime.MinValue;

        [IgnoreDataMember]
        public List<IDataConsumer> DataConsumers { get; private set; }

        public bool AddDataConsumer(IDataConsumer idk)
        {
            if (DataConsumers is null) DataConsumers = new List<IDataConsumer>();
            return DataConsumers.CheckAdd(idk);
        }

        public bool RemoveDataConsumer(IDataConsumer idk)
        {
            return (DataConsumers is not null) ? DataConsumers.CheckRemove(idk) : false;
        }

        public void Updated()
        {
            UpdateTime = DateTime.Now;
            DataConsumers?.ForEach(n => n.DataIsUpdated(this));
        }

        #endregion Data Provider

        #region Identification Numbers

        /// <summary>
        /// Order id.
        /// </summary>
        [DataMember]
        public int OrderId { get; set; } = -1;

        /// <summary>
        /// The Host order identifier.
        /// </summary>
        [DataMember, Browsable(true), ReadOnly(true), DisplayName("PermId"), GridColumnOrder(6), GridRenderer(typeof(NumberGridRenderer), 150)]
        public int PermId { get; set; } = -1;

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

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Description"), GridColumnOrder(0, 1, 0), GridRenderer(typeof(TextGridRenderer), 100, true)]
        public string Description { get; set; }

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("AccountId"), GridColumnOrder(1), GridRenderer(typeof(TextGridRenderer), 100)]
        public string AccountId { get; set; } = null;

        [IgnoreDataMember]
        public AccountInfo Account
        {
            get
            {
                if (AccountId is string id)
                {
                    if (!(m_Account is AccountInfo ac && ac == id))
                    {
                        m_Account = AccountPositionManager.GetAccountById(AccountId);
                    }

                    return m_Account;
                }

                else
                    return null;
            }

            set
            {
                m_Account = value;

                if (m_Account is AccountInfo ac)
                    AccountId = ac.AccountId;
                else
                    AccountId = null;
            }
        }

        [IgnoreDataMember]
        private AccountInfo m_Account = null;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("ConId"), GridColumnOrder(2), GridRenderer(typeof(NumberGridRenderer), 100)]
        public int ConId { get; set; } = -1;

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Contract"), GridColumnOrder(1, 0, 0), GridRenderer(typeof(ContractGridRenderer), 150, true)]
        public Contract Contract
        {
            get
            {
                if (ConId > 0)
                {
                    if (!(m_Contract is Contract c && c.ConId == ConId))
                    {
                        m_Contract = ContractManager.GetOrFetch(ConId);
                    }

                    return m_Contract;
                }

                else
                    return null;
            }

            set
            {
                m_Contract = value;

                if (m_Contract is Contract c)
                    ConId = c.ConId;
                else
                    ConId = -1;
            }
        }

        [IgnoreDataMember]
        private Contract m_Contract = null;

        #region Control

        [DataMember]
        public bool Transmit { get; set; } = true;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Mode Code"), GridColumnOrder(3), GridRenderer(typeof(TextGridRenderer), 100)]
        public string ModeCode { get; set; } = string.Empty;

        #endregion Control

        #region Order Setting

        [DataMember, Browsable(true)]
        public OrderType Type { get; set; } = OrderType.MarketLimit;

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

        #endregion Order Setting

        #region Timing Setting

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
        public DateTime LimitedEffectiveTime { get; set; }

        /// <summary>
        /// If set to true, allows orders to also trigger or fill outside of regular trading hours.
        /// </summary>
        [DataMember]
        public bool OutsideRegularTradeHours { get; set; } = true;

        #endregion Timing Setting

        #region Result

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Order Time"), GridColumnOrder(3), GridRenderer(typeof(TextGridRenderer), 300)]
        public DateTime OrderExecuteTime { get; set; } = TimeTool.MaxInvalid;

        [DataMember, Browsable(true)]
        public OrderStatus Status { get; set; } = OrderStatus.Inactive;

        [IgnoreDataMember]
        public bool IsEditable => Status == OrderStatus.Inactive || Status == OrderStatus.ApiPending || Status == OrderStatus.PendingSubmit || Status == OrderStatus.PreSubmitted || Status == OrderStatus.Submitted;

        [DataMember]
        public double Quantity { get; set; } = double.NaN;

        [DataMember, Browsable(true)]
        public double FilledQuantity { get; set; } = 0;

        [DataMember, Browsable(true)]
        public double RemainingQuantity { get; set; } = 0;

        [DataMember]
        public double LastFillPrice { get; set; } = double.NaN;

        [DataMember, Browsable(true)]
        public double AveragePrice { get; set; } = double.NaN;

        [DataMember]
        public double MarketCapPrice { get; set; } = double.NaN;

        [DataMember]
        public string HeldNotice { get; set; } = string.Empty;

        #endregion Result

        #region Equality

        public override int GetHashCode() => PermId.GetHashCode();

        public override bool Equals(object other)
        {
            if (other is OrderInfo od)
                return Equals(od);
            else if (other is ExecutionInfo tld)
                return Equals(tld);
            else if (other is Contract c)
                return Equals(c);
            else if (other is Tuple<string, Exchange, string> info)
                return Equals(info);
            else
                return false;
        }

        public bool Equals(OrderInfo other)
        {
            if (PermId > 0)
                return PermId == other.PermId;
            else if (OrderId > 0)
                return OrderId == other.OrderId;
            else
                return Contract == other.Contract;
        }

        public bool Equals(ExecutionInfo other) => PermId == other.PermId;

        public bool Equals(Contract other) => (ConId > 0 && ConId == other.ConId);

        public bool Equals((string name, Exchange exchange, string typeName) other) => Contract.Key == other;

        public static bool operator ==(OrderInfo left, OrderInfo right) => left.PermId == right.PermId;
        public static bool operator !=(OrderInfo left, OrderInfo right) => !(left == right);

        public static bool operator ==(OrderInfo left, ExecutionInfo right) => left.PermId == right.PermId;
        public static bool operator !=(OrderInfo left, ExecutionInfo right) => !(left == right);

        #endregion Equality
    }
}
