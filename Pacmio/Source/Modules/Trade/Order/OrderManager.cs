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
        //private static readonly Dictionary<int, OrderInfo> List = new Dictionary<int, OrderInfo>();

        //public static int Count => List.Count;





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


    }
}
