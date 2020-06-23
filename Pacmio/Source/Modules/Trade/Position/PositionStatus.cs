/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using Xu;

namespace Pacmio
{
    public class PositionStatus : IRow
    {
        public PositionStatus(Contract c)
        {
            Contract = c;
        }

        public Contract Contract { get; }

        #region Position Information

        public TradeActionType ActionType
        {
            get
            {
                if (Quantity > 0)
                    return TradeActionType.LongHold;
                else if (Quantity < 0)
                    return TradeActionType.ShortHold;
                else
                    return TradeActionType.None;
            }
        }

        public double Quantity { get; set; } = 0;

        public double AveragePrice { get; set; } = double.NaN;

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

        #region Risk Management

        //public double RiskDelta { get; set; } = double.NaN;

        public double Stop { get; set; } = double.NaN;

        public double Limit { get; set; } = double.NaN;

        #endregion Risk Management

        #region Grid View

        public object this[Column column]
        {
            get
            {
                return column switch
                {
                    ContractColumn _ => Contract,
                    NumericColumn sc when sc == Column_Quantity => Quantity,
                    NumericColumn sc when sc == Column_AveragePrice => AveragePrice,
                    NumericColumn dc when dc == Column_Value => Value,
                    NumericColumn dc when dc == MarketData.Column_Last => Contract.MarketData.Last,
                    NumericColumn dc when dc == Column_PNL => 20,

                    _ => null,
                };
            }
        }

        public static readonly NumericColumn Column_Quantity = new NumericColumn("QUANTITY");
        public static readonly NumericColumn Column_AveragePrice = new NumericColumn("AVG_PRICE");
        public static readonly NumericColumn Column_Value = new NumericColumn("VALUE");


        public static readonly NumericColumn Column_PNL = new NumericColumn("PNL");

        #endregion Grid View
    }
}
