using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacmio
{
    public static class OrderManager
    {
        private static Dictionary<int, OrderInfo> OrderIdToOrderLUT { get; } = new Dictionary<int, OrderInfo>();

        /// <summary>
        /// This method is supposed by be access by "IB.Client.CancelOrder(od.OrderId)" only
        /// </summary>
        /// <param name="od"></param>
        public static void InsertOrderWithoutPermId(OrderInfo od) 
        {
            lock (OrderIdToOrderLUT)
            {
                if (od.OrderId < 0)
                    throw new Exception("You must assign a proper OrderId to this order in order to list in the LUT");

                if (od.PermId > 0)
                    throw new Exception("Add fresh order only. Can not add an order with PermId assigned.");

                if (OrderIdToOrderLUT.ContainsKey(od.OrderId))
                    throw new Exception("Duplicated Order Id found in 'OrderIdToOrderLUT', the fresh order list. This is not allowed.");

                OrderIdToOrderLUT[od.OrderId] = od;
            }
        }

        private static Dictionary<int, OrderInfo> PermIdToOrderLUT { get; } = new Dictionary<int, OrderInfo>();

        public static OrderInfo QueryForOrder(int orderId, int permId = -1)
        {
            lock (OrderIdToOrderLUT)
            {
                lock (PermIdToOrderLUT) 
                {
                    if (OrderIdToOrderLUT.ContainsKey(orderId))
                    {
                        OrderInfo od = OrderIdToOrderLUT[orderId];

                        if (permId > 0)
                        {
                            od.PermId = permId;
                            if (!PermIdToOrderLUT.ContainsKey(permId))
                            {
                                PermIdToOrderLUT[permId] = od;
                                OrderIdToOrderLUT.Remove(orderId);
                            }
                            else
                                throw new Exception("Found an item in PermIdToOrderLUT with the same PermId" + permId + ", Please check the mechanismes of handling fresh OrderInfo.");
                        }
                        return od;
                    }
                    else if (PermIdToOrderLUT.ContainsKey(permId))
                    {
                        return PermIdToOrderLUT[permId];
                    }
                    else
                        return null;
                }
            }
        }


        private static Dictionary<string, TradeInfo> ExecIdToTradeLUT { get; } = new Dictionary<string, TradeInfo>();

        public static List<OrderInfo> QueryForOrders(Contract c)
        {
            lock (PermIdToOrderLUT)
                return PermIdToOrderLUT.Values
                    .Where(n => n.ConId == c.ConId)
                    .OrderBy(n => n.OrderTime)
                    .ToList();
        }

        public static List<TradeInfo> QueryForTrades(OrderInfo od)
        {
            lock (ExecIdToTradeLUT)
                return ExecIdToTradeLUT.Values
                    .Where(n => n.ConId == od.ConId && n.PermId > 0 && n.PermId == od.PermId)
                    .OrderBy(n => n.ExecuteTime)
                    .ToList();
        }

        public static List<TradeInfo> QueryForTrades(Contract c)
        {
            lock (ExecIdToTradeLUT)
                return ExecIdToTradeLUT.Values
                    .Where(n => n.Contract == c)
                    .OrderBy(n => n.ExecuteTime)
                    .ToList();
        }
    }
}
