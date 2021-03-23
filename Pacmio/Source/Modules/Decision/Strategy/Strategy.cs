/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The trade rule applies to each contract
/// 
/// 60% of the tradings today are done by HFT!
/// 
/// https://school.stockcharts.com/doku.php?id=trading_strategies:moving_momentum
/// 
/// Typically, chartists 
/// first establish a trading bias or long-term perspective.
/// Second, chartists wait for pullbacks or bounces that will improve the risk-reward ratio.
/// Third, chartists look for a reversal that indicates a subsequent upturn or downturn in price.
/// 
/// Bearish signals are ignored when the bias is bullish. Bullish signals are ignored when the bias is bearish.
/// 
/// ***************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Pacmio.Analysis;
using Xu;

namespace Pacmio
{
    public abstract class Strategy : IDataConsumer, IEquatable<Strategy>, IEnumerable<(BarFreq freq, DataType type, BarAnalysisSet bas)>
    {
        public abstract void Dispose();

        public virtual string Name => "Default TradeRule";

        public override int GetHashCode() => Name.GetHashCode();

        public bool Equals(Strategy other) => other.GetType() == GetType() && Name == other.Name;




        public IEnumerator<(BarFreq freq, DataType type, BarAnalysisSet bas)> GetEnumerator()
            => Indicators.Select(n => (n.Key.freq, n.Key.type, n.Value.bas)).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();





        /// <summary>
        /// Acquire from WatchListManager
        /// </summary>
        public WatchList WatchList
        {
            get => m_WatchList;
            
            set
            {
                if (m_WatchList is WatchList w) w.RemoveDataConsumer(this);
                m_WatchList = value;
                m_WatchList.AddDataConsumer(this);
            }
        }

        private WatchList m_WatchList = null;

        public List<Contract> ContractList { get; private set; }

        public void DataIsUpdated(IDataProvider provider)
        {
            if (provider is WatchList w)
            {
                if (ContractList is List<Contract>)
                {
                    var list_to_remove = ContractList.Where(n => !w.Contracts.Contains(n));
                    list_to_remove.RunEach(n => n.MarketData.RemoveDataConsumer(this));
                }

                ContractList = w.Contracts.ToList();
                ContractList.ForEach(n => n.MarketData.AddDataConsumer(this));
            }
            else if (provider is MarketData md)
            {

                // Shall I use Indicator to trigger decision????
            }
        }




        public (BarFreq freq, DataType type) PrimaryTimeFrame { get; protected set; }

        private Dictionary<(BarFreq freq, DataType type), (Indicator ind, BarAnalysisSet bas)> Indicators { get; } = new();

        public Indicator this[BarFreq freq, DataType type]
        {
            get
            {
                var key = (freq, type);
                return Indicators.ContainsKey(key) ? Indicators[key].ind : null;
            }
            set
            {
                value.Strategy = this;
                Indicators[(freq, type)] = (value, new BarAnalysisSet(value));
            }
        }

        /// <summary>
        /// Subscribe to MarketData8 
        /// </summary>
        public void Decide() 
        {
            WatchList.Contracts.AsParallel().RunEach(c => 
            { 
                
            
            
            });
        }










        // Step 1: Define WatchList (Filters) Group sort by time frame -> Filter has B.A.S 

        // Step 1a: optionally manually defined [[[[ Daily ]]]] Scanner for faster live trading

        // Step 2: Define Signal Group



        #region Training / Simulation Settings

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

        #endregion Training / Simulation Settings
    }
}
