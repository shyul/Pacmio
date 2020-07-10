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
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    [Serializable, DataContract(Name = "TradeData")]
    public class TradeData
    {
        /*
        public TradeData()
        {
            Setup();
        }

        public void Setup() 
        {
            Positions = new Dictionary<string, PositionStatus>();
            Orders = new List<OrderInfo>();
            Trades = new List<TradeInfo>();
        }*/


        [DataMember]
        public Dictionary<string, OrderInfo> CurrentOrder { get; private set; } = new Dictionary<string, OrderInfo>();

        [DataMember]
        public Dictionary<string, PositionStatus> Positions { get; private set; } = new Dictionary<string, PositionStatus>();

        public void Add(OrderInfo od) => Orders.CheckAdd(od);

        public void Add(TradeInfo ti) => Trades.CheckAdd(ti);

        [DataMember]
        public HashSet<OrderInfo> Orders { get; private set; } = new HashSet<OrderInfo>();

        [DataMember]
        public HashSet<TradeInfo> Trades { get; private set; } = new HashSet<TradeInfo>();

    }
}
