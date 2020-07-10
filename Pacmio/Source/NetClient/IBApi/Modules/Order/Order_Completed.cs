/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
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
        internal static bool IsReady_CompletedOrders { get; set; } = true;

        internal static void SendRequest_CompletedOrders(bool apiOnly = false)
        {
            if (Connected && IsReady_CompletedOrders)
            {
                IsReady_CompletedOrders = false;

                SendRequest(new string[] {
                    RequestType.ReqCompletedOrders.Param(),
                    apiOnly.Param()
                });
            }
        }

        /// <summary>
        /// Received CompletedOrder: (0)"101"-(1)"4762"-(2)"BA"-(3)"STK"-(4)""-(5)"0"-(6)"?"-(7)""-(8)"SMART"-(9)"USD"-(10)"BA"-(11)"BA"-(12)"BUY"-(13)"5"-(14)"LMT"-
        /// (15)"20.0"-(16)"0.0"-(17)"DAY"-(18)""-(19)"DU332281"-(20)"O"-(21)"0"-(22)""-(23)"130443186"-(24)"0"-(25)"0"-(26)"0"-(27)""-(28)""-(29)""-(30)""-(31)""-
        /// (32)""-(33)""-(34)""-(35)""-(36)""-(37)"0"-(38)""-(39)"-1"-(40)""-(41)""-(42)""-(43)""-(44)""-(45)""-(46)"0"-(47)"0"-(48)""-(49)"3"-(50)"0"-(51)""-
        /// (52)"0"-(53)"None"-(54)""-(55)"0"-(56)"0"-(57)"0"-(58)""-(59)"0"-(60)"0"-(61)""-(62)""-(63)""-(64)"0"-(65)"0"-(66)"0"-(67)""-(68)""-(69)""-(70)""-(71)""-
        /// (72)"IB"-(73)"0"-(74)"0"-(75)""-(76)"0"-(77)"Cancelled"-(78)"0"-(79)"0"-(80)"0"-(81)"21.0"-(82)"1.7976931348623157E308"-(83)"0"-(84)"1"-(85)"0"-(86)""-
        /// (87)"0.0"-(88)"2147483647"-(89)"0"-(90)"Not an insider or substantial shareholder"-(91)"0"-(92)"0"-(93)"9223372036854775807"-(94)"20200427-13:00:03 PST"-(95)"Expired"
        /// 
        /// (93)"9223372036854775807"-(94)"20200427-13:04:53 PST"-(95)"Rejected by System: The exchange is closed."
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_CompletedOrder(string[] fields)
        {
            //int orderId = fields[1].ToInt32(-1);
            int permId = fields[23].ToInt32();
            OrderInfo od = OrderManager.GetOrAdd(permId);

            od.ConId = fields[1].ToInt32(-1);

            if (od.Contract is Contract c) c.TradeData.Add(od);

            int totalQuantity = fields[13].ToInt32(0);
            od.Quantity = fields[12] == "BUY" ? totalQuantity : -totalQuantity;

            od.Type = ApiCode.GetEnum<OrderType>(fields[14]).Enum;
            od.LimitPrice = fields[15].ToDouble();
            od.AuxPrice = fields[16].ToDouble();
            od.TimeInForce = ApiCode.GetEnum<OrderTimeInForce>(fields[17]).Enum;

            if(od.TimeInForce == OrderTimeInForce.GoodAfterDate && fields[27].Length > 5) 
            {
                od.EffectiveDateTime = DateTime.ParseExact(fields[27].Substring(0, 17), "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);
            }
            else if (od.TimeInForce == OrderTimeInForce.GoodUntilDate && fields[33].Length > 5)
            {
                od.EffectiveDateTime = DateTime.ParseExact(fields[33].Substring(0, 17), "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);
            }
           
            // fields[18]  // order.OcaGroup = eDecoder.ReadString();
            od.AccountCode = fields[19];
            // fields[20]  // order.OpenClose = eDecoder.ReadString();
            // fields[21]  // order.Origin = eDecoder.ReadInt();
            od.Description = fields[22];  // order.OrderRef = eDecoder.ReadString();

            od.OutsideRegularTradeHours = fields[24] == "1";
            od.Hidden = fields[25] == "1";
            od.ModeCode = fields[32];
            od.AllOrNone = fields[47] == "1";

            int N = 57;
            if (!string.IsNullOrWhiteSpace(fields[53]))
            {
                N += 4;
            }

            od.TrailStopPrice = fields[N].ToDouble(); // 61
            od.TrailingPercent = fields[N + 1].ToDouble(); // 62
            int comboLegCount = fields[N + 3].ToInt32(); // 64
            N += 4 + comboLegCount * 8;
            int orderComboLegCount = fields[N].ToInt32(); // 65
            N += 1 + orderComboLegCount;
            // N = 66
            int smartComboRoutingParamsCount = fields[N].ToInt32(); // 66

            N += 3 + smartComboRoutingParamsCount * 2;
            // N = 69
            N += (double.IsNaN(fields[N].ToDouble())) ? 1 : 8;
            // N = 70 
            N += (string.IsNullOrWhiteSpace(fields[N])) ? 1 : 2; // readHedgeParams() 
            // N = 71
            N += 2; // readClearingParams();
            N += 1; // readNotHeld();
            // N = 74
            N += (fields[N] == "1") ? 4 : 1; // readDeltaNeutral(); // 74
            // N = 75
            if (!string.IsNullOrWhiteSpace(fields[N])) // readAlgoParams()
            {
                N += 1;
                int algoParamsCount = fields[N].ToInt32();
                N += 1 + algoParamsCount * 2;
            }
            else
            {
                N += 1;
            }
            // N = 76
            N += 1; // readSolicited();
            // N = 77; //Console.WriteLine("4) N = " + N);
            od.Status = ApiCode.GetEnum<OrderStatus>(fields[N]).Enum;

            Console.WriteLine("\nCompleted Orders: " + fields.ToStringWithIndex());
            Console.WriteLine(OrderManager.Count + " | " + od.Status + " | " + fields[90]);

            OrderManager.Update(od);
        }

        private static void Parse_CompletedOrdersEnd(string[] fields)
        {
            IsReady_CompletedOrders = true;
            Console.WriteLine("\nCompleted Orders End: " + fields.ToStringWithIndex());
            OrderManager.Update(); // TODO: Tranmit message
        }

        /// <summary>
        /// TODO: Parse_VerifyCompleted(string[] fields)
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_VerifyCompleted(string[] fields)
        {
            Console.WriteLine("\nVerify Completed Status: " + fields.ToStringWithIndex());
        }

    }
}
