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

namespace Pacmio
{
    public class PositionStatus
    {
        public PositionStatus(Account ac, MarketData md)
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

        public IEnumerable<OrderInfo> OrderHistory { get; }

        public IEnumerable<TradeInfo> TradeHistory { get; }



        public double ExpectedQuantity
        {
            get => m_ExpectedQuantity;
            set
            {
                double val = value;
                double qty = val - m_ExpectedQuantity;

                if (qty > 0) // Remove Quantity
                {


                }
                else if (qty < 0) // Add Quantity
                {


                }

            }
        }
        private double m_ExpectedQuantity = 0;

        public double EntryLimit { get; set; } = double.NaN;

        public double ExitLimit { get; set; } = double.NaN;

        #endregion Order

        #region Position Information




        public double Quantity { get; set; } = 0;

        public double AveragePrice { get; set; } = double.NaN;

        public double MarkPrice
        {
            get => m_MarkPrice;
            set
            {
                m_MarkPrice = value;
                if (Quantity < 0 && m_MarkPrice > ExitLimit)
                {

                }


            }
        }
        private double m_MarkPrice = double.NaN;








        public double BreakEvenPrice
        {
            get
            {
                double commission_for_current_position = 2 * PositionSimTools.EstimatedCommission(Math.Abs(Quantity), AveragePrice);
                return (Quantity * AveragePrice + commission_for_current_position) / Quantity;
            }
        }

        public double Value => double.IsNaN(AveragePrice) ? 0 : Math.Abs(Quantity * AveragePrice);

        #endregion  Position Information





        public List<IDataView> DataViews { get; } = new List<IDataView>();


        public void Update()
        {
            UpdateTime = DateTime.Now;

            foreach (IDataView idv in DataViews)
            {
                idv.SetAsyncUpdateUI();
            }
        }


        public DateTime UpdateTime { get; set; } = DateTime.MinValue;
    }

    public class PositionAnalysis
    {

    }
}
