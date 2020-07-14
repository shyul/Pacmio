/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        protected Dictionary<(BarFreq BarFreq, BarType BarType), BarAnalysisSet> Analyses { get; } = new Dictionary<(BarFreq BarFreq, BarType BarType), BarAnalysisSet>();

        public virtual void ClearBarAnalysisSet() => Analyses.Clear();

        //public BarFreq FilterBarFreq { get; set; }

        /// <summary>
        /// BarFreq for Trading || Evaluate Positions
        /// All other BarFreq are filters and should be calculated from high to low
        /// 
        /// How Filter works
        /// 1. Get the filter event
        /// 2. Download 300 units before the event, and one unit worth of data after the event for calculation and evaluations
        /// </summary>
        public BarFreq PrimaryBarFreq { get; set; }

        public virtual BarAnalysisSet this[BarFreq BarFreq, BarType BarType = BarType.Trades]
        {
            get
            {
                if (Analyses.ContainsKey((BarFreq, BarType)))
                    return Analyses[(BarFreq, BarType)];
                else
                    return null;
            }
            set
            {
                if (value is BarAnalysisSet bas)
                    Analyses[(BarFreq, BarType)] = new BarAnalysisSet(bas);
                else if (Analyses.ContainsKey((BarFreq, BarType)))
                    Analyses.Remove((BarFreq, BarType));
            }
        }

        public virtual void Calculate(Contract c, Period pd, CancellationTokenSource cts)
        {
            var list = Analyses.OrderByDescending(n => n.Key.BarFreq);

            int i = 0;
            foreach (var item in list)
            {
                BarTable bt = StrategyManager.BarTableSet.AddContract(c, item.Key.BarFreq, item.Key.BarType, ref pd, cts);




                BarAnalysisSet bas = item.Value;



                bt.CalculateOnly(bas);

                i++;
            }

            // Evaluate at the end
            Evaluate(c);
        }

        public virtual void Simulate(Contract c)
        {
            // Find all BarTables here

            // Sort the BarFreq and calculate higher time scale first

            // Lower Time Frame BarTable get loaded...

            // Lowest Time Frame gets Position Information

        }

        // !!! The Function Actually Makes The Purchase
        public void Evaluate(Contract c)
        {

        }

        #region Trading Timing

        /// <summary>
        /// The number of days for getting the bench mark
        /// </summary>
        public int TrainingDays { get; set; } = 2;

        /// <summary>
        /// The number of days enters the actual trade (Evaluate) or tradelog for simulation | final bench mark
        /// </summary>
        public int TradingDays { get; set; } = 1;

        /// <summary>
        /// Use MultiTime here.
        /// </summary>
        public Range<Time> TradingTimeRange { get; set; } = new Range<Time>(new Time(9, 30), new Time(16, 0));

        #endregion Trading Timing

        public bool Equals(Strategy other) => other is Strategy tr && Name == tr.Name;
    }
}
