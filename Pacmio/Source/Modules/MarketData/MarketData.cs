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
using Xu.GridView;
using System.Threading;

namespace Pacmio
{
    [Serializable, DataContract]
    [KnownType(typeof(BidAskData))]
    [KnownType(typeof(StockData))]
    public class MarketData : IEquatable<MarketData>
    {
        /// <summary>
        /// Run this after loading
        /// </summary>
        /// <param name="c"></param>
        public virtual void Initialize(Contract c)
        {
            Contract = c;
            Status = MarketTickStatus.Unknown;
        }

        [IgnoreDataMember, Browsable(true), ReadOnly(true), DisplayName("Contract"), GridColumnOrder(1, 0, 0), CellRenderer(typeof(ContractCellRenderer), 150, true)]
        public Contract Contract { get; private set; }




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


        [IgnoreDataMember]
        public virtual bool IsTickActive => IB.Client.ActiveMarketDataTicks.Values.Contains(Contract);

        /// <summary>
        /// https://interactivebrokers.github.io/tws-api/tick_types.html
        /// string genericTickList = "236,375";  // 292 is news and 233 is RTVolume
        /// 
        /// Has to support option change...
        /// 
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual bool StartTicks() => IB.Client.SendRequest_MarketData(Contract, "233,236,375");

        public virtual void StopTicks() => IB.Client.SendCancel_MarketTicks(TickerId);

        [DataMember]
        public int TickerId { get; set; } = int.MinValue;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Status"), GridColumnOrder(0, 1, 0), CellRenderer(typeof(TextCellRenderer), 70)]
        public MarketTickStatus Status { get; set; } = MarketTickStatus.Unknown;

        [DataMember]
        public double MinimumTick { get; set; } = double.NaN;

        [DataMember]
        public string BBOExchangeId { get; set; } = string.Empty;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Last"), GridColumnOrder(9, 5), CellRenderer(typeof(NumberCellRenderer), 60, false)]
        public double LastPrice { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Trade Time"), GridColumnOrder(2, 0, 0), CellRenderer(typeof(TextCellRenderer), 120, true)]
        public virtual DateTime LastTradeTime { get; set; } = DateTime.MinValue;

        #endregion Quote

        #region Trade



        #endregion Trade

        #region Equality 

        // https://stackoverflow.com/questions/4219261/overriding-operator-how-to-compare-to-null
        public bool Equals(MarketData other) => other is MarketData md && GetType() == other.GetType() && Contract == md.Contract;

        public override bool Equals(object other) => other is MarketData md && Equals(md);

        public static bool operator ==(MarketData s1, MarketData s2) => s1.Equals(s2);
        public static bool operator !=(MarketData s1, MarketData s2) => !s1.Equals(s2);

        public override int GetHashCode() => Contract.GetHashCode() ^ GetType().GetHashCode();

        public override string ToString() => GetType().Name + " for " + Contract.ToString();

        #endregion Equality
    }
}
