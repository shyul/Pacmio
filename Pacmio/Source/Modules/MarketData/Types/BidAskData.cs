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
        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Ask"), GridColumnOrder(6, 10), CellRenderer(typeof(NumberCellRenderer), 60)]
        public double Ask { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Ask Size"), GridColumnOrder(7, 11), CellRenderer(typeof(NumberCellRenderer), 70)]
        public double AskSize { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Ask Exchange"), GridColumnOrder(8, 12), CellRenderer(typeof(TextCellRenderer), 80, true)]
        public string AskExchange { get; set; } = string.Empty;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Bid"), GridColumnOrder(5, 10), CellRenderer(typeof(NumberCellRenderer), 60)]
        public double Bid { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Bid Size"), GridColumnOrder(4, 11), CellRenderer(typeof(NumberCellRenderer), 70)]
        public double BidSize { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Bid Exchange"), GridColumnOrder(3, 12), CellRenderer(typeof(TextCellRenderer), 80, true)]
        public string BidExchange { get; set; } = string.Empty;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Last Size"), GridColumnOrder(10), CellRenderer(typeof(NumberCellRenderer), 70)]
        public double LastSize { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Last.Ex"), GridColumnOrder(11), CellRenderer(typeof(TextCellRenderer), 50)]
        public string LastExchange { get; set; } = string.Empty;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Open"), GridColumnOrder(12), CellRenderer(typeof(NumberCellRenderer), 60)]
        public double Open { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("High"), GridColumnOrder(13), CellRenderer(typeof(NumberCellRenderer), 60)]
        public double High { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Low"), GridColumnOrder(14), CellRenderer(typeof(NumberCellRenderer), 60)]
        public double Low { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("P.Close"), GridColumnOrder(15), CellRenderer(typeof(NumberCellRenderer), 60)]
        public double PreviousClose { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Volume"), GridColumnOrder(16), CellRenderer(typeof(NumberCellRenderer), 70)]
        public double Volume { get; set; } = double.NaN;
    }
}
