/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xu;

namespace Pacmio
{
    /// <summary>
    /// Only used in live trades, and emulations
    /// </summary>
    public class PositionInfo : IEquatable<PositionInfo>
    {
        public PositionInfo(AccountInfo ac, Contract c)
        {
            AccountInfo = ac;
            Contract = c;
            Refreshed = true;
        }

        public AccountInfo AccountInfo { get; }

        public string AccountId => AccountInfo.AccountId;

        public Contract Contract { get; }

        public MarketData MarketData => Contract.MarketData;

        public double MarketPrice => MarketData.MarketPrice;

        public double AverageEntryPrice { get; private set; } = double.NaN;

        /// <summary>
        /// Get information from Execution data...
        /// </summary>
        public double AverageCommissionPerUnit { get; set; } = double.NaN;

        public double Quantity { get; private set; } = 0;

        public void Reset()
        {
            AverageEntryPrice = double.NaN;
            Quantity = 0;
            UpdateTime = DateTime.Now;

            // TODO: Emit delta here?? Or TradeInfo list is good enough?
        }

        public void Set(double qty, double price) 
        {
            AverageEntryPrice = price;
            Quantity = qty;
            UpdateTime = DateTime.Now;
            Refreshed = true;
            // TODO: Emit delta here?? Or TradeInfo list is good enough?
        }

        public double Cost => double.IsNaN(AverageEntryPrice) ? 0 : Math.Abs(Quantity) * AverageEntryPrice;

        public double UnrealizedPnL => double.IsNaN(AverageEntryPrice) ? 0 : (MarketPrice - AverageEntryPrice) * Quantity;

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

        public DateTime UpdateTime { get; private set; } = DateTime.MinValue;

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
