/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// 60% of the tradings today are done by HFT!
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
    public abstract class Strategy : IEquatable<Strategy>
    {
        public virtual string Name => "Default TradeRule";

        public override int GetHashCode() => Name.GetHashCode();

        public virtual int Order { get; set; } = 0;

        public bool Equals(Strategy other) => other.GetType() == GetType() && Name == other.Name;

        // Step 1: Define WatchList (Filters) Group sort by time frame -> Filter has B.A.S 

        public string Account { get; set; }

        public WatchList Scanner { get; set; }

        public EntryMethod EntryMethod { get; set; }

        public ExitMethod ExitMethod { get; set; }

        // Step 1a: optionally manually defined [[[[ Daily ]]]] Scanner for faster live trading

        // Step 2: Define Signal Group

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

        #region Training Settings

        /// <summary>
        /// The unit for training time frames
        /// </summary>
        public virtual BarFreq SimulateBarFreq { get; set; } = BarFreq.Daily;

        /// <summary>
        /// The number of days for getting the bench mark: RR ratio, win rate, volatility, max win, max loss, and so on.
        /// The commission model shall be defined by Simulate Engine.
        /// </summary>
        public virtual int SimulateLength { get; set; } = 5;

        /// <summary>
        /// The number of days enters the actual trade or tradelog for simulation | final bench mark.
        /// Only when the SimulationResult is positive (or above a threshold), does the trading start log, and this time, it logs the trades.
        /// </summary>
        public virtual int TradingLength { get; set; } = 1;

        #endregion Training Settings
    }
}
