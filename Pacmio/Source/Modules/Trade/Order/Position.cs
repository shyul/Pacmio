/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using Xu;
using Xu.GridView;
using IbXmlAE;

namespace Pacmio
{
    public class Position
    {
        public Position(Account ac, MarketData md)
        {
            Account = ac;
            MarketData = md;
        }

        public Account Account { get; }

        public MarketData MarketData { get; }

        public Contract Contract => MarketData.Contract;

        #region Order

        public OrderInfo CurrentOrder { get; private set; }

        public OrderStatus OrderStatus => CurrentOrder is OrderInfo od ? od.Status : OrderStatus.Inactive;

        public List<OrderInfo> OrderHistory { get; } = new List<OrderInfo>();

        public List<TradeInfo> TradeHistory { get; } = new List<TradeInfo>();



        public double ExpectedQuantity
        {
            get => m_ExpectedQuantity;
            set
            {
                double val = value;
                double qty = val - m_ExpectedQuantity;

                if (qty > 0) // Remove Quantity
                {
                    // 1. Try modify current order
                    // 2. Wait until current order is settled
                    // 3. Get the actual number
                    // 4. Setup new order and make up that number! 

                }
                else if (qty < 0) // Add Quantity
                {


                }

            }
        }
        private double m_ExpectedQuantity = 0;

        #endregion Order

        #region Position Information

        public double Quantity { get; set; } = 0;

        public double AveragePrice { get; set; } = double.NaN;

        public double Value => double.IsNaN(AveragePrice) ? 0 : Math.Abs(Quantity * AveragePrice);

        public double MarkPrice => MarketData.MarkPrice;

        public double PnL => double.IsNaN(AveragePrice) ? 0 : (MarkPrice - AveragePrice) * Quantity;



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



        #endregion  Position Information





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
    }


}
