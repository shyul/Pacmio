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

/// 3 Criteria for Finding Swing Stocks
/// https://www.youtube.com/watch?v=GHG3Kf-FYvw
/// 
/// 3 (Powerful) Swing Trading Strategies
/// https://www.youtube.com/watch?v=MK2V6GKBmf0

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio
{
    public class IndicatorSet : IEnumerable<Indicator>
    {
        public bool IsUptoTick(IEnumerable<BarTable> bts, DateTime tickTime)
        {
            var btList = bts.Where(bt => TimeFrameList.Contains((bt.BarFreq, bt.Type))).Where(bt => bt.LastCalculatedTickTime < tickTime);
            return btList.Count() == 0;
        }

        private List<Indicator> IndicatorList { get; } = new();

        public List<(BarFreq freq, DataType type)> TimeFrameList => IndicatorList.Select(n => (n.BarFreq, n.DataType)).ToList();

        /*
        public Indicator this[BarFreq freq, DataType type = DataType.Trades]
        {
            get
            {
                var key = (freq, type);
                return IndicatorList.ContainsKey(key) ? IndicatorList[key] : null;
            }

            set
            {
                var key = (freq, type);

                if (value is null)
                {
                    if (IndicatorList.ContainsKey(key))
                        IndicatorList.Remove(key);
                }
                else
                {
                    IndicatorList[key] = value;
                }
            }
        }

        public Indicator this[BarTable bt] => IndicatorList.ContainsKey((bt.BarFreq, bt.Type)) ? IndicatorList[(bt.BarFreq, bt.Type)] : null;
        */

        public IEnumerator<Indicator> GetEnumerator()
            => IndicatorList.
            OrderByDescending(n => n.BarFreq).
            ThenByDescending(n => n.DataType).
            GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public (BarFreq freq, DataType type) ExecutionTimeFrame { get; set; }

        public Indicator ExecutionIndicator { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bts"></param>
        /// <param name="bullish">Only Long Excute in bullish Periods</param>
        /// <param name="bearish">Only Bear Excute in bearish Periods</param>
        public void RunBackTest(BarTableSet bts, MultiPeriod bullish, MultiPeriod bearish)
        {
            foreach (var ind in this)
            {
                bts[ind.BarFreq, ind.DataType].CalculateRefresh(ind);
            }

            BarTable bt = bts[ExecutionTimeFrame.freq, ExecutionTimeFrame.type];
            bt.CalculateRefresh(ExecutionIndicator);

            // Collect Bullish Execution Based on Bullish Periods
            // Collect Bearish Execution Based on Bearish Periods
        }

        public (BarFreq freq, DataType type) FilterTimeFrame { get; set; } = (BarFreq.Daily, DataType.Trades);


        /// <summary>
        /// The first simple filter to narrow down the list before any complex BarAnalysis.
        /// </summary>
        public Indicator FilterIndicator { get; set; }

        public (IEnumerable<Bar> BullishBars, IEnumerable<Bar> BearishBars) RunFilter(BarTableSet bts, Period pd) => RunFilter(bts, FilterIndicator, pd, FilterTimeFrame.freq, FilterTimeFrame.type);

        public (MultiPeriod bullish, MultiPeriod bearish) RunFilterMultiPeriod(BarTableSet bts, Period pd) => RunFilterMultiPeriod(bts, FilterIndicator, pd, FilterTimeFrame.freq, FilterTimeFrame.type);

        public static (IEnumerable<Bar> BullishBars, IEnumerable<Bar> BearishBars) RunFilter(BarTableSet bts, Indicator filter, Period pd, BarFreq freq = BarFreq.Daily, DataType type = DataType.Trades)
        {
            BarTable bt = bts[freq, type];

            BarAnalysisSet bas = filter.BarAnalysisSet;
            bt.CalculateRefresh(bas);

            Indicator ind = filter;
            var BullishBars = bt.Bars.Where(b => pd.Contains(b.Time) && b.GetSignalScore(ind).Bullish >= ind.BullishPointLimit);
            var BearishBars = bt.Bars.Where(b => pd.Contains(b.Time) && b.GetSignalScore(ind).Bearish <= ind.BearishPointLimit);

            return (BullishBars, BearishBars);
        }

        public static (MultiPeriod bullish, MultiPeriod bearish) RunFilterMultiPeriod(BarTableSet bts, Indicator filter, Period pd, BarFreq freq = BarFreq.Daily, DataType type = DataType.Trades)
        {
            var (BullishBars, BearishBars) = RunFilter(bts, filter, pd, freq, type);

            MultiPeriod bullish = new MultiPeriod();
            MultiPeriod bearish = new MultiPeriod();
            BullishBars.RunEach(n => bullish.Add(ToDailyPeriod(n.Period)));
            BearishBars.RunEach(n => bearish.Add(ToDailyPeriod(n.Period)));

            return (bullish, bearish);
        }

        public static Period ToDailyPeriod(Period pd) => new Period(pd.Start.Date, pd.Stop.AddDays(1).Date);
    }
}
