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
    public class MarketData : IEquatable<MarketData>, IEquatable<Contract>
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



        #region Quote

        [DataMember]
        public MultiPeriod TradingPeriods { get; private set; } = new MultiPeriod();


        [IgnoreDataMember]
        public virtual bool IsTickActive => IB.Client.IsActive(this);

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
        public virtual bool StartTicks() => IB.Client.SendRequest_MarketData(this);

        public virtual void SnapshotTicks() => IB.Client.SendRequest_MarketData(this, true);

        public virtual void StopTicks() => IB.Client.SendCancel_MarketData(this);

        //[DataMember]
        //public int TickerId { get; set; } = int.MinValue;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Status"), GridColumnOrder(0, 1, 0), CellRenderer(typeof(TextCellRenderer), 100)]
        public MarketTickStatus Status { get; set; } = MarketTickStatus.Unknown;

        [DataMember]
        public double MinimumTick { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("P.Close"), GridColumnOrder(15), CellRenderer(typeof(NumberCellRenderer), 60)]
        public double PreviousClose { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Open"), GridColumnOrder(12), CellRenderer(typeof(NumberCellRenderer), 60)]
        public double Open { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("High"), GridColumnOrder(13), CellRenderer(typeof(NumberCellRenderer), 60)]
        public double High { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Low"), GridColumnOrder(14), CellRenderer(typeof(NumberCellRenderer), 60)]
        public double Low { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Volume"), GridColumnOrder(16), CellRenderer(typeof(NumberCellRenderer), 70)]
        public double Volume { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Last"), GridColumnOrder(9, 5), CellRenderer(typeof(NumberCellRenderer), 60, false)]
        public double LastPrice { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Last Size"), GridColumnOrder(10), CellRenderer(typeof(NumberCellRenderer), 70)]
        public double LastSize { get; set; } = double.NaN;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Last.Ex"), GridColumnOrder(11), CellRenderer(typeof(TextCellRenderer), 50)]
        public string LastExchange { get; set; } = string.Empty;

        [DataMember, Browsable(true), ReadOnly(true), DisplayName("Trade Time"), GridColumnOrder(2, 0, 0), CellRenderer(typeof(TextCellRenderer), 120, true)]
        public virtual DateTime LastTradeTime { get; set; } = DateTime.MinValue;

        #endregion Quote

        #region Trade

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



        [IgnoreDataMember]
        public double MarkPrice { get; set; } = double.NaN; // Modify, to feed the value into Position

        [IgnoreDataMember]
        public PositionStatus this[Account ac] => ac[Contract];




        [IgnoreDataMember] // Initialize
        public List<IDataView> DataViews { get; } = new List<IDataView>();


        public void Update()
        {
            UpdateTime = DateTime.Now;

            foreach (IDataView idv in DataViews)
            {
                idv.SetAsyncUpdateUI();
            }
        }

        [DataMember]
        public DateTime UpdateTime { get; set; } = DateTime.MinValue;








        #endregion Trade

        #region Equality 

        // https://stackoverflow.com/questions/4219261/overriding-operator-how-to-compare-to-null
        public bool Equals(MarketData other) => GetType() == other.GetType() && other is MarketData md && Contract == md.Contract;
        public static bool operator ==(MarketData s1, MarketData s2) => s1.Equals(s2);
        public static bool operator !=(MarketData s1, MarketData s2) => !s1.Equals(s2);

        public bool Equals(Contract other) => Contract == other;
        public static bool operator ==(MarketData s1, Contract s2) => s1.Equals(s2);
        public static bool operator !=(MarketData s1, Contract s2) => !s1.Equals(s2);

        public override bool Equals(object other) => other is MarketData md && Equals(md);

        public override int GetHashCode() => Contract.GetHashCode() ^ GetType().GetHashCode();

        public override string ToString() => GetType().Name + " for " + Contract.ToString();

        #endregion Equality
    }
}
