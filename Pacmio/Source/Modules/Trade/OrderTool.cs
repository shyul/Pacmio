/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The ultimate OrderTool
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Pacmio
{
    public static class OrderTool
    {
        public static Queue<OrderInfo> Queue { get; } = new Queue<OrderInfo>();

        public static void ExecuteOrder(this Contract c, Account ac, double quantity, double limitPrice)
        {
            if (c.PositionStatus.CurrentOrder is null)
            {
                OrderInfo od = new OrderInfo()
                {
                    Contract = c,
                    Quantity = quantity,
                    Type = OrderType.Limit,
                    LimitPrice = limitPrice,
                    AuxPrice = 0,
                    TimeInForce = OrderTimeInForce.GoodUntilCanceled,
                    AccountCode = ac.AccountCode,
                    OutsideRegularTradeHours = true,
                };

                lock (Queue)
                {
                    Queue.Enqueue(od);
                }
            }
        }

        public static void CancelOrder(this Contract c)
        {
            if (c.PositionStatus.CurrentOrder is OrderInfo od)
            {
                lock (Queue)
                {
                    if (od.Status == OrderStatus.Inactive)
                    {
                        od.Status = OrderStatus.Cancelled;
                    }
                    else if (od.Status > OrderStatus.Inactive && od.Status < OrderStatus.Filled)
                    {
                        IB.Client.CancelOrder(od.OrderId);
                    }
                }
            }
        }

        public static void CancelAllOrders() => IB.Client.SendRequest_GlobalCancel();

        public static void StartTask()
        {
            PlaceOrderTask = new Task(() => PlaceOrderWorker(), TaskCancelTs.Token);
            PlaceOrderTask.Start();
        }

        public static void StopTask()
        {
            if (!(TaskCancelTs is null)) TaskCancelTs.Cancel();
            int i = 1000;
            while ((!(PlaceOrderTask is null)) && PlaceOrderTask?.Status == TaskStatus.Running && i > 0) { Thread.Sleep(1); i--; }
            PlaceOrderTask?.Dispose();
        }

        private static CancellationTokenSource TaskCancelTs { get; set; }

        private static Task PlaceOrderTask { get; set; }

        public static void PlaceOrderWorker()
        {
            if (Queue.Count > 0)
            {
                lock (Queue)
                {
                    // The only function to allow dequeue
                    if (Queue.Dequeue() is OrderInfo od && od.Status == OrderStatus.Inactive)
                    {
                        // The only function to allow dequeue
                        IB.Client.PlaceOrder(od); // a copy of code in TradeData.cs
                    }
                }
            }
            else
            {
                Thread.Sleep(5);
            }
        }
    }
}
