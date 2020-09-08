/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Xu.GridView;

namespace Pacmio
{
    [Serializable, DataContract]
    public class BidAskData : MarketData
    {
        /*
        public BidAskData(Contract c) : base(c)
        {
            // Contract = c;
        }*/

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Ask")]
        public double Ask { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Ask Size")]
        public double AskSize { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Ask Exchange")]
        public string AskExchange { get; set; } = string.Empty;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Bid")]
        public double Bid { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Bid Size")]
        public double BidSize { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Bid Exchange")]
        public string BidExchange { get; set; } = string.Empty;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Open")]
        public double Open { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("High")]
        public double High { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Low")]
        public double Low { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Last Size")]
        public double LastSize { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Volume")]
        public double Volume { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Last Exchange")]
        public string LastExchange { get; set; } = string.Empty;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Previous Close")]
        public double PreviousClose { get; set; } = double.NaN;
    }
}
