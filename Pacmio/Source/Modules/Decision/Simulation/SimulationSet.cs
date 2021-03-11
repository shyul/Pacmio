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
            Strategy = tr;
            //Status = new PositionStatus(c);

            BarTableSet = new IntraDayBarTableSet(Contract);
        }

        public Contract Contract { get; }

        public Strategy Strategy { get; }

        public SimulationResult Result { get; } = new SimulationResult();

        public IntraDayBarTableSet BarTableSet { get; } 



        public bool Equals(SimulationSet other) => Contract == other.Contract && Strategy == other.Strategy;
    }

    public abstract class BarDecision : BarAnalysis
    {
        #region Simulation Actions

        public void AddLiquidity(DateTime time, double quantity)
        {


        }

        public void RemoveLiquidity(DateTime time, double quantity)
        {


        }

        public double SlippageRatio { get; set; } = 0.0001;

        #endregion Simulation Actions


        /// <summary>
        /// Bar Analysis vs Multi Time Frame
        /// </summary>
        public Dictionary<(BarFreq BarFreq, BarType BarType), BarAnalysisSet> BarAnalysisLUT { get; } = new Dictionary<(BarFreq BarFreq, BarType BarType), BarAnalysisSet>();

        public virtual void ClearBarAnalysisSet() => BarAnalysisLUT.Clear();

        public virtual BarAnalysisSet this[BarFreq BarFreq, BarType BarType = BarType.Trades]
        {
            get
            {
                if (BarAnalysisLUT.ContainsKey((BarFreq, BarType)))
                    return BarAnalysisLUT[(BarFreq, BarType)];
                else
                    return null;
            }
            set
            {
                if (value is BarAnalysisSet bas)
                    BarAnalysisLUT[(BarFreq, BarType)] = new BarAnalysisSet(bas);
                else if (BarAnalysisLUT.ContainsKey((BarFreq, BarType)))
                    BarAnalysisLUT.Remove((BarFreq, BarType));
            }
        }

        // Getting the tradabe score, and priority
        // The example values are showing using the trailing 5 days value to yield risk / reward ratio, win rate, and standard deviation of the returns.
        // The above result will yield the score for sorting the strategy
        // The the score will be valid for 1 day trading.
    }

    public class SimulationDatum 
    {
    
    
    }
}
