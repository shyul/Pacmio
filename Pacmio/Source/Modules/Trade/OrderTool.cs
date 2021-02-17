/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The ultimate OrderTool
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacmio
{
    public static class OrderTool
    {
        public static void Execute(Contract c, Account ac, double quantity, double limitPrice) 
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


        }

        public static void Cancel(Contract c) 
        {
        
        
        }


    }
}
