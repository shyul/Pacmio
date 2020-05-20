/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio.IB
{
    public partial class Client
    {
        /// <summary>
        /// Parse Order Status: (0)"3"-(1)"40"-(2)"PreSubmitted"-(3)"0"-(4)"100"-(5)"0"-(6)"2024247308"-(7)"0"-(8)"0"-(9)"182"-(10)""-(11)"0"
        /// Parse Order Status: (0)"3"-(1)"0"-(2)"PreSubmitted"-(3)"0"-(4)"100"-(5)"0"-(6)"885201288"-(7)"0"-(8)"0"-(9)"0"-(10)"child,trigger"-(11)"0"  *** because this is a cascaded order
        /// </summary>
        /// <param name="fields"></param>
        private void Parse_OrderStatus(string[] fields)
        {
            int orderId = fields[1].ToInt32(-1);
            int permId = fields[6].ToInt32();

            OrderInfo od = PendingOrder.ContainsKey(orderId) ? PendingOrder[orderId] : new OrderInfo() { OrderId = orderId };
            od.PermId = permId;
            od = OrderManager.Add(od);

            if (PendingOrder.ContainsKey(orderId) && permId > 0) PendingOrder.Remove(orderId);

            od.Status = ApiCode.GetEnum<OrderStatus>(fields[2]).Enum;
            od.FilledQuantity = fields[3].ToDouble();
            od.RemainingQuantity = fields[4].ToDouble();
            od.AveragePrice = fields[5].ToDouble();
            od.PermId = permId;
            od.ParentOrderId = fields[7].ToInt32();
            od.LastFillPrice = fields[8].ToDouble();
            od.ClientId = fields[9].ToInt32();
            od.HeldNotice = fields[10];
            od.MarketCapPrice = fields[11].ToDouble();

            OrderManager.Update(fields[0].ToInt32(-1), orderId.ToString());

            Console.WriteLine("\nParse Order Status: " + fields.ToStringWithIndex());

            if(od.Status == OrderStatus.Filled || od.Status == OrderStatus.Cancelled)
            {
                SendRequest_Postion();
                SendRequest_AccountSummary();
            }
        }
    }
}
