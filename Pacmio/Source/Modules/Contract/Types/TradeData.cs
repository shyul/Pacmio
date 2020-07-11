/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
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
    public class TradeData
    {
        #region Order / Active List

        private static readonly ConcurrentDictionary<int, OrderInfo> ActiveList = new ConcurrentDictionary<int, OrderInfo>();

        private static readonly ConcurrentDictionary<int, OrderInfo> PermList = new ConcurrentDictionary<int, OrderInfo>();

        public static void AddActive(OrderInfo od)
        {
            if (od.OrderId < 0)
                throw new Exception("Can not add OrderInfo without proper orderId");

            Account ac = od.Account;
            Contract c = od.Contract;

            bool valid = ActiveList.TryAdd(od.OrderId, od);

            if (!valid)
                throw new Exception("Error adding new OrderInfo to OrderManager.ActiveList");

            ac.CurrentOrders[c] = od;
        }

        public static OrderInfo GetOrAdd(int orderId, int permId)
        {
            if (permId < 1 || orderId < 0)
                throw new Exception("Can not add OrderInfo without proper permId / orderId");

            OrderInfo od = null;

            lock (PermList)
            {
                lock (ActiveList)
                    if (ActiveList.ContainsKey(orderId))
                    {
                        ActiveList.TryRemove(orderId, out OrderInfo od_out);
                        od_out.PermId = permId;
                        PermList.CheckAdd(permId, od_out);
                        od = od_out;
                    }

                if (od is null && PermList.ContainsKey(permId))
                {
                    od = PermList[permId];
                    od.PermId = permId;
                }
            }

            return od;
        }

        public static OrderInfo GetOrAdd(int orderId, int permId, int conId)
        {
            if (permId < 1 || orderId < 0)
                throw new Exception("Can not add OrderInfo without proper permId / orderId");

            OrderInfo od = GetOrAdd(orderId, permId);

            if (od is null)
            {
                Contract c = ContractList.GetOrFetch(conId);
                od = c.TradeData.GetOrAdd(permId);
                od.ConId = conId;
                od.OrderId = orderId;
            }
            else
            {
                od.ConId = conId;
                od.PermId = permId;

                if (od.Contract is Contract c)
                    return c.TradeData.GetOrAdd(od);
            }

            return od;
        }

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

        #endregion Order / Active List


        [DataMember]
        public Dictionary<string, OrderInfo> CurrentOrder { get; private set; } = new Dictionary<string, OrderInfo>();

        [DataMember]
        public Dictionary<string, PositionStatus> Positions { get; private set; } = new Dictionary<string, PositionStatus>();



        public void Add(TradeInfo ti) => Trades.CheckAdd(ti);



        [DataMember]
        public HashSet<TradeInfo> Trades { get; private set; } = new HashSet<TradeInfo>();

    }
}
