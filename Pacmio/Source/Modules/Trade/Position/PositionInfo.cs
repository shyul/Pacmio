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
        }

        public AccountInfo AccountInfo { get; }

        public string AccountId => AccountInfo.AccountId;

        public Contract Contract { get; }

        public MarketData MarketData => Contract.MarketData;

        public double MarketPrice => MarketData.MarketPrice;





        public double AverageEntryPrice { get; set; } = double.NaN;

        public double Quantity { get; set; } = 0;

        public double Cost => Quantity * AverageEntryPrice;

        /// <summary>
        /// TODO: What about short position?
        /// </summary>
        public double Value => Quantity * MarketPrice;

        public double PnL => double.IsNaN(AverageEntryPrice) ? 0 : (MarketPrice - AverageEntryPrice) * Quantity;

        public Strategy AssignedStrategy { get; set; }

        public OrderInfo LatestOrder { get; private set; }

        public OrderStatus OrderStatus => LatestOrder is OrderInfo od ? od.Status : OrderStatus.Inactive;



        /*
         * 
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

        /// <summary>
        /// When the order if filled
        /// </summary>
        public void CancelCurrentOrder()
        {

        }

        public void Remove()
        {


        }

        /*
         
        public List<IDataView> DataViews { get; } = new List<IDataView>();


        public void Update()
        {
            UpdateTime = DateTime.Now;

            foreach (IDataView idv in DataViews)
            {
                idv.DataIsUpdated();
            }
        }

        public DateTime UpdateTime { get; set; } = DateTime.MinValue;
         */

        #region Equality

        public bool Equals(PositionInfo other) => AccountInfo == other.AccountInfo && Contract == other.Contract;

        public static bool operator ==(PositionInfo s1, PositionInfo s2) => s1.Equals(s2);
        public static bool operator !=(PositionInfo s1, PositionInfo s2) => !s1.Equals(s2);

        public override bool Equals(object other) => other is AccountInfo ac ? Equals(ac) : false;

        public override int GetHashCode() => AccountInfo.GetHashCode() ^ Contract.GetHashCode();

        #endregion Equality
    }
}
