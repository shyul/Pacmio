/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Xu;

namespace Pacmio
{
    [Serializable, DataContract]
    public class StockData : BidAskData
    {
        [DataMember]
        public DateTime BarTableEarliestTime { get; set; } = DateTime.MinValue;

        [DataMember]
        public Dictionary<int, (DateTime Time, double Price, double Size, Exchange MarketMaker)> MarketDepth { get; private set; }
            = new Dictionary<int, (DateTime Time, double Price, double Size, Exchange MarketMaker)>();

        [DataMember]
        public double ShortStatus { get; set; } = double.NaN;

        [DataMember]
        public double ShortableShares { get; set; } = double.NaN;

        [DataMember]
        public Dictionary<DateTime, (DataSource DataSource, double Close, double Dividend)> DividendTable { get; private set; } = new Dictionary<DateTime, (DataSource DataSource, double Close, double Dividend)>();

        [DataMember]
        public Dictionary<DateTime, (DataSource DataSource, double Split)> SplitTable { get; private set; } = new Dictionary<DateTime, (DataSource DataSource, double Split)>();

        [DataMember]
        public Dictionary<DateTime, (DataSource DataSource, double EPS)> EPSTable { get; private set; } = new Dictionary<DateTime, (DataSource DataSource, double EPS)>();

        [DataMember]
        public Dictionary<DateTime, (DataSource DataSource, double Target)> TargetPriceList { get; private set; } = new Dictionary<DateTime, (DataSource DataSource, double Target)>();
    }
}
