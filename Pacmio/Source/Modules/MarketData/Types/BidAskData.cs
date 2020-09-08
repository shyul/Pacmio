/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
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

        [DataMember, GridColumn("Ask")]
        public double Ask { get; set; } = double.NaN;

        [DataMember, GridColumn("Ask Size")]
        public double AskSize { get; set; } = double.NaN;

        [DataMember, GridColumn("Ask Exchange")]
        public string AskExchange { get; set; } = string.Empty;

        [DataMember, GridColumn("Bid")]
        public double Bid { get; set; } = double.NaN;

        [DataMember, GridColumn("Bid Size")]
        public double BidSize { get; set; } = double.NaN;

        [DataMember, GridColumn("Bid Exchange")]
        public string BidExchange { get; set; } = string.Empty;

        [DataMember, GridColumn("Open")]
        public double Open { get; set; } = double.NaN;

        [DataMember, GridColumn("High")]
        public double High { get; set; } = double.NaN;

        [DataMember, GridColumn("Low")]
        public double Low { get; set; } = double.NaN;

        [DataMember, GridColumn("Last Size")]
        public double LastSize { get; set; } = double.NaN;

        [DataMember, GridColumn("Volume")]
        public double Volume { get; set; } = double.NaN;

        [DataMember, GridColumn("Last Exchange")]
        public string LastExchange { get; set; } = string.Empty;

        [DataMember, GridColumn("Previous Close")]
        public double PreviousClose { get; set; } = double.NaN;
    }
}
