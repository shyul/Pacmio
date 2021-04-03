/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
///
/// VWAP Cross, SMA, EMA, 200, 50, 20, 5
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using Xu;

namespace Pacmio.Analysis
{
    public class NumericColumnLevelAnalysis : BarAnalysis, ILevelAnalysis
    {



        public Dictionary<NumericColumn, double> StrengthLUT { get; } = new Dictionary<NumericColumn, double>();

        public DatumColumn Column_Result { get; }

        public string AreaName { get; }

        protected override void Calculate(BarAnalysisPointer bap)
        {


        }
    }
}