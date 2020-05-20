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
        /// <summary>
        /// Perm Id to Order Information Lookup Table
        /// </summary>
        private static readonly Dictionary<int, OrderInfo> List = new Dictionary<int, OrderInfo>();

        public static int Count => List.Count;

        public static OrderInfo Add(OrderInfo od)
        {
            if (!List.ContainsKey(od.PermId) && od.PermId > 0)
            {
                List.Add(od.PermId, od);
                return od;
            }
            else 
            {
                return List[od.PermId];
            }
        }

        public static OrderInfo Get(int permId)
        {
            if (List.ContainsKey(permId)) return List[permId];
            else return null;
        }

        public static IEnumerable<OrderInfo> GetActiveOrder(Account ac, Contract c)
        {
            return List.Values.Where(od => od.IsEditable && od.Account == ac.AccountCode && od.Contract == c);
        }

        public static DateTime UpdatedTime { get; set; }

        public static event StatusEventHandler UpdatedHandler;

        public static void Update(int statusCode, string message = "")
        {
            UpdatedTime = DateTime.Now;
            UpdatedHandler?.Invoke(statusCode, UpdatedTime, message);
        }
    }
}
