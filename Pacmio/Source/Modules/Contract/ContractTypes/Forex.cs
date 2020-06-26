/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// https://interactivebrokers.github.io/tws-api/basic_contracts.html#cash
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Pacmio.IB;
using Xu;

namespace Pacmio
{
    [Serializable, DataContract(Name = "Forex")]
    public class Forex : Contract, IMarketDepth
    {
        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type")]
        public override string TypeName => "FX";

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Security Type Full Name")]
        public override string TypeFullName => "Forex";

        [DataMember]
        public double Ask { get; set; } = -double.MinValue;

        [DataMember]
        public double AskSize { get; set; } = -double.MinValue;

        [DataMember]
        public string AskExchange { get; set; } = string.Empty;

        [DataMember]
        public double Bid { get; set; } = -double.MinValue;

        [DataMember]
        public double BidSize { get; set; } = -double.MinValue;

        [DataMember]
        public string BidExchange { get; set; } = string.Empty;

        [DataMember]
        public double Open { get; set; } = -double.MinValue;

        [DataMember]
        public double High { get; set; } = -double.MinValue;

        [DataMember]
        public double Low { get; set; } = -double.MinValue;

        [DataMember]
        public double Last { get => Price; set => Price = value; }

        [DataMember]
        public double LastSize { get; set; } = -double.MinValue;

        [DataMember]
        public double Volume { get; set; } = -double.MinValue;

        [DataMember]
        public string LastExchange { get; set; } = string.Empty;

        [DataMember]
        public double LastClose { get; set; } = -double.MinValue;

        public Dictionary<int, (DateTime Time, double Price, double Size, Exchange MarketMaker)> MarketDepth => throw new NotImplementedException();

        public void Cancel_MarketDepth()
        {
            throw new NotImplementedException();
        }

        public bool Request_MarketDepth()
        {
            throw new NotImplementedException();
        }
    }
}
