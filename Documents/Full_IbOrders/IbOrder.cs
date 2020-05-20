/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - shyu.lee@gmail.com
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;
using Xu;

namespace Pacmio.IB
{
    public partial class Client
    {
        public void PlaceOrder(Contract c, IbOrder order, bool useSmart = true)
        {
            var (valid_secType, secTypeCode) = ApiCode.GetIbCode(c.Type);
            var (valid_exchange, exchangeCode) = ApiCode.GetIbCode(c.Exchange);

            Console.WriteLine("PlaceOrder: " + c.ToString());

            if (Connected && valid_exchange && valid_secType && c is ITradable it)// && c.IsValid) // Also please check the RHD is active?
            {

                Console.WriteLine("Working on PlaceOrder: " + c.ToString());

                (int requestId, string requestType) = RegisterRequest(RequestType.PlaceOrder);

                string lastTradeDateOrContractMonth = "";
                double strike = 0;
                string right = "";
                string multiplier = "";

                if (c is IOption opt)
                {
                    lastTradeDateOrContractMonth = opt.LastTradeDateOrContractMonth;
                    strike = opt.Strike;
                    right = opt.Right;
                    multiplier = opt.Multiplier;
                }

                List<string> paramsList = new List<string>() {
                    requestType, // 0
                    requestId.ParamPos(), // 1

                    c.ConId.Param(), // 2
                    c.Name, // 3
                    secTypeCode, // 4
                    
                    lastTradeDateOrContractMonth, // 5
                    (strike == 0) ? "0" : strike.ToString("0.0###"), // 6
                    right, // 7
                    multiplier, // 8
                    useSmart ? "SMART" : exchangeCode, // 9 "ISLAND" exchange,
                    exchangeCode, // 10, primaryExch,
                    c.CurrencyCode, // 11, USD / currency,
                    "", // c.LocalSymbol, // 12
                    "", // c.TradingClass, // 13

                    "ISIN", // 14, ISIN
                    it.ISIN, // 15

                    order.Action, // 16
                    order.TotalQuantity.Param(), // 17, double type
                    order.OrderType, // 18
                    order.LimitPrice.Param(), // 19
                    order.AuxPrice.Param(), // 20
                    
                    order.TimeInForce, // 21
                    order.OneCancelsAllGroup, // 22
                    order.Account, // 23
                    order.OpenClose, // 24
                    order.Origin.Param(), // 25
                    order.OrderRef, // 26
                    order.Transmit.Param(), // 27
                    order.ParentId.Param(), // 28
                    order.BlockOrder.Param(), // 29
                    order.SweepToFill.Param(), // 30
                    order.DisplaySize.Param(), // 31
                    order.TriggerMethod.Param(), // 32
                    order.OutsideRegularTradeHours.Param(), // 33
                    order.Hidden.Param() // 34
                };

                // *** BAG order *** Ignored
                // *** BAG order ***
                // bool isBag = StringsAreEqual(Constants.BagSecType, contract.SecType);
                if (c is ICombo ic)
                {
                    // Record the combo legs from contract
                    if (ic.ComboLegs is null)
                    {
                        paramsList.Add("0"); // 35
                    }
                    else
                    {
                        paramsList.Add(ic.ComboLegs.Count.ParamPos()); // 35
                        foreach (ComboLeg leg in ic.ComboLegs)
                        {
                            paramsList.AddRange(new string[] {
                                leg.ConId.ParamPos(), // 35 + 1 + 8 x leg
                                leg.Ratio.Param(), // 35 + 2 + 8 x leg
                                leg.Action, // 35 + 3 + 8 x leg
                                leg.Exchange, // 35 + 4 + 8 x leg
                                leg.OpenClose.Param(), // 35 + 5 + 8 x leg
                                leg.ShortSaleSlot.Param(), // 35 + 6 + 8 x leg
                                leg.DesignatedLocation, // 35 + 7 + 8 x leg
                                leg.ExemptCode.Param(), // 35 + 8 + 8 x leg
                            });
                        }
                    }

                    // add order combo legs for BAG requests
                    if (order.OrderComboLegs is null)
                    {
                        paramsList.Add("0");
                    }
                    else
                    {
                        paramsList.Add(order.OrderComboLegs.Count.ParamPos());
                        foreach (OrderComboLeg leg in order.OrderComboLegs)
                        {
                            paramsList.Add(leg.Price.Param());
                        }
                    }

                    // Add Smart Combo Routing Parameters
                    if (order.SmartComboRoutingParams is null)
                    {
                        paramsList.Add("0");
                    }
                    else
                    {
                        paramsList.Add(order.SmartComboRoutingParams.Count.ParamPos());
                        foreach ((string Tag, string Value) in order.SmartComboRoutingParams)
                        {
                            paramsList.Add(Tag);
                            paramsList.Add(Value);
                        }
                    }
                }

                paramsList.AddRange(new string[] {
                    string.Empty, // 35 Deprecated sharesAllocation field
                    order.DiscretionaryAmt.Param(), // 36 "0"
                    order.GoodAfterTime, // 37
                    order.GoodTillDate, // 38

                    // add FA information
                    order.FaGroup, // 39
                    order.FaMethod, // 40
                    order.FaPercentage, // 41
                    order.FaProfile, // 42
                    order.ModelCode, // 43

                    // institutional short sale slot fields.
                    order.ShortSaleSlot.Param(),  // 44 // 0 only for retail, 1 or 2 only for institution.
                    order.DesignatedLocation,  // 45 // only populate when order.shortSaleSlot = 2.
                    order.ExemptCode.Param(), // 46

                    order.OneCancelsAllType.Param(), // 47
                    order.Rule80A, // 48
                    order.SettlingFirm, // 49
                    order.AllOrNone.Param(), // 50
                    order.MinQty.ParamPos(), // 51
                    order.PercentOffset.Param(), // 52
                    order.ETradeOnly.Param(), // 53
                    order.FirmQuoteOnly.Param(), // 54
                    order.NbboPriceCap.Param(), // 55
                    order.AuctionStrategy.ParamPos(), // 56
                    order.StartingPrice.Param(), // 57
                    order.StockRefPrice.Param(), // 58
                    order.Delta.Param(), // 59
                    order.StockRangeLower.Param(), // 60
                    order.StockRangeUpper.Param(), // 61

                    order.OverridePercentageConstraints.Param(), // 62

                    order.Volatility.Param(), // 63
                    order.VolatilityType.ParamPos(), // 64

                    // DeltaNeutral
                    order.DeltaNeutralOrderType, // 65
                    order.DeltaNeutralAuxPrice.Param(), // 66
                });

                if (!string.IsNullOrEmpty(order.DeltaNeutralOrderType))
                {
                    paramsList.AddRange(new string[] {
                        order.DeltaNeutralConId.Param(),
                        order.DeltaNeutralSettlingFirm,
                        order.DeltaNeutralClearingAccount,
                        order.DeltaNeutralClearingIntent,
                        order.DeltaNeutralOpenClose,
                        order.DeltaNeutralShortSale.Param(),
                        order.DeltaNeutralShortSaleSlot.ParamPos(),
                        order.DeltaNeutralDesignatedLocation
                    });
                }

                paramsList.Add(order.ContinuousUpdate.Param()); // 67
                paramsList.Add(order.ReferencePriceType.ParamPos()); // 68
                paramsList.Add(order.TrailStopPrice.Param()); // 69
                paramsList.Add(order.TrailingPercent.Param()); // 70

                // *** Scale Order ***
                paramsList.Add(order.ScaleInitLevelSize.ParamPos()); // 71
                paramsList.Add(order.ScaleSubsLevelSize.ParamPos()); // 72
                paramsList.Add(order.ScalePriceIncrement.Param()); // 73

                if (!double.IsNaN(order.ScalePriceIncrement) && order.ScalePriceIncrement > 0)
                {
                    paramsList.Add(order.ScalePriceAdjustValue.Param());
                    paramsList.Add(order.ScalePriceAdjustInterval.ParamPos());
                    paramsList.Add(order.ScaleProfitOffset.Param());
                    paramsList.Add(order.ScaleAutoReset.Param());
                    paramsList.Add(order.ScaleInitPosition.ParamPos());
                    paramsList.Add(order.ScaleInitFillQty.ParamPos());
                    paramsList.Add(order.ScaleRandomPercent.Param());
                }

                paramsList.Add(order.ScaleTable); // 74
                paramsList.Add(order.ActiveStartTime); // 75
                paramsList.Add(order.ActiveStopTime); // 76

                paramsList.Add(order.HedgeType); // 77
                if (!string.IsNullOrEmpty(order.HedgeType))
                {
                    paramsList.Add(order.HedgeParam);
                }

                paramsList.Add(order.OptOutSmartRouting.Param()); // 78

                paramsList.Add(order.ClearingAccount); // 79
                paramsList.Add(order.ClearingIntent); // 80

                paramsList.Add(order.NotHeld.Param()); // 81

                paramsList.Add("0"); // 82: DeltaNeutralContract

                // *** Algo ***
                paramsList.Add(order.AlgoStrategy); // 83
                if (!string.IsNullOrEmpty(order.AlgoStrategy))
                {
                    if (order.AlgoParams is null)
                    {
                        paramsList.Add("0");
                    }
                    else
                    {
                        paramsList.Add(order.AlgoParams.Count.ParamPos());
                        foreach ((string Tag, string Value) in order.AlgoParams)
                        {
                            paramsList.Add(Tag);
                            paramsList.Add(Value);
                        }
                    }
                }

                paramsList.Add(order.AlgoId); // 84
                paramsList.Add(order.WhatIf.Param()); // 85
                paramsList.Add(order.OrderMiscOptions.Param()); // 86
                paramsList.Add(order.Solicited.Param()); // 87
                paramsList.Add(order.RandomizeSize.Param()); // 88
                paramsList.Add(order.RandomizePrice.Param()); // 89

                // PEG BENCH
                if (order.OrderType == "PEG BENCH")
                {
                    paramsList.AddRange(new string[] {
                        order.ReferenceContractId.ParamPos(),
                        order.IsPeggedChangeAmountDecrease.Param(),
                        order.PeggedChangeAmount.Param(),
                        order.ReferenceChangeAmount.Param(),
                        order.ReferenceExchange});
                }

                paramsList.Add(order.Conditions.Param);  // 90 // order.Conditions.Count

                paramsList.AddRange(new string[] {
                    order.AdjustedOrderType, // 91
                    order.TriggerPrice.Param(), // 92
                    order.LimitPriceOffset.Param(), // 93
                    order.AdjustedStopPrice.Param(), // 94
                    order.AdjustedStopLimitPrice.Param(), // 95
                    order.AdjustedTrailingAmount.Param(), // 96
                    order.AdjustableTrailingUnit.Param(), // 97

                    order.ExtOperator, // 98

                    order.SoftDollarTierName, // 99
                    order.SoftDollarTierValue, // 100

                    order.CashQty.Param(), // 101

                    order.Mifid2DecisionMaker, // 102
                    order.Mifid2DecisionAlgo, // 103
                    order.Mifid2ExecutionTrader, // 104
                    order.Mifid2ExecutionAlgo, // 105

                    order.DontUseAutoPriceForHedge.Param(), // 106

                    order.IsOmsContainer.Param(), // 107

                    order.DiscretionaryUpToLimitPrice.Param(), // 108
                    order.UsePriceMgmtAlgo.Param()}); // 109

                SendRequest(paramsList);
            }
        }



    }

    public interface IOrder
    {
        int Id { get; }

        int PermId { get; }

        bool Equals(IOrder other);
    }

    public class IbOrder : IOrder, IEquatable<IOrder>, IEquatable<IbOrder>
    {
        /// <summary>
        /// Order id.
        /// </summary>
        public int Id { get; set; } = -1;

        /// <summary>
        /// The order ID of the parent order, used for bracket and auto trailing stop orders.
        /// </summary>
        public int ParentId { get; set; } = 0;

        /// <summary>
        /// The Host order identifier.
        /// </summary>
        public int PermId { get; set; } = -1;

        /// <summary>
        /// The Host order identifier.
        /// </summary>
        public int ParentPermId { get; set; } = -1;

        /// <summary>
        /// The account the trade will be allocated to.
        /// </summary>
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// The order reference.
        /// Intended for institutional customers only,
        /// although all customers may use it to identify the API client that sent the order when multiple API clients are running.
        /// </summary>
        public string OrderRef { get; set; } = string.Empty;

        /// <summary>
        /// Specifies whether the order will be transmitted by TWS. If set to false, the order will be created at TWS but will not be sent.
        /// </summary>
        public bool Transmit { get; set; } = true;

        /// <summary>
        /// The order's origin.
        /// Same as TWS "Origin" column. Identifies the type of customer from which the order originated.
        /// Valid values are 0 (customer), 1 (firm).
        /// </summary>
        public int Origin { get; set; } = 0;

        /// <summary>
        /// Identifies the side.
        /// Generally available values are BUY, SELL.
        /// Additionally, SSHORT, SLONG are available in some institutional-accounts only.
        /// For general account types, a SELL order will be able to enter a short position automatically if the order quantity is larger than your current long position.
        /// SSHORT is only supported for institutional account configured with Long/Short account segments or clearing with a separate account.
        /// SLONG is available in specially-configured institutional accounts to indicate that long position not yet delivered is being sold.
        /// </summary>
        public string Action { get; set; } = string.Empty;

        /// <summary>
        /// For institutional customers only. Valid values are O (open), C (close).
        /// Available for institutional clients to determine if this order is to open or close a position.
        /// When Action = "BUY" and OpenClose = "O" this will open a new position.
        /// When Action = "BUY" and OpenClose = "C" this will close and existing short position.
        /// </summary>
        public string OpenClose { get; set; } = string.Empty; //"O";

        /// <summary>
        /// The number of positions being bought/sold.
        /// </summary>
        public double TotalQuantity { get; set; }

        /// <summary>
        /// Identifies a minimum quantity order type.
        /// </summary>
        public int MinimumQty { get; set; } = -1;

        /// <summary>
        /// The order's type.
        /// </summary>
        public string OrderType { get; set; }

        /// <summary>
        /// The LIMIT price.
        /// Used for limit, stop-limit and relative orders. In all other cases specify zero. 
        /// For relative orders with no limit price, also specify zero.
        /// </summary>
        public double LimitPrice { get; set; } = double.NaN;

        /// <summary>
        /// Generic field to contain the stop price for STP LMT orders, trailing amount, etc.
        /// </summary>
        public double AuxPrice { get; set; } = double.NaN;

        /// <summary>
        /// Trail stop price for TRAILIMIT orders.
        /// </summary>
        public double TrailStopPrice { get; set; } = double.NaN;

        /// <summary>
        /// Specifies the trailing amount of a trailing stop order as a percentage.
        /// Observe the following guidelines when using the trailingPercent field:
        ///     - This field is mutually exclusive with the existing trailing amount. That is, the API client can send one or the other but not both.
        ///     - This field is read AFTER the stop price (barrier price) as follows: deltaNeutralAuxPrice stopPrice, trailingPercent, scale order attributes.
        ///     - The field will also be sent to the API in the openOrder message if the API client version is >= 56. 
        ///         It is sent after the stopPrice field as follows: stopPrice, trailingPct, basisPoint
        /// </summary>
        public double TrailingPercent { get; set; } = double.NaN;

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
        public string TimeInForce { get; set; }

        /// <summary>
        /// If set to true, allows orders to also trigger or fill outside of regular trading hours.
        /// </summary>
        public bool OutsideRegularTradeHours { get; set; } = false;

        /// <summary>
        /// Indicates whether or not all the order has to be filled on a single execution.
        /// </summary>
        public bool AllOrNone { get; set; }

        /// <summary>
        /// If set to true, the order will not be visible when viewing the market depth.
        /// This option only applies to orders routed to the ISLAND exchange.
        /// </summary>
        public bool Hidden { get; set; } = false;

        /// <summary>
        /// If set to true, specifies that the order is an ISE Block order.
        /// </summary>
        public bool BlockOrder { get; set; }

        /// <summary>
        /// If set to true, specifies that the order is a Sweep-to-Fill order.
        /// </summary>
        public bool SweepToFill { get; set; }

        /// <summary>
        /// The publicly disclosed order size, used when placing Iceberg orders.
        /// </summary>
        public int DisplaySize { get; set; }

        /// <summary>
        /// Allows to retrieve the commissions and margin information.
        /// When placing an order with this attribute set to true, the order will not be placed as such.
        /// Instead it will used to request the commissions and margin information that would result from this order.
        /// </summary>
        public bool WhatIf { get; set; } = false;

        /// <summary>
        /// Orders routed to IBDARK are tagged as “post only” and are held in IB's order book, 
        /// where incoming SmartRouted orders from other IB customers are eligible to trade against them.
        /// For IBDARK orders only.
        /// </summary>
        public bool NotHeld { get; set; } = false;

        /// <summary>
        /// DOC_TODO
        /// </summary>
        public bool Solicited { get; set; } = false;

        /// <summary>
        /// DOC_TODO
        /// </summary>
        public bool RandomizeSize { get; set; }

        /// <summary>
        /// DOC_TODO
        /// </summary>
        public bool RandomizePrice { get; set; }

        /// <summary>
        /// DOC_TODO
        /// </summary>
        public OrderCondition Conditions { get; } = new OrderCondition();

        /// <summary>
        /// DOC_TODO
        /// </summary>
        public ICollection<(string Tag, string Value)> OrderMiscOptions { get; set; }

        /// <summary>
        /// One-Cancels-All group identifier.
        /// </summary>
        public string OneCancelsAllGroup { get; set; }

        /// <summary>
        /// Tells how to handle remaining orders in an OCA group when one order or part of an order executes.
        /// Valid values are:
        ///     1 = Cancel all remaining orders with block.
        ///     2 = Remaining orders are proportionately reduced in size with block.
        ///     3 = Remaining orders are proportionately reduced in size with no block.
        /// If you use a value "with block" it gives the order overfill protection. 
        /// This means that only one order in the group will be routed at a time to remove the possibility of an overfill.
        /// </summary>
        public int OneCancelsAllType { get; set; }

        /// <summary>
        /// Specifies how Simulated Stop, Stop-Limit and Trailing Stop orders are triggered.
        /// Valid values are:\n
        /// 0 - The default value.The "double bid/ask" function will be used for orders for OTC stocks and US options.All other orders will used the "last" function.\n
        /// 1 - use "double bid/ask" function, where stop orders are triggered based on two consecutive bid or ask prices.\n
        /// 2 - "last" function, where stop orders are triggered based on the last price.\n
        /// 3 double last function.\n
        /// 4 bid/ask function.\n
        /// 7 last or bid/ask function.\n
        /// 8 mid-point function.\n
        /// </summary>
        public int TriggerMethod { get; set; }

        /// <summary>
        /// List of Per-leg price following the same sequence combo legs are added.
        /// The combo price must be left unspecified when using per-leg prices.
        /// </summary>
        public ICollection<OrderComboLeg> OrderComboLegs { get; set; }

        /// <summary>
        /// Advanced parameters for Smart combo routing.
        /// These features are for both guaranteed and nonguaranteed combination orders routed to Smart, and are available based on combo type and order type. 
        /// SmartComboRoutingParams is similar to AlgoParams in that it makes use of tag/value pairs to add parameters to combo orders. \n
        /// Make sure that you fully understand how Advanced Combo Routing works in TWS itself first: https://www.interactivebrokers.com/en/software/tws/usersguidebook/specializedorderentry/advanced_combo_routing.htm \n
        /// The parameters cover the following capabilities:
        /// 
        /// - Non-Guaranteed - Determine if the combo order is Guaranteed or Non-Guaranteed.
        ///    Tag = NonGuaranteed
        ///    Value = 0: The order is guaranteed
        ///    Value = 1: The order is non-guaranteed
        /// 
        ///  - Select Leg to Fill First - User can specify which leg to be executed first.
        ///    Tag = LeginPrio
        ///    Value = -1: No priority is assigned to either combo leg
        ///    Value = 0: Priority is assigned to the first leg being added to the comboLeg
        ///    Value = 1: Priority is assigned to the second leg being added to the comboLeg
        ///    Note: The LeginPrio parameter can only be applied to two-legged combo.
        /// 
        ///  - Maximum Leg-In Combo Size - Specify the maximum allowed leg-in size per segment
        ///    Tag = MaxSegSize
        ///    Value = Unit of combo size
        /// 
        ///  - Do Not Start Next Leg-In if Previous Leg-In Did Not Finish - Specify whether or not the system should attempt to fill the next segment before the current segment fills.
        ///    Tag = DontLeginNext
        ///    Value = 0: Start next leg-in even if previous leg-in did not finish
        ///    Value = 1: Do not start next leg-in if previous leg-in did not finish
        /// 
        ///  - Price Condition - Combo order will be rejected or cancelled if the leg market price is outside of the specified price range [CondPriceMin, CondPriceMax]
        ///    Tag = PriceCondConid: The ContractID of the combo leg to specify price condition on
        ///    Value = The ContractID
        ///    Tag = CondPriceMin: The lower price range of the price condition
        ///    Value = The lower price
        ///    Tag = CondPriceMax: The upper price range of the price condition
        ///    Value = The upper price
        /// </summary>
        public ICollection<(string Tag, string Value)> SmartComboRoutingParams { get; set; }

        /// <summary>
        /// The amount off the limit price allowed for discretionary orders.
        /// </summary>
        public double DiscretionaryAmt { get; set; } = 0; // double.NaN;

        /// <summary>
        /// Specifies the date and time after which the order will be active.
        /// Format: yyyymmdd hh:mm:ss {optional Timezone}
        /// </summary>
        public string GoodAfterTime { get; set; } = string.Empty;

        /// <summary>
        /// The date and time until the order will be active.
        /// You must enter GTD as the time in force to use this string. 
        /// The trade's "Good Till Date," format "YYYYMMDD hh:mm:ss (optional time zone)"
        /// </summary>
        public string GoodTillDate { get; set; } = string.Empty;

        /// <summary>
        /// Used for GTC orders.
        /// </summary>
        public string ActiveStartTime { get; set; } = string.Empty;

        /// <summary>
        /// Used for GTC orders.
        /// </summary>
        public string ActiveStopTime { get; set; } = string.Empty;

        /// <summary>
        /// Use to opt out of default SmartRouting for orders routed directly to ASX.
        /// This attribute defaults to false unless explicitly set to true.
        /// When set to false, orders routed directly to ASX will NOT use SmartRouting.
        /// When set to true, orders routed directly to ASX orders WILL use SmartRouting.
        /// </summary>
        public bool OptOutSmartRouting { get; set; } = false;

        /// <summary>
        /// Overrides TWS constraints.
        /// Precautionary constraints are defined on the TWS Presets page,
        /// and help ensure tha tyour price and size order values are reasonable.
        /// Orders sent from the API are also validated against these safety constraints,
        /// and may be rejected if any constraint is violated.
        /// To override validation, set this parameter’s value to True.
        /// </summary>
        public bool OverridePercentageConstraints { get; set; } = false;

        /// <summary>
        /// Specifies the true beneficiary of the order.
        /// For IBExecution customers. This value is required for FUT/FOP orders for reporting to the exchange.
        /// </summary>
        public string ClearingAccount { get; set; } = string.Empty;

        /// <summary>
        /// For exeuction-only clients to know where do they want their shares to be cleared at.
        /// Valid values are: IB, Away, and PTA (post trade allocation).
        /// </summary>
        public string ClearingIntent { get; set; } = string.Empty;

        /// <summary>
        /// The Financial Advisor group the trade will be allocated to.
        /// Use an empty string if not applicable.
        /// </summary>
        public string FaGroup { get; set; } = string.Empty;

        /// <summary>
        /// The Financial Advisor allocation profile the trade will be allocated to.
        /// Use an empty string if not applicable.
        /// </summary>
        public string FaProfile { get; set; } = string.Empty;

        /// <summary>
        /// The Financial Advisor allocation method the trade will be allocated to.
        /// Use an empty string if not applicable.
        /// </summary>
        public string FaMethod { get; set; } = string.Empty;

        /// <summary>
        /// The Financial Advisor percentage concerning the trade's allocation.
        /// Use an empty string if not applicable.
        /// </summary>
        public string FaPercentage { get; set; } = string.Empty;

        /// <summary>
        /// Model Code ???
        /// </summary>
        public string ModelCode { get; set; } = string.Empty;

        /// <summary>
        /// For institutions only. Valid values are: 1 (broker holds shares) or 2 (shares come from elsewhere).
        /// </summary>
        public int ShortSaleSlot { get; set; }

        /// <summary>
        /// Used only when shortSaleSlot is 2.
        /// For institutions only. Indicates the location where the shares to short come from. Used only when short
        /// sale slot is set to 2 (which means that the shares to short are held elsewhere and not with IB).
        /// </summary>
        public string DesignatedLocation { get; set; } = string.Empty;

        /// <summary>
        /// Only available with IB Execution-Only accounts with applicable securities
        /// Mark order as exempt from short sale uptick rule.
        /// </summary>
        public int ExemptCode { get; set; } = -1;

        /// <summary>
        /// Individual = 'I'
        /// Agency = 'A'
        /// AgentOtherMember = 'W'
        /// IndividualPTIA = 'J'
        /// AgencyPTIA = 'U'
        /// AgentOtherMemberPTIA = 'M'
        /// IndividualPT = 'K'
        /// AgencyPT = 'Y'
        /// AgentOtherMemberPT = 'N'
        /// </summary>
        public string Rule80A { get; set; }

        /// <summary>
        /// DOC_TODO
        /// Institutions only. Indicates the firm which will settle the trade.
        /// </summary>
        public string SettlingFirm { get; set; } = string.Empty;

        /// <summary>
        /// Identifies a minimum quantity order type.
        /// </summary>
        public int MinQty { get; set; } = -1;

        /// <summary>
        /// The percent offset amount for relative orders.
        /// </summary>
        public double PercentOffset { get; set; } = double.NaN;

        /// <summary>
        /// Trade with electronic quotes.
        /// </summary>
        public bool ETradeOnly { get; set; } = false;

        /// <summary>
        /// Trade with firm quotes.
        /// </summary>
        public bool FirmQuoteOnly { get; set; } = false;

        /// <summary>
        /// Maximum smart order distance from the NBBO.
        /// </summary>
        public double NbboPriceCap { get; set; } = double.NaN;

        /// <summary>
        /// For BOX orders only. Values include:
        ///      1 - match
        ///      2 - improvement
        ///      3 - transparent
        /// </summary>
        public int AuctionStrategy { get; set; } = -1;

        /// <summary>
        /// The auction's starting price.
        /// For BOX orders only.
        /// </summary>
        public double StartingPrice { get; set; } = double.NaN;

        /// <summary>
        /// The stock's reference price.
        /// The reference price is used for VOL orders to compute the limit price sent to an exchange 
        /// (whether or not Continuous Update is selected), and for price range monitoring.
        /// </summary>
        public double StockRefPrice { get; set; } = double.NaN;

        /// <summary>
        /// The stock's Delta.
        /// For orders on BOX only.
        /// </summary>
        public double Delta { get; set; } = double.NaN;

        /// <summary>
        /// The lower value for the acceptable underlying stock price range.
        /// For price improvement option orders on BOX and VOL orders with dynamic management.
        /// </summary>
        public double StockRangeLower { get; set; } = double.NaN;

        /// <summary>
        /// The upper value for the acceptable underlying stock price range.
        /// For price improvement option orders on BOX and VOL orders with dynamic management.
        /// </summary>
        public double StockRangeUpper { get; set; } = double.NaN;

        /// <summary>
        /// The option price in volatility, as calculated by TWS' Option Analytics.
        /// This value is expressed as a percent and is used to calculate the limit price sent to the exchange.
        /// </summary>
        public double Volatility { get; set; } = double.NaN;

        /// <summary>
        /// Values include:
        ///     1 - Daily Volatility
        ///     2 - Annual Volatility
        /// </summary>
        public int VolatilityType { get; set; } = -1;

        /// <summary>
        /// Specifies whether TWS will automatically update the limit price of the order as the underlying price moves.
        /// VOL orders only.
        /// </summary>
        public int ContinuousUpdate { get; set; }

        /// <summary>
        /// Specifies how you want TWS to calculate the limit price for options, and for stock range price monitoring.
        /// VOL orders only. Valid values include:
        ///     1 - Average of NBBO
        ///     2 - NBB or the NBO depending on the action and right.
        /// </summary>
        public int ReferencePriceType { get; set; } = -1;

        /// <summary>
        /// Enter an order type to instruct TWS to submit a delta neutral trade on full or partial execution of the VOL order.
        /// VOL orders only. For no hedge delta order to be sent, specify NONE.
        /// </summary>
        public string DeltaNeutralOrderType { get; set; } = string.Empty;

        /// <summary>
        /// Use this field to enter a value if the value in the deltaNeutralOrderType field
        /// is an order type that requires an Aux price, such as a REL order.
        /// VOL orders only.
        /// </summary>
        public double DeltaNeutralAuxPrice { get; set; } = double.NaN;

        /// <summary>
        /// DOC_TODO
        /// </summary>
        public int DeltaNeutralConId { get; set; } = 0;

        /// <summary>
        /// DOC_TODO
        /// </summary>
        public string DeltaNeutralSettlingFirm { get; set; } = string.Empty;

        /// <summary>
        /// DOC_TODO
        /// </summary>
        public string DeltaNeutralClearingAccount { get; set; } = string.Empty;

        /// <summary>
        /// DOC_TODO
        /// </summary>
        public string DeltaNeutralClearingIntent { get; set; } = string.Empty;

        /// <summary>
        /// Specifies whether the order is an Open or a Close order and is used when the hedge involves a CFD and and the order is clearing away.
        /// </summary>
        public string DeltaNeutralOpenClose { get; set; } = string.Empty;

        /// <summary>
        /// Used when the hedge involves a stock and indicates whether or not it is sold short.
        /// </summary>
        public bool DeltaNeutralShortSale { get; set; } = false;

        /// <summary>
        /// Has a value of 1 (the clearing broker holds shares) or 2 (delivered from a third party).
        /// If you use 2, then you must specify a deltaNeutralDesignatedLocation.
        /// </summary>
        public int DeltaNeutralShortSaleSlot { get; set; } = 0;

        /// <summary>
        /// Used only when deltaNeutralShortSaleSlot = 2.
        /// </summary>
        public string DeltaNeutralDesignatedLocation { get; set; } = string.Empty;

        /// <summary>
        /// DOC_TODO
        /// For EFP orders only.
        /// </summary>
        public double BasisPoints { get; set; } = double.NaN;

        /// <summary>
        /// DOC_TODO
        /// For EFP orders only.
        /// </summary>
        public int BasisPointsType { get; set; } = -1;

        /// <summary>
        /// Defines the size of the first, or initial, order component.
        /// For Scale orders only.
        /// </summary>
        public int ScaleInitLevelSize { get; set; } = -1;

        /// <summary>
        /// Defines the order size of the subsequent scale order components.
        /// For Scale orders only. Used in conjunction with scaleInitLevelSize().
        /// </summary>
        public int ScaleSubsLevelSize { get; set; } = -1;

        /// <summary>
        /// Defines the price increment between scale components.
        /// For Scale orders only. This value is compulsory.
        /// </summary>
        public double ScalePriceIncrement { get; set; } = double.NaN;

        /// <summary>
        /// DOC_TODO
        /// For extended Scale orders.
        /// </summary>
        public double ScalePriceAdjustValue { get; set; } = double.NaN;

        /// <summary>
        /// DOC_TODO
        /// For extended Scale orders.
        /// </summary>
        public int ScalePriceAdjustInterval { get; set; } = -1;

        /// <summary>
        /// DOC_TODO
        /// For extended Scale orders.
        /// </summary>
        public double ScaleProfitOffset { get; set; } = double.NaN;

        /// <summary>
        /// DOC_TODO
        /// For extended Scale orders.
        /// </summary>
        public bool ScaleAutoReset { get; set; } = false;

        /// <summary>
        /// DOC_TODO
        /// For extended Scale orders.
        /// </summary>
        public int ScaleInitPosition { get; set; } = -1;

        /// <summary>
        /// DOC_TODO
        /// For extended Scale orders.
        /// </summary>
        public int ScaleInitFillQty { get; set; } = -1;

        /// <summary>
        /// DOC_TODO
        /// For extended Scale orders.
        /// </summary>
        public bool ScaleRandomPercent { get; set; } = false;

        /// <summary>
        /// Used for scale orders.
        /// </summary>
        public string ScaleTable { get; set; } = string.Empty;

        /// <summary>
        /// For hedge orders.
        /// Possible values include:
        ///     D - delta
        ///     B - beta
        ///     F - FX
        ///     P - Pair
        /// </summary>
        public string HedgeType { get; set; } = string.Empty;

        /// <summary>
        /// DOC_TODO
        /// Beta = x for Beta hedge orders, ratio = y for Pair hedge order
        /// </summary>
        public string HedgeParam { get; set; } = string.Empty;

        /// <summary>
        /// Don't use auto price for hedge
        /// </summary>
        public bool DontUseAutoPriceForHedge { get; set; } = false;

        /// <summary>
        /// DOC_TODO
        /// </summary>
        public string AlgoId { get; set; } = string.Empty;

        /// <summary>
        /// The algorithm strategy.
        /// As of API verion 9.6, the following algorithms are supported:
        ///     ArrivalPx - Arrival Price
        ///     DarkIce - Dark Ice
        ///     PctVol - Percentage of Volume
        ///     Twap - TWAP (Time Weighted Average Price)
        ///     Vwap - VWAP (Volume Weighted Average Price)
        /// For more information about IB's API algorithms, 
        /// refer to https://www.interactivebrokers.com/en/software/api/apiguide/tables/ibalgo_parameters.htm
        /// </summary>
        public string AlgoStrategy { get; set; } = string.Empty;

        /// <summary>
        /// The list of parameters for the IB algorithm.
        /// For more information about IB's API algorithms,
        /// refer to https://www.interactivebrokers.com/en/software/api/apiguide/tables/ibalgo_parameters.htm
        /// </summary>
        public ICollection<(string Tag, string Value)> AlgoParams { get; set; }

        /// <summary>
        /// This is a regulartory attribute that applies to all US Commodity (Futures) Exchanges,
        /// provided to allow client to comply with CFTC Tag 50 Rules
        /// </summary>
        public string ExtOperator { get; set; } = string.Empty;

        /// <summary>
        /// The native cash quantity
        /// </summary>
        public double CashQty { get; set; } = double.NaN;

        /// <summary>
        /// The name of the Soft Dollar Tier
        /// </summary>
        public string SoftDollarTierName { get; set; } = string.Empty;

        /// <summary>
        /// The value of the Soft Dollar Tier
        /// </summary>
        public string SoftDollarTierValue { get; set; } = string.Empty;

        /// <summary>
        /// The display name of the Soft Dollar Tier
        /// </summary>
        public string SoftDollarTierDisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Identifies a person as the responsible party for investment decisions within the firm. Orders covered by MiFID 2 
        /// (Markets in Financial Instruments Directive 2) must include either Mifid2DecisionMaker or Mifid2DecisionAlgo field (but not both). 
        /// Requires TWS 969+.
        /// </summary>
        public string Mifid2DecisionMaker { get; set; } = string.Empty;

        /// <summary>
        /// Identifies the algorithm responsible for investment decisions within the firm. 
        /// Orders covered under MiFID 2 must include either Mifid2DecisionMaker or Mifid2DecisionAlgo, but cannot have both. 
        /// Requires TWS 969+.
        /// </summary>
        public string Mifid2DecisionAlgo { get; set; } = string.Empty;

        /// <summary>
        /// For MiFID 2 reporting; identifies a person as the responsible party for the execution of a transaction within the firm.
        /// Requires TWS 969+.
        /// </summary>
        public string Mifid2ExecutionTrader { get; set; } = string.Empty;

        /// <summary>
        /// For MiFID 2 reporting; identifies the algorithm responsible for the execution of a transaction within the firm.
        /// Requires TWS 969+.
        /// </summary>
        public string Mifid2ExecutionAlgo { get; set; } = string.Empty;

        #region Peg Bench

        /// <summary>
        /// Pegged-to-benchmark orders: this attribute will contain the conId of the contract against which the order will be pegged.
        /// </summary>
        public int ReferenceContractId { get; set; } = -1;

        /// <summary>
        /// Pegged-to-benchmark orders: indicates whether the order's pegged price should increase or decreases.
        /// </summary>
        public bool IsPeggedChangeAmountDecrease { get; set; } = false;

        /// <summary>
        /// Pegged-to-benchmark orders: amount by which the order's pegged price should move.
        /// </summary>
        public double PeggedChangeAmount { get; set; } = double.NaN;

        /// <summary>
        /// Pegged-to-benchmark orders: the amount the reference contract needs to move to adjust the pegged order.
        /// </summary>
        public double ReferenceChangeAmount { get; set; } = double.NaN;

        /// <summary>
        /// Pegged-to-benchmark orders: the exchange against which we want to observe the reference contract.
        /// </summary>
        public string ReferenceExchange { get; set; } = string.Empty;

        /// <summary>
        /// Adjusted Stop orders: the parent order will be adjusted to the given type when the adjusted trigger price is penetrated.
        /// </summary>
        public string AdjustedOrderType { get; set; } = string.Empty;

        /// <summary>
        /// DOC_TODO
        /// </summary>
        public double TriggerPrice { get; set; } = double.NaN;

        /// <summary>
        /// DOC_TODO
        /// </summary>
        public double LimitPriceOffset { get; set; } = double.NaN;

        /// <summary>
        /// Adjusted Stop orders: specifies the stop price of the adjusted (STP) parent
        /// </summary>
        public double AdjustedStopPrice { get; set; } = double.NaN;

        /// <summary>
        /// Adjusted Stop orders: specifies the stop limit price of the adjusted (STPL LMT) parent
        /// </summary>
        public double AdjustedStopLimitPrice { get; set; } = double.NaN;

        /// <summary>
        /// Adjusted Stop orders: specifies the trailing amount of the adjusted (TRAIL) parent
        /// </summary>
        public double AdjustedTrailingAmount { get; set; } = double.NaN;

        /// <summary>
        /// Adjusted Stop orders: specifies where the trailing unit is an amount (set to 0) or a percentage (set to 1)
        /// </summary>
        public int AdjustableTrailingUnit { get; set; } = 0;

        /// <summary>
        /// Set to true to convert order of type 'Primary Peg' to 'D-Peg'
        /// </summary>
        public bool DiscretionaryUpToLimitPrice { get; set; } = false;

        #endregion Peg Bench

        /// <summary>
        /// Set to true to create tickets from API orders when TWS is used as an OMS
        /// </summary>
        public bool IsOmsContainer { get; set; } = false;

        /// <summary>
        /// DOC_TODO
        /// </summary>
        public bool? UsePriceMgmtAlgo { get; set; } = false;

        /// <summary>
        /// DOC_TODO
        /// </summary>
        public string AutoCancelDate { get; set; } = string.Empty;

        public bool Equals(IOrder other)
        {
            if (this == other) return true;
            if (other == null) return false;
            if (PermId > 0) return PermId == other.PermId;
            if (Id > 0) return Id == other.Id;
            return false;
        }

        public bool Equals(IbOrder other)
        {
            if (this == other) return true;
            if (other == null) return false;
            if (PermId == other.PermId) return true;
            if (Id != 0 && Id == other.Id) return true;

            /*
            if (Id != other.Id ||
                ClientId != other.ClientId ||
                TotalQuantity != other.TotalQuantity ||
                LmtPrice != other.LmtPrice ||
                AuxPrice != other.AuxPrice ||
                OcaType != other.OcaType ||
                Transmit != other.Transmit ||
                ParentId != other.ParentId ||
                BlockOrder != other.BlockOrder ||
                SweepToFill != other.SweepToFill ||
                DisplaySize != other.DisplaySize ||
                TriggerMethod != other.TriggerMethod ||
                OutsideRth != other.OutsideRth ||
                Hidden != other.Hidden ||
                OverridePercentageConstraints != other.OverridePercentageConstraints ||
                AllOrNone != other.AllOrNone ||
                MinQty != other.MinQty ||
                PercentOffset != other.PercentOffset ||
                TrailStopPrice != other.TrailStopPrice ||
                TrailingPercent != other.TrailingPercent ||
                Origin != other.Origin ||
                ShortSaleSlot != other.ShortSaleSlot ||
                DiscretionaryAmt != other.DiscretionaryAmt ||
                ETradeOnly != other.ETradeOnly ||
                FirmQuoteOnly != other.FirmQuoteOnly ||
                NbboPriceCap != other.NbboPriceCap ||
                OptOutSmartRouting != other.OptOutSmartRouting ||
                AuctionStrategy != other.AuctionStrategy ||
                StartingPrice != other.StartingPrice ||
                StockRefPrice != other.StockRefPrice ||
                Delta != other.Delta ||
                StockRangeLower != other.StockRangeLower ||
                StockRangeUpper != other.StockRangeUpper ||
                Volatility != other.Volatility ||
                VolatilityType != other.VolatilityType ||
                ContinuousUpdate != other.ContinuousUpdate ||
                ReferencePriceType != other.ReferencePriceType ||
                DeltaNeutralAuxPrice != other.DeltaNeutralAuxPrice ||
                DeltaNeutralConId != other.DeltaNeutralConId ||
                DeltaNeutralShortSale != other.DeltaNeutralShortSale ||
                DeltaNeutralShortSaleSlot != other.DeltaNeutralShortSaleSlot ||
                BasisPoints != other.BasisPoints ||
                BasisPointsType != other.BasisPointsType ||
                ScaleInitLevelSize != other.ScaleInitLevelSize ||
                ScaleSubsLevelSize != other.ScaleSubsLevelSize ||
                ScalePriceIncrement != other.ScalePriceIncrement ||
                ScalePriceAdjustValue != other.ScalePriceAdjustValue ||
                ScalePriceAdjustInterval != other.ScalePriceAdjustInterval ||
                ScaleProfitOffset != other.ScaleProfitOffset ||
                ScaleAutoReset != other.ScaleAutoReset ||
                ScaleInitPosition != other.ScaleInitPosition ||
                ScaleInitFillQty != other.ScaleInitFillQty ||
                ScaleRandomPercent != other.ScaleRandomPercent ||
                WhatIf != other.WhatIf ||
                NotHeld != other.NotHeld ||
                ExemptCode != other.ExemptCode ||
                RandomizePrice != other.RandomizePrice ||
                RandomizeSize != other.RandomizeSize ||
                Solicited != other.Solicited ||
                ConditionsIgnoreRth != other.ConditionsIgnoreRth ||
                ConditionsCancelOrder != other.ConditionsCancelOrder ||
                Tier != other.Tier ||
                CashQty != other.CashQty ||
                dontUseAutoPriceForHedge != other.dontUseAutoPriceForHedge ||
                IsOmsContainer != other.IsOmsContainer ||
                UsePriceMgmtAlgo != other.UsePriceMgmtAlgo ||
                FilledQuantity != other.FilledQuantity ||
                RefFuturesConId != other.RefFuturesConId ||
                AutoCancelParent != other.AutoCancelParent ||
                ImbalanceOnly != other.ImbalanceOnly ||
                RouteMarketableToBbo != other.RouteMarketableToBbo ||
                ParentPermId != other.ParentPermId)
            {
                return false;
            }

            if (Util.StringCompare(Action, other.Action) != 0 ||
                Util.StringCompare(OrderType, other.OrderType) != 0 ||
                Util.StringCompare(Tif, other.Tif) != 0 ||
                Util.StringCompare(ActiveStartTime, other.ActiveStartTime) != 0 ||
                Util.StringCompare(ActiveStopTime, other.ActiveStopTime) != 0 ||
                Util.StringCompare(OcaGroup, other.OcaGroup) != 0 ||
                Util.StringCompare(OrderRef, other.OrderRef) != 0 ||
                Util.StringCompare(GoodAfterTime, other.GoodAfterTime) != 0 ||
                Util.StringCompare(GoodTillDate, other.GoodTillDate) != 0 ||
                Util.StringCompare(Rule80A, other.Rule80A) != 0 ||
                Util.StringCompare(FaGroup, other.FaGroup) != 0 ||
                Util.StringCompare(FaProfile, other.FaProfile) != 0 ||
                Util.StringCompare(FaMethod, other.FaMethod) != 0 ||
                Util.StringCompare(FaPercentage, other.FaPercentage) != 0 ||
                Util.StringCompare(OpenClose, other.OpenClose) != 0 ||
                Util.StringCompare(DesignatedLocation, other.DesignatedLocation) != 0 ||
                Util.StringCompare(DeltaNeutralOrderType, other.DeltaNeutralOrderType) != 0 ||
                Util.StringCompare(DeltaNeutralSettlingFirm, other.DeltaNeutralSettlingFirm) != 0 ||
                Util.StringCompare(DeltaNeutralClearingAccount, other.DeltaNeutralClearingAccount) != 0 ||
                Util.StringCompare(DeltaNeutralClearingIntent, other.DeltaNeutralClearingIntent) != 0 ||
                Util.StringCompare(DeltaNeutralOpenClose, other.DeltaNeutralOpenClose) != 0 ||
                Util.StringCompare(DeltaNeutralDesignatedLocation, other.DeltaNeutralDesignatedLocation) != 0 ||
                Util.StringCompare(HedgeType, other.HedgeType) != 0 ||
                Util.StringCompare(HedgeParam, other.HedgeParam) != 0 ||
                Util.StringCompare(Account, other.Account) != 0 ||
                Util.StringCompare(SettlingFirm, other.SettlingFirm) != 0 ||
                Util.StringCompare(ClearingAccount, other.ClearingAccount) != 0 ||
                Util.StringCompare(ClearingIntent, other.ClearingIntent) != 0 ||
                Util.StringCompare(AlgoStrategy, other.AlgoStrategy) != 0 ||
                Util.StringCompare(AlgoId, other.AlgoId) != 0 ||
                Util.StringCompare(ScaleTable, other.ScaleTable) != 0 ||
                Util.StringCompare(ModelCode, other.ModelCode) != 0 ||
                Util.StringCompare(ExtOperator, other.ExtOperator) != 0 ||
                Util.StringCompare(AutoCancelDate, other.AutoCancelDate) != 0 ||
                Util.StringCompare(Shareholder, other.Shareholder) != 0)
            {
                return false;
            }

            if (!Util.VectorEqualsUnordered(AlgoParams, other.AlgoParams))
            {
                return false;
            }

            if (!Util.VectorEqualsUnordered(SmartComboRoutingParams, other.SmartComboRoutingParams))
            {
                return false;
            }

            // compare order combo legs
            if (!Util.VectorEqualsUnordered(OrderComboLegs, other.OrderComboLegs))
            {
                return false;
            }

            return true;
            */

            return false;
        }
    }
    
}
