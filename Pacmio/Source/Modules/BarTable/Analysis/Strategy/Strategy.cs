/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xu;

namespace Pacmio
{
    public abstract class Strategy : IEquatable<Strategy>
    {
        public Strategy(string name)
        {
            Name = name;
        }

        public string Name { get; set; } = "Default TradeRule";

        public int Order { get; set; } = 0;

        protected Dictionary<BarFreq, BarAnalysisSet> Analyses { get; } = new Dictionary<BarFreq, BarAnalysisSet>();

        public BarAnalysisSet this[BarFreq freq]
        {
            get
            {
                if (Analyses.ContainsKey(freq))
                    return Analyses[freq];
                else
                    return null;
            }
            set
            {
                if (value is BarAnalysisSet bas)
                    Analyses[freq] = bas;
                else if (Analyses.ContainsKey(freq))
                    Analyses.Remove(freq);
            }
        }

        public void Clear() => Analyses.Clear();

        /*
        public virtual BarAnalysisSet BarAnalysisSet(BarFreq barFreq)
        {
            if (Analyses.ContainsKey(barFreq))
                return Analyses[barFreq];
            else
                return null;
        }*/


        public virtual void Calculate(BarTable bt, int i)
        {


        }

        public void Simulate(BarTable bt)
        {

        }

        // !!! The Function Actually Makes The Purchase
        public void Evaluate(Contract c)
        {

        }

        #region Trading Timing

        public int TrainingDays { get; set; } = 2;

        public int TradingDays { get; set; } = 1;

        /// <summary>
        /// Use MultiTime here.
        /// </summary>
        public Range<Time> TradingTimeRange { get; set; } = new Range<Time>(new Time(9, 30), new Time(16, 0));

        #endregion Trading Timing

        public bool Equals(Strategy other) => other is Strategy tr && Name == tr.Name;
    }
}
