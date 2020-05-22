/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    /// <summary>
    /// All static alike functions!!
    /// </summary>
    public abstract class ObsoleteStrategy : IEquatable<ObsoleteStrategy>
    {
        public virtual string Name { get; }

        /// <summary>
        /// Smallest Bar Frequency as the primary trading length.
        /// </summary>
        public BarFreq PrimaryBarFreq { get; set; } = BarFreq.Minute;


        #region Order Sizing

        public double MaximumPositionPercent { get; set; } = 0.05;

        /// <summary>
        /// Tradelist also defines the risk percent for the whole account
        /// We define the maximum risk here
        /// will pick up which ever comes tighter.
        /// </summary>
        public double MaximumRiskPercent { get; set; } = 0.05;

        #endregion

        public abstract IEnumerable<Contract> GetWatchList();


        public readonly ObsoleteBarAnalysisStack BarAnalysisStack = new ObsoleteBarAnalysisStack();


        // Initialize. and guarantee all basic elements are met.
        public abstract void Setup(Contract c);

        public abstract void RunLiveTrade(Contract c);

        public abstract void Simulate(Contract c, SimulationAccount ac);



        public bool Equals(ObsoleteStrategy other) => Name == other.Name;
    }



}
