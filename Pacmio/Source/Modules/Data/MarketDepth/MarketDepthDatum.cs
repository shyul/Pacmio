/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.Serialization;
using Xu;
using Xu.GridView;

namespace Pacmio
{
    /// <summary>
    /// Received MarketDepthL2: (0)"13"-(1)"1"-(2)"20000001"-(3)"0"-(4)"AMEX"-(5)"0"-(6)"1"-(7)"213.50"-(8)"1"-(9)"1"
    /// </summary>
    public class MarketDepthDatum
    {
        public MarketDepthDatum(int depth) => Depth = depth;

        /// <summary>
        /// Position of the Depth
        /// </summary>

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Position"), GridColumnOrder(1, 10), GridRenderer(typeof(NumberGridRenderer), 70)]
        public int Depth { get; }

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Ask"), GridColumnOrder(5, 10), GridRenderer(typeof(NumberGridRenderer), 70)]
        public double Ask { get; set; }

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Ask Size"), GridColumnOrder(4, 11), GridRenderer(typeof(NumberGridRenderer), 80)]
        public double AskSize { get; set; }

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Ask Exch"), GridColumnOrder(3), GridRenderer(typeof(TextGridRenderer), 90)]
        public string AskExchangeCode { get; set; } = string.Empty;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Ask Ops"), GridColumnOrder(2), GridRenderer(typeof(TextGridRenderer), 70)]
        public int AskOperation { get; set; }

        [DataMember]
        public DateTime AskTime { get; set; }

        [DataMember]
        public bool AskIsSmartDepth { get; set; }



        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Bid"), GridColumnOrder(6, 10), GridRenderer(typeof(NumberGridRenderer), 70)]
        public double Bid { get; set; }

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Bid Size"), GridColumnOrder(7, 11), GridRenderer(typeof(NumberGridRenderer), 80)]
        public double BidSize { get; set; }

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Bid Exch"), GridColumnOrder(8), GridRenderer(typeof(TextGridRenderer), 90)]
        public string BidExchangeCode { get; set; } = string.Empty;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Bid Ops"), GridColumnOrder(9), GridRenderer(typeof(TextGridRenderer), 70)]
        public int BidOperation { get; set; }

        [DataMember]
        public DateTime BidTime { get; set; }

        [DataMember]
        public bool BidIsSmartDepth { get; set; }

    }
}
