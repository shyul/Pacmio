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
    public class IndicatorSet : IEnumerable<(BarFreq freq, DataType type, Indicator ind)>
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
                }
                else
                {
                    IndicatorLUT[key] = value;
                }
            }
        }

        public Indicator this[BarTable bt] => IndicatorLUT.ContainsKey((bt.BarFreq, bt.Type)) ? IndicatorLUT[(bt.BarFreq, bt.Type)] : null;

        public IEnumerator<(BarFreq freq, DataType type, Indicator ind)> GetEnumerator()
            => IndicatorLUT.
            OrderByDescending(n => n.Key.freq).
            ThenByDescending(n => n.Key.type).
            Select(n => (n.Key.freq, n.Key.type, n.Value)).
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
            foreach(var item in this) 
            {
                bts[item.freq, item.type].CalculateRefresh(item.ind);
            }

            BarTable bt = bts[ExecutionTimeFrame.freq, ExecutionTimeFrame.type];
            bt.CalculateRefresh(ExecutionIndicator);

            // Collect Bullish Execution Based on Bullish Periods
            // Collect Bearish Execution Based on Bearish Periods
        }

        public (BarFreq freq, DataType type) FilterTimeFrame { get; set; } = (BarFreq.Daily, DataType.Trades);

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



        /*
        public (MultiPeriod bullish, int bullcount, MultiPeriod bearish, int bearcount) RunFilter(BarTableSet bts, BarFreq freqLimit = BarFreq.Daily)
        {
            var inds = IndicatorLUT.Where(n => n.Key.freq >= freqLimit).OrderByDescending(n => n.Key.freq).ThenBy(n => n.Key.type);

            Dictionary<(BarFreq freq, DataType type), (MultiPeriod bullish, MultiPeriod bearish)> Periods = new();

            Period range = new Period();

            MultiPeriod bullish = new MultiPeriod();
            MultiPeriod bearish = new MultiPeriod();

            int i = 0, p = 0, n = 0;
            foreach (var item in inds)
            {
                Console.WriteLine(">>>>>>>>>>>>>>> Run Indicator: " + item.Value.Name + " <<<<<<<<<<<<<<<<<<<");

                BarTable bt = bts[item.Key.freq, item.Key.type];

                range.Insert(bt.Period);

                BarAnalysisSet bas = this[bt].BarAnalysisSet;
                bt.CalculateRefresh(bas);

                Indicator ind = this[item.Key.freq, item.Key.type];
                var BullishBars = bt.Bars.Where(n => n.GetSignalScore(ind).Bullish >= ind.BullishPointLimit);
                var BearishBars = bt.Bars.Where(n => n.GetSignalScore(ind).Bearish <= ind.BearishPointLimit);

                if (item.Key.freq == BarFreq.Daily)
                {
                    p = BullishBars.Count();
                    n = BearishBars.Count();
                }

                if (i == 0)
                {
                    BullishBars.RunEach(n => bullish.Add(ToDailyPeriod(n.Period)));
                    BearishBars.RunEach(n => bearish.Add(ToDailyPeriod(n.Period)));
                }
                else
                {
                    bullish.Remove(new MultiPeriod(BullishBars.Select(n => n.Period)));
                    bearish.Remove(new MultiPeriod(BearishBars.Select(n => n.Period)));
                }
                i++;
            }

            Console.WriteLine(">>>>>>>>>>>>>>> Run Indicator: Completed!");

            return (bullish, p, bearish, n);
        }*/

        public static Period ToDailyPeriod(Period pd) => new Period(pd.Start.Date, pd.Stop.AddDays(1).Date);
    }
}
