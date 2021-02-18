/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{



    [Serializable, DataContract(Name = "TradeData")]
    public class OrderInfoManager
    {
        #region Order / Active List



        [DataMember]
        public HashSet<OrderInfo> Orders { get; private set; } = new HashSet<OrderInfo>();

        public OrderInfo GetOrAdd(OrderInfo od)
        {
            if (od.PermId < 1)
                throw new Exception("Can not add OrderInfo without proper permId");

            lock (Orders)
            {
                var list = Orders.Where(n => n.PermId == od.PermId);

                if (list.Count() > 0)
                    return list.First();
                else
                {
                    Orders.Add(od);
                    return od;
                }
            }
        }

        public OrderInfo GetOrAdd(int permId)
        {
            if (permId < 1)
                throw new Exception("Can not add OrderInfo without proper permId");

            var list = Orders.Where(n => n.PermId == permId);

            if (list.Count() > 0)
                return list.First();
            else
            {
                OrderInfo od = new OrderInfo { PermId = permId };
                return od;
            }
        }

        #region Order Actions

        public const string EntryOrderDescription = "Entry Order";
        public const string StopLossOrderDescription = "Stop Loss Order";
        public const string ProfitTakerOrderDescription = "Profit Taker Order";
        public const string ExitOrderDescription = "Exit Order";

        public static void PlaceOrder(OrderInfo od)
        {
            if (od.Account.CurrentOrders.ContainsKey(od.Contract) && od.Account.CurrentOrders[od.Contract] is OrderInfo odi && odi.IsEditable)
            {
                throw new Exception("There is an on-going order for this contract at the same account!");
            }

            IB.Client.PlaceOrder(od);
        }



        public static void CancelAllOrders() => IB.Client.SendRequest_GlobalCancel();

        public static void CloseAllPositions()
        {
            CancelAllOrders();
            foreach (Account ac in AccountManager.List)
                ac.CloseAllPositions();
        }

        #endregion Order Actions

        public static void Request_OpenOrders() => IB.Client.SendRequest_OpenOrders();

        public static void Request_AllOpenOrders() => IB.Client.SendRequest_AllOpenOrders();

        public static void Request_CompleteOrders(bool apiOnly = false) => IB.Client.SendRequest_CompletedOrders(apiOnly);


        #endregion Order / Active List




        public void Add(TradeInfo ti) => Trades.CheckAdd(ti);



        [DataMember]
        public HashSet<TradeInfo> Trades { get; private set; } = new HashSet<TradeInfo>();

    }
}
