/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// Possible Order States
/// 
/// ApiPending - indicates order has not yet been sent to IB server, for instance if there is a delay in receiving the security definition. Uncommonly received.
/// PendingSubmit - indicates the order was sent from TWS, but confirmation has not been received that it has been received by the destination.Most commonly because exchange is closed.
/// PendingCancel - indicates that a request has been sent to cancel an order but confirmation has not been received of its cancellation.
/// PreSubmitted - indicates that a simulated order type has been accepted by the IB system and that this order has yet to be elected. The order is held in the IB system until the election criteria are met.At that time the order is transmitted to the order destination as specified.
/// Submitted - indicates that your order has been accepted at the order destination and is working.
/// ApiCancelled - after an order has been submitted and before it has been acknowledged, an API client can request its cancellation, producing this state.
/// Cancelled - indicates that the balance of your order has been confirmed cancelled by the IB system.This could occur unexpectedly when IB or the destination has rejected your order.
/// Filled - indicates that the order has been completely filled.
/// Inactive - indicates an order is not working, possible reasons include:
///         it is invalid or triggered an error. A corresponding error code is expected to the error() function.
///         the order is to short shares but the order is being held while shares are being located.
///         an order is placed manually in TWS while the exchange is closed.
///         an order is blocked by TWS due to a precautionary setting and appears there in an untransmitted state
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {
        public static bool IsReady_OpenOrders { get; set; } = true;

        internal static void SendRequest_OpenOrders()
        {
            if (Connected && IsReady_OpenOrders)
            {
                IsReady_OpenOrders = false;
                SendRequest(new string[] {
                    RequestType.RequestOpenOrders.Param(),
                    "1",
                });
            }
        }

        internal static void SendRequest_AllOpenOrders()
        {
            if (Connected && IsReady_OpenOrders)
            {
                IsReady_OpenOrders = false;
                SendRequest(new string[] {
                    RequestType.RequestAllOpenOrders.Param(),
                    "1",
                });
            }
        }

        // Parse Errors: (0)"4"-(1)"2"-(2)"1"-(3)"321"-(4)"Error validating request:-'bN' : cause - The API interface is currently in Read-Only mode."

        /// <summary>
        /// Received OpenOrder: (0)"5"-(1)"0"-(2)"269753"-(3)"GILD"-(4)"STK"-(5)""-(6)"0"-(7)"?"-(8)""-(9)"SMART"-(10)"USD"-(11)"GILD"-(12)"NMS"-(13)"SELL"-(14)"100"-
        /// (15)"LMT"-(16)"78.38"-(17)"0.0"-(18)"GTC"-(19)""-(20)"DU332281"-(21)""-(22)"0"-(23)"Xu's BookTrader"-(24)"0"-(25)"975677963"-(26)"0"-(27)"1"-(28)"0"-(29)""-
        /// (30)"975677963.1/DU332281/100"-(31)""-(32)""-(33)""-(34)""-(35)""-(36)""-(37)""-(38)""-(39)""-(40)"0"-(41)""-(42)"-1"-(43)"0"-(44)""-(45)""-(46)""-(47)""-
        /// (48)""-(49)""-(50)"0"-(51)"0"-(52)"0"-(53)""-(54)"3"-(55)"0"-(56)"0"-(57)""-(58)"0"-(59)"0"-(60)""-(61)"0"-(62)"None"-(63)""-(64)"0"-(65)""-(66)""-(67)""-
        /// (68)"?"-(69)"0"-(70)"0"-(71)""-(72)"0"-(73)"0"-(74)""-(75)""-(76)""-(77)""-(78)""-(79)"0"-(80)"0"-(81)"0"-(82)""-(83)""-(84)""-(85)""-(86)"0"-(87)""-(88)"IB"-
        /// (89)"0"-(90)"0"-(91)""-(92)"0"-(93)"0"-(94)"PreSubmitted"-(95)"1.7976931348623157E308"-(96)"1.7976931348623157E308"-(97)"1.7976931348623157E308"-
        /// (98)"1.7976931348623157E308"-(99)"1.7976931348623157E308"-(100)"1.7976931348623157E308"-(101)"1.7976931348623157E308"-(102)"1.7976931348623157E308"-
        /// (103)"1.7976931348623157E308"-(104)""-(105)""-(106)""-(107)""-(108)""-(109)"0"-(110)"0"-(111)"0"-(112)"None"-(113)"1.7976931348623157E308"-
        /// (114)"79.38"-(115)"1.7976931348623157E308"-(116)"1.7976931348623157E308"-(117)"1.7976931348623157E308"-(118)"1.7976931348623157E308"-(119)"0"-
        /// (120)""-(121)""-(122)""-(123)"0"-(124)"1"-(125)"0"-(126)"0"-(127)"0"
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_OpenOrder(string[] fields)
        {
            int orderId = fields[1].ToInt32(-1);
            int permId = fields[25].ToInt32();
            OrderInfo od = OrderManager.GetOrAdd(orderId, permId);

            od.ConId = fields[2].ToInt32(-1);
            if (od.Contract is Contract c) c.TradeData.Add(od);

            int totalQuantity = fields[14].ToInt32(0);
            od.Quantity = fields[13] == "BUY" ? totalQuantity : -totalQuantity;

            od.Type = ApiCode.GetEnum<OrderType>(fields[15]).Enum;
            od.LimitPrice = fields[16].ToDouble();
            od.AuxPrice = fields[17].ToDouble();
            od.TimeInForce = ApiCode.GetEnum<OrderTimeInForce>(fields[18]).Enum;
            // fields[19]  // order.OcaGroup = eDecoder.ReadString();
            od.AccountCode = fields[20];
            // fields[21]  // order.OpenClose = eDecoder.ReadString();
            // fields[22]  // order.Origin = eDecoder.ReadInt();
            od.Description = fields[23];  // order.OrderRef = eDecoder.ReadString();
            od.ClientId = fields[24].ToInt32();

            od.OutsideRegularTradeHours = fields[26] == "1";
            od.Hidden = fields[27] == "1";
            od.ModeCode = fields[35];

            if (od.TimeInForce == OrderTimeInForce.GoodAfterDate && fields[29].Length > 5)
            {
                od.EffectiveDateTime = DateTime.ParseExact(fields[29].Substring(0, 17), "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);
            }
            else if (od.TimeInForce == OrderTimeInForce.GoodUntilDate && fields[36].Length > 5)
            {
                od.EffectiveDateTime = DateTime.ParseExact(fields[36].Substring(0, 17), "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);
            }

            od.AllOrNone = fields[52] == "1";

            int N = 66;
            if (!string.IsNullOrWhiteSpace(fields[62]))
            {
                N += 8;
            }

            od.TrailStopPrice = fields[N].ToDouble(); // 74
            od.TrailingPercent = fields[N + 1].ToDouble(); // 75

            // N + 2 / 76
            // N + 3 / 77
            // N + 4 / 78

            int comboLegCount = fields[N + 5].ToInt32(); // 79

            N += 6 + comboLegCount * 8;

            int orderComboLegCount = fields[N].ToInt32(); // 80

            N += 1 + orderComboLegCount;
            // N = 81
            //Console.WriteLine("1) N = " + N);

            int smartComboRoutingParamsCount = fields[N].ToInt32(); // 81

            N += 3 + smartComboRoutingParamsCount * 2;
            // N = 84
            //Console.WriteLine("2) N = " + N);

            N += (double.IsNaN(fields[N].ToDouble())) ? 1 : 8;
            // N = 85 

            N += (string.IsNullOrWhiteSpace(fields[N])) ? 1 : 2;
            // N = 86
            //Console.WriteLine("3) N = " + N);

            N += 4;
            // N = 90
            N += (fields[N] == "1") ? 4 : 1;
            // N = 91
            //Console.WriteLine("3.5) N = " + N);
            // readAlgoParams()
            if (!string.IsNullOrWhiteSpace(fields[N]))
            {
                N += 1;
                int algoParamsCount = fields[N].ToInt32();
                N += 1 + algoParamsCount * 2;
            }
            else
            {
                N += 1;
            }
            // N = 92
            N += 1;
            // N = 93;
            //Console.WriteLine("4) N = " + N);

            bool whatIf = fields[N] == "1" ? true : false;
            od.Status = (whatIf) ? OrderStatus.Inactive : ApiCode.GetEnum<OrderStatus>(fields[N + 1]).Enum;

            if (whatIf)
            {
                string InitMarginBefore = fields[N + 2];
                string MaintMarginBefore = fields[N + 3];
                string EquityWithLoanBefore = fields[N + 4];
                string InitMarginChange = fields[N + 5];
                string MaintMarginChange = fields[N + 6];
                string EquityWithLoanChange = fields[N + 7];
                string InitMarginAfter = fields[N + 8];
                string MaintMarginAfter = fields[N + 9];
                string EquityWithLoanAfter = fields[N + 10];
                string Commission = fields[N + 11];
                string MinCommission = fields[N + 12];
                string MaxCommission = fields[N + 13];
                string CommissionCurrency = fields[N + 14];
                string WarningText = fields[N + 15];

                Console.WriteLine("InitMarginBefore = " + InitMarginBefore);
                Console.WriteLine("MaintMarginBefore = " + MaintMarginBefore);
                Console.WriteLine("EquityWithLoanBefore = " + EquityWithLoanBefore);
                Console.WriteLine("InitMarginChange = " + InitMarginChange);
                Console.WriteLine("MaintMarginChange = " + MaintMarginChange);
                Console.WriteLine("EquityWithLoanChange = " + EquityWithLoanChange);
                Console.WriteLine("InitMarginAfter = " + InitMarginAfter);
                Console.WriteLine("MaintMarginAfter = " + MaintMarginAfter);
                Console.WriteLine("EquityWithLoanAfter = " + EquityWithLoanAfter);
                Console.WriteLine("Commission = " + Commission);
                Console.WriteLine("MinCommission = " + MinCommission);
                Console.WriteLine("MaxCommission = " + MaxCommission);
                Console.WriteLine("CommissionCurrency = " + CommissionCurrency);
                Console.WriteLine("WarningText = " + WarningText);
            }

            Console.WriteLine("\nParse Open Order | " + od.Status + ": " + fields.ToStringWithIndex());

            OrderManager.Update(od);
            // Emit the signal
        }

        /// <summary>
        /// Parse Open Order End: (0)"53"-(1)"1"
        /// int msgVersion = ReadInt();
        /// eWrapper.openOrderEnd();
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_OpenOrderEnd(string[] fields)
        {
            string msgVersion = fields[1];
            IsReady_OpenOrders = true;

            Console.WriteLine("\nParse Open Order End: " + fields.ToStringWithIndex());
            // TODO: fields[0].ToInt32(-1), "Parse Open Order End: " + msgVersion
            OrderManager.Update();
        }
    }
}
