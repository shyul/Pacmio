/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio.Analysis
{
    public class DivergenceSignal : SignalAnalysis
    {
        public DivergenceSignal(ISingleData osc)
        {

        }

        public DivergenceSignal(ISingleData source, ISingleData osc)
        {


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
