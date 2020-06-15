/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Xu;
using Xu.Chart;

namespace Pacmio
{
    public class TradeParameter
    {
        // Time Period

        public Indicator Indicator { get; set;  } = new Indicator();


        public IndicatorParameter Parameter { get; } = new IndicatorParameter();


        public string Name => Indicator.Name;

        public int Order { get => IsManuallyAdded ? m_order : int.MinValue; set => m_order = IsManuallyAdded ? value : int.MinValue; }
        private int m_order = int.MinValue; 

        public bool IsManuallyAdded { get; set; } = false;

    }

    public class Indicator 
    {
        public string Name { get; }

        public int Order { get; set; }

        public List<BarAnalysis> BarAnalysesTimeFrame1 { get; }

        // List the scope of usable MAs, and testable range of intervals...

        // Different Time Frame

        public List<BarAnalysis> BarAnalysesTimeFrame2 { get; }


        // Choices of SMA / EMA / Different Oscillators and so on...


        // Will also be the one getting the Elevation Factors

        // Here is how many days for generating new TradeParameter
        public int BackTestingLength { get; } = 5;

        // Will re-generate the IndicatorParameter after 2 days.
        public int ValidTradingLength { get; } = 2;

        // So we use the past five days of trading history to generate the next 2 days trading parameter
    }

    public class IndicatorParameter 
    {

        // Fit MA Cross -> SMA, EMA, ...
        // MA Cross intervals?

        // Levels

        // Confirmation Parameters


        // Validation Parameters

    }

    public static class Strategy
    {
        public static Dictionary<Contract, TradeParameter> WatchList = new Dictionary<Contract, TradeParameter>();

        public static void GetWatchList() // Get it from the Market Scanner
        {
        
        }
        
        public static void GetWatchList(List<Contract> list) 
        {
        
        }


        public static void GetWatchList(int daysSinceIPO, Range<double> priceRange, Range<double> volumeRange) // Types // Sinc 
        {
        
        
        }

        public static void CleanUpWatchList() 
        {
        
        
        }


        public static List<Indicator> IndicatorList = new List<Indicator>();

        public static void Optimize() 
        {
            // We have to run one symbol of a time, or the RAM is going to explode.

        
        }


        public static void Simulate() 
        {
        
        
        
        }


        public static void LiveTrade() 
        {
        
        
        }


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
    }


}
