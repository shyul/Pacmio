﻿/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;
using Xu;
using System.Threading.Tasks;
using System.Security.Policy;
using System.Diagnostics.Eventing.Reader;

namespace Pacmio
{
    /// <summary>
    /// Master List of all trades in the program
    /// </summary>
    public static class OrderManager
    {
        public const string EntryOrderDescription = "Entry Order";
        public const string StopLossOrderDescription = "Stop Loss Order";
        public const string ProfitTakerOrderDescription = "Profit Taker Order";
        public const string ExitOrderDescription = "Exit Order";

        /// <summary>
        /// Perm Id to Order Information Lookup Table
        /// </summary>
        private static readonly Dictionary<int, OrderInfo> List = new Dictionary<int, OrderInfo>();

        public static int Count => List.Count;

        public static OrderInfo GetOrAdd(int permId)
        {
            lock (List)
            {
                if (!List.ContainsKey(permId))
                {
                    List.Add(permId, new OrderInfo() { PermId = permId });
                }

                

                return List[permId];
            }
        }

        public static OrderInfo GetOrAdd(OrderInfo od)
        {
            if (od.PermId < 1) throw new Exception("Can not add OrderInfo without proper permId");

            if (!List.ContainsKey(od.PermId))
            {
                List.Add(od.PermId, od);
            }
            /*
            OrderInfo od2 = List[od.PermId];
            if(od2.Contract is Contract c)
            {
                c.TradeData.Add(od2);
            }*/

            return List[od.PermId];
        }

        #region Active List

        private static readonly ConcurrentDictionary<int, OrderInfo> ActiveList = new ConcurrentDictionary<int, OrderInfo>();

        public static bool AddNew(OrderInfo od)
        {
            Account ac = od.Account;
            Contract c = od.Contract;

            c.TradeData.Add(od);

            bool valid = ActiveList.TryAdd(od.OrderId, od);
            ac.CurrentOrders[c] = od;

            return valid;
        }

        public static OrderInfo GetOrAdd(int orderId, int permId)
        {
            lock (ActiveList)
                if (ActiveList.ContainsKey(orderId))
                {
                    OrderInfo od = ActiveList[orderId];
                    if (permId > 0)
                    {
                        od.PermId = permId;
                        ActiveList.TryRemove(orderId, out _);
                        return GetOrAdd(od);
                    }
                    else
                        return od;
                }
                else
                {
                    return GetOrAdd(permId);
                }
        }

        #endregion Active List

        #region Order Actions

        public static void PlaceOrder(OrderInfo od)
        {
            if (od.Account.CurrentOrders.ContainsKey(od.Contract) && od.Account.CurrentOrders[od.Contract] is OrderInfo odi && odi.IsEditable)
            {
                throw new Exception("There is an on-going order for this contract at the same account!");
            }

            IB.Client.PlaceOrder(od);
        }

        public static void ModifyOrder(OrderInfo od)
        {

        }

        public static void CancelOrder(OrderInfo od)
        {

        }

        public static void CancelAllOrders() => IB.Client.SendRequest_GlobalCancel();

        public static void CloseAllPositions()
        {
            CancelAllOrders();
            foreach (Account ac in AccountManager.List)
                ac.CloseAllPositions();
        }

        #endregion Order Actions

        #region Data Requests

        public static void Request_OpenOrders() => IB.Client.SendRequest_OpenOrders();

        public static void Request_AllOpenOrders() => IB.Client.SendRequest_AllOpenOrders();

        public static void Request_CompleteOrders(bool apiOnly = false) => IB.Client.SendRequest_CompletedOrders(apiOnly);

        #region Updates

        public static DateTime UpdatedTime { get; set; }

        public static void Update(OrderInfo od)
        {
            UpdatedTime = DateTime.Now;
        }

        public static void Update()
        {
            UpdatedTime = DateTime.Now;
        }

        #endregion Updates

        #endregion Data Requests

        #region File system

        private static string FileName => Root.ResourcePath + "Orders.Json";

        public static void Save()
        {
            lock (List)
                List.Values.ToList().SerializeJsonFile(FileName);
        }

        /// <summary>
        /// This function should be used only at the startup of this application.
        /// </summary>
        public static void Load()
        {
            if (File.Exists(FileName))
            {
                List<OrderInfo> data = Serialization.DeserializeJsonFile<List<OrderInfo>>(FileName);

                data.AsParallel().ForAll(od => {
                    lock (List) List[od.PermId] = od;
                    if (od.IsEditable)
                        od.Account.CurrentOrders[od.Contract] = od;
                    /*
                    if (od.Contract is Contract c)
                    {
                        c.TradeData.Add(od);
                    }*/
                });
            }
        }

        #endregion File system
    }
}
