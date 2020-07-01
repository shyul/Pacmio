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
using System.Diagnostics;

namespace Pacmio
{
    public class SimulationSet : IEquatable<SimulationSet>
    {
        public SimulationSet(Contract c, TradeRule tr)
        {
            Contract = c;
            TradeRule = tr;
            Status = new PositionStatus(c);
        }

        public readonly Contract Contract;

        public readonly TradeRule TradeRule;

        public readonly PositionStatus Status;

        public readonly SimulationResult Result = new SimulationResult();

        #region Simulation Actions

        public void AddLiquidity(DateTime time, double quantity)
        {


        }

        public void RemoveLiquidity(DateTime time, double quantity)
        {


        }

        public double SlippageRatio { get; set; } = 0.0001;

        /// <summary>
        /// Commission Calculator based on IB Tiered Fee Structure.
        /// Calculated when adding Liquidity.
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public static double EstimatedCommission(double quantity, double price)
        {
            quantity = Math.Abs(quantity);
            double value = quantity * price;
            double comms = quantity * 0.0035;
            if (comms < 0.35) comms = 0.35;
            else if (comms > 0.01 * value) comms = 0.01 * value;

            double exchange_fee = 0.00045 * quantity;
            double transaction_fee = 0.0000221 * value;
            double finra_fee = 0.00056 * comms;
            double nyse_pass_fee = 0.000175 * comms;

            return comms + exchange_fee + transaction_fee + finra_fee + nyse_pass_fee;
        }

        #endregion Simulation Actions

        public bool Equals(SimulationSet other) => Contract == other.Contract && TradeRule == other.TradeRule;
    }
}
