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

// New Types for Bar
// ===================
// 1. TrendLine Type (Will be derived)
// 2. Signal Type
// 3. Pattern Type
// 4. Position Status
// 5. Better Tags
// 
// <<<<<<<<<< Get them from the Technical Anlysis (TA)'s purpose >>>>>>>>>>
// 1. For example, RSI: see how the overbought and oversold are causing price reversals
// 2. Moving average crossings. Test difference time periods

// Condition
// --------------------
// Result: List of symbol matching the condition
// 1. Volume, 
// 2. Price Range
// 3. Indicators + Elevation Factors

//public Dictionary<Contract, int> Parameters = new Dictionary<Contract, int>();

// Indication
// --------------------
// Elevation Factors (Bullish)
// Deprication Factors (Bearish)
// Signals
// Result: Signals list, sum of all signals at the close of a Bar
// --------------------
// 1. A pool of bullish and bearish factor-tets: <<<<<<<<<< Get them from the Technical Anlysis (TA)'s purpose >>>>>>>>>>
// 2. Test each of them individually for each contract. Signal clarity -> bullish signal to bullish upside rate / Bearish to downside rate. Delay, strength, winrate
// 3. Classic combination: Moving Average Cross (s) + Oscillator (s)
// 4. Combined Score to simulated trades: KPI: winrate, per trade profit

// Confirmation
// --------------------
// 1. Signal reseaches a certain level for the last Bar
// 2. Match time frame
// 3. One technical situation is met for current Bar.
// Buy Limit, Buy Stop Limit
// Result: Long / Short. Scale in??? Most unlikely.

// Validation
// --------------------
// 1. Entry Stop is met
// 2. Breakeven Stop is met
// 3. Profit taking limit is met
// Result: Remove liquidation, scale out, Sell / Cover

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
    public class IndicatorSet : IEnumerable<(BarFreq freq, DataType type, BarAnalysisSet bas)>
    {
        public IndicatorSet(IndicatorExec ie, BarFreq freq, DataType type) 
        {
            ExecutingIndicator = ie;
            ExecutingTimeFrame = (freq, type);
            Indicators[ExecutingTimeFrame] = ExecutingIndicator;
        }

        public bool IsUptoTick(IEnumerable<BarTable> bts, DateTime tickTime)
        {
            var btList = bts.Where(bt => TimeFrameList.Contains((bt.BarFreq, bt.Type))).Where(bt => bt.LastCalculatedTickTime < tickTime);
            return btList.Count() == 0;
        }

        #region Indicators

        public IndicatorExec ExecutingIndicator { get; set; }

        public (BarFreq freq, DataType type) ExecutingTimeFrame { get; set; }

        private Dictionary<(BarFreq freq, DataType type), Indicator> Indicators { get; } = new();

        public List<(BarFreq freq, DataType type)> TimeFrameList => Indicators.Keys.ToList();

        public Indicator this[BarFreq freq, DataType type = DataType.Trades]
        {
            get
            {
                var key = (freq, type);
                return Indicators.ContainsKey(key) ? Indicators[key] : null;
            }
            set
            {
                var key = (freq, type);
                if (key != ExecutingTimeFrame)
                    Indicators[(freq, type)] = value;
                else
                    throw new Exception("Can not override default executing indicator");
            }
        }

        public IEnumerator<(BarFreq freq, DataType type, BarAnalysisSet bas)> GetEnumerator()
            => Indicators.Select(n => (n.Key.freq, n.Key.type, new BarAnalysisSet(n.Value))).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion Indicators

        #region Order

        public double MinimumRiskRewardRatio { get; set; }

        public double MinimumTradeSize { get; set; }

        #endregion Order

        public OrderInfo GenerateOrder(ExecutionDatum ed)
        {


            return new OrderInfo();
        }

        // Step 1: Define WatchList (Filters) Group sort by time frame -> Filter has B.A.S 

        // Step 1a: optionally manually defined [[[[ Daily ]]]] Scanner for faster live trading

        // Step 2: Define Signal Group



        #region Training Settings

        /// <summary>
        /// The unit for training time frames
        /// </summary>
        public virtual BarFreq TrainingUnitFreq { get; set; } = BarFreq.Daily;

        /// <summary>
        /// The number of days for getting the bench mark: RR ratio, win rate, volatility, max win, max loss, and so on.
        /// The commission model shall be defined by Simulate Engine.
        /// </summary>
        public virtual int TrainingLength { get; set; } = 5;

        /// <summary>
        /// The number of days enters the actual trade or tradelog for simulation | final bench mark.
        /// Only when the SimulationResult is positive (or above a threshold), does the trading start log, and this time, it logs the trades.
        /// </summary>
        public virtual int TradingLength { get; set; } = 1;

        public BarTable GetTrainingTable(Contract c) => BarTableManager.GetOrCreateDailyBarTable(c, TrainingUnitFreq);

        #endregion Training Settings
    }
}
