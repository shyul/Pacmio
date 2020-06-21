/// ***************************************************************************
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
        private static readonly ConcurrentDictionary<int, OrderInfo> List = new ConcurrentDictionary<int, OrderInfo>();

        public static int Count => List.Count;

        public static OrderInfo Get(int permId)
        {
            if (List.ContainsKey(permId)) return List[permId];
            else return null;
        }

        public static OrderInfo GetOrAdd(OrderInfo od)
        {
            if (!List.ContainsKey(od.PermId) && od.PermId > 0)
            {
                List.TryAdd(od.PermId, od);

                if (od.IsEditable)
                    od.Account.CurrentOrders[od.Contract] = od;

                return od;
            }
            else 
            {
                return List[od.PermId];
            }
        }

        public static IEnumerable<OrderInfo> GetActiveOrder(Account ac, Contract c)
        {
            return List.Values.Where(od => od.IsEditable && od.AccountCode == ac.AccountCode && od.Contract == c);
        }

        #region Order Actions

        public static void CloseAllPositions()
        {
            CancelAllOrders();
            foreach (Account ac in AccountManager.List)
                ac.CloseAllPositions();
        }

        public static void CancelAllOrders() => Root.IBClient.SendRequest_GlobalCancel();

        #endregion Order Actions

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

        #region File system

        private static string FileName => Root.ResourcePath + "Orders.Json";

        public static void Save()
        {
            lock (List)
            {
                List.Values.ToArray().SerializeJsonFile(FileName);
            }
        }

        public static void Load()
        {
            if (File.Exists(FileName))
            {
                var list = Serialization.DeserializeJsonFile<OrderInfo[]>(FileName);
                foreach (OrderInfo od in list)
                {
                    GetOrAdd(od);
                }
            }
        }

        #endregion File system
    }
}
