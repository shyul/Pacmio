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
using System.Runtime.Serialization;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public class PositionStatus : IRow
    {
        public Contract Contract { get; set; }

        public OrderInfo CurrentOrder { get; private set; }

        #region Position Information

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

        //public double RiskDelta { get; set; } = double.NaN;

        public double Stop { get; set; } = double.NaN;

        public double Limit { get; set; } = double.NaN;






        #region Grid View

        public object this[Column column] => throw new NotImplementedException();

        public static readonly NumericColumn Column_Quantity = new NumericColumn("QUANTITY");
        public static readonly NumericColumn Column_AveragePrice = new NumericColumn("AVG_PRICE");
        public static readonly NumericColumn Column_Value = new NumericColumn("VALUE");

        public static readonly NumericColumn Column_PNL = new NumericColumn("PNL");

        #endregion Grid View
    }
}
