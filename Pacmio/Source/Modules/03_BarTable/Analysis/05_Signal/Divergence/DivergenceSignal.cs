/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio
{
    public class DivergenceSignal : SignalAnalysis
    {
        public DivergenceSignal(TimePeriod tif, BarFreq barFreq, ISingleData osc, PriceType priceType = PriceType.Trades)
            : base(tif, barFreq, priceType)
        {
            Label = "("  + "," +  ")";
        }

        public DivergenceSignal(TimePeriod tif, BarFreq barFreq, ISingleData source, ISingleData osc, PriceType priceType = PriceType.Trades)
            : base(tif, barFreq, priceType)
        {

            Label = "(" + "," + ")";
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ MaximumPivotDistance ^ MinimumPivotDistance;


        public int MaximumPivotDistance { get; }

        public int MinimumPivotDistance { get; }

        /// <summary>
        /// Add peak detection
        /// </summary>
        public ISingleData SourceAnalysis { get; }

        /// <summary>
        /// Add peak detection
        /// </summary>
        public ISingleData IndicatorColumn { get; }



        protected override void Calculate(BarAnalysisPointer bap)
        {
            // Yield Trend Lines

            // Yield Divergence Type
        }
    }
}
