/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    [Serializable, DataContract]
    public class BidAskData : MarketData
    {
        [DataMember]
        public double Ask { get; set; } = double.NaN;

        [DataMember]
        public double AskSize { get; set; } = double.NaN;

        [DataMember]
        public string AskExchange { get; set; } = string.Empty;

        [DataMember]
        public double Bid { get; set; } = double.NaN;

        [DataMember]
        public double BidSize { get; set; } = double.NaN;

        [DataMember]
        public string BidExchange { get; set; } = string.Empty;

        [DataMember]
        public double Open { get; set; } = double.NaN;

        [DataMember]
        public double High { get; set; } = double.NaN;

        [DataMember]
        public double Low { get; set; } = double.NaN;

        [DataMember]
        public double LastSize { get; set; } = double.NaN;

        [DataMember]
        public double Volume { get; set; } = double.NaN;

        [DataMember]
        public string LastExchange { get; set; } = string.Empty;

        [DataMember]
        public double PreviousClose { get; set; } = double.NaN;
    }
}
