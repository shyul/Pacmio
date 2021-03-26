/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xu;

namespace Pacmio
{
    public static class OrderManager
    {
        public const string EntryOrderDescription = "Entry";
        public const string StopLossOrderDescription = "Stop Loss";
        public const string ProfitTakerOrderDescription = "Profit Taker";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="ac"></param>
        /// <param name="quantity">(od.Quantity > 0) ? "BUY" : "SELL"</param>
        /// <param name="tradeType"></param>
        /// <param name="tif">OrderTimeInForce.GoodUntilCanceled</param>
        /// <param name="effectiveDateTime"></param>
        /// <param name="outsideRegularTradeHours"></param>
        /// <param name="type"></param>
        /// <param name="limitPrice"></param>
        /// <param name="stopPrice"></param>
        public static void PlaceOrder(this Contract c, AccountInfo ac, double quantity,
            TradeType tradeType, OrderTimeInForce tif, DateTime effectiveDateTime, bool outsideRegularTradeHours = true,
            OrderType type = OrderType.Market, bool useSmart = true, double limitPrice = 0, double stopPrice = 0,
            bool whatIf = false, bool modify = false)
        {
            if (quantity != 0)
            {
                string desc = tradeType switch
                {
                    TradeType.Entry => EntryOrderDescription,
                    TradeType.ProfitTaker => ProfitTakerOrderDescription,
                    TradeType.StopLoss => StopLossOrderDescription,
                    _ => string.Empty
                };

                OrderInfo od = new()
                {
                    Contract = c,
                    Quantity = quantity,
                    Type = type,
                    LimitPrice = limitPrice,
                    AuxPrice = stopPrice,
                    TimeInForce = tif,
                    AccountId = ac.AccountId,
                    OutsideRegularTradeHours = outsideRegularTradeHours,
                    Description = desc
                };

                if (tif == OrderTimeInForce.GoodUntilDate || tif == OrderTimeInForce.GoodAfterDate)
                    od.LimitedEffectiveTime = effectiveDateTime;

                // This method will register this order to OrderIdToOrderLUT
                IB.Client.PlaceOrder(od, whatIf, modify, useSmart);
            }
        }

        public static void Cancel(this OrderInfo od)
        {
            if (od.Status > OrderStatus.Inactive && od.Status < OrderStatus.Filled)
            {
                IB.Client.CancelOrder(od.OrderId);
            }
        }

        public static void CancelAllOrders(this Contract c)
        {
            var orders = PermIdToOrderLUT.Values.Where(n => n.ConId == c.ConId && n.Status > OrderStatus.Inactive && n.Status < OrderStatus.Filled);

            foreach (var od in orders)
            {
                od.Cancel();
            }
        }

        public static void CancelAllOrders() => IB.Client.SendRequest_GlobalCancel();

        #region Order History

        public static void Request_OpenOrders() => IB.Client.SendRequest_OpenOrders();

        public static void Request_AllOpenOrders() => IB.Client.SendRequest_AllOpenOrders();

        public static void Request_CompleteOrders(bool apiOnly = false) => IB.Client.SendRequest_CompletedOrders(apiOnly);


        #endregion Order History

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
                DataProvider.Updated();
            }
        }

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
                    else if (permId > 0)
                    {
                        return GetOrCreateOrderByPermId(permId);
                    }
                    else 
                        return null;
                }
            }
        }

        private static Dictionary<int, OrderInfo> OrderIdToOrderLUT { get; } = new Dictionary<int, OrderInfo>();

        public static OrderInfo GetOrCreateOrderByPermId(int permId)
        {
            if (permId < 0)
                throw new Exception("The permId has to be greater than 0: " + permId);

            OrderInfo res = null;
            lock (PermIdToOrderLUT)
                if (!PermIdToOrderLUT.ContainsKey(permId))
                {
                    res = new OrderInfo() { PermId = permId };
                    PermIdToOrderLUT[permId] = res;
                }
                else
                    res = PermIdToOrderLUT[permId];

            DataProvider.Updated();
            return res;
        }

        public static IEnumerable<OrderInfo> QueryForOrders(this Contract c)
        {
            lock (PermIdToOrderLUT)
                return PermIdToOrderLUT.Values
                    .Where(n => n.ConId == c.ConId)
                    .OrderBy(n => n.OrderExecuteTime);
        }

        private static Dictionary<int, OrderInfo> PermIdToOrderLUT { get; } = new Dictionary<int, OrderInfo>();

        //public static IEnumerable<OrderInfo> List => PermIdToOrderLUT.Values;

        public static OrderInfo[] GetList()
        {
            lock (PermIdToOrderLUT)
                return PermIdToOrderLUT.Values.ToArray();
        }

        #region Updates

        public static SimpleDataProvider DataProvider { get; } = new SimpleDataProvider();

        #endregion Updates

        #region File system

        private static string FileName => Root.ResourcePath + "OrderLog.Json";

        public static void Save()
        {
            /*
            lock (PermIdToOrderLUT)
                PermIdToOrderLUT.Values.ToList().SerializeJsonFile(FileName);*/
        }

        public static void Load()
        {
            /*
            if (File.Exists(FileName))
            {
                List<OrderInfo> data = Serialization.DeserializeJsonFile<List<OrderInfo>>(FileName);
                lock (PermIdToOrderLUT)
                {
                    data.AsParallel().ForAll(od =>
                    {
                        PermIdToOrderLUT[od.PermId] = od;
                    });
                }
            }*/
        }

        #endregion File system
    }
}
