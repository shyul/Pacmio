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
    public class TrailingApexPtDatum : IDatum
    {
        public TrailingApexPtDatum(TrailingApexPtAnalysis tpa)
        {
            TrailingPivotPtAnalysis = tpa;
        }

        public TrailingApexPtAnalysis TrailingPivotPtAnalysis { get; }

        public Range<double> PivotRange { get; } = new Range<double>(double.MaxValue, double.MinValue);
        
        public Range<double> TotalLevelRange { get; set; }

        public Dictionary<int, ApexPt> PositivePivotPtList { get; } = new Dictionary<int, ApexPt>();

        public Dictionary<int, ApexPt> NegativePivotPtList { get; } = new Dictionary<int, ApexPt>();

        public KeyValuePair<int, ApexPt>[] PivotPts => PositivePivotPtList.Concat(NegativePivotPtList).OrderBy(n => n.Key).ToArray();
    }
}
