/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// The intermediate calculation for most pattern analysis
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Xu;

namespace Pacmio.Analysis
{
    public class TrailingPivotPtDatum : IDatum
    {
        public TrailingPivotPtDatum(TrailingPivotPtAnalysis tpa)
        {
            TrailingPivotPtAnalysis = tpa;
        }

        public TrailingPivotPtAnalysis TrailingPivotPtAnalysis { get; }

        public Range<double> PivotRange { get; } = new Range<double>(double.MaxValue, double.MinValue);
        
        public Range<double> TotalLevelRange { get; set; }

        public Dictionary<int, PivotPt> PositivePivotPtList { get; } = new Dictionary<int, PivotPt>();

        public Dictionary<int, PivotPt> NegativePivotPtList { get; } = new Dictionary<int, PivotPt>();

        public KeyValuePair<int, PivotPt>[] PivotPts => PositivePivotPtList.Concat(NegativePivotPtList).OrderBy(n => n.Key).ToArray();
    }
}
