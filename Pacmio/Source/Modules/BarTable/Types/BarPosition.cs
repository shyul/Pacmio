/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// BarTable Data Types
/// 
/// ***************************************************************************

using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Text;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public sealed class BarPosition
    {
        public BarPosition(Bar b, IAnalysisSetting ias)
        {
            Bar = b;
            AnalysisSetting = ias;
            //Snapshot();
            Reset();
        }

        public void Reset()
        {
            ActionType = TradeActionType.None;
            Quantity = 0;
            AveragePrice = double.NaN;
            SignalDatums.Clear();
        }

        public void Snapshot()
        {
            if (Bar.Table.LastBar_1 is Bar b_1 && b_1[AnalysisSetting] is BarPosition bp_1)
            {
                Quantity = bp_1.Quantity;
                AveragePrice = bp_1.AveragePrice;

                if (Quantity == 0)
                    ActionType = TradeActionType.None;
                else if (Quantity > 0)
                    ActionType = TradeActionType.LongHold;
                else if (Quantity < 0)
                    ActionType = TradeActionType.ShortHold;
            }
            else
                Reset();
        }

        public void Snapshot(PositionStatus ps)
        {
            double new_qty = ps.Quantity;
            if (Quantity < new_qty)
            {
                ActionType = (Quantity < 0) ? TradeActionType.Cover : TradeActionType.Long;
            }
            else if (Quantity > new_qty)
            {
                ActionType = (Quantity > 0) ? TradeActionType.Sell : TradeActionType.Short;
            }
            else
            {
                ActionType = ps.ActionType;
            }

            Quantity = new_qty;
            AveragePrice = ps.AveragePrice;
        }

        public Bar Bar { get; }

        public IAnalysisSetting AnalysisSetting { get; }

        public Dictionary<SignalColumn, SignalDatum> SignalDatums { get; } = new Dictionary<SignalColumn, SignalDatum>();

        public SignalDatum this[SignalColumn column]
        {
            get
            {
                if (SignalDatums.ContainsKey(column))
                    return SignalDatums[column];
                else
                    return null;
            }
            set
            {
                if (!SignalDatums.ContainsKey(column))
                    SignalDatums.Add(column, value);
                else
                {
                    if (value is null)
                        SignalDatums.Remove(column);
                    else
                        SignalDatums[column] = value;
                }
            }
        }

        public TradeActionType ActionType { get; private set; } = TradeActionType.None;

        public double Quantity { get; private set; } = 0;

        public double AveragePrice { get; private set; } = double.NaN;

        public double Value => double.IsNaN(AveragePrice) ? 0 : Math.Abs(Quantity * AveragePrice);

        public double PnL => Value == 0 ? 0 : Quantity * (Bar.Close - AveragePrice);

        public double PnL_Percent => Value == 0 ? 0 : 100 * (Bar.Close - AveragePrice) / AveragePrice;
    }
}
