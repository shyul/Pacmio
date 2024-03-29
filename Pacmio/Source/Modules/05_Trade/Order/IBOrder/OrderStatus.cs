﻿/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using Xu;

namespace Pacmio.IB
{
    public static partial class Client
    {
        /// <summary>
        /// Parse Order Status: (0)"3"-(1)"40"-(2)"PreSubmitted"-(3)"0"-(4)"100"-(5)"0"-(6)"2024247308"-(7)"0"-(8)"0"-(9)"182"-(10)""-(11)"0"
        /// Parse Order Status: (0)"3"-(1)"0"-(2)"PreSubmitted"-(3)"0"-(4)"100"-(5)"0"-(6)"885201288"-(7)"0"-(8)"0"-(9)"0"-(10)"child,trigger"-(11)"0"  *** because this is a cascaded order
        /// </summary>
        /// <param name="fields"></param>
        private static void Parse_OrderStatus(string[] fields)
        {
            int orderId = fields[1].ToInt32(-1);
            int permId = fields[6].ToInt32();

            //if (OrderManager.QueryForOrder(orderId, permId) is OrderInfo od)
            if (permId > 0 && OrderManager.GetOrCreateOrderByPermId(permId) is OrderInfo od)
            {
                od.Status = fields[2].ToOrderStatus();
                od.FilledQuantity = fields[3].ToDouble();
                od.RemainingQuantity = fields[4].ToDouble();
                od.AveragePrice = fields[5].ToDouble();
                od.ParentOrderId = fields[7].ToInt32();
                od.LastFillPrice = fields[8].ToDouble();
                od.ClientId = fields[9].ToInt32();
                od.HeldNotice = fields[10];
                od.MarketCapPrice = fields[11].ToDouble();

                if (od.Status == OrderStatus.Filled || od.Status == OrderStatus.Cancelled || od.Status == OrderStatus.ApiCancelled)
                {
                    RemoveRequest(orderId, false);
                }

                od.Updated();
                OrderManager.DataProvider.Updated();

                Console.WriteLine("Order Status: " + fields.ToStringWithIndex());
            }
            else
            {
                Console.WriteLine("\nUnable to locate this order: " + fields[2].ToOrderStatus() + " | Parse Order Status: " + fields.ToStringWithIndex());
            }
        }
    }
}
