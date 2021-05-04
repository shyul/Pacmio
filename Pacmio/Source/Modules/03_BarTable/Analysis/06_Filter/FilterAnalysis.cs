/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Filter 1) Signal Set; 2) Signal Threshold Range<double>(), within? / outside?; 3) Ranking Column?
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio
{
    public abstract class FilterAnalysis : SingleDataAnalysis
    {
        protected FilterAnalysis(BarFreq barFreq, PriceType priceType)
        {
            BarFreq = barFreq;
            PriceType = priceType;
        }

        public BarFreq BarFreq { get; }

        public PriceType PriceType { get; }

        public BarAnalysisList BarAnalysisList { get; protected set; }

        public Dictionary<string, string> TradeIdeaFilterTokens { get; } = new Dictionary<string, string>();

        public FilterScanResult RunScan(BarTableSet bts, Period pd)
        {
            BarTable bt = bts[BarFreq, PriceType];
            bt.CalculateRefresh(BarAnalysisList);
            var allbars = bt.Bars.Where(b => pd.Contains(b.Time));

            List<Bar> bullishBars = new();
            List<Bar> bearishBars = new();

            foreach (Bar b in allbars)
            {
                if (b[Column_Result] > 0) bullishBars.Add(b);
                else if (b[Column_Result] < 0) bearishBars.Add(b);
            }

            return new(bts.Contract, allbars, bullishBars, bearishBars);
        }
    }
}
