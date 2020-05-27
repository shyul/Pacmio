/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - shyu.lee@gmail.com
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
    public class ObsoleteScalping : ObsoleteStrategy
    {
        public ObsoleteScalping()
        {
            MA_Volume = new EMA(TableList.Column_Volume, 20) { Color = Color.DeepSkyBlue, LineWidth = 1 };
            MA_Volume.LineSeries.Side = AlignType.Left;

            VWAP = new VWAP(new Frequency(TimeUnit.Days)) { Color = Color.HotPink };
            ADX = new ADX(14) { Order = 100, HasXAxisBar = true };

            MA_Fast = new EMA(5) { Color = Color.Teal };
            MA_Slow = new EMA(13) { Color = Color.Peru };
            MA_Cross = new DualData(MA_Fast, MA_Slow);

            Indicator = new RSI(14) { Order = 98 };
            Indicator_Reference_Cross = new ConstantData((ISingleData)Indicator, new Range<double>(49, 51));

            PSAR_Stop = new PSAR(0.02, 0.2);
            Chanderlier_Stop = new Chanderlier(22, 3) { Color = Color.Blue, Low_Color = Color.Plum };
        }

        public override string Name => "Scalping_" + PrimaryBarFreq + "_vs_" + TrendTableBarFreq;

        public BarFreq TrendTableBarFreq { get; set; } = BarFreq.Minutes_3;

        private SMA MA_Volume { get; }

        private VWAP VWAP { get; }
        private ADX ADX { get; }
        private SMA MA_Fast { get; }

        private SMA MA_Slow { get; }

        private DualData MA_Cross { get; }

        private BarAnalysis Indicator { get; }

        private ConstantData Indicator_Reference_Cross { get; }

        private PSAR PSAR_Stop { get; }

        private Chanderlier Chanderlier_Stop { get; }



        /// <summary>
        /// Search criteria
        /// Get from Scanner
        /// Use a static list here.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<Contract> GetWatchList()
        {
            //var list = ContractManager.GetOrFetch(new List<string>() { "XOM", "BAC", "CSCO", "BMY" }, "US", null, null); //, "ET"
            //var list = ContractManager.GetOrFetch(new List<string>() { "TQQQ", "FB", "AMD", "GOOGL", "VXX", "SNAP", "INTC", "PENN", "XOM", "UGAZ", "MRNA", "WFC", "DAL", "OXY", "BAC", "ET", "AAPL", "MSFT", "LUV",
            //"UBER", "BA", "DIS", "C", "SIRI", "T", "PFE", "FCX", "TWTR", "JPM", "GM", "GOLD", "CARR", "SLB", "CSCO", "GILD", "KMI", "CVX", "V", "MPC", "KO", "BMY", "SBUX", "RTX", "WMB", "EBAY", "SQ", "ABBV", "VZ", "HPQ", "CTL", "EBD", "COP" }, "US", null, null); //, "ET"

            //var list = ContractManager.GetOrFetch(new List<string>() { "XLNX", "LULU","CCL", "NATI","DAL", "UAL", "HAL", 
            //"PINS", "RCL", "MGM", "CARR", "PCG", "VIAC", "CTL", "LYFT", "KEY", "RF", "SYF", "MRVL", "WORK", "COG", "IMMU", "TLRY", "OSTK", "IO", "CHEF", "PLAY", "VVUS","NVDA","AMGN","UPRO","ALXN" }, "US", null, null);
            //var list = ContractManager.GetOrFetch(new List<string>() { "XLNX", "LULU","CCL", "NATI","DAL" }, "US", null, null);


            //var list = ContractList.GetOrFetch(new List<string>() { "USO", "TQQQ", "UBER", "GOLD", "PENN", "BA", "LUV", "SNAP", "TQQQ", "ABBV", "AMD", "PLAY", "LYFT", "MGM", "COG", "IMMU", "VIAC", "PINS", "OSTK", "BYND", "UBER", "BA", "DIS", "C", "SIRI", "T", "PFE", "FCX", "TWTR", "JPM", "GM", "GOLD", "CARR", "SLB", "CSCO", "GILD", "KMI", "CVX", "V", "MPC", "KO", "BMY", "SBUX", "RTX", "WMB", "EBAY", "SQ", "ABBV", "VZ", "HPQ", "CTL", "EBD", "COP" }, "US", null, null); //, "ET"
            var list = ContractList.GetOrFetch(new List<string>() { "VXX", "UGAZ", "FB", "AMD", "GILD", "GOLD", "PENN", "BA", "LUV", "SNAP", "TQQQ", "ABBV", "AMD", "PLAY", "LYFT", "MGM", "COG", "IMMU", "VIAC", "PINS", "OSTK", "BYND", "UBER", "BA", "DIS" }, "US", null, null);
            //var list = ContractList.GetOrFetch(new List<string>() { "PLAY", "LYFT", "SPY", "FB", "AMD", "GILD", "TQQQ" }, "US", null, null);

            foreach (Contract c in list)
            {
                BarTable bt_day = TableList.GetTable(c, BarFreq.Daily, BarType.Trades);
                bt_day.Reset(new Period(DateTime.MinValue, DateTime.Now.AddDays(1)), null, null);
            }

            return list;
        }

        public List<BarAnalysis> BarAnalysisConfig()
        {
            //EMA_cross = new DualData(new EMA(5) { Color = Color.Teal }, new EMA(13) { Color = Color.Peru });
            //var mfi = new MFI(14) { Order = 99 };
            //var divergence = new Divergence(rsi);


            //var boll = new Bollinger(20, 2.0);
            List<BarAnalysis> barAnalyses = new List<BarAnalysis>
            {
                MA_Volume,
                //ema5,
                //smma5,
                //ema5_smma5_cross,
               
                //new BoxRange(14),
                //new Bollinger(100, 2.0),
                new Bollinger(20, 2.0),
            
                //rsi,
   
                //new SMA(5) { Color = Color.Blue },
                //new SMA(20) { Color = Color.Green },
                //new SMA(50) { Color = Color.Tomato },
                //new EMA(200) { Color = Color.Salmon.Opaque(50), LineWidth = 10 },
                //new EMA(50) { Color = Color.Green, LineWidth = 2 },
                //new EMA(25) { Color = Color.DarkGreen, LineWidth = 1 },
                //new HMA(16) { Color = Color.LimeGreen },
                //new WMA(16) { Color = Color.LimeGreen },
                //new EMA(5) { Color = Color.SteelBlue },
                //new SMMA(5) { Color = Color.CadetBlue },
                //new RSI(14),
                //new MFI(14),
          
                //new STO(14, 3, 3),
                //new ADX(60),
                
                //new WaveTrend(10, 21, 4, 0.015),
                PSAR_Stop,
                Chanderlier_Stop,
                VWAP,
                //new Pivot(BarFreq.Daily),
                MA_Cross,
                //divergence
                Indicator,
                //new MACD(12, 26, 9) { Order = 99 },
                ADX,


                Indicator_Reference_Cross,
                //new CandleStick(),
            };
            return barAnalyses;
        }

        public override void Setup(Contract c)
        {


            BarTable bt = TableList.GetTable(c, PrimaryBarFreq, BarType.Trades);
            var bas = BarAnalysisConfig();
            ChartList.GetForm(bt, bas);
            if (bt.IsEmpty) { bt.Reset(ObsoleteStrategyMaster.SimulatePeriod, null, null); }
        }

        public double MaxPositionValue { get; set; } = 12000;

        public Account LiveTradeAccount { get; set; }

        public override void RunLiveTrade(Contract c)
        {
            PositionStatus ps = LiveTradeAccount.GetPosition(c);
            BarTable bt = TableList.GetTable(c, PrimaryBarFreq, BarType.Trades);

            Bar b = bt.LastBar; // The Last Bar, and it keeps changing!!!
            Bar b_1 = bt[bt.LastIndex - 1];
            var bars = bt[bt.LastIndex, 4].Take(3);

            Console.WriteLine("$$$$ Analyzing: " + bt.Name);

            if ((ps.Quantity > 0 && b.Low < ps.Stop) || (ps.Quantity < 0 && b.High > ps.Stop))
            {
                if (!b.PositionHasExited)
                {
                    LiveTradeAccount.Exit(bt.Contract);
                }
                b.PositionHasExited = true;
            }
            else
            {
                Analyze(b, b_1, bars, LiveTradeAccount, ps);
            }

            b.SnapShotPosition(ps);
        }

        public override void Simulate(Contract c, SimulationAccount ac)
        {
            BarTable bt = TableList.GetTable(c, PrimaryBarFreq, BarType.Trades);

            DateTime startTime = ObsoleteStrategyMaster.SimulatePeriod.Start;
            DateTime stopTime = ObsoleteStrategyMaster.SimulatePeriod.Stop;

            int StartPt = bt.IndexOf(ref startTime);
            int StopPt = bt.IndexOf(ref stopTime);

            if (StartPt < 20) StartPt = 20;
            if (StopPt >= bt.Count) StopPt = bt.Count;
            if (StartPt > StopPt) return;

            PositionStatus ps = ac.GetPosition(c);

            for (int i = StartPt; i < StopPt; i++)
            {
                // Go over the the stop loss and great trades if the criteria is met
                Bar b = bt[i]; // The Last Bar, and it keeps changing!!!

                b.ResetPositionTrackingInfo();
            }

            for (int i = StartPt; i < StopPt; i++)
            {
                // Go over the the stop loss and great trades if the criteria is met
                Bar b = bt[i]; // The Last Bar, and it keeps changing!!!
                Bar b_1 = bt[i - 1];

                //var bars = bt[i, 5].Take(4); // Never include the last bar
                var bars = bt[i, 4].Take(3);
                ps.Set(ps.Quantity, ps.CostBasis);

                ac.SetSimulationQuoteCondition(bt, b);

                if ((ps.Quantity > 0 && b.Low < ps.Stop) || (ps.Quantity < 0 && b.High > ps.Stop))
                {
                    if (!b.PositionHasExited)
                    {
                        ac.Exit(bt.Contract);
                    }
                    b.PositionHasExited = true;

                }
                else
                    // Go over the analysis
                    Analyze(b, b_1, bars, ac, ps);
                /*
                if ((ps.Quantity > 0 && b.Low < ps.Stop) || (ps.Quantity < 0 && b.High > ps.Stop))
                    ac.Exit(bt.Contract);*/

                //ac.SetSimulationQuoteCondition(bt, b);

                b.SnapShotPosition(ps);
            }

            // Apply the trade log

        }

        private void Analyze(Bar b, Bar b_1, IEnumerable<Bar> bars, Account ac, PositionStatus ps)
        {
            /*
            Bar b = last_bars.Last();
            Bar b_1 = last_bars.ElementAt(last_bars.Count() - 2);
            var bars = last_bars.Take(last_bars.Count() - 1);*/

            BarTable bt = b.Table;
            Contract c = bt.Contract;

            var ma_cross_datums = bars.Select(b => b[MA_Cross.Signal_Column]);
            var ma_cross_datum_types = ma_cross_datums.SelectMany(n => ((DualDataSignalDatum)n).Types);

            DualDataSignalDatum ma_cross_csd = (DualDataSignalDatum)b_1[MA_Cross.Signal_Column];
            ConstantSignalDatum indicator_csd = (ConstantSignalDatum)b_1[Indicator_Reference_Cross.Signal_Column];
            //PositionStatus ps = ac.GetPosition(b.Table.Contract);


            // Exit Strategies for a long position
            if (ps.Quantity > 0)
            {
                double psar_stop = b_1[PSAR_Stop.Result_Column];
                double ch_stop = b_1[Chanderlier_Stop.High_Column];

                if (ma_cross_csd.Types.Contains(DualDataType.CrossDown)) // b.Low < ps.Stop || 
                {
                    ac.Exit(c);
                    b.PositionHasExited = true;
                }
                else if (!b.PositionHasAnalyzed)
                {
                    b.PositionHasAnalyzed = true;
                    double delta = Math.Abs(ps.Stop - ps.Limit) / 2;
                    double costbasis = ps.CostBasis + 0.03;
                    if (b.High > costbasis && ps.Stop < costbasis) // I will let go of this one because High is still a tracking factor
                    {
                        ac.Modify(bt.Contract, costbasis, ps.Limit);
                    }
                    else if (b.High > ps.Limit && ps.Stop < ps.Limit) // I will let go of this one because High is still a tracking factor
                    {
                        ac.Modify(bt.Contract, ps.Limit, ps.Limit + delta);
                    }
                    else
                    {
                        //double stop = b_1.Low;
                        //double stop = bars.Skip(bars.Count() - 2).Take(2).Select(b => b.Low).Min();

                        List<double> stops = new List<double>() { ps.Stop, psar_stop, ch_stop };
                        double stop = stops.Max();

                        ac.Modify(bt.Contract, stop, ps.Limit + delta);
                        //ac.Modify(bt.Contract, Math.Min(stop, ps.Stop + delta), ps.Limit + delta);
                        //ac.Modify(bt.Contract, ps.Stop + delta, ps.Limit + delta);
                    }

                }
            }
            // Exit Strategies for a short position
            else if (ps.Quantity < 0)
            {
                double psar_stop = b_1[PSAR_Stop.Result_Column];
                double ch_stop = b_1[Chanderlier_Stop.Low_Column];

                if (ma_cross_csd.Types.Contains(DualDataType.CrossUp)) // b.High > ps.Stop || 
                {
                    ac.Exit(c);
                    b.PositionHasExited = true;
                }
                else if (!b.PositionHasAnalyzed)
                {
                    b.PositionHasAnalyzed = true;
                    double delta = Math.Abs(ps.Stop - ps.Limit) / 2;
                    double costbasis = ps.CostBasis - 0.03;
                    if (b.Low < costbasis && ps.Stop > costbasis)
                    {
                        ac.Modify(bt.Contract, costbasis, ps.Limit - 2 * delta);
                    }
                    else if (b.Low < ps.Limit && ps.Stop > ps.Limit) // I will let go of this one because High is still a tracking factor
                    {
                        ac.Modify(bt.Contract, ps.Limit, ps.Limit - delta);
                    }
                    else
                    {
                        //double stop = b_1.High;
                        //double stop = bars.Skip(bars.Count() - 2).Take(2).Select(b => b.High).Max();

                        List<double> stops = new List<double>() { ps.Stop, psar_stop, ch_stop };
                        double stop = stops.Min();

                        ac.Modify(bt.Contract, stop, ps.Limit - delta);
                        //ac.Modify(bt.Contract, Math.Max(stop, ps.Stop - delta), ps.Limit - delta);
                        //ac.Modify(bt.Contract, ps.Stop - delta, ps.Limit - delta);
                    }
                }
            }

            double vol_avg = b_1[MA_Volume.Result_Column];
            double vol = b_1.Volume;

            bool vol_good = vol > vol_avg;

            // Entry Strategies, and the time effective should be set separately, however, I will let it go this time.
            if (ps.Quantity == 0 && vol_good && !b.PositionHasAnalyzed && !b.PositionHasExited && (b.Time.Hour > 9 || (b.Time.Hour == 9 && b.Time.Minute > 29)) && b.Time.Hour < 16)
            //if (ps.Quantity == 0 && vol_good && !b.PositionHasAnalyzed && !b.PositionHasExited && b.Time.Hour > 9 && b.Time.Hour < 12)
            {
                if (indicator_csd.Type == ConstantDataType.CrossUp || indicator_csd.Type == ConstantDataType.ExitAbove)
                {
                    if (ma_cross_datum_types.Contains(DualDataType.CrossUp) && !ma_cross_datum_types.Contains(DualDataType.CrossDown))
                    {
                        double psar_stop = b_1[PSAR_Stop.Result_Column];
                        double ch_stop = b_1[Chanderlier_Stop.High_Column];
                        List<double> stops = new List<double>() { b_1.Low, psar_stop, ch_stop };
                        double stop = stops.Max();

                        //double stop = b_1.Low;
                        //double stop = bars.Select(b => b.Low).Min();

                        double delta = b.Open - stop; // Use open as the reference price at the decision bar
                        if (delta > 0 && b.Low > stop)
                        {
                            double risk = delta / b.Open; // 0.008 ( x 100 = 0.8)
                            if (risk < 0.01)
                            {
                                double limit = b.Open + delta * 1.2; // 1:2 R/R
                                double size = Math.Min(ac.TotalValue * ObsoleteStrategyMaster.MaximumRiskPercentPerContract / risk,
                                                       ac.TotalValue * ObsoleteStrategyMaster.MaximumPositionSizePercent);

                                size = Math.Min(size, MaxPositionValue);
                                double quantity = size / b.Open;

                                if (quantity > 100) quantity -= quantity % 100;


                                ac.Modify(c, stop, limit);
                                ac.Entry(c, quantity);
                            }
                        }
                    }
                }
                else if (ma_cross_csd.Types.Contains(DualDataType.CrossUp) && !ma_cross_datum_types.Contains(DualDataType.CrossDown))
                {
                    if (indicator_csd.Type == ConstantDataType.Above ||
                        indicator_csd.Type == ConstantDataType.CrossUp ||
                        indicator_csd.Type == ConstantDataType.ExitAbove)
                    {
                        double psar_stop = b_1[PSAR_Stop.Result_Column];
                        double ch_stop = b_1[Chanderlier_Stop.High_Column];
                        List<double> stops = new List<double>() { b_1.Low, psar_stop, ch_stop };
                        double stop = stops.Max();

                        //double stop = b_1.Low;
                        //double stop = bars.Select(b => b.Low).Min();

                        double delta = b.Open - stop;

                        if (delta > 0 && b.Low > stop)
                        {
                            double risk = delta / b.Open;
                            if (risk < 0.01)
                            {
                                double limit = b.Open + delta * 1.2; // 1:2 R/R
                                double size = Math.Min(ac.TotalValue * ObsoleteStrategyMaster.MaximumRiskPercentPerContract / risk,
                                                       ac.TotalValue * ObsoleteStrategyMaster.MaximumPositionSizePercent);

                                size = Math.Min(size, MaxPositionValue);

                                // Shall we also calcuate the reward v.s the commission?
                                double quantity = size / b.Open;

                                if (quantity > 100) quantity -= quantity % 100;

                                ac.Modify(c, stop, limit);
                                ac.Entry(c, quantity);
                            }
                        }
                    }
                }
                else if (indicator_csd.Type == ConstantDataType.CrossDown || indicator_csd.Type == ConstantDataType.ExitBelow)
                {
                    if (ma_cross_datum_types.Contains(DualDataType.CrossDown) && !ma_cross_datum_types.Contains(DualDataType.CrossUp))
                    {
                        double psar_stop = b_1[PSAR_Stop.Result_Column];
                        double ch_stop = b_1[Chanderlier_Stop.Low_Column];
                        List<double> stops = new List<double>() { b_1.High, psar_stop, ch_stop };
                        double stop = stops.Min();

                        //double stop = b_1.High;
                        //double stop = bars.Select(b => b.High).Max();

                        double delta = stop - b.Open;
                        if (delta > 0 && b.High < stop)
                        {
                            double risk = delta / b.Open;
                            if (risk < 0.01)
                            {
                                double limit = b.Open - delta * 1.2; // 1:2 R/R
                                double size = Math.Min(ac.TotalValue * ObsoleteStrategyMaster.MaximumRiskPercentPerContract / risk,
                                                       ac.TotalValue * ObsoleteStrategyMaster.MaximumPositionSizePercent);

                                size = Math.Min(size, MaxPositionValue);

                                //double quantity = -size / b.Open;

                                double quantity = size / b.Open;

                                if (quantity > 100) quantity = quantity % 100 - quantity;
                                else quantity = -quantity;



                                ac.Modify(c, stop, limit);
                                ac.Entry(c, quantity);
                            }
                        }
                    }
                }
                else if (ma_cross_csd.Types.Contains(DualDataType.CrossDown) && !ma_cross_datum_types.Contains(DualDataType.CrossUp))
                {
                    if (indicator_csd.Type == ConstantDataType.Below ||
                        indicator_csd.Type == ConstantDataType.CrossDown ||
                        indicator_csd.Type == ConstantDataType.ExitBelow)
                    {
                        double psar_stop = b_1[PSAR_Stop.Result_Column];
                        double ch_stop = b_1[Chanderlier_Stop.Low_Column];
                        List<double> stops = new List<double>() { b_1.High, psar_stop, ch_stop };
                        double stop = stops.Min();

                        //double stop = b_1.High;
                        //double stop = bars.Select(b => b.High).Max();

                        double delta = stop - b.Open;
                        if (delta > 0 && b.High < stop)
                        {
                            double risk = delta / b.Open;
                            if (risk < 0.01)
                            {
                                double limit = b.Open - delta * 1.2; // 1:2 R/R
                                double size = Math.Min(ac.TotalValue * ObsoleteStrategyMaster.MaximumRiskPercentPerContract / risk,
                                                       ac.TotalValue * ObsoleteStrategyMaster.MaximumPositionSizePercent);

                                size = Math.Min(size, MaxPositionValue);

                                //double quantity = -size / b.Open;

                                double quantity = size / b.Open;

                                if (quantity > 100) quantity = quantity % 100 - quantity;
                                else quantity = -quantity;

                                ac.Modify(c, stop, limit);
                                ac.Entry(c, quantity);
                            }
                        }
                    }
                }
                b.PositionHasAnalyzed = true;
            }
        }
    }
}
