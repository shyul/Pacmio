/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xu;
using Xu.GridView;

namespace Pacmio
{
    public class OrderInfoGridView : GridWidget<OrderInfo>
    {
        public OrderInfoGridView() : base("Order History")
        {
            SourceRows = OrderManager.List;
            OrderManager.DataProvider.AddDataConsumer(this);
            DataIsUpdated(null);
        }

        ~OrderInfoGridView()
        {
            OrderManager.DataProvider.RemoveDataConsumer(this);
            Dispose();
        }

        //public int MaximumRows { get; set; } = int.MaxValue;
    }
}
