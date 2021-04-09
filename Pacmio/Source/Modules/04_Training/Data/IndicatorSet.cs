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
    public class IndicatorSet : IEnumerable<(BarFreq freq, DataType type, BarAnalysisSet bas)>
    {
        public bool IsUptoTick(IEnumerable<BarTable> bts, DateTime tickTime)
        {
            var btList = bts.Where(bt => TimeFrameList.Contains((bt.BarFreq, bt.Type))).Where(bt => bt.LastCalculatedTickTime < tickTime);
            return btList.Count() == 0;
        }

        private Dictionary<(BarFreq freq, DataType type), Indicator> IndicatorLUT { get; } = new();

        public List<(BarFreq freq, DataType type)> TimeFrameList => IndicatorLUT.Keys.ToList();

        public Indicator this[BarFreq freq, DataType type = DataType.Trades]
        {
            get
            {
                var key = (freq, type);
                return IndicatorLUT.ContainsKey(key) ? IndicatorLUT[key] : null;
            }

            set
            {
                var key = (freq, type);

                if (value is null)
                {
                    if (IndicatorLUT.ContainsKey(key))
                        IndicatorLUT.Remove(key);

                    if (BarAnalysisSetLUT.ContainsKey(key))
                        BarAnalysisSetLUT.Remove(key);
                }
                else
                {
                    IndicatorLUT[key] = value;
                    BarAnalysisSetLUT[key] = new BarAnalysisSet(value);
                }
            }
        }

        public BarAnalysisSet this[BarTable bt] => BarAnalysisSetLUT.ContainsKey((bt.BarFreq, bt.Type)) ? BarAnalysisSetLUT[(bt.BarFreq, bt.Type)] : null;

        private Dictionary<(BarFreq freq, DataType type), BarAnalysisSet> BarAnalysisSetLUT { get; } = new();

        public IEnumerator<(BarFreq freq, DataType type, BarAnalysisSet bas)> GetEnumerator()
            => BarAnalysisSetLUT.
            OrderByDescending(n => n.Key.freq).
            ThenByDescending(n => n.Key.type).
            Select(n => (n.Key.freq, n.Key.type, n.Value)).
            GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public (MultiPeriod bullish, MultiPeriod bearish) RunDailyScreener(BarTableSet bts)
        {
            var inds = IndicatorLUT.Where(n => n.Key.freq >= BarFreq.Daily).OrderByDescending(n => n.Key.freq);

            Dictionary<(BarFreq freq, DataType type), (MultiPeriod bullish, MultiPeriod bearish)> Periods = new();

            Period range = new Period();

            MultiPeriod bullish = new MultiPeriod();
            MultiPeriod bearish = new MultiPeriod();

            int i = 0;
            foreach (var item in inds)
            {
                Console.WriteLine(">>>>>>>>>>>>>>> Run Indicator: " + item.Value.Name);

                BarTable bt = bts[item.Key.freq, item.Key.type];

                range.Insert(bt.Period);

                BarAnalysisSet bas = this[bt];
                bt.CalculateRefresh(bas);

                Indicator ind = this[item.Key.freq, item.Key.type];
                var BullishBars = bt.Bars.Where(n => n.GetSignalScore(ind).Bullish > ind.BullishPointLimit);
                var BearishBars = bt.Bars.Where(n => n.GetSignalScore(ind).Bearish < ind.BearishPointLimit);

                if (i == 0)
                {
                    BullishBars.RunEach(n => bullish.Add(n.Period));
                    BearishBars.RunEach(n => bearish.Add(n.Period));
                }
                else
                {
                    bullish.Remove(new MultiPeriod(BullishBars.Select(n => n.Period)));
                    bearish.Remove(new MultiPeriod(BearishBars.Select(n => n.Period)));
                }
                i++;
            }

            Console.WriteLine(">>>>>>>>>>>>>>> Run Indicator: Completed!");

            return (bullish, bearish);
        }
    }
}
