﻿/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Xu;
using Xu.GridView;

namespace Pacmio
{
    /// <summary>
    /// Only used in live trades, and emulations
    /// </summary>
    [Serializable, DataContract]
    public class PositionInfo : IDataProvider, IEquatable<PositionInfo>
    {
        public PositionInfo(AccountInfo ac, Contract c)
        {
            AccountInfo = ac;
            Contract = c;
            Refreshed = true;
        }

        public AccountInfo AccountInfo { get; }

        [Browsable(true), ReadOnly(true), DisplayName("Account ID"), GridColumnOrder(1, 0, 0), GridRenderer(typeof(TextGridRenderer), 150, true)]
        public string AccountId => AccountInfo.AccountId;

        [Browsable(true), ReadOnly(true), DisplayName("Contract"), GridColumnOrder(1, 0, 0), GridRenderer(typeof(ContractGridRenderer), 150, true)]
        public Contract Contract { get; }

        public MarketData MarketData => Contract.MarketData;

        [Browsable(true), ReadOnly(true), DisplayName("Market Price"), GridColumnOrder(2), GridRenderer(typeof(NumberGridRenderer), 100)]
        public double MarketPrice => MarketData.MarketPrice;

        [Browsable(true), ReadOnly(true), DisplayName("Average Entry Price"), GridColumnOrder(3), GridRenderer(typeof(NumberGridRenderer), 100)]
        public double AverageEntryPrice { get; private set; } = double.NaN;

        /// <summary>
        /// Get information from Execution data...
        /// </summary>
        [Browsable(true), ReadOnly(true), DisplayName("Average Commission/Unit"), GridColumnOrder(4), GridRenderer(typeof(NumberGridRenderer), 100)]
        public double AverageCommissionPerUnit { get; set; } = double.NaN;

        [Browsable(true), ReadOnly(true), DisplayName("Quantity"), GridColumnOrder(5), GridRenderer(typeof(NumberGridRenderer), 100)]
        public double Quantity { get; private set; } = 0;

        public void Reset()
        {
            AverageEntryPrice = double.NaN;
            Quantity = 0;
            UpdateTime = DateTime.Now;
            Updated();
            DataConsumers.Clear();
        }

        public void Set(double qty, double price)
        {
            AverageEntryPrice = price;
            Quantity = qty;
            UpdateTime = DateTime.Now;
            Refreshed = true;
            Updated();
        }

        [Browsable(true), ReadOnly(true), DisplayName("Cost"), GridColumnOrder(6), GridRenderer(typeof(NumberGridRenderer), 100)]
        public double Cost => double.IsNaN(AverageEntryPrice) ? 0 : Math.Abs(Quantity) * AverageEntryPrice;

        [Browsable(true), ReadOnly(true), DisplayName("Unrealized PnL"), GridColumnOrder(7), GridRenderer(typeof(NumberGridRenderer), 100)]
        public double UnrealizedPnL => double.IsNaN(AverageEntryPrice) ? 0 : (MarketPrice - AverageEntryPrice) * Quantity;

        [Browsable(true), ReadOnly(true), DisplayName("Value"), GridColumnOrder(8), GridRenderer(typeof(NumberGridRenderer), 100)]
        public double Value => Cost + UnrealizedPnL; // - possible commission...

        /*
        /// <summary>
        /// To be deleted! Merge this feature to IMarketDataAnalysis
        /// </summary>
        public double BreakEvenPrice
        {
            get
            {
                double commission_for_current_position = 2 * PositionSimTools.EstimatedCommission(Math.Abs(Quantity), AveragePrice);
                return (Quantity * AveragePrice + commission_for_current_position) / Quantity;
            }
        }
         */

        public void EmergencyClose()
        {
            Contract.CancelAllOrders();

            if (Quantity != 0)
                Contract.PlaceOrder(AccountInfo, -Quantity, TradeType.StopLoss, OrderTimeInForce.GoodUntilCanceled, DateTime.Now);
        }

        public DateTime UpdateTime { get; protected set; } = DateTime.MinValue;

        public List<IDataConsumer> DataConsumers { get; } = new List<IDataConsumer>();

        public bool AddDataConsumer(IDataConsumer idk)
        {
            return DataConsumers.CheckAdd(idk);
        }

        public bool RemoveDataConsumer(IDataConsumer idk)
        {
            if (idk is DockForm df) df.ReadyToShow = false;
            return DataConsumers.CheckRemove(idk);
        }

        public void Updated()
        {
            UpdateTime = DateTime.Now;
            DataConsumers?.ForEach(n => n.DataIsUpdated(this));
        }

        public bool Refreshed { get; set; } = true;

        #region Equality

        public bool Equals(PositionInfo other) => AccountInfo == other.AccountInfo && Contract == other.Contract;

        public static bool operator ==(PositionInfo s1, PositionInfo s2) => s1.Equals(s2);
        public static bool operator !=(PositionInfo s1, PositionInfo s2) => !s1.Equals(s2);

        public override bool Equals(object other) => other is AccountInfo ac ? Equals(ac) : false;

        public override int GetHashCode() => AccountInfo.GetHashCode() ^ Contract.GetHashCode();

        #endregion Equality
    }
}