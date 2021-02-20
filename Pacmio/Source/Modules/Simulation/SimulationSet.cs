/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;


namespace Pacmio
{
    public class SimulationSet : IEquatable<SimulationSet>
    {
        public SimulationSet(Contract c, Strategy tr)
        {
            Contract = c;
            TradeRule = tr;
            //Status = new PositionStatus(c);
        }

        public readonly Contract Contract;

        public readonly Strategy TradeRule;

        public readonly SimulationResult Result = new SimulationResult();



        #region Simulation Actions

        public void AddLiquidity(DateTime time, double quantity)
        {


        }

        public void RemoveLiquidity(DateTime time, double quantity)
        {


        }

        public double SlippageRatio { get; set; } = 0.0001;

        #endregion Simulation Actions

        public bool Equals(SimulationSet other) => Contract == other.Contract && TradeRule == other.TradeRule;
    }
}
