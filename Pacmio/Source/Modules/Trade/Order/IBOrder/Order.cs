/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;

namespace Pacmio.IB
{
    public static partial class Client
    {
        // TODO: #1: remove this, and merge this with OrderManager

        /// <summary>
        /// https://interactivebrokers.github.io/tws-api/bracket_order.html
        /// </summary>
        /// <param name="od"></param>
        /// <param name="whatIf"></param>
        /// <param name="modify"></param>
        /// <param name="useSmart"></param>
        public static void PlaceOrder(OrderInfo od, bool whatIf = false, bool modify = false, bool useSmart = true)
        {
            Contract c = od.Contract;

            Console.WriteLine("\n##### Place Order: " + c.ToString() + " #####");

            if (Connected && c.Exchange.Param() is string exchangeCode &&
                c is IBusiness it &&
                od.Type.Param() is string orderTypeCode &&
                od.TimeInForce.Param() is string TifCode && od.Quantity != 0 && od.Status == OrderStatus.Inactive) // && !modify))// && c.IsValid) // Also please check the RHD is active?
            {
                od.Status = OrderStatus.Started;

                if ((!modify && !ActiveRequestContains(od.OrderId)) || od.OrderId == -1)
                {
                    var (orderId, _) = RegisterRequest(RequestType.PlaceOrder);
                    od.OrderId = orderId;
                    OrderManager.InsertOrderWithoutPermId(od);
                }

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

                string goodAfterDate = (od.TimeInForce == OrderTimeInForce.GoodAfterDate) ? od.LimitedEffectiveTime.ToString("yyyyMMdd HH:mm:ss") : string.Empty;
                string goodTillDate = (od.TimeInForce == OrderTimeInForce.GoodUntilDate) ? od.LimitedEffectiveTime.ToString("yyyyMMdd HH:mm:ss") : string.Empty;

                //if (od.TimeInForce == OrderTimeInForce.GoodAfterDate) goodAfterDate = od.GoodAfterDate.ToString("yyyyMMdd HH:mm:ss EST");
                //if (od.TimeInForce == OrderTimeInForce.GoodUntilDate) goodAfterDate = od.EffectiveDateTime.ToString("yyyyMMdd HH:mm:ss EST");

                List<string> paramsList = new() {
                    ((int)RequestType.PlaceOrder).ToString(), // 0
                    od.OrderId.ParamPos(), // 1

                    c.ConId.Param(), // 2
                    c.Name, // 3
                    c.TypeCode(), // 4
                    
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

                    (od.Quantity > 0) ? "BUY" : "SELL", // order.Action, // 16
                    Math.Abs(od.Quantity).Param(), // 17, double type
                    orderTypeCode, // 18
                    od.LimitPrice.Param(), // 19
                    od.AuxPrice.Param(), // 20
                    
                    TifCode, // 21
                    string.Empty, // 22
                    od.AccountId, // 23
                    string.Empty, // 24 OpenClose
                    "0", // 25 // Origin
                    od.Description, // 26 // OrderRef
                    od.Transmit.Param(), // "1", // 27 // Transmit
                    od.ParentOrderId.Param(), // 28
                    "0", // BlockOrder.Param(), // 29
                    "0", // order.SweepToFill.Param(), // 30
                    "0", // order.DisplaySize.Param(), // 31
                    "0", // order.TriggerMethod.Param(), // 32
                    (od.Type != OrderType.MidPrice) ? od.OutsideRegularTradeHours.Param() : "0", // 33
                    od.Hidden.Param(), // 34
                    // *** BAG order *** Ignored
                    string.Empty, // 35 Deprecated sharesAllocation field
                    "0", // order.DiscretionaryAmt.Param(), // 36 "0"
                    goodAfterDate, // order.GoodAfterTime, // 37
                    goodTillDate, // order.GoodTillDate, // 38
                    // add FA information
                    string.Empty, // order.FaGroup, // 39
                    string.Empty, // order.FaMethod, // 40
                    string.Empty, // order.FaPercentage, // 41
                    string.Empty, // order.FaProfile, // 42
                    string.Empty, // order.ModelCode, // 43
                    // institutional short sale slot fields.
                    "0", // order.ShortSaleSlot.Param(),  // 44 // 0 only for retail, 1 or 2 only for institution.
                    string.Empty, // order.DesignatedLocation,  // 45 // only populate when order.shortSaleSlot = 2.
                    "-1", // order.ExemptCode.Param(), // 46
                    "0", // order.OneCancelsAllType.Param(), // 47
                    string.Empty, // order.Rule80A, // 48
                    string.Empty, // order.SettlingFirm, // 49
                    od.AllOrNone.Param(), // 50
                    string.Empty, // order.MinQty.ParamPos(), // 51
                    string.Empty, // order.PercentOffset.Param(), // 52
                    "0", // order.ETradeOnly.Param(), // 53
                    "0", // order.FirmQuoteOnly.Param(), // 54
                    string.Empty, // order.NbboPriceCap.Param(), // 55
                    "0", // order.AuctionStrategy.ParamPos(), // 56
                    string.Empty, // order.StartingPrice.Param(), // 57
                    string.Empty, // order.StockRefPrice.Param(), // 58
                    string.Empty, // order.Delta.Param(), // 59
                    string.Empty, // order.StockRangeLower.Param(), // 60
                    string.Empty, // order.StockRangeUpper.Param(), // 61
                    "0", // order.OverridePercentageConstraints.Param(), // 62
                    string.Empty, // order.Volatility.Param(), // 63
                    "0", // order.VolatilityType.ParamPos(), // 64
                    // DeltaNeutral
                    string.Empty, // order.DeltaNeutralOrderType, // 65
                    string.Empty, // order.DeltaNeutralAuxPrice.Param(), // 66
                };
                paramsList.Add("0"); // order.ContinuousUpdate.Param()); // 67
                paramsList.Add("0"); // order.ReferencePriceType.ParamPos()); // 68
                paramsList.Add(string.Empty); // order.TrailStopPrice.Param()); // 69
                paramsList.Add(string.Empty); // order.TrailingPercent.Param()); // 70
                // *** Scale Order ***
                paramsList.Add(string.Empty); // order.ScaleInitLevelSize.ParamPos()); // 71
                paramsList.Add(string.Empty); // order.ScaleSubsLevelSize.ParamPos()); // 72
                paramsList.Add(string.Empty); // order.ScalePriceIncrement.Param()); // 73
                paramsList.Add(string.Empty); // order.ScaleTable); // 74
                paramsList.Add(string.Empty); // order.ActiveStartTime); // 75
                paramsList.Add(string.Empty); // order.ActiveStopTime); // 76
                paramsList.Add(string.Empty); // order.HedgeType); // 77
                paramsList.Add("0"); // order.OptOutSmartRouting.Param()); // 78
                paramsList.Add(string.Empty); // order.ClearingAccount); // 79
                paramsList.Add(string.Empty); // order.ClearingIntent); // 80
                paramsList.Add("0"); // order.NotHeld.Param()); // 81
                paramsList.Add("0"); // 82: DeltaNeutralContract
                // *** Algo ***
                paramsList.Add(string.Empty); // order.AlgoStrategy); // 83
                paramsList.Add(string.Empty); // order.AlgoId); // 84
                paramsList.Add(whatIf.Param()); // 85
                paramsList.Add(string.Empty); // order.OrderMiscOptions.Param()); // 86
                paramsList.Add("0"); // order.Solicited.Param()); // 87
                paramsList.Add("0"); // order.RandomizeSize.Param()); // 88
                paramsList.Add("0"); // order.RandomizePrice.Param()); // 89
                paramsList.Add("0"); // order.Conditions.Param);  // 90 // order.Conditions.Count
                paramsList.AddRange(new string[] {
                    string.Empty, // order.AdjustedOrderType, // 91
                    "1.79769313486232E+308", // order.TriggerPrice.Param(), // 92
                    "-1.79769313486232E+308", // order.LimitPriceOffset.Param(), // 93
                    "1.79769313486232E+308", // order.AdjustedStopPrice.Param(), // 94
                    "1.79769313486232E+308", // order.AdjustedStopLimitPrice.Param(), // 95
                    "1.79769313486232E+308", // order.AdjustedTrailingAmount.Param(), // 96
                    "0", // order.AdjustableTrailingUnit.Param(), // 97
                    string.Empty, // order.ExtOperator, // 98
                    string.Empty, // order.SoftDollarTierName, // 99
                    string.Empty, // order.SoftDollarTierValue, // 100
                    string.Empty, // order.CashQty.Param(), // 101
                    string.Empty, // order.Mifid2DecisionMaker, // 102
                    string.Empty, // order.Mifid2DecisionAlgo, // 103
                    string.Empty, // order.Mifid2ExecutionTrader, // 104
                    string.Empty, // order.Mifid2ExecutionAlgo, // 105

                    "0", // order.DontUseAutoPriceForHedge.Param(), // 106
                    "0", // order.IsOmsContainer.Param(), // 107
                    "0", // order.DiscretionaryUpToLimitPrice.Param(), // 108
                    "0", // order.UsePriceMgmtAlgo.Param() // 109
                });

                od.OrderExecuteTime = DateTime.Now;
                od.Updated();

                SendRequest(paramsList);
            }
            else if (od.Status == OrderStatus.Cancelled)
            {
                Console.WriteLine("This order is already cancelled.");
            }
            else
            {
                throw new Exception("Invalid Order Parameter, please double check!");
            }
        }


        public static void CancelOrder(int orderId) => RemoveRequest(orderId, RequestType.PlaceOrder);

        internal static void SendRequest_GlobalCancel()
        {
            if (Connected)
                SendRequest(new string[] {
                    RequestType.RequestGlobalCancel.Param(),
                    "1",
                });
        }







        internal static void SendRequest_AutoOpenOrders(bool autoBind)
        {
            if (Connected)
            {
                (_, string typeStr) = RegisterRequest(RequestType.RequestAutoOpenOrders);

                SendRequest(new string[] {
                    typeStr,
                    "1",
                    autoBind.Param(),
                });
            }
        }
    }
}
