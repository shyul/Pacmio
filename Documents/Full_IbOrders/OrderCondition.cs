/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - shyu.lee@gmail.com
/// 
/// Interactive Brokers API
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;
using Xu;

namespace Pacmio
{
    public class OrderCondition
    {
        public string Param => "0";

        /*
        paramsList.Add(order.Conditions.Count);

        if (order.Conditions.Count > 0)
        {
            foreach (OrderCondition item in order.Conditions)
            {
                paramsList.Add((int)item.Type);
                item.Serialize(paramsList);
            }

            paramsList.Add(order.ConditionsIgnoreRth);
            paramsList.Add(order.ConditionsCancelOrder);
        }
        */
    }
}
