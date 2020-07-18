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
    [KnownType(typeof(BidAskData))]
    [KnownType(typeof(HistoricalData))]
    public class MarketData
    {
        #region Basic Info  

        [DataMember]
        public HashSet<string> DerivativeTypes { get; private set; } = new HashSet<string>();

        [DataMember]
        public HashSet<string> ValidExchanges { get; private set; } = new HashSet<string>();

        [DataMember]
        public HashSet<string> OrderTypes { get; private set; } = new HashSet<string>();

        /// <summary>
        /// TODO: Change to Rules
        /// </summary>
        [DataMember]
        public HashSet<string> MarketRules { get; private set; } = new HashSet<string>();

        #endregion Basic Info 

        #region Quote

        [DataMember]
        public MultiPeriod TradingPeriods { get; private set; } = new MultiPeriod();

        [DataMember]
        public int TickerId { get; set; } = int.MinValue;

        [DataMember]
        public MarketTickStatus Status { get; set; } = MarketTickStatus.Unknown;

        [DataMember]
        public double MinimumTick { get; set; } = double.NaN;

        [DataMember]
        public string BBOExchangeId { get; set; } = string.Empty;

        [DataMember]
        public double LastPrice { get; set; } = double.NaN;

        [DataMember]
        public virtual DateTime LastTradeTime { get; set; } = DateTime.MinValue;

        #endregion Quote

        #region Trade



        #endregion Trade
    }
}
