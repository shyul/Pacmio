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
    public abstract class FilterAnalysis : SingleDataAnalysis, IEquatable<FilterAnalysis>
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

        public FilterScreenResult RunScan(BarTableSet bts, Period pd)
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

            return new(bts.Contract, this, allbars, bullishBars, bearishBars);
        }

        #region Equality

        public override int GetHashCode() => GetType().GetHashCode() ^ Name.GetHashCode() ^ BarFreq.GetHashCode() ^ PriceType.GetHashCode();
        public bool Equals(FilterAnalysis other) => GetType() == other.GetType() && Name == other.Name && BarFreq == other.BarFreq && PriceType == other.PriceType;
        public static bool operator !=(FilterAnalysis s1, FilterAnalysis s2) => !s1.Equals(s2);
        public static bool operator ==(FilterAnalysis s1, FilterAnalysis s2) => s1.Equals(s2);
        public override bool Equals(object other) => other is FilterAnalysis ba && Equals(ba);

        #endregion Equality
    }
}
