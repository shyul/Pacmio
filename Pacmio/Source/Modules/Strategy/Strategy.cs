/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// ***************************************************************************

using IbXmlScannerParameter;
using Pacmio.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xu;

namespace Pacmio
{

    public class BarTableSetting : IEquatable<BarTableSetting>
    {
        public BarFreq BarFreq { get; set; }

        public BarType BarType { get; set; }

        public bool Equals(BarTableSetting other)
        {
            throw new NotImplementedException();
        }
    }

    // 60% is HFT trading!
    public abstract class Strategy : IEquatable<Strategy>
    {
        public Strategy(string name)
        {
            Name = name;
        }

        public string Name { get; set; } = "Default TradeRule";

        public int Order { get; set; } = 0;

        public MultiTimePeriod TradeTimeOfDay { get; set; }

        public bool IsDayTrade => true;

        // Example custom indicators for filter / trade result
        public SMA DailySMA5 { get; }

        public Indicator DailyFilter { get; }

        public Indicator TradeIndicator
        {
            get;

        }


        // Step 1: Define WatchList (Filters) Group sort by time frame -> Filter has B.A.S 

        // Step 1a: optionally manually defined [[[[ Daily ]]]] Scanner for faster live trading

        // Step 2: Define Signal Group

        /// <summary>
        /// Bar Analysis vs Multi Time Frame
        /// </summary>
        protected Dictionary<(BarFreq BarFreq, BarType BarType), BarAnalysisSet> BarAnalysisSets { get; } = new Dictionary<(BarFreq BarFreq, BarType BarType), BarAnalysisSet>();

        public virtual void ClearBarAnalysisSet() => BarAnalysisSets.Clear();

        public virtual BarAnalysisSet this[BarFreq BarFreq, BarType BarType = BarType.Trades]
        {
            get
            {
                if (BarAnalysisSets.ContainsKey((BarFreq, BarType)))
                    return BarAnalysisSets[(BarFreq, BarType)];
                else
                    return null;
            }
            set
            {
                if (value is BarAnalysisSet bas)
                    BarAnalysisSets[(BarFreq, BarType)] = new BarAnalysisSet(bas);
                else if (BarAnalysisSets.ContainsKey((BarFreq, BarType)))
                    BarAnalysisSets.Remove((BarFreq, BarType));
            }
        }

        public virtual void Calculate(Contract c, Period pd, CancellationTokenSource cts)
        {
            var list = BarAnalysisSets.OrderByDescending(n => n.Key.BarFreq);

            int i = 0;
            foreach (var item in list)
            {
                BarTable bt = StrategyManager.BarTableSet.AddContract(c, item.Key.BarFreq, item.Key.BarType, ref pd, cts);




                BarAnalysisSet bas = item.Value;

                // Find all BarTables here
                // Sort the BarFreq and calculate higher time scale first

                // Lower Time Frame BarTable get loaded...

                // Lowest Time Frame gets Position Information

                bt.CalculateOnly(bas);

                i++;
            }

            // Evaluate at the end
            Evaluate(c);
        }



        // !!! The Function Actually Makes The Purchase
        public void Evaluate(Contract c)
        {

        }

        #region Trading Timing


        public (int, Frequency) WaitLengthForStandingOrder { get; }

        public double MaximumPriceGoingPositionFromDecisionPointPrecent { get; }

        public double MaximumPriceGoinNegativeFromDecisionPointPrecent { get; }


        /// <summary>
        /// The number of days for getting the bench mark
        /// </summary>
        public virtual int SimulateDays => 5;

        /// <summary>
        /// The number of days enters the actual trade (Evaluate) or tradelog for simulation | final bench mark
        /// </summary>
        public virtual int TradingDays => 1;


        public virtual void Simulate(Contract c, Period pd, CancellationTokenSource cts)
        {
            var list = BarAnalysisSets.OrderByDescending(n => n.Key.BarFreq);


            int i = 0;
            foreach (var item in list)
            {
                BarTable bt = StrategyManager.BarTableSet.AddContract(c, item.Key.BarFreq, item.Key.BarType, ref pd, cts);
                BarAnalysisSet bas = item.Value;

                // Apply intermediate result and level
                bt.CalculateOnly(bas);

                // Get intermediate result and level

                // Get decision wether to pursue the lower time frames....

                i++;
            }
        }


        #endregion Trading Timing

        public bool Equals(Strategy other) => other is Strategy tr && Name == tr.Name;
    }
}
